/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

// ReSharper disable EmptyGeneralCatchClause

namespace Codefarts.GridMapping.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Codefarts.CoreProjectCode;
    using Codefarts.CoreProjectCode.Services;
    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.GridMapping.Common;
    using Codefarts.GridMapping.Editor.DrawingTools;
    using Codefarts.GridMapping.Models;
    using Codefarts.GridMapping.Utilities;
    using Codefarts.Localization;

    using UnityEditor;

    using UnityEngine;

    using Color = Codefarts.GridMapping.Common.Color;

    using Vector2 = Codefarts.GridMapping.Common.Vector2;
    using Vector3 = Codefarts.GridMapping.Common.Vector3;

    /// <summary>
    /// Provides a editor for the <see cref="GridMap"/> component.
    /// </summary>
    [CustomEditor(typeof(GridMap))]
    public partial class GridMapEditor : Editor
    {
        /// <summary>
        /// Used to store the current active working layer. See <see cref="ActiveLayer"/>.
        /// </summary>
        private int activeLayer;

        /// <summary>
        /// Used to hold the checked state of the apply material check box.
        /// </summary>
        private bool applyMaterial = true;

        /// <summary>
        /// Determine weather a prefab is automatically centered to align within a grid cell when drawing.
        /// </summary>
        private bool autoCenterPrefab = true;

        /// <summary>
        /// Determine weather a prefab is automatically scaled to fit within a grid cell when drawing.
        /// </summary>
        private bool autoScalePrefab = true;

        /// <summary>
        /// used to hold button content.
        /// </summary>
        private GUIContent[] buttonContent;

        /// <summary>
        /// The current prefab category index.
        /// </summary>
        private int currentPrefabCategoryIndex;

        /// <summary>
        /// Stores the currently selected prefab type.
        /// </summary>
        private int currentPrefabIndex;

        /// <summary>
        /// Holds a reference to the current drawing tool.
        /// </summary>
        private IEditorTool<GridMapEditor> currentTool;

        /// <summary>
        /// Determines weather to draw the ma grid in the scene view.
        /// </summary>
        private bool drawGridLines = true;

        /// <summary>
        /// Determine weather to draw guide lines in the scene view.
        /// </summary>
        private bool drawGuideLines;

        /// <summary>
        /// Holds the current draw mode state.
        /// </summary>
        private Guid drawingToolIndex;

        /// <summary>
        /// Holds a value indicating whether or not the mouse marker is shown.
        /// </summary>
        private bool drawMarker;

        /// <summary>
        /// Contains a 3D array of game object references for quick access.
        /// </summary>
        private GridMapGameObjectTracker gameObjects;

        /// <summary>
        /// Used to record when the editor has been enabled.
        /// </summary>
        private bool hasBeenEnabled;

        /// <summary>
        /// Holds the position of the highlight marker
        /// </summary>
        private Vector3 highlightPosition;

        /// <summary>
        /// Stores the scroll values for the map layer list.
        /// </summary>
        private Vector2 layerScroll;

        /// <summary>
        /// Used to store layer icons.
        /// </summary>
        private GUIStyle[] layerStyles;

        /// <summary>
        /// Used to hold onto references to prefabs so they can be instantiated
        /// </summary>
        private Dictionary<string, List<PrefabDataModel>> loadedPrefabs;

        /// <summary>
        /// Used to determine if map information is displayed.
        /// </summary>
        private bool mapInfoShown = true;

        /// <summary>
        /// Stores the scroll values for the recently used material list.
        /// </summary>
        private Vector2 materialScroll;

        /// <summary>
        /// Holds the location of the mouse hit location.
        /// </summary>
        private Vector3 mouseHitPosition;

        /// <summary>
        /// Holds the current prefab categories
        /// </summary>
        private string[] prefabCategories;

        /// <summary>
        /// Stores the scroll values for the recently used prefabs list.
        /// </summary>
        private Vector2 prefabScroll;

        /// <summary>
        /// Used to store the images of the X rotation buttons.
        /// </summary>
        private GUIContent[] rotationContentForXAxis;

        /// <summary>
        /// Used to store the images of the Y rotation buttons.
        /// </summary>
        private GUIContent[] rotationContentForYAxis;

        /// <summary>
        /// Used to store the images of the Z rotation buttons.
        /// </summary>
        private GUIContent[] rotationContentForZAxis;

        /// <summary>
        /// Holds a reference to a rotations window.
        /// </summary>
        private RotationsWindow rotationWindow;

        /// <summary>
        /// Stores the current selection index for the X rotation.
        /// </summary>
        private int rotationX;

        /// <summary>
        /// Stores the current selection index for the Y rotation.
        /// </summary>
        private int rotationY;

        /// <summary>
        /// Stores the current selection index for the Z rotation.
        /// </summary>
        private int rotationZ;

        /// <summary>
        /// Used to determine if rotation buttons are being shown.
        /// </summary>
        private string showingRotation;

        /// <summary>
        /// Gets or sets the active layer index.
        /// </summary>
        public int ActiveLayer
        {
            get
            {
                return this.activeLayer;
            }

            set
            {
                // get reference to the GridMap component
                var map = (GridMap)this.target;

                value = value < 0 ? 0 : value;
                value = value > map.Layers.Length - 1 ? map.Layers.Length - 1 : value;
                this.activeLayer = value;
                SettingsManager.Instance.SetValue(GlobalConstants.ActiveLayerKey, this.activeLayer);
            }
        }

        /// <summary>
        /// Gets or sets the active layer name.
        /// </summary>
        public string ActiveLayerName
        {
            get
            {
                // get reference to the GridMap component
                var map = (GridMap)this.target;
                return map.Layers[this.activeLayer].Name;
            }

            set
            {
                // get reference to the GridMap component
                var map = (GridMap)this.target;
                map.Layers[this.activeLayer].Name = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not to apply the material.
        /// </summary>
        public bool ApplyMaterial
        {
            get
            {
                return this.applyMaterial;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a prefab is automatically centered to align within a grid cell when drawing.
        /// </summary>
        public bool AutoCenterPrefab
        {
            get
            {
                return this.autoCenterPrefab;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a prefab is automatically scaled to fit within a grid cell when drawing.
        /// </summary>
        public bool AutoScalePrefab
        {
            get
            {
                return this.autoScalePrefab;
            }
        }

        /// <summary>
        /// Gets the currently selected prefab category name.
        /// </summary>
        /// <remarks>Will return null if no categories are available.</remarks>
        public string CurrentPrefabCategory
        {
            get
            {
                // check if prefab category array is available
                if (this.prefabCategories == null)
                {
                    return null;
                }

                // check if index is out of bounds and fix it
                this.currentPrefabCategoryIndex = this.currentPrefabCategoryIndex > this.prefabCategories.Length - 1 ? this.prefabCategories.Length - 1 : this.currentPrefabCategoryIndex;

                return this.currentPrefabCategoryIndex < 0 ? null : this.prefabCategories[this.currentPrefabCategoryIndex];
            }
        }

        /// <summary>
        /// Gets the current list of prefab types based on the currently selected prefab category;
        /// </summary>
        /// <remarks>Will return null if no categories are available.</remarks>
        public IList<PrefabDataModel> CurrentPrefabList
        {
            get
            {
                // get category
                var cat = this.CurrentPrefabCategory;

                // if null return null
                if (cat == null)
                {
                    return null;
                }

                if (this.loadedPrefabs != null)
                {
                    return this.loadedPrefabs.ContainsKey(cat) ? this.loadedPrefabs[cat] : null;
                }

                return new List<PrefabDataModel>();
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the mouse marker is shown.
        /// </summary>
        public bool DrawMarker
        {
            get
            {
                return this.drawMarker;
            }
        }

        /// <summary>
        /// Gets a 3D array of game object references for quick access.
        /// </summary>
        public GridMapGameObjectTracker GameObjects
        {
            get
            {
                return this.gameObjects;
            }
        }

        /// <summary>
        /// Gets the position of the highlight marker
        /// </summary>
        public Vector3 HighlightPosition
        {
            get
            {
                return this.highlightPosition;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse marker has a start position.
        /// </summary>
        public bool MarkerHasStartPosition { get; set; }

        /// <summary>
        /// Gets or sets when drawing rectangles.
        /// </summary>
        public Vector3 MarkerStartPosition { get; set; }

        /// <summary>
        /// Gets the current selection index for the X rotation.
        /// </summary>
        public int SelectedXRotationIndex
        {
            get
            {
                return this.rotationX;
            }
        }

        /// <summary>
        /// Gets the current selection index for the Y rotation.
        /// </summary>
        public int SelectedYRotationIndex
        {
            get
            {
                return this.rotationY;
            }
        }

        /// <summary>
        /// Gets the current selection index for the Z rotation.
        /// </summary>
        public int SelectedZRotationIndex
        {
            get
            {
                return this.rotationZ;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the editor is selecting a custom prefab or a prefab defined in a prefab file.
        /// </summary>
        public bool SelectingCustomPrefab
        {
            get
            {
                return this.currentPrefabIndex == 0;
            }
        }

        /// <summary>
        /// Calculates the location in grid (Column/Row) of the vector position (X/Z).
        /// </summary>
        /// <param name="vector">
        /// The reference vector that is to be converted into grid location.
        /// </param>
        /// <returns>
        /// Returns a <see cref="Vector2"/> type representing the Column and Row where the mouse of positioned over.
        /// </returns>
        public Point GetGridPosition(Vector3 vector)
        {
#if PERFORMANCE
            var performance = PerformanceTesting<PerformanceID>.Instance;
            performance.Start(PerformanceID.GetGridPosition);
#endif

            // get reference to the grid map component
            var map = (GridMap)this.target;

            // calculate column and row location from mouse hit location
            var position = new Vector3(vector.X / map.CellWidth, map.transform.position.y, vector.Z / map.CellHeight);

            // round the numbers to the nearest whole number using 5 decimal place precision
            position = new Vector3((int)Math.Round(position.X, 5, MidpointRounding.ToEven), 0, (int)Math.Round(position.Z, 5, MidpointRounding.ToEven));

            // do a check to ensure that the row and column are with the bounds of the grid map         
            var col = (int)position.X;
            var row = (int)position.Z;
            row = row < 0 ? 0 : row;
            row = row > map.Rows - 1 ? map.Rows - 1 : row;
            col = col < 0 ? 0 : col;
            col = col > map.Columns - 1 ? map.Columns - 1 : col;

#if PERFORMANCE
            performance.Stop(PerformanceID.GetGridPosition);
#endif
            // return the column and row values
            return new Point(col, row);
        }

        /// <summary>
        /// Gets the grid position that the mouse is currently over.
        /// </summary>
        /// <returns>Returns a <see cref="Point"/> type representing the column and row that the mouse is over.</returns>
        public Point GetMouseGridPosition()
        {
            return this.GetGridPosition(this.mouseHitPosition);
        }

        /// <summary>
        /// Gets the rotation value in Euler angles based of a selection index.    
        /// </summary>
        /// <param name="index">
        /// The selection index of the rotation.
        /// </param>
        /// <returns>
        /// Returns the rotation value in Euler angles.
        /// </returns>
        public int GetSelectedRotationValue(int index)
        {
            // get tool tip text for the selection
            var toolTip = this.rotationContentForXAxis[index].tooltip;

            // if no tooltip available then the center button was clicked so just return the index.
            if (string.IsNullOrEmpty(toolTip))
            {
                return index;
            }

            // parse the tool tip as a integer and return the value
            return int.Parse(toolTip);
        }

        /// <summary>
        /// Returns true or false depending if the mouse is positioned over the active grid map layer.
        /// </summary>
        /// <returns>Will return true if the mouse is positioned over the active grid map layer.</returns>
        public bool IsMouseOverLayer()
        {
            // get reference to the grid map component
            var map = (GridMap)this.target;

            // return true or false depending if the mouse is positioned over the map
            return this.mouseHitPosition.X > 0 && this.mouseHitPosition.X < (map.Columns * map.CellWidth)
                   && this.mouseHitPosition.Z > 0 && this.mouseHitPosition.Z < (map.Rows * map.CellHeight);
        }

        /// <summary>
        /// When the <see cref="GameObject"/> is unselected this method will clean up resources.
        /// </summary>
        public void OnDisable()
        {
            if (this.currentTool != null)
            {
                this.currentTool.Shutdown();
                GridMappingService.Instance.OnDrawingToolSelected(this.currentTool, null, this.target as GridMap);
                this.currentTool = null;
            }

            GridMappingService.Instance.CurrentMaterialChanged -= this.GridMappingServiceCurrentMaterialChanged;

            // unload resource images from the rotationContentForXAxis field.             
            foreach (var content in this.rotationContentForXAxis)
            {
                try
                {
                    Resources.UnloadAsset(content.image);
                }
                catch
                {
                }
            }

            // unload resource images from the rotationContentForYAxis field.
            foreach (var content in this.rotationContentForYAxis)
            {
                try
                {
                    Resources.UnloadAsset(content.image);
                }
                catch
                {
                }
            }

            // unload resource images from the rotationContentForZAxis field.
            foreach (var content in this.rotationContentForZAxis)
            {
                try
                {
                    Resources.UnloadAsset(content.image);
                }
                catch
                {
                }
            }

            // clear the loaded prefabs list
            if (this.loadedPrefabs != null)
            {
                foreach (var prefab in this.loadedPrefabs)
                {
                    // remove reference to prefab
                    prefab.Value.Clear();
                }

                this.loadedPrefabs.Clear();
            }

            this.loadedPrefabs = null;
            this.hasBeenEnabled = false;
        }

        /// <summary>
        /// When the <see cref="GameObject"/> is selected set the current tool to the view tool.
        /// </summary>
        public void OnEnable()
        {
            EditorCallbackService.Instance.Register(() =>
                {
                    if (this.target == null)
                    {
                        return;
                    }

                    Tools.current = Tool.View;
                    Tools.viewTool = ViewTool.FPS;

                    // get reference to the GridMapping component
                    var map = (GridMap)this.target;
                    this.gameObjects = new GridMapGameObjectTracker(map, true, false);

                    // load prefabs specified in the resources folder
                    this.loadedPrefabs = new Dictionary<string, List<PrefabDataModel>>();
                    this.LoadPrefabs();

                    // load rotation texture resources
                    this.LoadContent();

                    // load settings
                    this.LoadSettings();

                    GridMappingService.Instance.CurrentMaterialChanged += this.GridMappingServiceCurrentMaterialChanged;
                    GridMappingService.Instance.DrawingToolChanged += this.UpdateDrawingToolList;

                    // register tools from settings
                    this.UpdateDrawingToolList(this, EventArgs.Empty);

                    var settings = SettingsManager.Instance;
                    var lastUid = settings.GetSetting(GlobalConstants.LastDrawingToolUidKey, string.Empty);
                    var restoreLast = settings.GetSetting(GlobalConstants.RestoreLastDrawingToolKey, false);
                    Guid guid;
                    try
                    {
                        guid = !string.IsNullOrEmpty(lastUid) ? new Guid(lastUid) : Pencil.GetUid();
                    }
                    catch
                    {
                        // something went wrong default to pencil and report the warning
                        guid = Pencil.GetUid();
                        var local = LocalizationManager.Instance;
                        Debug.LogWarning(local.Get("ERR_FailedToGetLastToolUID"));
                    }

                    // setup current tool if restoreLast is true
                    var manager = DrawingToolManager.Instance;
                    this.drawingToolIndex = restoreLast && manager.HasTool(guid) ? guid : Guid.Empty;

                    // if drawing tool index has a guid attempt to make it the current tool
                    if (this.drawingToolIndex != Guid.Empty)
                    {
                        this.currentTool = manager.GetTool(this.drawingToolIndex);
                        this.currentTool.Startup(this);
                    }

                    // we are now considered to be enabled
                    this.hasBeenEnabled = true;

                    // be sure to raise the DrawingToolSelected
                    GridMappingService.Instance.OnDrawingToolSelected(null, this.currentTool, this.target as GridMap);

                    // remember to refresh the inspector widow
                    Helpers.RedrawInspector();
                });
        }

        /// <summary>
        /// Called when drawing the inspector UI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // if not yet enabled just exit
            if (!this.hasBeenEnabled)
            {
                return;
            }

            // put some spacing between tool sections
            GUILayout.Space(4);

            // vertical orientation
            GUILayout.BeginVertical();

            // draw map info
            this.DrawMapInfo();

            // check to draw tools in the inspector
            var settings = SettingsManager.Instance;
            var drawTools = settings.GetSetting(GlobalConstants.ShowInspectorToolsKey, true);
            if (drawTools)
            {
                // put some spacing between tool sections
                GUILayout.Space(4);

                // draw map layer controls
                this.DrawMapLayerControls();

                // put some spacing between tool sections
                GUILayout.Space(4);

                // draw drawing helper controls
                this.DrawDrawingHelpers();

                // put some spacing between tool sections
                GUILayout.Space(4);

                // draw drawing helper controls
                this.DrawDrawingToolButtons();

                // put some spacing between tool sections
                GUILayout.Space(4);

                // draw any tool specific controls
                this.DrawAdditionalToolControls();

                // put some spacing between tool sections
                GUILayout.Space(4);

                // draw prefab type selection buttons
                this.DrawPrefabTypes();

                // put some spacing between tool sections
                GUILayout.Space(4);

                // draw rotation information
                this.DrawRotationControls();

                // put some spacing between tool sections
                GUILayout.Space(4);

                // draw the recently used prefabs list
                this.DrawRecentPrefabList();

                // draw the recently used materials list
                this.DrawRecentMaterialList();
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Lets the Editor handle an event in the scene view.
        /// </summary>
        public void OnSceneGUI()
        {
            if (!this.hasBeenEnabled)
            {
                return;
            }

            // draw scene view controls
            this.DrawInSceneControls();

            // if UpdateHitPosition return true we should update the scene views so that the marker will update in real time
            if (this.UpdateHitPosition())
            {
                Helpers.RepaintSceneView();
            }

            // Calculate the location of the marker based on the location of the mouse
            this.RecalculateHighlightPosition();

            // if a current tool is a available update it
            if (this.currentTool != null)
            {
                this.currentTool.Update();
            }

            // if the mouse is positioned over the layer allow drawing actions to occur
            if (this.IsMouseOverLayer())
            {
                // handle mouse wheel input
                this.HandleMouseWheelInput();
            }

            // draw scene gizmo's
            this.DrawGizmos();
        }

        /// <summary>
        /// Sets the current prefab selection to the user selected prefab option.
        /// </summary>
        public void SetPrefabSelectionToCustom()
        {
            this.currentPrefabIndex = 0;
        }

        /// <summary>
        /// Used to draw old style rotation controls.
        /// </summary>
        internal void DrawButtonGridRotationControls()
        {
            // get reference to settings and localization
            var local = LocalizationManager.Instance;

            GUILayout.BeginHorizontal();

            // x rotation button
            if (GUILayout.Button(string.Format(local.Get("XColonValue"), this.GetSelectedRotationValue(this.rotationX))))
            {
                this.showingRotation = this.showingRotation == "X" ? this.showingRotation = string.Empty : "X";
            }

            // y rotation button
            if (GUILayout.Button(string.Format(local.Get("YColonValue"), this.GetSelectedRotationValue(this.rotationY))))
            {
                this.showingRotation = this.showingRotation == "Y" ? this.showingRotation = string.Empty : "Y";
            }

            // z rotation button
            if (GUILayout.Button(string.Format(local.Get("ZColonValue"), this.GetSelectedRotationValue(this.rotationZ))))
            {
                this.showingRotation = this.showingRotation == "Z" ? this.showingRotation = string.Empty : "Z";
            }

            GUILayout.EndHorizontal();

            // determine if the X rotation button is the currently active button
            if (this.showingRotation == "X")
            {
                this.DrawRotationButtonGrid(0);
            }

            // determine if the Y rotation button is the currently active button
            if (this.showingRotation == "Y")
            {
                this.DrawRotationButtonGrid(1);
            }

            // determine if the Z rotation button is the currently active button
            if (this.showingRotation == "Z")
            {
                this.DrawRotationButtonGrid(2);
            }
        }

        /// <summary>
        /// Used to draw the rotation button grid.
        /// </summary>
        /// <param name="axisIndex">Values 0, 1, 2 correspond to X, Y, Z respectively.</param>
        /// <returns>Returns false if canceled.</returns>
        internal bool DrawRotationButtonGrid(int axisIndex)
        {
            var settings = SettingsManager.Instance;

            var rotationIndex = 0;
            GUIContent[] rotationContent = null;
            var keyValue = string.Empty;

            // store values based on the axisIndex
            switch (axisIndex)
            {
                case 0:
                    rotationIndex = this.rotationX;
                    rotationContent = this.rotationContentForXAxis;
                    keyValue = GlobalConstants.PreviousXRotationKey;
                    break;

                case 1:
                    rotationIndex = this.rotationY;
                    rotationContent = this.rotationContentForYAxis;
                    keyValue = GlobalConstants.PreviousYRotationKey;
                    break;

                case 2:
                    rotationIndex = this.rotationZ;
                    rotationContent = this.rotationContentForZAxis;
                    keyValue = GlobalConstants.PreviousZRotationKey;
                    break;
            }

            // draw selection grid for the predefined rotation values
            var value = GUILayout.SelectionGrid(rotationIndex, rotationContent, 3, GUILayout.MaxHeight(200), GUILayout.MaxWidth(200));

            // if the center button selected we just treat it like a cancel operation
            if (value == 4)
            {
                this.showingRotation = string.Empty;
                return false;
            }

            // determine if a different selection occurred and if so store the selected index in settings and stop showing the rotation control grid
            if (value != rotationIndex)
            {
                switch (axisIndex)
                {
                    case 0:
                        this.rotationX = value;
                        break;

                    case 1:
                        this.rotationY = value;
                        break;

                    case 2:
                        this.rotationZ = value;
                        break;
                }

                settings.SetValue(keyValue, value);
                this.showingRotation = string.Empty;
            }

            return true;
        }

        /// <summary>
        /// Used to invoke the <see cref="IEditorTool{T}.OnInspectorGUI"/> method for the current drawing tool.
        /// </summary>
        private void DrawAdditionalToolControls()
        {
            if (this.currentTool != null)
            {
                // wrap in try catch so any exceptions that may be thrown by third party tools will not 
                // corrupt the rest of the inspector controls
                try
                {
                    this.currentTool.OnInspectorGUI();
                }
                catch (Exception ex)
                {
                    // if an exception does occur then we still want to ensure we report it out to the console
                    Debug.LogException(ex);
                }
            }
        }

        /// <summary>
        /// Draws the drawing helper controls.
        /// </summary>
        private void DrawDrawingHelpers()
        {
            // get reference to settings and localization
            var local = LocalizationManager.Instance;
            var settings = SettingsManager.Instance;

            // get the drawing helper style
            var style = settings.GetSetting(GlobalConstants.DrawingHelperStyleKey, 0);
            switch (style)
            {
                case 0:
                    GUILayout.BeginHorizontal();

                    // draw controls for changing guideline visibility
                    var value = GUILayout.Toggle(this.drawGuideLines, local.Get("Guidelines"));
                    if (value != this.drawGuideLines)
                    {
                        this.drawGuideLines = value;
                        SceneView.RepaintAll();
                    }

                    // draw controls for changing grid visibility
                    value = GUILayout.Toggle(this.drawGridLines, local.Get("Grid"));
                    if (value != this.drawGridLines)
                    {
                        this.drawGridLines = value;
                        SceneView.RepaintAll();
                    }

                    // draw controls for changing marker visibility
                    value = GUILayout.Toggle(this.drawMarker, local.Get("Marker"));
                    if (value != this.drawMarker)
                    {
                        this.drawMarker = value;
                        SceneView.RepaintAll();
                    }

                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();

                    // draw controls to turn auto scaling on or off
                    this.autoScalePrefab = GUILayout.Toggle(this.autoScalePrefab, local.Get("AutoScale"));

                    // draw controls to turn auto centering on or off
                    this.autoCenterPrefab = GUILayout.Toggle(this.autoCenterPrefab, local.Get("AutoCenter"));

                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    break;

                case 1:
                    var values = new[] { this.drawGuideLines, this.drawGridLines, this.drawMarker, this.autoScalePrefab, this.autoCenterPrefab };
                    var text = new[] { local.Get("Guidelines"), local.Get("Grid"), local.Get("Marker"), local.Get("AutoScale"), local.Get("AutoCenter") };
                    var results = Controls.ControlGrid.DrawCheckBoxGrid(values, text, 3, GUI.skin.button);
                    this.drawGuideLines = results[0];
                    this.drawGridLines = results[1];
                    this.drawMarker = results[2];
                    this.autoScalePrefab = results[3];
                    this.autoCenterPrefab = results[4];

                    // if guidelines grid or marker changed repaint the scene views
                    if (results[0] != values[0] && results[1] != values[1] && results[2] != values[2])
                    {
                        foreach (SceneView view in SceneView.sceneViews)
                        {
                            view.Repaint();
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Draw the drawing mode buttons.
        /// </summary>
        private void DrawDrawingToolButtons()
        {
            var values = DrawingToolManager.Instance.Tools.ToArray();
            var settings = SettingsManager.Instance;
            var size = settings.GetSetting(GlobalConstants.DrawingModeButtonSizeKey, 64.0f);
            var buttonsPerRow = settings.GetSetting(GlobalConstants.DrawModeButtonsPerRowKey, 3);

            Controls.ControlGrid.DrawGenericGrid(
                (tools, index, style, options) =>
                {
                    var tool = tools[index];
                    var result = GUILayout.Toggle(this.drawingToolIndex == tool.Uid, tool.ButtonContent, style, options);

                    // detect a tool change
                    if (result && this.drawingToolIndex != tool.Uid)
                    {
                        // if there is a current tool shut it down
                        if (this.currentTool != null)
                        {
                            this.currentTool.Shutdown();
                        }

                        // store reference to the old tool
                        var oldTool = this.currentTool;

                        // save index to tool
                        this.drawingToolIndex = tool.Uid;

                        // set current tool
                        this.currentTool = tool;
                        this.currentTool.Startup(this);

                        // save tool UID as the last tool used
                        settings.SetValue(GlobalConstants.LastDrawingToolUidKey, this.currentTool.Uid.ToString());

                        // notify service so code hooks can receive notifications
                        GridMappingService.Instance.OnDrawingToolSelected(oldTool, tool, this.target as GridMap);
                    }

                    // return same tool reference
                    return tool;
                },
                values,
                buttonsPerRow,
                GUI.skin.button,
                GUILayout.Height(size),
                GUILayout.Width(size));
        }

        /// <summary>
        /// Used to draw the gizmos in scene view when the map is selected.
        /// </summary>
        private void DrawGizmos()
        {
            // get reference to the GridMap component
            var map = (GridMap)this.target;

            // store map width, height and position and active layer height
            var mapWidth = map.Columns * map.CellWidth;
            var mapHeight = map.Rows * map.CellHeight;
            var transform = map.transform;
            var activeLayerHeight = this.ActiveLayer * map.Depth;

            var settings = SettingsManager.Instance;
            Handles.matrix = transform.localToWorldMatrix;

            // check to see weather grid lines are to be drawn
            if (this.drawGridLines)
            {
                // draw layer border
                Handles.color = settings.GetColorSetting(GlobalConstants.BorderColorKey, Color.White);
                Handles.DrawLine(                             // top
                    new Vector3(0, activeLayerHeight, 0),
                    new Vector3(mapWidth, activeLayerHeight, 0));
                Handles.DrawLine(                             // left
                     new Vector3(0, activeLayerHeight, 0),
                     new Vector3(0, activeLayerHeight, mapHeight));
                Handles.DrawLine(                             // right
                      new Vector3(mapWidth, activeLayerHeight, 0),
                      new Vector3(mapWidth, activeLayerHeight, mapHeight));
                Handles.DrawLine(                             // bottom
                      new Vector3(0, activeLayerHeight, mapHeight),
                      new Vector3(mapWidth, activeLayerHeight, mapHeight));

                // draw grid cells
                Handles.color = settings.GetColorSetting(GlobalConstants.GridColorKey, Color.Gray);
                for (float i = 1; i < map.Columns; i++)
                {
                    Handles.DrawLine(    // vertical
                         new Vector3(i * map.CellWidth, activeLayerHeight, 0),
                         new Vector3(i * map.CellWidth, activeLayerHeight, mapHeight));
                }

                for (float i = 1; i < map.Rows; i++)
                {
                    Handles.DrawLine(     // horizontal
                          new Vector3(0, activeLayerHeight, i * map.CellHeight),
                          new Vector3(mapWidth, activeLayerHeight, i * map.CellHeight));
                }
            }

            // Draw marker position
            Handles.color = settings.GetColorSetting(GlobalConstants.MarkerColorKey, Color.Red);
            if (this.drawMarker)
            {
                var markerScale = settings.GetSetting(GlobalConstants.MarkerScaleKey, 1.025f);
                switch (this.MarkerHasStartPosition)
                {
                    case false:
                        ExtensionMethodsForUnity.DrawWireCube(this.highlightPosition, new Vector3(map.CellWidth, map.Depth, map.CellHeight) * markerScale);
                        break;

                    case true:
                        // get settings relating to drawing
                        var holdAltToDraw = settings.GetSetting(GlobalConstants.HoldAltToDrawKey, false);

                        // check settings and see of we can draw. A value equal to 0 means drawing can occur
                        var canDraw = 0;
                        var current = Event.current;
                        canDraw += holdAltToDraw && !current.alt ? 1 : 0;
                        canDraw += !holdAltToDraw && current.alt ? 1 : 0; // maintains v1 behavior
                        if (canDraw == 0)
                        {
                            var bounds = new Bounds();
                            bounds.min =
                                new Vector3(
                                    Math.Min(this.MarkerStartPosition.X, this.highlightPosition.X) - ((map.CellWidth * markerScale) / 2),
                                    -((map.Depth * markerScale) / 2),
                                    Math.Min(this.MarkerStartPosition.Z, this.highlightPosition.Z) - ((map.CellHeight * markerScale) / 2));
                            bounds.max =
                                new Vector3(
                                    Math.Max(this.MarkerStartPosition.X, this.highlightPosition.X) + ((map.CellWidth * markerScale) / 2),
                                    (map.Depth * markerScale) / 2,
                                    Math.Max(this.MarkerStartPosition.Z, this.highlightPosition.Z) + ((map.CellHeight * markerScale) / 2));

                            if (bounds.size.x < map.CellWidth && bounds.size.z < map.CellHeight)
                            {
                                ExtensionMethodsForUnity.DrawWireCube(this.highlightPosition, new Vector3(map.CellWidth, map.Depth, map.CellHeight) * markerScale);
                            }
                            else
                            {
                                ExtensionMethodsForUnity.DrawWireCube(bounds.center + new Vector3(0, this.highlightPosition.Y, 0), bounds.size);
                            }
                        }

                        break;
                }
            }

            // if guide lines are not to be drawn we can just exit here
            if (!this.drawGuideLines)
            {
                return;
            }

            // draw guidelines
            Handles.color = settings.GetColorSetting(GlobalConstants.GuidelineColorKey, Color.Yellow);

            // draw layer guide line
            Handles.DrawLine(
                new Vector3(this.highlightPosition.X, -(map.Depth / 2f), this.highlightPosition.Z),
                new Vector3(this.highlightPosition.X, (map.Layers.Length * map.Depth) - (map.Depth / 2f), this.highlightPosition.Z));

            // draw column guide line
            Handles.DrawLine(
                new Vector3(0, this.highlightPosition.Y, this.highlightPosition.Z),
                new Vector3(map.CellWidth * map.Columns, this.highlightPosition.Y, this.highlightPosition.Z));

            // draw row guide line
            Handles.DrawLine(
                new Vector3(this.highlightPosition.X, this.highlightPosition.Y, 0),
                new Vector3(this.highlightPosition.X, this.highlightPosition.Y, map.CellHeight * map.Rows));
        }

        /// <summary>
        /// Used to draw the layer list.
        /// </summary>
        /// <param name="scrollable">If true will display scroll bars for the list.</param>
        private void DrawLayerList(bool scrollable)
        {
            // get reference to the GridMap component
            var map = (GridMap)this.target;

            var settings = SettingsManager.Instance;
            var showLocks = settings.GetSetting(GlobalConstants.ShowLayerLocksKey, true);
            var showVisibility = settings.GetSetting(GlobalConstants.ShowLayerVisibilityKey, true);
            var showActiveLayer = settings.GetSetting(GlobalConstants.ShowActiveLayerKey, true);
            var layerStyle = settings.GetSetting(GlobalConstants.LayerListStyleKey, 1);
            var layerIconSize = settings.GetSetting(GlobalConstants.LayerIconSizeKey, 16);
            var listHeight = settings.GetSetting(GlobalConstants.LayerListHeightKey, 96);
            var showDelete = settings.GetSetting(GlobalConstants.ShowDeleteLayerButtonKey, true);
            var showMove = settings.GetSetting(GlobalConstants.ShowMoveLayerButtonsKey, true);

            // draw the list if map layers are scrollable
            var scrollStarted = false;
            if (scrollable && map.Layers.Length > 3)
            {
                var height = Math.Min((map.Layers.Length * layerIconSize) + 2, listHeight);
                this.layerScroll = GUILayout.BeginScrollView(this.layerScroll, false, false, GUILayout.Height(height));
                scrollStarted = true;
            }

            switch (layerStyle)
            {
                case 0:
                    var index = GUILayout.SelectionGrid(this.ActiveLayer, map.GetLayerNames(), 1);

                    // if the selected layer has changed be sure to immediately update the scene views
                    if (index != this.ActiveLayer)
                    {
                        this.ActiveLayer = index;
                        EditorCallbackService.Instance.Register(SceneView.RepaintAll);
                    }

                    break;

                case 1:
                    var local = LocalizationManager.Instance;

                    GUILayout.BeginVertical();
                    for (var i = 0; i < map.Layers.Length; i++)
                    {
                        var layer = map.Layers[i];
                        GUILayout.BeginHorizontal(GUILayout.ExpandHeight(false));
                        GUILayout.Space(4);

                        if (showLocks)
                        {
                            var content = this.layerStyles[layer.Locked ? 2 : 3];
                            GUI.SetNextControlName(string.Format("chkLayerLock{0}", i));
                            layer.Locked = GUILayout.Toggle(layer.Locked, string.Empty, content, GUILayout.Width(layerIconSize), GUILayout.MaxWidth(layerIconSize), GUILayout.Height(layerIconSize), GUILayout.MaxHeight(layerIconSize));
                        }

                        if (showVisibility)
                        {
                            var content = this.layerStyles[layer.Visible ? 0 : 1];
                            GUI.SetNextControlName(string.Format("icoLayerVisible{0}", i));
                            var visible = GUILayout.Toggle(layer.Visible, string.Empty, content, GUILayout.Width(layerIconSize), GUILayout.MaxWidth(layerIconSize), GUILayout.Height(layerIconSize), GUILayout.MaxHeight(layerIconSize));
                            if (visible != layer.Visible)
                            {
                                //Undo.RegisterSceneUndo(local.Get("LayerVisibility"));
                                map.SetLayerVisibility(i, visible, objs => Undo.RecordObjects(objs, local.Get("LayerVisibility")));
                            }
                        }

                        if (showActiveLayer)
                        {
                            GUI.SetNextControlName(string.Format("icoActiveLayer{0}", i));
                            GUILayout.Label(GUIContent.none, i == this.activeLayer ? this.layerStyles[4] : GUI.skin.label, GUILayout.Width(layerIconSize), GUILayout.MaxWidth(layerIconSize), GUILayout.Height(layerIconSize), GUILayout.MaxHeight(layerIconSize));
                        }

                        if (i == this.activeLayer && showActiveLayer)
                        {
                            GUILayout.Space(GUI.skin.label.padding.left + GUI.skin.label.padding.right);
                        }

                        // check whether or not to show the move buttons
                        if (showMove)
                        {
                            Action<SwapModel> moveCallback = model =>
                                    {
                                        var localizedString = local.Get("MoveLayer");
                                        Undo.RecordObject(model.Map, localizedString);
                                        //Undo.RecordObject(model.Map.Layers[model.LayerIndex], localizedString);
                                        //Undo.RecordObject(model.Map.Layers[model.NewLayerIndex], localizedString);

                                        Undo.RegisterCompleteObjectUndo(model.LayerAObjects, localizedString);
                                        Undo.RegisterCompleteObjectUndo(model.LayerBObjects, localizedString);
                                    };

                            Action<SwapModel> moveCompletedCallback = model =>
                            {
                                EditorUtility.SetDirty(model.Map);
                                //EditorUtility.SetDirty(model.Map.Layers[model.LayerIndex]);
                                //EditorUtility.SetDirty(model.Map.Layers[model.NewLayerIndex]);
                                foreach (var gameObject in model.LayerAObjects)
                                {
                                    EditorUtility.SetDirty(gameObject);
                                }

                                foreach (var gameObject in model.LayerBObjects)
                                {
                                    EditorUtility.SetDirty(gameObject);
                                }
                            };

                            // show move up button
                            if (GUILayout.Button(this.buttonContent[3], GUI.skin.label, GUILayout.Width(layerIconSize), GUILayout.Height(layerIconSize), GUILayout.MaxHeight(layerIconSize)))
                            {
                                map.MoveLayerUp(i, 1, moveCallback, moveCompletedCallback);
                                this.Repaint();
                            }

                            // show move down button
                            if (GUILayout.Button(this.buttonContent[4], GUI.skin.label, GUILayout.Width(layerIconSize), GUILayout.Height(layerIconSize), GUILayout.MaxHeight(layerIconSize)))
                            {
                                map.MoveLayerDown(i, 1, moveCallback, moveCompletedCallback);
                                this.Repaint();
                            }
                        }

                        if (this.activeLayer == i)
                        {
                            GUI.SetNextControlName(string.Format("txtLayerName{0}", i));
                            layer.Name = EditorGUILayout.TextField(layer.Name, GUILayout.Height(layerIconSize), GUILayout.MaxHeight(layerIconSize));
                        }
                        else
                        {
                            GUI.SetNextControlName(string.Format("btnLayerName{0}", i));
                            if (GUILayout.Button(layer.Name, GUILayout.Height(layerIconSize), GUILayout.MaxHeight(layerIconSize)))
                            {
                                this.ActiveLayer = i;
                                GUI.FocusControl(string.Format("txtLayerName{0}", i));
                                EditorCallbackService.Instance.Register(SceneView.RepaintAll);
                            }
                        }

                        // check whether or not to show delete button
                        if (showDelete && !layer.Locked && map.Layers.Length > 1 && GUILayout.Button(this.buttonContent[2], GUI.skin.label, GUILayout.Width(layerIconSize), GUILayout.Height(layerIconSize), GUILayout.MaxHeight(layerIconSize)))
                        {
                            var promptToDeleteLayer = settings.GetSetting(GlobalConstants.PromptToDeleteLayerKey, true);

                            if (!promptToDeleteLayer || (promptToDeleteLayer && EditorUtility.DisplayDialog(
                                local.Get("AreYouSure"),
                                string.Format(local.Get("ConfirmDeleteLayer"), map.Layers[i].Name),
                                local.Get("Accept"),
                                local.Get("Cancel"))))
                            {
                                map.DeleteLayer(i, true);
                            }
                        }

                        GUILayout.EndHorizontal();
                        GUILayout.Space(1);
                    }

                    GUILayout.EndVertical();

                    break;
            }

            if (scrollStarted)
            {
                GUILayout.Space(3);
                GUILayout.EndScrollView();
            }
        }

        /// <summary>
        /// Draws map info controls.
        /// </summary>
        private void DrawMapInfo()
        {
            var local = LocalizationManager.Instance;
            var settings = SettingsManager.Instance;

            if (settings.GetSetting(GlobalConstants.ShowMapInfoFoldoutKey, true))
            {
                // draw map information foldout control (allows map information to be tucked away)
                this.mapInfoShown = EditorGUILayout.Foldout(this.mapInfoShown, local.Get("MapInformation"));

                // check if map information should be shown
                if (!this.mapInfoShown)
                {
                    return;
                }
            }

            // get reference to the GridMap component
            var map = (GridMap)this.target;

            GUILayout.BeginVertical();

            var needsRepaint = false;
            Vector2 value;
            int columnValue;
            int rowValue;
            var needsResize = false;
            switch (settings.GetSetting(GlobalConstants.MapInfoStyleKey, 1))
            {
                case 0:
                    // draws controls for map dimensions (Columns / Rows)
                    GUILayout.Label(local.Get("MapDimensions"));

                    columnValue = EditorGUILayout.IntField(local.Get("Columns"), map.Columns);
                    rowValue = EditorGUILayout.IntField(local.Get("Rows"), map.Rows);

                    // restrict the values
                    columnValue = columnValue < 1 ? 1 : columnValue;
                    rowValue = rowValue < 1 ? 1 : rowValue;

                    // check if needs to resize 
                    needsResize = columnValue != map.Columns || rowValue != map.Rows;

                    // will be used to determine if we need to redraw the scene views
                    needsRepaint = Math.Abs(columnValue - map.Columns) > Mathf.Epsilon || Math.Abs(rowValue - map.Rows) > Mathf.Epsilon;

                    // set columns & rows
                    map.Columns = columnValue;
                    map.Rows = rowValue;

                    // put some space between control sections
                    GUILayout.Space(4);

                    // draws controls for cell dimensions
                    GUILayout.Label(local.Get("CellSize"));
                    value.X = EditorGUILayout.FloatField(local.Get("Width"), map.CellWidth);
                    value.Y = EditorGUILayout.FloatField(local.Get("Height"), map.CellHeight);

                    // restrict value
                    value.X = value.X < 0.00001f ? 0.00001f : value.X;
                    value.Y = value.Y < 0.00001f ? 0.00001f : value.Y;

                    // determine if a repaint is in order
                    if (!needsRepaint)
                    {
                        needsRepaint = Math.Abs(value.X - map.CellWidth) > Mathf.Epsilon || Math.Abs(value.Y - map.CellHeight) > Mathf.Epsilon;
                    }

                    // set cell size
                    map.CellWidth = value.X;
                    map.CellHeight = value.Y;

                    // put some space between control sections
                    GUILayout.Space(4);

                    // draw the layer thickness control
                    value.X = map.Depth;
                    GUILayout.Label(local.Get("LayerThickness"));
                    map.Depth = EditorGUILayout.FloatField(local.Get("Depth"), map.Depth);

                    // determine if a repaint is in order
                    if (!needsRepaint)
                    {
                        needsRepaint = Math.Abs(value.X - map.Depth) > Mathf.Epsilon;
                    }

                    break;

                case 1:
                    GUILayout.BeginHorizontal();

                    GUILayout.BeginVertical();
                    GUILayout.Label(local.Get("MapSize"), GUILayout.Width(60));

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(local.Get("Columns"), GUILayout.Width(58), GUILayout.MinWidth(58));
                    columnValue = EditorGUILayout.IntField(map.Columns, GUILayout.MaxWidth(55));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(local.Get("Rows"), GUILayout.Width(58), GUILayout.MinWidth(58));
                    rowValue = EditorGUILayout.IntField(map.Rows, GUILayout.MaxWidth(55));
                    GUILayout.EndHorizontal();

                    // restrict the values
                    columnValue = columnValue < 1 ? 1 : columnValue;
                    rowValue = rowValue < 1 ? 1 : rowValue;

                    // check if needs to resize 
                    needsResize = columnValue != map.Columns || rowValue != map.Rows;

                    // will be used to determine if we need to redraw the scene views
                    needsRepaint = Math.Abs(columnValue - map.Columns) > Mathf.Epsilon || Math.Abs(rowValue - map.Rows) > Mathf.Epsilon;

                    // set columns & rows
                    map.Columns = columnValue;
                    map.Rows = rowValue;

                    GUILayout.EndVertical();

                    // put some space between control sections
                    GUILayout.Space(4);

                    GUILayout.BeginVertical();
                    GUILayout.Label(local.Get("CellSize"), GUILayout.Width(60));

                    // draws controls for cell dimensions
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(local.Get("Width"), GUILayout.MinWidth(40), GUILayout.Width(40));
                    value.X = EditorGUILayout.FloatField(map.CellWidth, GUILayout.MaxWidth(55));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(local.Get("Height"), GUILayout.MinWidth(40), GUILayout.Width(40));
                    value.Y = EditorGUILayout.FloatField(map.CellHeight, GUILayout.MaxWidth(55));
                    GUILayout.EndHorizontal();

                    // restrict value
                    value.X = value.X < 0.00001f ? 0.00001f : value.X;
                    value.Y = value.Y < 0.00001f ? 0.00001f : value.Y;

                    // determine if a repaint is in order
                    if (!needsRepaint)
                    {
                        needsRepaint = Math.Abs(value.X - map.CellWidth) > Mathf.Epsilon || Math.Abs(value.Y - map.CellHeight) > Mathf.Epsilon;
                    }

                    // set cell size
                    map.CellWidth = value.X;
                    map.CellHeight = value.Y;

                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();

                    // put some space between control sections
                    GUILayout.Space(4);

                    // draw the layer thickness control
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(local.Get("LayerThickness"), GUILayout.MaxWidth(100));
                    value.X = EditorGUILayout.FloatField(map.Depth, GUILayout.MaxWidth(55));
                    GUILayout.EndHorizontal();

                    // restrict & set map depth  
                    map.Depth = value.X < 0 ? 0 : value.X;

                    // determine if a repaint is in order
                    if (!needsRepaint)
                    {
                        needsRepaint = Math.Abs(value.X - map.Depth) > Mathf.Epsilon;
                    }

                    break;
            }

            GUILayout.EndVertical();

            // rebuild game objects array if needed
            if (needsResize)
            {
                this.gameObjects.Scan(false);
            }

            // if we need to repaint then do so before exiting
            if (needsRepaint)
            {
                SceneView.RepaintAll();
            }
        }

        /// <summary>
        /// Draws controls for the map layers.
        /// </summary>
        private void DrawMapLayerControls()
        {
            // get reference to the GridMap component
            var map = (GridMap)this.target;

            // get localization instance
            var local = LocalizationManager.Instance;

            var settings = SettingsManager.Instance;
            var layerCountStyle = settings.GetSetting(GlobalConstants.LayerCountStyleKey, 1);
            var layerCountFieldDisabled = settings.GetSetting(GlobalConstants.LayerCountFieldDisabledKey, false);
            GUILayout.BeginVertical();

            var layerCount = map.Layers.Length;
            switch (layerCountStyle)
            {
                case 0:
                    // provide field for setting layer count
                    if (layerCountFieldDisabled)
                    {
                        EditorGUILayout.LabelField(local.Get("LayerCount"), map.Layers.Length.ToString());
                    }
                    else
                    {
                        layerCount = EditorGUILayout.IntField(local.Get("LayerCount"), map.Layers.Length);
                    }

                    layerCount = layerCount < 1 ? 1 : layerCount;
                    if (layerCount != map.Layers.Length)
                    {
                        map.SetLayerCount(layerCount);
                    }

                    break;

                case 1:
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(local.Get("LayerCount"));

                    // show remove button
                    if (GUILayout.Button(this.buttonContent[1], GUI.skin.label, GUILayout.MaxHeight(16), GUILayout.MaxWidth(16)))
                    {
                        layerCount = layerCount - 1 < 1 ? 1 : layerCount - 1;
                        map.SetLayerCount(layerCount);
                    }

                    // provide field for setting layer count
                    if (layerCountFieldDisabled)
                    {
                        GUILayout.Label(map.Layers.Length.ToString());
                    }
                    else
                    {
                        layerCount = EditorGUILayout.IntField(map.Layers.Length);
                    }

                    // ensure layer count is greater then or equal to 1
                    layerCount = layerCount < 1 ? 1 : layerCount;

                    // if the layer count changes set new layer count
                    if (layerCount != map.Layers.Length)
                    {
                        map.SetLayerCount(layerCount);
                    }

                    // show add button
                    if (GUILayout.Button(this.buttonContent[0], GUI.skin.label, GUILayout.MaxHeight(16), GUILayout.MaxWidth(16)))
                    {
                        map.AddLayer(true);
                        this.gameObjects.Scan(false);
                    }
                    
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    break;

                case 2:
                    // show add layer button
                    if (GUILayout.Button(local.Get("AddLayer"), GUILayout.MaxWidth(75)))
                    {
                        map.AddLayer(true);
                        this.gameObjects.Scan(false);
                    }

                    break;
            }

            // ensure active layer does not exceed layer count
            this.activeLayer = this.activeLayer > map.Layers.Length - 1 ? map.Layers.Length - 1 : this.activeLayer;

            // put a bit of space layer count label and the layer list
            GUILayout.Space(4);

            // draw the actual layer list
            if (settings.GetSetting(GlobalConstants.ShowLayersInInspectorKey, true))
            {
                if (settings.GetSetting(GlobalConstants.LayerListStyleKey, 1) < 1)
                {
                    // draw current layer name control for changing the layer name
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(local.Get("Layers"));
                    var layerName = this.ActiveLayerName;
                    var value = EditorGUILayout.TextField(string.IsNullOrEmpty(layerName) ? local.Get("NoneBracket") : layerName);
                    this.ActiveLayerName = value == local.Get("NoneBracket") ? layerName : value;
                    GUILayout.EndHorizontal();
                }

                this.DrawLayerList(settings.GetSetting(GlobalConstants.ScrollLayersKey, true));
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws the prefab select buttons.
        /// </summary>
        private void DrawPrefabTypes()
        {
            GUILayout.BeginVertical();

            // get reference to settings and localization
            var settings = SettingsManager.Instance;
            var local = LocalizationManager.Instance;

            // draw the prefab category drop down
            this.currentPrefabCategoryIndex = this.currentPrefabCategoryIndex > this.prefabCategories.Length - 1 ? this.prefabCategories.Length - 1 : this.currentPrefabCategoryIndex;
            int index;

            // if no index selected report no prefab categories available
            if (this.currentPrefabCategoryIndex < 0)
            {
                GUILayout.Label(local.Get("ERR_NoPrefabCategoriesDefined"));
            }
            else
            {
                // draw prefab categories drop down
                index = EditorGUILayout.Popup(this.currentPrefabCategoryIndex, this.prefabCategories);
                if (index != this.currentPrefabCategoryIndex)
                {
                    this.currentPrefabCategoryIndex = index;
                    this.currentPrefabIndex = 0;

                    // save index in settings
                    settings.SetValue(GlobalConstants.CurrentPrefabCategoryIndexKey, index);
                    settings.SetValue(GlobalConstants.CurrentPrefabIndexKey, this.currentPrefabIndex);
                }
            }

            // put some spacing between controls
            GUILayout.Space(4);

            // build content array from list
            var list = this.CurrentPrefabList;

            // setup content array for prefab buttons
            var content = new GUIContent[list == null ? 1 : list.Count + 1];
            var texturesPath = Path.Combine(settings.GetSetting(CoreGlobalConstants.ResourceFolderKey, "Codefarts.Unity"), "Textures");
            texturesPath = texturesPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            var getPath = new Func<string, string>(x => Path.Combine(texturesPath, x).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

            content[0] = new GUIContent(string.Empty, Resources.Load(getPath("Gear"), typeof(Texture2D)) as Texture2D, local.Get("Prefab"));

            // setup prefabs from list if there are any
            if (list != null)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    content[i + 1] = new GUIContent(item.Texture, item.Name);
                }
            }

            // draws the prefab type buttons
            var buttonsPerRow = settings.GetSetting(GlobalConstants.PrefabButtonsPerRowKey, 3);

            // update current prefab type index
            this.currentPrefabIndex = this.currentPrefabIndex >= content.Length - 1 ? content.Length - 1 : this.currentPrefabIndex;

            var values = new bool[content.Length];
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = this.currentPrefabIndex == i;
            }

            var size = settings.GetSetting(GlobalConstants.PrefabButtonSizeKey, 64);
            index = this.currentPrefabIndex;

            Controls.ControlGrid.DrawGenericGrid(
                (v, i, s, o) =>
                {
                    var result = GUILayout.Toggle(v[i], content[i], s, o);
                    if (result != v[i])
                    {
                        index = i;
                    }

                    return result;
                },
                values,
                buttonsPerRow,
                GUI.skin.button,
                GUILayout.Height(size),
                GUILayout.Width(size));

            // set current prefab if changed
            if (index != this.currentPrefabIndex)
            {
                // update index and save index in settings
                this.currentPrefabIndex = index;
                settings.SetValue(GlobalConstants.CurrentPrefabIndexKey, index);

                switch (index)
                {
                    case 0:
                        // do nothing here as it will be handled elsewhere
                        break;

                    default:
                        var gridMappingService = GridMappingService.Instance;
                        if (list == null)
                        {
                            gridMappingService.CurrentPrefab = null;
                        }
                        else
                        {
                            var item = list[index - 1];
                            gridMappingService.CurrentPrefab = item.Prefab;
                            gridMappingService.CurrentPrefab.name = item.Name;
                        }

                        break;
                }
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws a list of recent materials used.
        /// </summary>
        private void DrawRecentMaterialList()
        {
            // don't draw if prefab is the selected type
            if (this.currentPrefabIndex == 0)
            {
                return;
            }

            GUILayout.BeginVertical();

            // get reference to settings instance
            var settings = SettingsManager.Instance;

            // let localization instance
            var local = LocalizationManager.Instance;

            // show apply material check box
            var boolValue = GUILayout.Toggle(this.applyMaterial, local.Get("ApplyMaterial"));

            // check if state changed
            if (boolValue != this.applyMaterial)
            {
                this.applyMaterial = boolValue;
                settings.SetValue(GlobalConstants.ApplyMaterialKey, this.applyMaterial);
            }

            // draw the current material selection
            GUILayout.Label(local.Get("CurrentMaterial"));

            // get grid mapping service
            var gridMappingService = GridMappingService.Instance;

            // draw current material control
            var value =
                (Material)
                EditorGUILayout.ObjectField(
                    gridMappingService.CurrentMaterial != null ? gridMappingService.CurrentMaterial.name : string.Empty,
                    gridMappingService.CurrentMaterial,
                      typeof(Material),
                    true);

            // if a different material was selected make it the current material and save the instance id in settings
            if (value != gridMappingService.CurrentMaterial)
            {
                gridMappingService.CurrentMaterial = value;
                if (value != null)
                {
                    // ensure there are no duplicates in the list
                    var i = 1;
                    while (i < gridMappingService.RecentlyUsedMaterials.Count)
                    {
                        if (gridMappingService.RecentlyUsedMaterials[i] != null && value.GetInstanceID() == gridMappingService.RecentlyUsedMaterials[i].GetInstanceID())
                        {
                            gridMappingService.RecentlyUsedMaterials.RemoveAt(i);
                        }
                        else
                        {
                            i++;
                        }
                    }

                    // inset selection at top of recently used list
                    gridMappingService.RecentlyUsedMaterials.Insert(0, value);

                    // store current selection in settings
                    settings.SetValue(GlobalConstants.CurrentMaterialKey, AssetDatabase.AssetPathToGUID(GridMapping.Helpers.GetSourcePrefab(value)));
                }
                else
                {
                    settings.RemoveValue(GlobalConstants.CurrentMaterialKey);
                }
            }

            // add some spacing
            GUILayout.Space(4);

            // get various settings related to the recent material list
            var maxItems = settings.GetSetting(GlobalConstants.MaxNumberOfRecentMaterialsKey, 64);
            var maxHeight = settings.GetSetting(GlobalConstants.MaxHeightOfRecentMaterialsKey, 64);
            var showRecentMaterials = settings.GetSetting(GlobalConstants.RecentInspectorMaterialListEnabledKey, true);
            var showAsList = settings.GetSetting(GlobalConstants.ShowRecentMaterialAsListKey, true);
            var buttonSize = settings.GetSetting(GlobalConstants.RecentMaterialButtonSizeKey, 32);
            var showPreview = settings.GetSetting(GlobalConstants.ShowRecentMaterialAssetPreviewsKey, true);
            var showAddMaterial = settings.GetSetting(GlobalConstants.ShowAddMaterialButtonsKey, true);

            GUILayout.EndVertical();

            // check if recent materials list should be shown
            if (!showRecentMaterials)
            {
                return;
            }

            // provides a button to add new materials to the list
            if (showAddMaterial && GUILayout.Button(local.Get("AddMaterial"), GUILayout.MaxWidth(100)))
            {
                gridMappingService.RecentlyUsedMaterials.Add(null);
            }

            GUILayout.Space(4);

            // limit max items in the list based on settings
            while (gridMappingService.RecentlyUsedMaterials.Count > 0 && gridMappingService.RecentlyUsedMaterials.Count > maxItems)
            {
                gridMappingService.RecentlyUsedMaterials.RemoveAt(gridMappingService.RecentlyUsedMaterials.Count - 1);
            }

            this.materialScroll = GUILayout.BeginScrollView(this.materialScroll, false, false, GUILayout.Height(maxHeight), GUILayout.MaxHeight(maxHeight));
            GUILayout.BeginVertical();
            if (showAsList)
            {
                this.DrawRecentMaterialsAsList(buttonSize, showPreview);
            }
            else
            {
                this.DrawRecentMaterialsAsSelectionGrid(buttonSize, showPreview);
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        /// <summary>
        /// Draws the recent materials list as list.
        /// </summary>
        /// <param name="buttonSize">
        /// The button size of each button in the grid.
        /// </param>
        /// <param name="showPreview">
        /// If true will use <see cref="AssetPreview.GetAssetPreview"/> as the texture.
        /// </param>
        private void DrawRecentMaterialsAsList(float buttonSize, bool showPreview)
        {
            // get references to services and settings
            var gridMappingService = GridMappingService.Instance;
            var settings = SettingsManager.Instance;
            var showRemoveButtons = settings.GetSetting(GlobalConstants.ShowRecentMaterialRemoveButtonsKey, true);
            var showAsButtons = settings.GetSetting(GlobalConstants.ShowRecentMaterialsAsButtonsKey, false);
            var showSelectButton = settings.GetSetting(GlobalConstants.ShowRecentMaterialsSelectButtonsKey, true);
            var local = LocalizationManager.Instance;

            // loop through each material in the recently used list
            var i = 0;
            while (i < gridMappingService.RecentlyUsedMaterials.Count)
            {
                // get material reference
                var material = gridMappingService.RecentlyUsedMaterials[i];

                GUILayout.BeginHorizontal();

                // check if remove buttons should be shown
                var wasRemoved = false;
                if (showRemoveButtons)
                {
                    // hold on to button click state for later use
                    wasRemoved = GUILayout.Button("X", GUILayout.MaxWidth(25));
                    if (wasRemoved)
                    {
                        gridMappingService.RecentlyUsedMaterials.RemoveAt(i);
                    }
                }

                // do index range check to make sure we are still in range after drawing button
                // NOTE: COULD USE BETTER CODE HERE
                if (i < gridMappingService.RecentlyUsedMaterials.Count)
                {
                    // if not showing a buttons then draw the material selection field
                    if (!showAsButtons)
                    {
                        var value = (Material)EditorGUILayout.ObjectField(material, typeof(Material), true);
                        if (value != material)
                        {
                            gridMappingService.RecentlyUsedMaterials.Insert(0, value);
                            gridMappingService.RecentlyUsedMaterials.RemoveAt(i);
                            gridMappingService.RecentlyUsedMaterials[i] = value;
                        }
                    }

                    // setup text for button
                    var buttonText = material != null ? material.name : local.Get("NoneBracket");
                    if (!showAsButtons && showSelectButton)
                    {
                        buttonText = local.Get("Select");
                    }

                    // setup button content
                    var content = new GUIContent(buttonText);
                    if (showPreview && showAsButtons && material != null)
                    {
                        content.image = AssetPreview.GetAssetPreview(material);
                    }

                    // draw the button 
                    if ((showSelectButton || showAsButtons) &&
                       GUILayout.Button(content, GUILayout.MaxWidth(showAsButtons ? int.MaxValue : 50), GUILayout.Height(buttonSize)))
                    {
                        gridMappingService.CurrentMaterial = material;
                        if (material != null)
                        {
                            settings.SetValue(GlobalConstants.CurrentMaterialKey, material.GetInstanceID());
                        }
                    }
                }

                GUILayout.EndHorizontal();

                // if button was not clicked increment the index
                if (!wasRemoved)
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// Draws the recent materials list as grid.
        /// </summary>
        /// <param name="buttonSize">
        /// The button size of each button in the grid.
        /// </param>
        /// <param name="showPreview">
        /// If true will use <see cref="AssetPreview.GetAssetPreview"/> as the texture.
        /// </param>
        private void DrawRecentMaterialsAsSelectionGrid(float buttonSize, bool showPreview)
        {
            // get references to services and settings
            var gridMappingService = GridMappingService.Instance;
            var settings = SettingsManager.Instance;

            // get number of material buttons to show per row
            var buttonsPerRow = settings.GetSetting(GlobalConstants.RecentMaterialButtonColumnsKey, 5);
            var count = 0;

            // setup button style
            var style = new GUIStyle(GUI.skin.button)
                            {
                                imagePosition = ImagePosition.ImageAbove,
                                alignment = TextAnchor.LowerCenter
                            };

            // begin laying out buttons
            GUILayout.BeginHorizontal();
            foreach (var material in gridMappingService.RecentlyUsedMaterials)
            {
                // get material if null continue to next one
                if (material == null)
                {
                    continue;
                }

                // setup content
                var content = new GUIContent { text = material.name };

                // if showing preview get a preview texture
                if (showPreview)
                {
                    content.image = AssetPreview.GetAssetPreview(material);
                }

                // draw button
                if (GUILayout.Button(content, style, GUILayout.MaxWidth(buttonSize), GUILayout.MaxHeight(buttonSize)))
                {
                    // set current material
                    gridMappingService.CurrentMaterial = material;
                    if (gridMappingService.CurrentMaterial != null)
                    {
                        settings.SetValue(GlobalConstants.CurrentMaterialKey, gridMappingService.CurrentMaterial.GetInstanceID());
                    }
                }

                // increment row count and check if need to start a new row
                count++;
                if (count >= buttonsPerRow)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    count = 0;
                }
            }

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a list of recent prefabs used.
        /// </summary>
        private void DrawRecentPrefabList()
        {
            // don't draw if quick prefab selection is not gear icon
            if (this.currentPrefabIndex != 0)
            {
                return;
            }

            GUILayout.BeginVertical();

            // get reference to settings instance
            var settings = SettingsManager.Instance;

            // let localization instance
            var local = LocalizationManager.Instance;

            // the currently selected prefab 
            GUILayout.Label(local.Get("CurrentPrefab"));

            // get grid mapping service
            var gridMappingService = GridMappingService.Instance;

            // draw current prefab control
            var value =
                (GameObject)
                EditorGUILayout.ObjectField(
                    gridMappingService.CurrentPrefab != null ? gridMappingService.CurrentPrefab.name : string.Empty,
                    gridMappingService.CurrentPrefab,
                    typeof(GameObject),
                    true);

            // if selection has changed add or move it to the top of the recently used prefab list
            if (value != gridMappingService.CurrentPrefab)
            {
                gridMappingService.CurrentPrefab = value;
                if (value != null)
                {
                    // ensure there are no duplicates in the list
                    var i = 1;
                    while (i < gridMappingService.RecentlyUsedPrefabs.Count)
                    {
                        if (gridMappingService.RecentlyUsedPrefabs[i] != null && value.GetInstanceID() == gridMappingService.RecentlyUsedPrefabs[i].GetInstanceID())
                        {
                            gridMappingService.RecentlyUsedPrefabs.RemoveAt(i);
                        }
                        else
                        {
                            i++;
                        }
                    }

                    // inset selection at top of recently used list
                    gridMappingService.RecentlyUsedPrefabs.Insert(0, value);

                    // store current selection in settings
                    settings.SetValue(GlobalConstants.CurrentPrefabKey, AssetDatabase.AssetPathToGUID(GridMapping.Helpers.GetSourcePrefab(value)));
                }
                else
                {
                    settings.RemoveValue(GlobalConstants.CurrentPrefabKey);
                }
            }

            // add some spacing
            GUILayout.Space(4);

            // get various settings related to the recent prefab list
            var maxItems = settings.GetSetting(GlobalConstants.MaxNumberOfRecentPrefabsKey, 64);
            var maxHeight = settings.GetSetting(GlobalConstants.MaxHeightOfRecentPrefabsKey, 64);
            var showRecentPrefabs = settings.GetSetting(GlobalConstants.RecentInspectorPrefabListEnabledKey, true);
            var showAsList = settings.GetSetting(GlobalConstants.ShowRecentPrefabAsListKey, true);
            var buttonSize = settings.GetSetting(GlobalConstants.RecentPrefabButtonSizeKey, 32);
            var showPreview = settings.GetSetting(GlobalConstants.ShowRecentPrefabAssetPreviewsKey, true);
            var showAddPrefab = settings.GetSetting(GlobalConstants.ShowAddPrefabButtonsKey, true);

            GUILayout.EndVertical();

            // check if recent prefabs list should be shown
            if (!showRecentPrefabs)
            {
                return;
            }

            // provides a button to add new prefabs to the list
            if (showAddPrefab && GUILayout.Button(local.Get("AddPrefab"), GUILayout.MaxWidth(100)))
            {
                gridMappingService.RecentlyUsedPrefabs.Add(null);
            }

            GUILayout.Space(4);

            // limit max items in the list based on settings
            while (gridMappingService.RecentlyUsedPrefabs.Count > 0 && gridMappingService.RecentlyUsedPrefabs.Count > maxItems)
            {
                gridMappingService.RecentlyUsedPrefabs.RemoveAt(gridMappingService.RecentlyUsedPrefabs.Count - 1);
            }

            this.prefabScroll = GUILayout.BeginScrollView(this.prefabScroll, false, false, GUILayout.Height(maxHeight), GUILayout.MaxHeight(maxHeight));

            if (showAsList)
            {
                this.DrawRecentPrefabsAsList(buttonSize, showPreview);
            }
            else
            {
                this.DrawRecentPrefabsAsSelectionGrid(buttonSize, showPreview);
            }

            GUILayout.EndScrollView();
        }

        /// <summary>
        /// Draws the recent prefabs list as list.
        /// </summary>
        /// <param name="buttonSize">
        /// The button size of each button in the grid.
        /// </param>
        /// <param name="showPreview">
        /// If true will use <see cref="AssetPreview.GetAssetPreview"/> as the texture.
        /// </param>
        private void DrawRecentPrefabsAsList(float buttonSize, bool showPreview)
        {
            // get references to services and settings
            var gridMappingService = GridMappingService.Instance;
            var settings = SettingsManager.Instance;
            var showRemoveButtons = settings.GetSetting(GlobalConstants.ShowRecentPrefabRemoveButtonsKey, true);
            var showAsButtons = settings.GetSetting(GlobalConstants.ShowRecentPrefabsAsButtonsKey, false);
            var showSelectButton = settings.GetSetting(GlobalConstants.ShowRecentPrefabsSelectButtonsKey, true);
            var local = LocalizationManager.Instance;

            // loop through each prefab in the recently used list
            var i = 0;
            while (i < gridMappingService.RecentlyUsedPrefabs.Count)
            {
                // get prefab reference
                var prefab = gridMappingService.RecentlyUsedPrefabs[i];

                GUILayout.BeginHorizontal();

                // check if remove buttons should be shown
                var wasRemoved = false;
                if (showRemoveButtons)
                {
                    // hold on to button click state for later use
                    wasRemoved = GUILayout.Button("X", GUILayout.MaxWidth(25));
                    if (wasRemoved)
                    {
                        gridMappingService.RecentlyUsedPrefabs.RemoveAt(i);
                    }
                }

                // do index range check to make sure we are still in range after drawing button
                // NOTE: COULD USE BETTER CODE HERE
                if (i < gridMappingService.RecentlyUsedPrefabs.Count)
                {
                    // if not showing a buttons then draw the prefab selection field
                    if (!showAsButtons)
                    {
                        var value = (GameObject)EditorGUILayout.ObjectField(prefab, typeof(GameObject), true);
                        if (value != prefab)
                        {
                            gridMappingService.RecentlyUsedPrefabs.Insert(0, value);
                            gridMappingService.RecentlyUsedPrefabs.RemoveAt(i);
                            gridMappingService.RecentlyUsedPrefabs[i] = value;
                        }
                    }

                    // setup text for button
                    var buttonText = prefab != null ? prefab.name : local.Get("NoneBracket");
                    if (!showAsButtons && showSelectButton)
                    {
                        buttonText = local.Get("Select");
                    }

                    // setup button content
                    var content = new GUIContent(buttonText);
                    if (showPreview && showAsButtons && prefab != null)
                    {
                        content.image = AssetPreview.GetAssetPreview(prefab);
                    }

                    // draw the button 
                    if ((showSelectButton || showAsButtons) &&
                        GUILayout.Button(content, GUILayout.MaxWidth(showAsButtons ? int.MaxValue : 50), GUILayout.Height(buttonSize)))
                    {
                        gridMappingService.CurrentPrefab = prefab;
                        if (prefab != null)
                        {
                            settings.SetValue(GlobalConstants.CurrentPrefabKey, prefab.GetInstanceID());
                        }
                    }
                }

                GUILayout.EndHorizontal();

                // if button was not clicked increment the index
                if (!wasRemoved)
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// Draws the recent prefabs list as grid.
        /// </summary>
        /// <param name="buttonSize">
        /// The button size of each button in the grid.
        /// </param>
        /// <param name="showPreview">
        /// If true will use <see cref="AssetPreview.GetAssetPreview"/> as the texture.
        /// </param>
        private void DrawRecentPrefabsAsSelectionGrid(float buttonSize, bool showPreview)
        {
            // get references to services and settings
            var gridMappingService = GridMappingService.Instance;
            var settings = SettingsManager.Instance;

            // get number of prefab buttons to show per row
            var buttonsPerRow = settings.GetSetting(GlobalConstants.RecentPrefabButtonColumnsKey, 5);
            var count = 0;

            // setup button style
            var style = new GUIStyle(GUI.skin.button)
                            {
                                imagePosition = ImagePosition.ImageAbove,
                                alignment = TextAnchor.LowerCenter
                            };

            // begin laying out buttons
            GUILayout.BeginHorizontal();
            for (var i = 0; i < gridMappingService.RecentlyUsedPrefabs.Count; i++)
            {
                // get prefab if null continue to next one
                var prefab = gridMappingService.RecentlyUsedPrefabs[i];
                if (prefab == null)
                {
                    continue;
                }

                // setup content for button
                var content = new GUIContent { text = prefab.name };

                // if showing preview get a preview texture
                if (showPreview)
                {
                    content.image = AssetPreview.GetAssetPreview(prefab);
                }

                // draw button
                if (GUILayout.Button(content, style, GUILayout.MaxWidth(buttonSize), GUILayout.MaxHeight(buttonSize)))
                {
                    // set current prefab if clicked
                    gridMappingService.CurrentPrefab = gridMappingService.RecentlyUsedPrefabs[i];
                    if (gridMappingService.CurrentPrefab != null)
                    {
                        settings.SetValue(GlobalConstants.CurrentPrefabKey, gridMappingService.CurrentPrefab.GetInstanceID());
                    }
                }

                // increment row count and check if need to start a new row
                count++;
                if (count >= buttonsPerRow)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    count = 0;
                }
            }

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws controls that specify how a prefab is rotated when drawn.
        /// </summary>
        private void DrawRotationControls()
        {
            // get reference to settings and localization
            var local = LocalizationManager.Instance;
            var settings = SettingsManager.Instance;

            var rotationStyle = settings.GetSetting(GlobalConstants.RotationStyleKey, 1);

            switch (rotationStyle)
            {
                case 0:
                    break;

                case 1:
                    GUILayout.Label(local.Get("Rotation"));
                    this.DrawButtonGridRotationControls();
                    break;

                case 2:
                    if (this.rotationWindow == null)
                    {
                        this.rotationWindow = EditorWindow.GetWindow<RotationsWindow>();
                        this.rotationWindow.Editor = this;
                        this.rotationWindow.Show();
                    }

                    break;
            }
        }

        /// <summary>
        /// Used to handles the <see cref="GridMappingService.CurrentMaterialChanged"/> event.
        /// </summary>
        /// <param name="sender">Typically a reference to the object that threw the event.</param>
        /// <param name="e">A <see cref="EventArgs"/> type containing event arguments.</param>
        private void GridMappingServiceCurrentMaterialChanged(object sender, EventArgs e)
        {
            this.Repaint();
        }

        /// <summary>
        /// Handles mouse wheel input for the map.
        /// </summary>
        private void HandleMouseWheelInput()
        {
            var settings = SettingsManager.Instance;
            var current = Event.current;
            if (current.type == EventType.Ignore)
            {
                return;
            }

            // check for mouse wheel movement
            var holdMouseWheel = settings.GetSetting(GlobalConstants.MouseWheelChangesLayersKey, false);
            if (!holdMouseWheel || current.type != EventType.ScrollWheel)
            {
                return;
            }

            // get settings relating to changing the active layer
            var holdShiftToChangeActiveLayer = settings.GetSetting(GlobalConstants.HoldShiftToChangeLayersKey, true);
            var holdAltToChangeActiveLayer = settings.GetSetting(GlobalConstants.HoldAltToChangeLayersKey, false);
            var holdControlToChangeActiveLayer = settings.GetSetting(GlobalConstants.HoldControlToChangeLayersKey, false);

            // check settings and see of we can change the active layer. A value equal to 0 means the change can occur
            var canChangeLayer = 0;
            canChangeLayer += holdShiftToChangeActiveLayer && !current.shift ? 1 : 0;
            canChangeLayer += holdAltToChangeActiveLayer && !current.alt ? 1 : 0;
            canChangeLayer += holdControlToChangeActiveLayer && !current.control ? 1 : 0;
            if (canChangeLayer == 0)
            {
                // change the active layer
                if (current.delta.y < 0)
                {
                    this.ActiveLayer--;
                }

                if (current.delta.y > 0)
                {
                    this.ActiveLayer++;
                }

                this.Repaint();
                current.Use();
            }
        }

        /// <summary>
        /// Loads content from resources folder for the rotation controls.
        /// </summary>
        private void LoadContent()
        {
            var settings = SettingsManager.Instance;
            var texturesPath = Path.Combine(settings.GetSetting(CoreGlobalConstants.ResourceFolderKey, "Codefarts.Unity"), "Textures");
            texturesPath = texturesPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            var getPath = new Func<string, string>(x => Path.Combine(texturesPath, x).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

            this.rotationContentForXAxis = new[]
                {
                    new GUIContent(Resources.Load(getPath("Red225"), typeof(Texture2D)) as Texture2D, "225"), 
                    new GUIContent(Resources.Load(getPath("Red270"), typeof(Texture2D)) as Texture2D, "270"), 
                    new GUIContent(Resources.Load(getPath("Red315"), typeof(Texture2D)) as Texture2D, "315"), 
                    new GUIContent(Resources.Load(getPath("Red180"), typeof(Texture2D)) as Texture2D, "180"), 
                    new GUIContent(),             
                    new GUIContent(Resources.Load(getPath("Red0"), typeof(Texture2D)) as Texture2D, "0"), 
                    new GUIContent(Resources.Load(getPath("Red135"), typeof(Texture2D)) as Texture2D, "135"), 
                    new GUIContent(Resources.Load(getPath("Red90"), typeof(Texture2D)) as Texture2D, "90"), 
                    new GUIContent(Resources.Load(getPath("Red45"), typeof(Texture2D)) as Texture2D, "45")
                };

            this.rotationContentForYAxis = new[]
                {
                    new GUIContent(Resources.Load(getPath("Green225"), typeof(Texture2D)) as Texture2D, "225"), 
                    new GUIContent(Resources.Load(getPath("Green270"), typeof(Texture2D)) as Texture2D, "270"), 
                    new GUIContent(Resources.Load(getPath("Green315"), typeof(Texture2D)) as Texture2D, "315"), 
                    new GUIContent(Resources.Load(getPath("Green180"), typeof(Texture2D)) as Texture2D, "180"), 
                    new GUIContent(),             
                    new GUIContent(Resources.Load(getPath("Green0"), typeof(Texture2D)) as Texture2D, "0"), 
                    new GUIContent(Resources.Load(getPath("Green135"), typeof(Texture2D)) as Texture2D, "135"), 
                    new GUIContent(Resources.Load(getPath("Green90"), typeof(Texture2D)) as Texture2D, "90"), 
                    new GUIContent(Resources.Load(getPath("Green45"), typeof(Texture2D)) as Texture2D, "45")
                };

            this.rotationContentForZAxis = new[]
                {
                    new GUIContent(Resources.Load(getPath("Blue225"), typeof(Texture2D)) as Texture2D, "225"), 
                    new GUIContent(Resources.Load(getPath("Blue270"), typeof(Texture2D)) as Texture2D, "270"), 
                    new GUIContent(Resources.Load(getPath("Blue315"), typeof(Texture2D)) as Texture2D, "315"), 
                    new GUIContent(Resources.Load(getPath("Blue180"), typeof(Texture2D)) as Texture2D, "180"), 
                    new GUIContent(),             
                    new GUIContent(Resources.Load(getPath("Blue0"), typeof(Texture2D)) as Texture2D, "0"), 
                    new GUIContent(Resources.Load(getPath("Blue135"), typeof(Texture2D)) as Texture2D, "135"), 
                    new GUIContent(Resources.Load(getPath("Blue90"), typeof(Texture2D)) as Texture2D, "90"), 
                    new GUIContent(Resources.Load(getPath("Blue45"), typeof(Texture2D)) as Texture2D, "45")
                };

            this.layerStyles = new[]
                {
                    new GUIStyle { stretchHeight = true, stretchWidth = true, normal = new GUIStyleState { background = Resources.Load(getPath("Visible"), typeof(Texture2D)) as Texture2D } }, 
                    new GUIStyle { stretchHeight = true, stretchWidth = true, normal = new GUIStyleState { background = Resources.Load(getPath("NotVisible"), typeof(Texture2D)) as Texture2D } }, 
                    new GUIStyle { stretchHeight = true, stretchWidth = true, normal = new GUIStyleState { background = Resources.Load(getPath("Locked"), typeof(Texture2D)) as Texture2D } }, 
                    new GUIStyle { stretchHeight = true, stretchWidth = true, normal = new GUIStyleState { background = Resources.Load(getPath("Unlocked"), typeof(Texture2D)) as Texture2D } }, 
                    new GUIStyle { stretchHeight = true, stretchWidth = true, normal = new GUIStyleState { background = Resources.Load(getPath("Pen"), typeof(Texture2D)) as Texture2D } } 
                };

            this.buttonContent = new[]
                {
                    new GUIContent(Resources.Load(getPath("Add"), typeof(Texture2D)) as Texture2D), 
                    new GUIContent(Resources.Load(getPath("Subtract"), typeof(Texture2D)) as Texture2D), 
                    new GUIContent(Resources.Load(getPath("Delete"), typeof(Texture2D)) as Texture2D),  
                    new GUIContent(Resources.Load(getPath("Up"), typeof(Texture2D)) as Texture2D),  
                    new GUIContent(Resources.Load(getPath("Down"), typeof(Texture2D)) as Texture2D),  
                    new GUIContent(Resources.Load(getPath("Restricted"), typeof(Texture2D)) as Texture2D)  
                    };
        }

        /// <summary>
        /// The load prefabs.
        /// </summary>
        private void LoadPrefabs()
        {
            var settings = SettingsManager.Instance;

            // attempt to load a text file(s) from a resources folder
            var resFolders = new List<string>();
            var resourcesFolders = settings.GetSetting(GlobalConstants.PrefabIndexResourceFolderKey, "Codefarts.Unity/GridMappingPrefabIndexFiles");

            if (resourcesFolders.Contains("\r\n"))
            {
                resFolders.AddRange(resourcesFolders.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                resFolders.Add(resourcesFolders);
            }

            // process each resource folder
            foreach (var folder in resFolders)
            {
                // get all text asset files from resource folder
                var files = Resources.LoadAll(folder, typeof(TextAsset));

                // check if nothing to do
                if (files == null)
                {
                    return;
                }

                // process each file that was found
                foreach (var file in files)
                {
                    var separatorString = " "; // default is space character
                    var currentCategory = string.Empty;
                    var textAsset = (TextAsset)file;
                    var lines = textAsset.text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        // check for comment line
                        if (line.Trim().StartsWith("//"))
                        {
                            // check if a separator was specified other wise treat like normal comment and ignore
                            if (line.Trim().ToLower().StartsWith("// separator"))
                            {
                                // get the separator string is specified
                                var tempValue = line.Trim().Substring(12).Trim();
                                if (!string.IsNullOrEmpty(tempValue))
                                {
                                    separatorString = tempValue;
                                }
                            }

                            // check if a category was specified other wise treat like normal comment and ignore
                            if (line.Trim().ToLower().StartsWith("// category"))
                            {
                                // get the separator string is specified
                                var tempValue = line.Trim().Substring(11).Trim();
                                if (!string.IsNullOrEmpty(tempValue))
                                {
                                    currentCategory = tempValue;
                                }
                            }
                        }
                        else
                        {
                            // split parts
                            var parts = line.Trim().Split(new[] { separatorString }, StringSplitOptions.RemoveEmptyEntries);

                            // should have 3 parts (prefab / texture / tooltip)
                            if (parts.Length == 3)
                            {
                                // ensure trimmed parts
                                for (int i = 0; i < parts.Length; i++)
                                {
                                    parts[i] = parts[i].Trim();
                                }

                                // try to load prefab
                                var prefab = Resources.Load(parts[0], typeof(GameObject)) as GameObject;

                                // check if successful
                                if (prefab == null)
                                {
                                    // did not or could not load prefab so log error & skip it
                                    Debug.LogWarning("Could not load prefab!\r\n" + line);
                                    continue;
                                }

                                Texture2D texture = null;
                                if (!string.IsNullOrEmpty(parts[1]))
                                {
                                    texture = Resources.Load(parts[1], typeof(Texture2D)) as Texture2D;
                                }

                                // check if successful
                                if (texture == null)
                                {
                                    // did not or could not load prefab texture so use asset preview
                                    texture = AssetPreview.GetAssetPreview(prefab);
                                }

                                // check if category specified if no category defined then prefabs will not be added
                                if (!string.IsNullOrEmpty(currentCategory))
                                {
                                    // check if category exists and if not add it
                                    if (!this.loadedPrefabs.ContainsKey(currentCategory))
                                    {
                                        this.loadedPrefabs.Add(currentCategory, new List<PrefabDataModel>());
                                    }

                                    // if no name specified use the prefab name
                                    if (string.IsNullOrEmpty(parts[2]))
                                    {
                                        parts[2] = Path.GetFileNameWithoutExtension(parts[0]);
                                    }

                                    // add the prefab to the loaded prefabs list
                                    this.loadedPrefabs[currentCategory].Add(new PrefabDataModel { Name = parts[2], Category = currentCategory, Prefab = prefab, Texture = texture });
                                }
                            }
                        }
                    }
                }

                this.prefabCategories = this.loadedPrefabs.Keys.ToArray();
            }
        }

        /// <summary>
        /// Loads various settings.
        /// </summary>
        private void LoadSettings()
        {
            // load color settings
            var settings = SettingsManager.Instance;
            settings.ValueChanged += (s, e) => SceneView.RepaintAll();

            try
            {
                // restore various settings
                this.mapInfoShown = settings.GetSetting(GlobalConstants.MapInfoStyleKey, true);
                this.autoScalePrefab = settings.GetSetting(GlobalConstants.AutoScalePrefabKey, true);
                this.autoCenterPrefab = settings.GetSetting(GlobalConstants.AutoCenterPrefabKey, true);
                this.drawGridLines = settings.GetSetting(GlobalConstants.ShowGridLinesKey, true);
                this.drawMarker = settings.GetSetting(GlobalConstants.ShowMarkerKey, true);
                this.drawGuideLines = settings.GetSetting(GlobalConstants.ShowGuidelinesKey, false);
                this.rotationX = settings.GetSetting(GlobalConstants.PreviousXRotationKey, 5);
                this.rotationY = settings.GetSetting(GlobalConstants.PreviousYRotationKey, 5);
                this.rotationZ = settings.GetSetting(GlobalConstants.PreviousZRotationKey, 5);
                this.applyMaterial = settings.GetSetting(GlobalConstants.ApplyMaterialKey, true);

                // check if there is a key present for the currently selected material
                int id;
                if (settings.TryGetSetting(GlobalConstants.CurrentPrefabCategoryIndexKey, out id))
                {
                    // set prefab category index = retrieved value
                    this.currentPrefabCategoryIndex = id;
                }

                // restore active layer
                if (settings.TryGetSetting(GlobalConstants.ActiveLayerKey, out id))
                {
                    this.ActiveLayer = id;
                }

                // restore previous selected prefab selection index
                var gridMappingService = GridMappingService.Instance;
                if (settings.TryGetSetting(GlobalConstants.CurrentPrefabIndexKey, out id))
                {
                    // set prefab category index = retrieved value
                    this.currentPrefabIndex = id;
                    if (this.currentPrefabIndex - 1 >= 0 && this.currentPrefabIndex - 1 <= this.CurrentPrefabList.Count - 1)
                    {
                        gridMappingService.CurrentPrefab = this.CurrentPrefabList[this.currentPrefabIndex - 1].Prefab;
                    }
                }

                // check if there is a key present for the currently selected material
                string guid;
                if (settings.TryGetSetting(GlobalConstants.CurrentMaterialKey, out guid))
                {
                    // attempt to get a reference to the material using the prefab guid id
                    gridMappingService.CurrentMaterial = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Material)) as Material;
                }

                // check if there is a key present for the currently selected prefab
                if (this.currentPrefabIndex == 0 && settings.TryGetSetting(GlobalConstants.CurrentPrefabKey, out guid))
                {
                    // attempt to get a reference to the prefab using the prefab guid id
                    gridMappingService.CurrentPrefab = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(GameObject)) as GameObject;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// Recalculates the position of the marker based on the location of the mouse pointer.
        /// </summary>
        private void RecalculateHighlightPosition()
        {
#if PERFORMANCE
            var performance = PerformanceTesting<PerformanceID>.Instance;
            performance.Start(PerformanceID.RecalculateHighlightPosition);
#endif

            // get reference to the grid map component
            var map = (GridMap)this.target;

            // store the grid location (Column/Row) based on the current location of the mouse pointer
            var gridPosition = this.GetGridPosition(this.mouseHitPosition);

            // store the grid position in local space
            var position = new Vector3(gridPosition.X * map.CellWidth, 0, gridPosition.Y * map.CellHeight);

            // set the highlightPosition value
            this.highlightPosition = new Vector3(
                                         position.X + (map.CellWidth / 2),
                                         this.ActiveLayer * map.Depth,
                                         position.Z + (map.CellHeight / 2));

#if PERFORMANCE
            performance.Stop(PerformanceID.RecalculateHighlightPosition);
#endif
        }

        /// <summary>
        /// Handles the <see cref="GridMappingService.DrawingToolChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        private void UpdateDrawingToolList(object sender, EventArgs e)
        {
            // rebuild the tool list  
            var settings = SettingsManager.Instance;

            // setup default tool list values
            var defaultTools = new[]
                                   {
                                       typeof(Pencil).FullName, typeof(PrefabInformation).FullName, typeof(PrefabPicker).FullName, typeof(Rectangle).FullName
                                   };

            // get added tools from settings
            var drawToolSetting = settings.GetSetting(GlobalConstants.DrawingToolListKey, string.Join(",", defaultTools));
            var strings = drawToolSetting.Split(
                new[]
                    {
                        ','
                    },
                    StringSplitOptions.RemoveEmptyEntries);
            var addedTools = strings.Select(item => item.Trim()).ToList();

            // attempt to create each tool
            string errors = null;
            var list = new List<IEditorTool<GridMapEditor>>();
            foreach (var tool in addedTools)
            {
                try
                {
                    // get the type
                    var type = Type.GetType(tool, true);
                    if (type == null)
                    {
                        continue;
                    }

                    // attempt to create the type
                    var temp = type.Assembly.CreateInstance(tool) as IEditorTool<GridMapEditor>;
                    if (temp != null)
                    {
                        list.Add(temp);
                    }
                }
                catch (Exception ex)
                {
                    // log the error message
                    errors += string.Format("{0}\r\n", ex.Message);
                }
            }

            // get map and service references
            var map = this.target as GridMap;
            var gridMappingService = GridMappingService.Instance;

            // shut down and remove reference to the current drawing tool if present
            if (this.currentTool != null)
            {
                this.currentTool.Shutdown();
                this.currentTool = null;
                gridMappingService.OnDrawingToolSelected(null, null, map);
            }

            // get tool manager service & remove all existing tools and register new ones
            // based of the tools that were just created
            var manager = DrawingToolManager.Instance;
            manager.Clear();
            list.ForEach(x =>
                {
                    if (!manager.HasTool(x.Uid))
                    {
                        manager.Register(x);
                    }
                });

            // if there was a drawing tool specified before try to make it the current tool again
            if (this.drawingToolIndex != Guid.Empty)
            {
                var toolReference = manager.HasTool(this.drawingToolIndex) ? manager.GetTool(this.drawingToolIndex) : null;
                if (toolReference != null)
                {
                    this.currentTool = toolReference;
                    this.currentTool.Startup(this);
                    gridMappingService.OnDrawingToolSelected(null, this.currentTool, map);
                }
            }

            // remember to update the inspector for immediate results
            Helpers.RedrawInspector();

            // report any errors as warning in the console
            if (errors != null)
            {
                Debug.LogWarning(string.Format("Errors occurred while updating the tool list\r\n{0}", errors));
            }
        }

        /// <summary>
        /// Calculates the position of the mouse over the grid map in local space coordinates.
        /// </summary>
        /// <returns>Returns true if the mouse is over the grid map.</returns>
        private bool UpdateHitPosition()
        {
            // get reference to the grid map component
            var map = (GridMap)this.target;

            // build a plane object that 
            // rotate up the same as map rotation
            var directionVector = Vector3.MoveTowards(Vector3.Up, map.transform.up, 1);
            directionVector.Normalize();
            directionVector *= this.ActiveLayer * map.Depth;

            var p = new Plane(map.transform.up, map.transform.position + directionVector);

            // build a ray type from the current mouse position
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            // stores the hit location
            var hit = new Vector3();

            // stores the distance to the hit location
            float dist;

            // cast a ray to determine what location it intersects with the plane
            if (p.Raycast(ray, out dist))
            {
                // the ray hits the plane so we calculate the hit location in world space
                hit = ray.origin + (ray.direction.normalized * dist);
            }

            // convert the hit location from world space to local space
            var value = map.transform.InverseTransformPoint(hit);

            var changed = value != this.mouseHitPosition;
            this.mouseHitPosition = value;

            // return true indicating a successful hit test
            return changed;
        }
    }
}