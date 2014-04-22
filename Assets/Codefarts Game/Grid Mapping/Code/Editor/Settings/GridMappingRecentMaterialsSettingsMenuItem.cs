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
    /// The grid mapping recent materials settings menu item.
    /// </summary>
    public class GridMappingRecentMaterialsSettingsMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridMappingRecentMaterialsSettingsMenuItem"/> class.
        /// </summary>
        public static void Draw()
        {
            var local = LocalizationManager.Instance;

            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.RecentInspectorMaterialListEnabledKey, local.Get("SETT_EnableRecentMaterialsInInspector"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowRecentMaterialRemoveButtonsKey, local.Get("SETT_ShowRemoveButtons"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowRecentMaterialsAsButtonsKey, local.Get("SETT_ShowAsButtons"), false, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowAddMaterialButtonsKey, local.Get("SETT_ShowAddMaterialButton"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowRecentMaterialsSelectButtonsKey, local.Get("SETT_ShowSelectButton"), true, Helpers.RedrawInspector);

            SettingHelpers.DrawSettingsIntField(GlobalConstants.MaxNumberOfRecentMaterialsKey, local.Get("SETT_MaxNumberOfItems"), 100, 1, int.MaxValue, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.MaxHeightOfRecentMaterialsKey, local.Get("SETT_MaxControlHeight"), 64, 1, int.MaxValue, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.RecentMaterialButtonSizeKey, local.Get("SETT_ButtonSize"), 32, 1, int.MaxValue, Helpers.RedrawInspector);

            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowRecentMaterialAsListKey, local.Get("SETT_ShowAsList"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.RecentMaterialButtonColumnsKey, local.Get("SETT_ButtonsPerRow"), 5, 1, int.MaxValue, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowRecentMaterialAssetPreviewsKey, local.Get("SETT_ShowAssetPreview"), true, Helpers.RedrawInspector);
        }
    }
}