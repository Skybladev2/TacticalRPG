/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Editor
{
    using System.Diagnostics;

    using Codefarts.CoreProjectCode;
    using Codefarts.CoreProjectCode.Services;
    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.GridMapping.Editor.Settings;
    using Codefarts.Localization;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Provides menu items that integrate into unity's menu.
    /// </summary>
    [InitializeOnLoad]
    public class GridMappingSetup
    {
        /// <summary>
        /// Initializes static members of the <see cref="GridMappingSetup"/> class.
        /// </summary>
        static GridMappingSetup()
        {
            EditorCallbackService.Instance.Register(() =>
            {
                var local = LocalizationManager.Instance;
                var config = UnityPreferencesManager.Instance;

                config.Register(local.Get("SETT_GridMappingGeneral"), GridMappingGeneralSettingsMenuItem.Draw);
                config.Register(local.Get("SETT_GridMappingColors"), GridMappingColorSettingsMenuItem.Draw);
                config.Register(local.Get("SETT_GridMappingPrefabs"), GridMappingPrefabsSettingsMenuItem.Draw);
                config.Register(local.Get("SETT_GridMappingRecentMaterials"), GridMappingRecentMaterialsSettingsMenuItem.Draw);
                config.Register(local.Get("SETT_GridMappingRecentPrefabs"), GridMappingRecentPrefabsSettingsMenuItem.Draw);
                config.Register(local.Get("SETT_GridMappingLayers"), GridMappingLayersSettingsMenuItem.Draw);
                config.Register(local.Get("SETT_GridMappingAutoMaterials"), GridMappingAutomaticMaterialCreationSettingsMenuItem.Draw);
                config.Register(local.Get("SETT_GridMappingDrawingRules"), GridMappingDrawingRulesSettingsMenuItem.Draw);
                config.Register(local.Get("SETT_GridMappingSceneView"), GridMappingSceneViewSettingsMenuItem.Draw);
                config.Register(local.Get("SETT_GridMappingDrawingTools"), GridMappingDrawingToolsSettingsMenuItem.Draw);  
            });
        }

        /// <summary>
        /// provides two menu items for unity's menu.                                              
        /// </summary>
        [MenuItem("GameObject/Create Other/Create New Grid Map", priority = 100)]
        public static void NewGridMapMap()
        {
            // get reference to settings
            var settings = SettingsManager.Instance;

            // get the default map name to use from settings
            var name = settings.GetSetting(GlobalConstants.DefaultMapNameKey, "Map");

            // create the object and add the grid map component
            var go = new GameObject(name);
            var grid = go.AddComponent<GridMap>();

            // get default map values
            grid.CellWidth = settings.GetSetting(GlobalConstants.DefaultCellWidthKey, 1);
            grid.CellHeight = settings.GetSetting(GlobalConstants.DefaultCellHeightKey, 1);
            grid.Columns = settings.GetSetting(GlobalConstants.DefaultColumnCountKey, 10);
            grid.Rows = settings.GetSetting(GlobalConstants.DefaultRowCountKey, 5);
            grid.Depth = settings.GetSetting(GlobalConstants.DefaultLayerDepthKey, 1);

            Selection.objects = new Object[] { go };
        }

        /// <summary>
        /// Provides a menu item for grid mapping online documentation.                                              
        /// </summary>
        [MenuItem("Window/Codefarts/Online Documentation/Grid Mapping")]
        public static void OnlineGridMappingDocumentation()
        {
            Process.Start("http://www.codefarts.com/page/GridMappingDocumentation");
        }
    }
}