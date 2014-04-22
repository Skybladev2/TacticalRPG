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
    using Codefarts.Localization;

    /// <summary>
    /// The grid mapping recent materials settings menu item.
    /// </summary>
    public class GridMappingLayersSettingsMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridMappingLayersSettingsMenuItem"/> class.
        /// </summary>
        public static void Draw()
        {
            var local = LocalizationManager.Instance;

            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowLayersInInspectorKey, local.Get("SETT_ShowLayersInInspector"), true, Helpers.RedrawInspector);
            var items = new[] { local.Get("SETT_OldLayerCountStyle"), local.Get("SETT_Version2LayerCountStyle"), local.Get("SETT_Version2SimpleStyle") };
            SettingHelpers.DrawSettingsPopup(GlobalConstants.LayerCountStyleKey, local.Get("SETT_LayerCountStyle"), items, 1, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.LayerCountFieldDisabledKey, local.Get("SETT_DisablelayerCountField"), false, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ScrollLayersKey, local.Get("SETT_ScrollLayers"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.LayerListHeightKey, local.Get("SETT_LayerListHeight"), 96, 16, 4096, Helpers.RedrawInspector);
            items = new[] { local.Get("SETT_OldLayerStyle"), local.Get("SETT_Version2LayerStyle") };
            SettingHelpers.DrawSettingsPopup(GlobalConstants.LayerListStyleKey, local.Get("SETT_LayerListStyle"), items, 1, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowLayerLocksKey, local.Get("SETT_ShowLayerLock"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowLayerVisibilityKey, local.Get("SETT_ShowLayerVisibility"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowActiveLayerKey, local.Get("SETT_ShowActiveLayer"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowDeleteLayerButtonKey, local.Get("SETT_ShowDeleteLayerButtons"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowMoveLayerButtonsKey, local.Get("SETT_ShowMoveLayerButtons"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.LayerIconSizeKey, local.Get("SETT_LayerIconSize"), 16, 8, 256, Helpers.RedrawInspector);

            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.PromptToDeleteLayerKey, local.Get("SETT_PromptToDeleteLayer"), true);
        }
    }
}