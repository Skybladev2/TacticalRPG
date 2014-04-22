/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.CoreProjectCode.Editor.Settings
{
    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.Localization;
    using UnityEditor;

    /// <summary>
    /// Provides a menu for general grid mapping settings.
    /// </summary>
    public class GlobalSettingsMenuItem
    {
        /// <summary>
        /// Used to draw the global settings.
        /// </summary>
        public static void Draw()
        {
            var local = LocalizationManager.Instance;
            var textValue = "Codefarts.Unity"; // set default value
            var settings = SettingsManager.Instance;

            if (settings.HasValue(CoreGlobalConstants.ResourceFolderKey))
            {
                textValue = settings.GetSetting(CoreGlobalConstants.ResourceFolderKey, "Codefarts.Unity");
            }

            var value = EditorGUILayout.TextField(local.Get("SETT_ResourceFolder"), textValue);
            if (value != textValue)
            {
                settings.SetValue(CoreGlobalConstants.ResourceFolderKey, value);
            }  
        }
    }
}