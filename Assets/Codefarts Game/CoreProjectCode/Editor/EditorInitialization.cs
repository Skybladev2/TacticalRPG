/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.CoreProjectCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Codefarts.CoreProjectCode.Services;
    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.CoreProjectCode.Settings.Xml;
    using Codefarts.Localization;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Handles settings registration.
    /// </summary>
    [InitializeOnLoad]
    public class EditorInitialization
    {
        /// <summary>
        /// Holds a value indicating whether the RunCallbacks method has been called at least once before.
        /// </summary>
        private static bool ranOnce;

        /// <summary>
        /// Initializes static members of the <see cref="EditorInitialization"/> class.
        /// </summary>
        static EditorInitialization()
        {
            EditorApplication.update += RunCallbacks;
        }

        /// <summary>
        /// Runs any registered callbacks.
        /// </summary>
        /// <remarks>The first time this method is run it will setup settings, localization and register global settings, then exit without running any callbacks.</remarks>
        private static void RunCallbacks()
        {
            // wait 5 sec before calling callbacks the first time
            if (!ranOnce)
            {
                ranOnce = true;

                // setup the settings system
                SetupSettings();

                // load localization strings
                LoadLocalizationData();

                // register global settings
                RegisterGlobalSettings();

                return;
            }

            // invoke callbacks from editor callback service     
            EditorCallbackService.Instance.Run();
        }

        /// <summary>
        /// Registers global settings
        /// </summary>
        private static void RegisterGlobalSettings()
        {
            var local = LocalizationManager.Instance;
            var config = UnityPreferencesManager.Instance;
            config.Register(local.Get("SETT_GlobalSettings"), Editor.Settings.GlobalSettingsMenuItem.Draw);
        }

        /// <summary>
        /// Sets up and loads settings for the settings service.
        /// </summary>
        private static void SetupSettings()
        {
            // attempt to get the setting file location from EditorPrefs
            var settingsFilename = EditorPrefs.GetString(UnityPreferencesManager.SettingsFileKey, null);

            // check if settings not found
            if (string.IsNullOrEmpty(settingsFilename))
            {
                // set default path is my documents folder
                settingsFilename = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                settingsFilename = Path.Combine(settingsFilename, "CodefartsSettings.xml");

                // attempt to create initial settings file if not already present
                if (!File.Exists(settingsFilename))
                {
                    // create initial settings file
                    File.WriteAllText(settingsFilename, "<?xml version=\"1.0\"?>\r\n<settings>\r\n</settings>");
                }

                // save in EditorPrefs
                EditorPrefs.SetString(UnityPreferencesManager.SettingsFileKey, settingsFilename);
            }

            // if no file exists report to user
            if (!File.Exists(settingsFilename))
            {
                EditorUtility.DisplayDialog("Warning", "No settings file exists! Click \"Edit->Preferences\" and select \"Codefarts\" to setup a settings file.", "Ok");
                return;
            }

            // set a xml file based settings
            SettingsManager.Instance.Values = new XmlDocumentLinqValues(settingsFilename);
        }

        /// <summary>
        /// Sets up and loads localized strings for the localization system.
        /// </summary>
        private static void LoadLocalizationData()
        {
            // only register current culture data
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            var settings = SettingsManager.Instance;

            // get centralized resources path 
            var resourcePath = settings.GetSetting(CoreGlobalConstants.ResourceFolderKey, "Codefarts.Unity");

            // get localization path
            var localPath = Path.Combine(resourcePath, "Localization");
            localPath = Path.Combine(localPath, currentCulture.Name).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // attempt to read localization data for the current culture
            var data = Resources.LoadAll(localPath, typeof(TextAsset));
            if (data == null || data.Length == 0)
            {       
                Debug.LogWarning(string.Format("No localization file(s) found for '{0}'", currentCulture.Name));
                return;
            }

            var entries = new Dictionary<string, string>();
            foreach (TextAsset item in data)
            {
                try
                {
                    var lines = item.text.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        var index = line.IndexOf(" ", StringComparison.Ordinal);
                        if (index == -1)
                        {
                            continue;
                        }

                        var keyValue = line.Substring(0, index).Trim();
                        var value = line.Substring(index).Trim();
                        value = value.Replace(@"\r\n", "\r\n");
                        var prefix = string.Empty;
                        if (!entries.ContainsKey(prefix + keyValue))
                        {
                            entries.Add(prefix + keyValue, value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }

            // register localization entries
            var local = LocalizationManager.Instance;
            local.Register(currentCulture, entries);
        }
    }
}