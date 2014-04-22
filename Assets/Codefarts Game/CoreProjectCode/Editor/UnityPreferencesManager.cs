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
    using System.Linq;

    using Codefarts.CoreProjectCode.Interfaces;
    using Codefarts.CoreProjectCode.Models;
    using Codefarts.CoreProjectCode.Settings.Xml;
    using Codefarts.Localization;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Provides a class for showing settings in the unity preferences window.
    /// </summary>
    public class UnityPreferencesManager
    {
        /// <summary>
        /// Provides a unique id key for storing and retrieving the location of the main xml settings file.
        /// </summary>
        public const string SettingsFileKey = "CodefartsSettingsFile";

        /// <summary>
        ///  Holds a reference to a singleton instance of a <see cref="UnityPreferencesManager"/> type.
        /// </summary>
        private static UnityPreferencesManager singleton;

        /// <summary>
        /// Holds a list of <see cref="CallbackModel{T}"/> types that implement <see cref="IRun"/>.
        /// </summary>
        private readonly List<IRun> callbacks;
        
        /// <summary>
        /// Holds the scroll value for the settings
        /// </summary>
        private Vector2 scroll;

        /// <summary>
        ///  Holds the selected settings index.
        /// </summary>
        private int selectedSettingsIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityPreferencesManager"/> class.
        /// </summary>
        public UnityPreferencesManager()
        {
            this.callbacks = new List<IRun>();
        }

        /// <summary>
        /// Gets a singleton instance of the configuration tool.
        /// </summary>
        public static UnityPreferencesManager Instance
        {
            get
            {
                // if singleton has not yet been created create it
                return singleton ?? (singleton = new UnityPreferencesManager());
            }
        }

        /// <summary>
        /// Adds preferences section named "Codefarts" to the Preferences Window.
        /// </summary>                                                        
        [PreferenceItem("Codefarts")]
        public static void PreferencesGUI()
        {
            // Preferences GUI
            Instance.DrawGUI();
        }

        /// <summary>
        /// Registers a <see cref="Action{T}"/> callback.
        /// </summary>
        /// <typeparam name="T">The type of data that the <see cref="callback"/> takes as a parameter.</typeparam>
        /// <param name="title">The title for the settings.</param>
        /// <param name="callback">A reference to a callback.</param>
        public void Register<T>(string title, Action<T> callback)
        {
            this.Register(title, callback, default(T));
        }

        /// <summary>
        /// Registers a <see cref="Action{T}"/> callback.
        /// </summary>
        /// <typeparam name="T">
        /// The type of data that the <see cref="callback"/> takes as a parameter.
        /// </typeparam>
        /// <param name="title">The title for the settings.</param>
        /// <param name="callback">
        /// A reference to a callback.
        /// </param>
        /// <param name="data">
        /// The data that will be passed as a parameter when the <see cref="callback"/> is invoked.
        /// </param>
        public void Register<T>(string title, Action<T> callback, T data)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            var modal = new SettingsMenuModel<T> { Title = title, Callback = callback, Data = data };
            this.callbacks.Add(modal);
        }

        /// <summary>
        /// Registers a <see cref="Action"/> callback.
        /// </summary>
        /// <param name="title">The title for the settings.</param>
        /// <param name="callback">A reference to a callback.</param>
        public void Register(string title, Action callback)
        {
            this.Register<object>(title, x => callback(), null);
        }

        /// <summary>
        /// The the draw GUI.
        /// </summary>
        internal void DrawGUI()
        {
            // get reference to localization manager
            var local = LocalizationManager.Instance;
            GUILayout.Label(local.Get("SettingsFile"));
            GUILayout.BeginHorizontal();
            var settingsFileName = EditorPrefs.GetString(SettingsFileKey, string.Empty);
            EditorGUILayout.TextField(settingsFileName);

            if (GUILayout.Button("...", GUILayout.MaxWidth(32)))
            {
                this.ShowSelectSaveFileDialog();
            }
            
            GUILayout.EndHorizontal();

            // check if settings file is null or empty or the file does not exist
            if (string.IsNullOrEmpty(settingsFileName) || !File.Exists(settingsFileName))
            {
                GUILayout.Label(local.Get("MissingSettingsFile"), "ErrorLabel");
                return;
            }

            GUILayout.Label(local.Get("Settings"));

            this.selectedSettingsIndex = EditorGUILayout.Popup(this.selectedSettingsIndex, this.GetTitles().ToArray());

            // draw current control
            this.scroll = GUILayout.BeginScrollView(this.scroll, false, false);
            this.callbacks[this.selectedSettingsIndex].Run();
            GUILayout.EndScrollView();

            GUILayout.FlexibleSpace();
        }

        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> of strings containing the <see cref="ISettingTitle.Title"/> values.
        /// </summary>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> of strings containing the <see cref="ISettingTitle.Title"/> values.</returns>
        private IEnumerable<string> GetTitles()
        {
            return this.callbacks.Cast<ISettingTitle>().Select(x => x.Title);
        }

        /// <summary>
        /// Handles the click event for the "..." button.
        /// </summary>
        private void ShowSelectSaveFileDialog()
        {
            // attempt to get the setting file location from EditorPrefs
            var previousFullPath = EditorPrefs.GetString(SettingsFileKey, "CodefartsSettings.xml").Replace("/", "\\");

            // get reference to localization manager
            var local = LocalizationManager.Instance;

            // open a save file dialog and get a user selected file
            var path = EditorUtility.SaveFilePanel(local.Get("SaveSettingsXml"), string.Empty, previousFullPath, "xml");

            // check if a path was selected
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            // check if the path changed
            if (!string.IsNullOrEmpty(previousFullPath) && string.Compare(previousFullPath, path, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                // some settings file already exist so just copy file to preserve existing settings
                try
                {
                    if (File.Exists(previousFullPath))
                    {
                        // delete destination file first is ti already exists
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }

                        File.Copy(previousFullPath, path);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    return;
                }
            }

            // update setting entry in EditorPrefs
            EditorPrefs.SetString(SettingsFileKey, path);

            // if previous path is missing create the file
            try
            {
                // create if file does not already exist
                if (!File.Exists(path))
                {
                    // create a empty settings file if it does not exist
                    File.WriteAllText(path, "<?xml version=\"1.0\"?>\r\n<settings>\r\n</settings>");
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            // setup settings
            var settings = Settings.SettingsManager.Instance;
            settings.Values = new XmlDocumentLinqValues(path);
        }
    }
}