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
    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.GridMapping.Editor.Processors;

    /// <summary>
    /// Provides various keys as global constant values for use with the settings system.
    /// </summary>
    public class GlobalConstants
    {
        /// <summary>
        /// Provides a unique key that identifies the Map2D drawing tool list setting.
        /// </summary>
        public const string Map2DDrawingToolListKey = "Map2D.EditorDrawingToolListKey";

        /// <summary>
        /// Provides a unique key that identifies the Map2D layer tool list setting.
        /// </summary>
        public const string Map2DLayerToolListKey = "Map2D.LayerToolListKey";
        
        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether a checkerboard background will be shown behind the Map2D Editor map when editing. 
        /// </summary>
        public const string Map2DShowCheckerboardBackground = "Map2D.ShowCheckerboardBackground";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how the rotation window displays it's rotation grids. 
        /// </summary>
        public const string RotationWindowOrientationKey = "GridMapping.RotationWindowOrientation";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how the rotation controls are shown in the inspector. 
        /// </summary>
        public const string RotationStyleKey = "GridMapping.RotationStyle";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to weather or not the last drawing tool is restored when a map is selected or if it defaults to the first available one. 
        /// </summary>
        public const string RestoreLastDrawingToolKey = "GridMapping.RestoreLastDrawingTool";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the last used drawing tool id. 
        /// </summary>
        public const string LastDrawingToolUidKey = "GridMapping.LastDrawingToolUID";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the last used Map2D editor drawing tool id. 
        /// </summary>
        public const string Map2DLastEditorDrawingToolUidKey = "Map2D.LastEditorDrawingToolUID";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the last used Map2D layer tool id. 
        /// </summary>
        public const string Map2DLastLayerDrawingToolUidKey = "Map2D.LastLayerToolUID";
      
        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how items the "Shader type" list appear in the Automatic Material Creation window. 
        /// </summary>
        public const string AutoMaterialCreationShaderListStyleKey = "GridMapping.AutoMaterialCreationShaderListStyle";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how many buttons per row to show when drawing the prefab buttons. 
        /// </summary>
        public const string PrefabButtonsPerRowKey = "GridMapping.PrefabButtonsPerRow";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how many buttons per row to show when drawing the draw move buttons. 
        /// </summary>
        public const string DrawModeButtonsPerRowKey = "GridMapping.DrawModeButtonsPerRow";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how many buttons per row to show when drawing the map editor buttons. 
        /// </summary>
        public const string Map2DDrawModeButtonsPerRowKey = "Map2D.EditorDrawModeButtonsPerRow";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how many buttons per row to show when drawing the layer buttons. 
        /// </summary>
        public const string Map2DLayerButtonsPerRowKey = "Map2D.LayerButtonsPerRow";
      
        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to weather the user should be prompted before removing a rule, or loading/starting a new rule set if there are changes in the rule editor. 
        /// </summary>
        public const string DrawingRulesConfirmationKey = "GridMapping.DrawingRulesConfirmation";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to weather the rule checking information is output to the console. 
        /// </summary>
        public const string DrawingRulesOutputToConsoleKey = "GridMapping.DrawingRulesOutputToConsole";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to where the mouse co-ordinates are displayed. 
        /// </summary>
        public const string MouseCoordinatesKey = "GridMapping.MouseCoordinates";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how high the drawing helpers check boxes are displayed. 
        /// </summary>
        public const string DrawingHelperStyleKey = "GridMapping.DrawingHelperStyle";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how high each item in the drawing rules list is. 
        /// </summary>
        public const string DrawingRulesEntrySizeKey = "GridMapping.DrawingRulesEntrySize";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how big the show drawing rules buttons are. 
        /// </summary>
        public const string ShowDrawingRulesButtonSizeKey = "GridMapping.ShowDrawingRulesButtonSize";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how big the prefab buttons are. 
        /// </summary>
        public const string PrefabButtonSizeKey = "GridMapping.PrefabButtonSize";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how big the draw mode buttons are. 
        /// </summary>
        public const string DrawingModeButtonSizeKey = "GridMapping.DrawingModeButtonSize";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how big the map2D editor draw mode buttons are. 
        /// </summary>
        public const string Map2DDrawingModeButtonSizeKey = "Map2D.EditorDrawingModeButtonSize";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how big the map2D layer editor buttons are. 
        /// </summary>
        public const string Map2DLayerButtonSizeKey = "Map2D.LayerButtonSize";
        
        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the unique identifier to be added to the end of a texture asset filename so the <see cref="MakeReadableTextureProcessor"/> processor can identify it. 
        /// </summary>
        public const string MakeReadableTextureKey = "GridMapping.MakeReadableTexture";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the available shaders that will be listed. 
        /// </summary>
        public const string AutoMaterialCreationShadersKey = "GridMapping.AutoMaterialCreation.Shaders";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the default state of the free form
        /// check box in the auto material creation window. 
        /// </summary>
        public const string AutoMaterialCreationFreeformKey = "GridMapping.AutoMaterialCreation.Freeform";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the default state of the tile width field in the auto material creation window. 
        /// </summary>
        public const string AutoMaterialCreationDefaultWidthKey = "GridMapping.AutoMaterialCreation.DefaultWidth";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the default state of the tile height field in the auto material creation window. 
        /// </summary>
        public const string AutoMaterialCreationDefaultHeightKey = "GridMapping.AutoMaterialCreation.DefaultHeight";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the default color displayed in the auto material creation window. 
        /// </summary>
        public const string DefaultAutoMaterialCreationColorKey = "GridMapping.AutoMaterialCreation.DefaultColor";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to a resources folder where GridMapping will look for it's prefab index file resources.
        /// </summary>
        public const string PrefabIndexResourceFolderKey = "GridMapping.PrefabIndexResourceFolder";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to where material assets created by the automatic material creation window will be saved to.
        /// </summary>
        public const string AutoMaterialCreationFolderKey = "GridMapping.AutoMaterialCreationFolder";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating whether the marker gizmo will be shown.
        /// </summary>
        public const string ShowMarkerKey = "GridMapping.ShowMarker";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to scale value that is applied to the marker gizmo.
        /// </summary>
        public const string MarkerScaleKey = "GridMapping.MarkerScale";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the map information will be displayed within a foldout.
        /// </summary>
        public const string ShowMapInfoFoldoutKey = "GridMapping.ShowMapInfoFoldout";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the user will be prompted for confirmation before removing a layer.
        /// </summary>
        public const string PromptToDeleteLayerKey = "GridMapping.PromptToDeleteLayer";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether move buttons will be shown for each layer.
        /// </summary>
        public const string ShowMoveLayerButtonsKey = "GridMapping.ShowMoveLayerButtons";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether delete buttons will be show for each layer.
        /// </summary>
        public const string ShowDeleteLayerButtonKey = "GridMapping.ShowDeleteLayerButtons";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the layer count field is disabled.
        /// </summary>
        public const string LayerCountFieldDisabledKey = "GridMapping.LayerCountFieldDisabled";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how big the layer icons will be.
        /// </summary>
        public const string LayerListHeightKey = "GridMapping.LayerListHeight";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how big the layer icons will be.
        /// </summary>
        public const string LayerIconSizeKey = "GridMapping.LayerIconSize";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how the layer count will be shown.
        /// </summary>
        public const string LayerCountStyleKey = "GridMapping.LayerCountStyle";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how the layer list will be shown.
        /// </summary>
        public const string LayerListStyleKey = "GridMapping.LayerListStyle";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the list of layers will be visible in the inspector.
        /// </summary>
        public const string ShowLayersInInspectorKey = "GridMapping.ShowLayersInInspector";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the active layer indicator will be shown.
        /// </summary>
        public const string ShowActiveLayerKey = "GridMapping.ShowActiveLayer";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the layers list has visibility checkboxes.
        /// </summary>
        public const string ShowLayerVisibilityKey = "GridMapping.ShowLayerVisibility";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the layers list has checkboxes to lock the layer.
        /// </summary>
        public const string ShowLayerLocksKey = "GridMapping.ShowLayerLocks";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the layers list is scrollable.
        /// </summary>
        public const string ScrollLayersKey = "GridMapping.ScrollLayers";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the currently selected prefab index from the quick prefab selection grid.
        /// </summary>
        public const string CurrentPrefabIndexKey = "GridMapping.CurrentPrefabIndex";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how prefabs are named when they are drawn.
        /// </summary>
        /// <remarks>The string should be of a format usable by <see cref="string.Format(string,object)"/>.</remarks>
        public const string PrefabNameFormatKey = "GridMapping.PrefabNameFormat";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the control key must be held in combination with scrolling the mouse wheel in order to change the active layer.
        /// </summary>
        public const string HoldControlToChangeLayersKey = "GridMapping.HoldControlToChangeLayers";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the alt key must be held in combination with scrolling the mouse wheel in order to change the active layer.
        /// </summary>
        public const string HoldAltToChangeLayersKey = "GridMapping.HoldAltToChangeLayers";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the shift key must be held in combination with scrolling the mouse wheel in order to change the active layer.
        /// </summary>
        public const string HoldShiftToChangeLayersKey = "GridMapping.HoldShiftToChangeLayers";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether mouse wheel will change the active layer.
        /// </summary>
        public const string MouseWheelChangesLayersKey = "GridMapping.MouseWheelChangesLayers";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the control key must be held in order to erase.
        /// </summary>
        public const string HoldControlToEraseKey = "GridMapping.HoldControlToErase";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the alt key must be held in order to erase.
        /// </summary>
        public const string HoldAltToEraseKey = "GridMapping.HoldAltToErase";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the shift key must be held in order to erase.
        /// </summary>
        public const string HoldShiftToEraseKey = "GridMapping.HoldShiftToErase";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the control key must be held in order to draw.
        /// </summary>
        public const string HoldControlToDrawKey = "GridMapping.HoldControlToDraw";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the alt key must be held in order to draw.
        /// </summary>
        public const string HoldAltToDrawKey = "GridMapping.HoldAltToDraw";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the shift key must be held in order to draw.
        /// </summary>
        public const string HoldShiftToDrawKey = "GridMapping.HoldShiftToDraw";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to what the initial name of the map will be when created.
        /// </summary>
        public const string DefaultMapNameKey = "GridMapping.DefaultMapName";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the currently selected prefab category index.
        /// </summary>
        public const string CurrentPrefabCategoryIndexKey = "GridMapping.CurrentPrefabCategoryIndex";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the add prefab button is visible.
        /// </summary>
        public const string ShowAddPrefabButtonsKey = "GridMapping.ShowAddPrefabButtons";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the add material button is visible.
        /// </summary>
        public const string ShowAddMaterialButtonsKey = "GridMapping.ShowAddMaterialButtons";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent material list will show asset previews.
        /// </summary>
        public const string ShowRecentPrefabAssetPreviewsKey = "GridMapping.ShowRecentPrefabAssetPreviews";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent material list will show asset previews.
        /// </summary>
        public const string ShowRecentMaterialAssetPreviewsKey = "GridMapping.ShowRecentMaterialAssetPreviews";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how many buttons per row when showing the recent prefab list as a selection grid.
        /// </summary>
        public const string RecentPrefabButtonColumnsKey = "GridMapping.RecentPrefabButtonColumns";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to how many buttons per row when showing the recent material list as a selection grid.
        /// </summary>
        public const string RecentMaterialButtonColumnsKey = "GridMapping.RecentMaterialButtonColumns";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent prefabs is shown as a list or selection grid.
        /// </summary>
        public const string ShowRecentPrefabAsListKey = "GridMapping.ShowRecentPrefabAsList";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent materials is shown as a list or selection grid.
        /// </summary>
        public const string ShowRecentMaterialAsListKey = "GridMapping.ShowRecentMaterialAsList";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent prefab button size.
        /// </summary>
        public const string RecentPrefabButtonSizeKey = "GridMapping.RecentPrefabButtonSize";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent material button size.
        /// </summary>
        public const string RecentMaterialButtonSizeKey = "GridMapping.RecentMaterialButtonSize";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent prefab list will show select buttons.
        /// </summary>
        public const string ShowRecentPrefabsSelectButtonsKey = "GridMapping.ShowRecentPrefabsSelectButtons";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent material list will show select buttons.
        /// </summary>
        public const string ShowRecentMaterialsSelectButtonsKey = "GridMapping.ShowRecentMaterialsSelectButtons";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent prefab list will be shown as buttons.
        /// </summary>
        public const string ShowRecentPrefabsAsButtonsKey = "GridMapping.ShowRecentPrefabsAsButtons";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent material list will be shown as buttons.
        /// </summary>
        public const string ShowRecentMaterialsAsButtonsKey = "GridMapping.ShowRecentMaterialsAsButtons";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent prefabs list has remove buttons.
        /// </summary>
        public const string ShowRecentPrefabRemoveButtonsKey = "GridMapping.ShowRecentPrefabRemoveButtons";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent materials list has remove buttons.
        /// </summary>
        public const string ShowRecentMaterialRemoveButtonsKey = "GridMapping.ShowRecentMaterialRemoveButtons";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent prefabs list is shown in the inspector.
        /// </summary>
        public const string RecentInspectorPrefabListEnabledKey = "GridMapping.RecentInspectorPrefabListEnabled";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the recent materials list is shown in the inspector.
        /// </summary>
        public const string RecentInspectorMaterialListEnabledKey = "GridMapping.RecentInspectorMaterialListEnabled";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the maximum control height for the recent prefabs list before scrolling is enabled.
        /// </summary>
        public const string MaxHeightOfRecentPrefabsKey = "GridMapping.MaxHeightOfRecentPrefabs";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the maximum control height for the recent materials list before scrolling is enabled.
        /// </summary>
        public const string MaxHeightOfRecentMaterialsKey = "GridMapping.MaxHeightOfRecentMaterials";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the number of items that can be stored in the recent materials list.
        /// </summary>
        public const string MaxNumberOfRecentMaterialsKey = "GridMapping.MaxNumberOfRecentMaterials";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the default control size for the quick tools menu system.
        /// </summary>
        public const string MaxNumberOfRecentPrefabsKey = "GridMapping.MaxNumberOfRecentPrefabs";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the default control size for the quick tools menu system.
        /// </summary>
        public const string DefaultQuickToolControlSizeKey = "QuickTools.DefaultControlsSize";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to whether the quick tools menu system is enabled.
        /// </summary>
        public const string EnableQuickToolsKey = "QuickTools.EnableQuickTools";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating the currently selected material.
        /// </summary>
        public const string CurrentMaterialKey = "GridMapping.CurrentMaterial";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating the currently selected prefab.
        /// </summary>
        public const string CurrentPrefabKey = "GridMapping.CurrentPrefab";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to weather the map info is displayed.
        /// </summary>
        public const string MapInfoStyleKey = "GridMapping.MapInformation";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to weather the tools will be shown in the inspector.
        /// </summary>
        public const string ShowInspectorToolsKey = "GridMapping.ShowToolsInInspector";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to weather the drawn prefabs will be visible in the hierarchy.
        /// </summary>
        public const string ShowPrefabsInHierarchyKey = "GridMapping.ShowPrefabsInHierarchy";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to weather prefabs are automatically scaled.
        /// </summary>
        public const string AutoScalePrefabKey = "GridMapping.AutoScalePrefabs";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to weather prefabs are automatically centered.
        /// </summary>
        public const string AutoCenterPrefabKey = "GridMapping.AutoCenterPrefabs";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to weather the grid is displayed.
        /// </summary>
        public const string ShowGridLinesKey = "GridMapping.ShowGrid";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating weather the guidelines are displayed.
        /// </summary>
        public const string ShowGuidelinesKey = "GridMapping.ShowGuidelines";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the current X rotation selection index.
        /// </summary>
        public const string PreviousXRotationKey = "GridMapping.XRotationIndex";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the current Y rotation selection index.
        /// </summary>
        public const string PreviousYRotationKey = "GridMapping.YRotationIndex";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the current Z rotation selection index.
        /// </summary>
        public const string PreviousZRotationKey = "GridMapping.ZRotationIndex";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to weather the current material selection 
        /// is applied to drawn prefabs.
        /// </summary>
        public const string ApplyMaterialKey = "GridMapping.ApplyMaterialOnDraw";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the active layer selection. 
        /// </summary>
        public const string ActiveLayerKey = "GridMapping.ActiveLayerIndex";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the border color. 
        /// </summary>
        public const string BorderColorKey = "GridMapping.BorderColor";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the grid color. 
        /// </summary>
        public const string GridColorKey = "GridMapping.GridColor";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the marker color. 
        /// </summary>
        public const string MarkerColorKey = "GridMapping.MarkerColor";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the guideline color. 
        /// </summary>
        public const string GuidelineColorKey = "GridMapping.GuidelineColor";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the default cell width. 
        /// </summary>
        public const string DefaultCellWidthKey = "GridMapping.DefaultCellHeight";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the default cell height. 
        /// </summary>
        public const string DefaultCellHeightKey = "GridMapping.DefaultCellHeight";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the default column count. 
        /// </summary>
        public const string DefaultColumnCountKey = "GridMapping.DefaultColumnCount";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the default row count. 
        /// </summary>
        public const string DefaultRowCountKey = "GridMapping.DefaultRowCount";

        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to the default layer depth. 
        /// </summary>
        public const string DefaultLayerDepthKey = "GridMapping.DefaultLayerDepthKey";

        /// <summary>
        /// Provides a unique key that identifies the grid map menu name.
        /// </summary>
        public const string ShowGridMapMenuKey = "Grid Map Menu - CC9D3B6A-CBF0-404D-AE44-F94E5A446E1C";

        /// <summary>
        /// Used as the default string value added to the end of texture assets files so the <see cref="MakeReadableTextureProcessor"/> processor can identify it.
        /// </summary>
        public const string ReadableTextureString = "_NewFileNameTempPewpzIRReadableTex";

        /// <summary>
        /// Provides a unique key that identifies the drawing tool list setting.
        /// </summary>
        public const string DrawingToolListKey = "GridMapping.DrawingToolList";
    }
}
