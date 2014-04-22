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
    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.GridMapping.Editor;
    using Codefarts.Localization;

    /// <summary>
    /// Provides a menu for general grid mapping settings.
    /// </summary>
    public class GridMappingGeneralSettingsMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridMappingGeneralSettingsMenuItem"/> class.
        /// </summary>
        public static void Draw()
        {
            var local = LocalizationManager.Instance;

            var items = new[] { local.Get("SETT_ShowMapInfoOldStyle"), local.Get("SETT_ShowMapInfoVer2Style") };
            var drawingHelperItems = new[] { local.Get("SETT_AsCheckBox"), local.Get("SETT_AsButtons") };
            SettingHelpers.DrawSettingsPopup(GlobalConstants.MapInfoStyleKey, local.Get("SETT_MapInfoStyle"), items, 1, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowMapInfoFoldoutKey, local.Get("SETT_ShowMapInfoAsFoldout"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowInspectorToolsKey, local.Get("SETT_ShowInspectorTools"), true, Helpers.RedrawInspector);

            SettingHelpers.DrawSettingsPopup(GlobalConstants.DrawingHelperStyleKey, local.Get("SETT_DrawingHelperStyle"), drawingHelperItems, 0, Helpers.RedrawInspector);

            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.AutoScalePrefabKey, local.Get("SETT_AutoScalePrefabs"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.AutoCenterPrefabKey, local.Get("SETT_AutoCenterPrefabs"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowGridLinesKey, local.Get("SETT_ShowGrid"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowGuidelinesKey, local.Get("SETT_ShowGuidelines"), false, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowMarkerKey, local.Get("SETT_ShowMarker"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsFloatField(GlobalConstants.MarkerScaleKey, local.Get("SETT_MarkerScale"), 1.025f, 0.01f, 10);

            SettingHelpers.DrawSettingsFloatField(GlobalConstants.DefaultCellWidthKey, local.Get("SETT_DefaultCellWidth"), 1, 0.01f, 100);
            SettingHelpers.DrawSettingsFloatField(GlobalConstants.DefaultCellHeightKey, local.Get("SETT_DefaultCellHeight"), 1, 0.01f, 100);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.DefaultColumnCountKey, local.Get("SETT_DefaultMapColumns"), 20, 1, 1000, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.DefaultRowCountKey, local.Get("SETT_DefaultMapRows"), 10, 1, 1000, Helpers.RedrawInspector);

            SettingHelpers.DrawSettingsFloatField(GlobalConstants.DefaultLayerDepthKey, local.Get("SETT_DefaultLayerDepth"), 1, 0, 1000, Helpers.RedrawInspector);

            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.ShowPrefabsInHierarchyKey, local.Get("SETT_ShowPrefabsInHierarchy"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsTextField(GlobalConstants.DefaultMapNameKey, local.Get("SETT_DefaultMapName"), local.Get("Map"), Helpers.RedrawInspector);

            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.HoldShiftToDrawKey, local.Get("SETT_HoldShiftToDraw"), false);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.HoldShiftToEraseKey, local.Get("SETT_HoldShiftToErase"), false);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.HoldAltToDrawKey, local.Get("SETT_HoldAltToDraw"), false);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.HoldAltToEraseKey, local.Get("SETT_HoldAltToErase"), false);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.HoldControlToDrawKey, local.Get("SETT_HoldControlToDraw"), false);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.HoldControlToEraseKey, local.Get("SETT_HoldControlToErase"), false);

            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.MouseWheelChangesLayersKey, local.Get("SETT_MouseWheelChangesLayers"), false, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.HoldShiftToChangeLayersKey, local.Get("SETT_HoldShiftToChangeLayers"), true, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.HoldAltToChangeLayersKey, local.Get("SETT_HoldAltToChangeLayers"), false, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.HoldControlToChangeLayersKey, local.Get("SETT_HoldControlToChangeLayers"), false, Helpers.RedrawInspector);

            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.RestoreLastDrawingToolKey, local.Get("SETT_RestoreLastDrawingTool"), false, Helpers.RedrawInspector);

            // rotation display setting
            var settings = SettingsManager.Instance;
            var rotationItems = new[] { local.Get("Hide"), local.Get("SETT_AsButtons"), local.Get("SETT_AsRotationWindow") };
            SettingHelpers.DrawSettingsPopup(GlobalConstants.RotationStyleKey, local.Get("SETT_RotationStyle"), rotationItems, 1, () =>
                {
                    var value = settings.GetSetting(GlobalConstants.RotationStyleKey, 1);
                    if (value != 2)
                    {
                        UnityEditor.EditorWindow.GetWindow<RotationsWindow>().Close();
                    }
                    Helpers.RedrawInspector();
                });

            // rotation window orientation setting
            var rotationStyle = settings.GetSetting(GlobalConstants.RotationStyleKey, 1);
            if (rotationStyle == 2)
            {
                SettingHelpers.DrawSettingsCheckBox(GlobalConstants.RotationWindowOrientationKey, local.Get("SETT_RotationWindowOrientation"), true, null);
            }
        }
    }
}