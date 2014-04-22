/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.GridMapping.Editor.DrawingTools
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Codefarts.CoreProjectCode;
    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.Localization;

    using UnityEditor;

    using UnityEngine;

    using Vector3 = Codefarts.GridMapping.Common.Vector3;

    /// <summary>
    /// The rectangle tool for drawing rectangles and filled rectangles.
    /// </summary>
    public class Rectangle : IEditorTool<GridMapEditor>
    {
        /// <summary>
        /// Used to hold the content of the tool buttons.
        /// </summary>
        private GUIContent[] buttonContent;

        /// <summary>
        /// Used to hold a reference to the editor associated with the tool.
        /// </summary>
        private GridMapEditor editor;

        /// <summary>
        /// Used to determine whether or not to draw a filed rectangle.
        /// </summary>
        private bool fillRectangle;

        /// <summary>
        /// Used to determine if the tool is the active tool.
        /// </summary>
        private bool isActive;

        /// <summary>
        /// Used to track whether the mouse button is held down.
        /// </summary>
        /// <remarks>It may seem redundant to use <see cref="isMouseDown" /> and <see cref="GridMapEditor.MarkerHasStartPosition" /> to track the mouse down state but if we relied upon
        /// <see cref="GridMapEditor.MarkerHasStartPosition" /> alone it could cause possible issues seeing as how other code may be manipulating the value at any given time.
        /// </remarks>
        private bool isMouseDown;

        /// <summary>
        /// Used to hold a reference to the map associated with the tool.
        /// </summary>
        private GridMap map;

        /// <summary>
        /// Gets a value representing the content to be used for the tools button.
        /// </summary>
        public GUIContent ButtonContent
        {
            get
            {
                if (this.buttonContent == null)
                {
                    var settings = SettingsManager.Instance;
                    var texturesPath = Path.Combine(settings.GetSetting(CoreGlobalConstants.ResourceFolderKey, "Codefarts.Unity"), "Textures");
                    texturesPath = texturesPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                    var getPath = new Func<string, string>(x => Path.Combine(texturesPath, x).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                    this.buttonContent = new GUIContent[2];
                    this.buttonContent[0] = new GUIContent(Resources.Load(getPath("DrawRectangle"), typeof(Texture2D)) as Texture2D, this.Title);
                    this.buttonContent[1] = new GUIContent(Resources.Load(getPath("DrawFilledRectangle"), typeof(Texture2D)) as Texture2D, this.Title);
                }

                return this.buttonContent[this.fillRectangle ? 1 : 0];
            }
        }

        /// <summary>
        /// Gets a value representing the title or name of the tool.
        /// </summary>
        public string Title
        {
            get
            {
                return LocalizationManager.Instance.Get("Rectangle");
            }
        }

        /// <summary>
        /// Gets a <see cref="Guid"/> value that uniquely identifies the tool.
        /// </summary>
        public Guid Uid
        {
            get
            {
                return new Guid("47D72DD7-7A77-4D73-961D-40B5152FD1A9");
            }
        }

        /// <summary>
        /// Called by the editor to notify the tool to draw something.
        /// </summary>
        public void Draw()
        {
            // does nothing
        }

        /// <summary>
        /// Called by <see cref="GridMapEditor"/> to give the tool the ability to include additional tools in the inspector.
        /// </summary>
        public void OnInspectorGUI()
        {
            var local = LocalizationManager.Instance;

            // draw option for toggling filled & regular rectangles
            GUILayout.BeginVertical();
            this.fillRectangle = GUILayout.Toggle(this.fillRectangle, local.Get("Fill"), GUI.skin.button);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Called by the editor to allow the tool to draw it self on the tool bar.
        /// </summary>
        /// <returns>If true the tool has drawn it self and no further drawing is necessary, otherwise false.</returns>
        public bool DrawTool()
        {
            return false;
        }

        /// <summary>
        /// Method will be called when the tool is no longer of use by the system or a new tool is being selected.
        /// </summary>
        public void Shutdown()
        {
            this.isActive = false;
            this.map = null;
            if (this.editor != null)
            {
                this.editor.MarkerHasStartPosition = false;
            }

            this.editor = null;
            this.isMouseDown = false;
            this.buttonContent = null;
        }

        /// <summary>
        /// Called when the tool is set as the active tool.
        /// </summary>
        /// <param name="editor">A reference to the editor that this tool should work against.</param>
        public void Startup(GridMapEditor editor)
        {
            if (this.isActive)
            {
                return;
            }

            this.map = (GridMap)editor.target;
            this.editor = editor;
            this.editor.MarkerHasStartPosition = false;
            this.isMouseDown = false;
            this.isActive = true;
            Tools.current = Tool.View;
            Tools.viewTool = ViewTool.FPS;
        }

        /// <summary>
        /// Called by <see cref="GridMapEditor"/> to update the tool on every call to OnSceneGUI.
        /// </summary>
        public void Update()
        {
            var current = Event.current;
            if (!this.isActive)
            {
                return;
            }

            var rect = new Rect(0, 0, Screen.width, Screen.height);

            if ((current.type == EventType.Ignore && current.rawType == EventType.MouseUp) || !rect.Contains(current.mousePosition))
            {
                Tools.current = Tool.View;
                Tools.viewTool = ViewTool.FPS;
                this.isMouseDown = false;
                this.editor.MarkerHasStartPosition = false;
                SceneView.RepaintAll();
                return;
            }

            // get settings relating to drawing
            var settings = SettingsManager.Instance;

            // check if mouse up event occurred
            if (this.isMouseDown && current.type == EventType.MouseUp)
            {
                // draw rectangle of prefabs
                this.HandleMouseUp();

                // we can return as we have handled a mouse up event
                return;
            }

            // check if mouse is down or dragging & is over the layer and if so draw
            if (this.isMouseDown || current.type != EventType.MouseDown || !this.editor.IsMouseOverLayer())
            {
                return;
            }

            // check for acceptable button
            if (current.button != 0 && current.button != 1)
            {
                return;
            }

            // get settings relating to drawing/erasing depending on button input
            var key = current.button == 0 ? GlobalConstants.HoldAltToDrawKey : GlobalConstants.HoldAltToEraseKey;
            var holdAlt = settings.GetSetting(key, false);

            // check settings and see of we can begin. A value equal to 0 means we can perform
            var canDo = 0;
            canDo += holdAlt && !current.alt ? 1 : 0;
            canDo += !holdAlt && current.alt ? 1 : 0; // maintains v1 behavior

            // if can't do just exit
            if (canDo != 0)
            {
                return;
            }

            // if mouse button is pressed then we begin operation
            this.isMouseDown = true;
            this.editor.MarkerHasStartPosition = true;
            this.editor.MarkerStartPosition = this.editor.HighlightPosition;
            current.Use();
        }

        /// <summary>
        /// Responsible for drawing the actual rectangle on the map.
        /// </summary>
        /// <param name="erasing">If true will perform an erase instead of drawing.</param>
        private void DrawRectangle(bool erasing)
        {
            // do not draw if not the view tool
            if (Tools.current != Tool.View)
            {
                return;
            }

            // checked if layer locked or not visible
            var layerModel = this.map.Layers[this.editor.ActiveLayer];
            if (layerModel.Locked || !layerModel.Visible)
            {
                return;
            }

            // calculate the min max grid cell locations
            var bounds = new Bounds();

            bounds.min = new Vector3(
                Math.Min(this.editor.MarkerStartPosition.X, this.editor.HighlightPosition.X),
                -(this.map.Depth / 2),
                Math.Min(this.editor.MarkerStartPosition.Z, this.editor.HighlightPosition.Z));
            bounds.max = new Vector3(
                Math.Max(this.editor.MarkerStartPosition.X, this.editor.HighlightPosition.X),
                this.map.Depth / 2,
                Math.Max(this.editor.MarkerStartPosition.Z, this.editor.HighlightPosition.Z));

            var tl = this.editor.GetGridPosition(bounds.min);
            var br = this.editor.GetGridPosition(bounds.max);

            GameObject prefab;
            if (!erasing)
            {
                prefab = Helpers.CreatePrimitivePrefab(this.editor);

                // if prefab is null we just exit and draw nothing
                if (prefab == null)
                {
                    return;
                }

                // set local rotation first so the GetBoundWithChildren calculates properly
                prefab.transform.localEulerAngles = new Vector3(
                    this.editor.GetSelectedRotationValue(this.editor.SelectedXRotationIndex),
                    this.editor.GetSelectedRotationValue(this.editor.SelectedYRotationIndex),
                    this.editor.GetSelectedRotationValue(this.editor.SelectedZRotationIndex));
            }
            else
            {
                Func<int, int, int, GameObject, bool> eraseCallback = (c, r, l, obj) =>
                    {
                        // check for any existing object and destroy it if it exists
                        var existing = this.editor.GameObjects.Get(c, r, l);
                        if (existing != null)
                        {
                            Undo.DestroyObjectImmediate(existing);   
                        }

                        return false;
                    };

                if (this.fillRectangle)
                {
                    this.map.EraseFilledRectangle(this.editor.ActiveLayer, tl.X, tl.Y, br.X - tl.X + 1, br.Y - tl.Y + 1, eraseCallback);
                }
                else
                {
                    this.map.EraseRectangle(this.editor.ActiveLayer, tl.X, tl.Y, br.X - tl.X + 1, br.Y - tl.Y + 1, eraseCallback);
                }

                // we are performing an erase operation so we can just exit here
                return;
            }

            var prefabName = prefab.name;
            var list = new List<GameObject>(); // holds a list of created prefabs that were drawn

            // get grid mapping service
            var gridMappingService = GridMappingService.Instance;
            var settings = SettingsManager.Instance;
            var nameFormat = settings.GetSetting(GlobalConstants.PrefabNameFormatKey, "{0}_l{1}_c{2}_r{3}");

            var callback = new Func<int, int, int, GameObject, bool>((c, r, l, obj) =>
            {
#if PERFORMANCE
                var perf = PerformanceTesting<PerformanceID>.Instance;
                perf.Start(this.fillRectangle ? PerformanceID.FillRectangleCallbacks : PerformanceID.RectangleCallbacks);
#endif
                // register object for undo
                Undo.RegisterCreatedObjectUndo(obj, obj.name);

                // give the prefab a name that represents it's location within the grid map
                obj.name = string.Format(nameFormat, prefabName, l, c, r);

                // check for any existing object and destroy it if it exists
                var existing = this.editor.GameObjects.Get(c, r, l);
                if (existing != null)
                {
                    Undo.DestroyObjectImmediate(existing);
                }

                this.editor.GameObjects.Set(c, r, l, obj);

                EditorUtility.SetDirty(obj);

                // report that the prefab was created
                gridMappingService.OnPrefabDrawn(map, c, r, l, obj);

                // add to the created list
                list.Add(obj);

                // set local rotation  
                obj.transform.localEulerAngles = new Vector3(
                    this.editor.GetSelectedRotationValue(this.editor.SelectedXRotationIndex),
                    this.editor.GetSelectedRotationValue(this.editor.SelectedYRotationIndex),
                    this.editor.GetSelectedRotationValue(this.editor.SelectedZRotationIndex));

#if PERFORMANCE
                perf.Stop(this.fillRectangle ? PerformanceID.FillRectangleCallbacks : PerformanceID.RectangleCallbacks);
#endif
                return true;
            });

            if (this.fillRectangle)
            {
                // update array
                this.map.FillRectangle(
                    this.editor.ActiveLayer,
                    tl.X,
                    tl.Y,
                    br.X - tl.X + 1,
                    br.Y - tl.Y + 1,
                    prefab,
                    this.editor.AutoCenterPrefab,
                    this.editor.AutoScalePrefab,
                    callback);
            }
            else
            {
                // update array
                this.map.DrawRectangle(
                    this.editor.ActiveLayer,
                    tl.X,
                    tl.Y,
                    br.X - tl.X + 1,
                    br.Y - tl.Y + 1,
                    prefab,
                    this.editor.AutoCenterPrefab,
                    this.editor.AutoScalePrefab,
                    callback);
            }

            // check if user setting says to hide draw prefabs from hierarchy
            if (!settings.GetSetting(GlobalConstants.ShowPrefabsInHierarchyKey, true))
            {
                foreach (var item in list)
                {
                    item.hideFlags |= HideFlags.HideInHierarchy;
                }

                // refresh the hierarchy window
                EditorApplication.RepaintHierarchyWindow();
            }
        }

        /// <summary>
        /// Responsible for handling muse up events.
        /// </summary>
        private void HandleMouseUp()
        {
#if PERFORMANCE
            var perf = PerformanceTesting<PerformanceID>.Instance;
#endif
            // get settings relating to drawing
            var settings = SettingsManager.Instance;
            var current = Event.current;

            // determine based on mouse button if we are drawing or erasing
            var performingDraw = current.button == 0;
            performingDraw = current.button == 1 ? false : performingDraw;

#if PERFORMANCE
            var perfEnum = performingDraw ? PerformanceID.DrawRectangle : PerformanceID.EraseRectangle;
#endif
            // get settings relating to drawing or erasing
            var holdShift = settings.GetSetting(performingDraw ? GlobalConstants.HoldShiftToDrawKey : GlobalConstants.HoldShiftToEraseKey, false);
            var holdAlt = settings.GetSetting(performingDraw ? GlobalConstants.HoldAltToDrawKey : GlobalConstants.HoldAltToEraseKey, false);
            var holdControl = settings.GetSetting(performingDraw ? GlobalConstants.HoldControlToDrawKey : GlobalConstants.HoldControlToEraseKey, false);

            // check settings and see of we can draw or erase. A value equal to 0 means it passed the test can occur
            var canDo = 0;
            canDo += holdShift && !current.shift ? 1 : 0;
            canDo += holdAlt && !current.alt ? 1 : 0;
            canDo += holdControl && !current.control ? 1 : 0;
            canDo += !holdAlt && current.alt ? 1 : 0; // maintains v1 behavior
            if (canDo == 0)
            {
#if PERFORMANCE
                perf.Start(perfEnum);
#endif
                this.DrawRectangle(!performingDraw);
                current.Use();
#if PERFORMANCE
                perf.Stop(perfEnum);
#endif
            }

            this.isMouseDown = false;
            this.editor.MarkerHasStartPosition = false;
        }
    }
}
