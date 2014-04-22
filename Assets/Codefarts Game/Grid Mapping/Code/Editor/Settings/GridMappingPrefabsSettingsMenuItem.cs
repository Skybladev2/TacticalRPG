/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

using UnityEngine;

namespace Codefarts.GridMapping.Editor.Settings
{
    using Codefarts.CoreProjectCode;
    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.GridMapping.Editor;
    using Codefarts.Localization;
    using System.IO;

    /// <summary>
    /// Provides a menu for general grid map settings.
    /// </summary>
    public class GridMappingPrefabsSettingsMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridMappingPrefabsSettingsMenuItem"/> class.
        /// </summary>
        public static void Draw()
        {
            var local = LocalizationManager.Instance;

            // SettingHelpers.DrawSettingsIntField(GlobalConstants.RecentQuickPrefabButtonColumnsKey, local.Get("SETT_ButtonsPerRow"), 3, 1, int.MaxValue, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.PrefabButtonsPerRowKey, local.Get("SETT_PrefabButtonsPerRow"), 3, 1, 100, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsFloatField(GlobalConstants.PrefabButtonSizeKey, local.Get("SETT_PrefabButtonSize"), 64, 1, 100, Helpers.RedrawInspector);

            // get the default value for the resource folder(s)
            var settings = SettingsManager.Instance;
            var defaultValue = settings.GetSetting(CoreGlobalConstants.ResourceFolderKey, string.Empty);
            if (defaultValue == string.Empty)
            {
                defaultValue = "Codefarts.Unity/GridMappingPrefabIndexFiles";
            }

            GUILayout.Label(local.Get("SETT_PrefabIndexResourceFolder"));
            SettingHelpers.DrawSettingsTextBox(GlobalConstants.PrefabIndexResourceFolderKey, defaultValue, Helpers.RedrawInspector);
        }
    }
}