/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Editor.Settings
{
    using Codefarts.GridMapping.Editor;
    using Codefarts.Localization;

    /// <summary>
    /// The grid mapping recent prefabs settings menu item.
    /// </summary>
    public class GridMappingRecentPrefabsSettingsMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridMappingRecentPrefabsSettingsMenuItem"/> class.
        /// </summary>
        public static void Draw()
        {
            var local = LocalizationManager.Instance;

            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.RecentInspectorPrefabListEnabledKey, local.Get("SETT_EnableRecentPrefabsInInspector"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowRecentPrefabRemoveButtonsKey, local.Get("SETT_ShowRemoveButtons"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowRecentPrefabsAsButtonsKey, local.Get("SETT_ShowAsButtons"), false, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowAddPrefabButtonsKey, local.Get("SETT_ShowAddPrefabButton"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowRecentPrefabsSelectButtonsKey, local.Get("SETT_ShowSelectButton"), true, Helpers.RedrawInspector);

            SettingHelpers.DrawSettingsIntField(GlobalConstants.MaxNumberOfRecentPrefabsKey, local.Get("SETT_MaxNumberOfItems"), 100, 1, int.MaxValue, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.MaxHeightOfRecentPrefabsKey, local.Get("SETT_MaxControlHeight"), 64, 1, int.MaxValue, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.RecentPrefabButtonSizeKey, local.Get("SETT_ButtonSize"), 32, 1, int.MaxValue, Helpers.RedrawInspector);

            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowRecentPrefabAsListKey, local.Get("SETT_ShowAsList"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.RecentPrefabButtonColumnsKey, local.Get("SETT_ButtonsPerRow"), 5, 1, int.MaxValue, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowRecentPrefabAssetPreviewsKey, local.Get("SETT_ShowAssetPreview"), true, Helpers.RedrawInspector);
        }
    }
}