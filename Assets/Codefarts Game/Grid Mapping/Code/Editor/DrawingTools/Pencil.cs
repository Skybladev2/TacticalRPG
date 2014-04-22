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
    using Codefarts.GridMapping.Common;
    using Codefarts.GridMapping.Utilities;
    using Codefarts.GridMapping.Windows;
    using Codefarts.Localization;

    using UnityEditor;

    using UnityEngine;

    using Color = Codefarts.GridMapping.Common.Color;
    using Helpers = Codefarts.GridMapping.Editor.Helpers;
    using Random = System.Random;
    using Vector3 = UnityEngine.Vector3;

    /// <summary>
    /// Provides a pencil drawing tool that also supports Drawing rules.
    /// </summary>
    public class Pencil : IEditorTool<GridMapEditor>
    {
        /// <summary>
        /// Holds a list of drawing rules that the map editor will use when drawing.
        /// </summary>
        private readonly Dictionary<string, DrawingRuleListItem> drawingRules = new Dictionary<string, DrawingRuleListItem>();

        /// <summary>
        /// Used to hold a list of game objects to be destroyed.
        /// </summary>
        private readonly List<GameObject> gameObjectsToRemove = new List<GameObject>();

        /// <summary>
        /// Contains the <see cref="GUIContent"/> for the delete rule button.
        /// </summary>
        private GUIContent deleteRuleContent;

        /// <summary>
        /// Used to log each location the user has drawn in one constant drawing motion.
        /// </summary>
        /// <remarks>Used to help performance by not drawing over the same area a second time.</remarks>
        private GenericImage<Color> drawRecord;

        /// <summary>
        /// Holds a reference to a editor associated with the map.
        /// </summary>
        private GridMapEditor editor;

        /// <summary>
        /// Holds a state in order to determine if the tool is active.
        /// </summary>
        private bool isActive;

        /// <summary>
        /// Holds a state in order to determine if the user is drawing.
        /// </summary>
        private bool isDrawing;

        /// <summary>
        /// Used to hold the mouse down state.
        /// </summary>
        private bool isMouseDown;

        /// <summary>
        /// Used to log the last position that was drawn to.
        /// </summary>
        private Point lastPosition;

        /// <summary>
        /// Holds a reference to the map object that the tool will act on.
        /// </summary>
        private GridMap map;

        /// <summary>
        /// Holds a value indicating whether or not the list of drawing rules is shown.
        /// </summary>
        private bool showDrawingRules;

        /// <summary>
        /// Contains the <see cref="GUIContent"/> for the tool button.
        /// </summary>
        private GUIContent toolButtonContent;

        /// <summary>
        /// Gets a value representing the content to be used for the tools button.
        /// </summary>
        public GUIContent ButtonContent
        {
            get
            {
                if (this.toolButtonContent == null)
                {
                    var settings = SettingsManager.Instance;
                    var texturesPath = Path.Combine(settings.GetSetting(CoreGlobalConstants.ResourceFolderKey, "Codefarts.Unity"), "Textures");
                    texturesPath = texturesPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                    var getPath = new Func<string, string>(x => Path.Combine(texturesPath, x).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                    this.toolButtonContent = new GUIContent(Resources.Load(getPath("Draw"), typeof(Texture2D)) as Texture2D, this.Title);
                }

                return this.toolButtonContent;
            }
        }

        /// <summary>
        /// Gets a value representing the title or name of the tool.
        /// </summary>
        public string Title
        {
            get
            {
                return LocalizationManager.Instance.Get("Pencil");
            }
        }

        /// <summary>
        /// Gets the unique id for the tool.
        /// </summary>
        public Guid Uid
        {
            get
            {
                return GetUid();
            }
        }

        /// <summary>
        /// The get unique id.
        /// </summary>
        /// <returns>
        /// The <see cref="Guid"/> for the tool.
        /// </returns>
        public static Guid GetUid()
        {
            return new Guid("EB421F34-47C9-400C-BDB8-A158DB2E85C5");
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
            // get reference to settings and localization
            var settings = SettingsManager.Instance;
            var local = LocalizationManager.Instance;

            GUILayout.BeginVertical();  // main wrapper
            var buttonHeight = settings.GetSetting(GlobalConstants.ShowDrawingRulesButtonSizeKey, 22);
            var maxHeight = GUILayout.MaxHeight(buttonHeight);
            var minHeight = GUILayout.MinHeight(buttonHeight);

            // check if showing the drawing rules
            if (this.showDrawingRules)
            {
                GUILayout.BeginHorizontal();  // drawing rules buttons

                // provide an add button
                if (GUILayout.Button(local.Get("Add"), minHeight, maxHeight))
                {
                    this.AddDrawingRule();
                }

                // allow spacing between controls
                GUILayout.FlexibleSpace();

                // draw the drawing rules toggle button
                this.showDrawingRules = GUILayout.Toggle(this.showDrawingRules, local.Get("DrawingRules"), GUI.skin.button, minHeight, maxHeight);

                GUILayout.EndHorizontal();  // drawing rules buttons
            }
            else
            {
                this.showDrawingRules = GUILayout.Toggle(this.showDrawingRules, local.Get("DrawingRules"), GUI.skin.button, minHeight, maxHeight);
            }

            // if showing drawing rules show the list
            if (this.showDrawingRules)
            {
                var entryHeight = settings.GetSetting(GlobalConstants.DrawingRulesEntrySizeKey, 22);
                var maxWidth = GUILayout.MaxWidth(entryHeight);
                maxHeight = GUILayout.MaxHeight(entryHeight);
                var removedKeys = new Stack<string>();

                // draw each entry
                foreach (var pair in this.drawingRules)
                {
                    // get key name and entry
                    var model = this.drawingRules[pair.Key];

                    GUILayout.BeginHorizontal(); // drawing rule entry

                    // provide a button for editing the rules file
                    if (GUILayout.Button(this.toolButtonContent, GUI.skin.label, GUILayout.ExpandWidth(false), maxWidth, maxHeight))
                    {
                        this.EditDrawingRule(pair.Key);
                    }

                    // provide a toggle button for toggling the rule on or off
                    model.IsChecked = GUILayout.Toggle(model.IsChecked, Path.GetFileNameWithoutExtension(pair.Key), GUI.skin.button, maxHeight);

                    // provide a delete button
                    if (GUILayout.Button(this.deleteRuleContent, GUI.skin.label, GUILayout.ExpandWidth(false), maxWidth, maxHeight))
                    {
                        // remove the rule
                        removedKeys.Push(pair.Key);
                    }

                    GUILayout.EndHorizontal();   // drawing rule entry
                }

                // remove rules that the user removed
                while (removedKeys.Count > 0)
                {
                    var file = removedKeys.Pop();
                    this.drawingRules.Remove(file);
                    this.map.DrawingRules.Remove(file);
                }
            }

            GUILayout.EndVertical(); // main wrapper
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
            this.deleteRuleContent = null;
        }

        /// <summary>
        /// Called when the tool is set as the active tool.
        /// </summary>
        /// <param name="mapEditor">A reference to the editor that this tool should work against.</param>
        public void Startup(GridMapEditor mapEditor)
        {
            if (this.isActive)
            {
                return;
            }

            this.map = (GridMap)mapEditor.target;
            this.editor = mapEditor;

            // load drawing rules
            this.drawingRules.Clear();
            foreach (var file in this.map.DrawingRules)
            {
                var rules = DrawingRulesEditor.LoadRulesFromFile(file, true);
                this.drawingRules.Add(
                    file,
                    new DrawingRuleListItem
                        {
                            LastLoaded = DateTime.Now,
                            IsChecked = true,
                            Rules = rules
                        });
            }

            var settings = SettingsManager.Instance;
            var texturesPath = Path.Combine(settings.GetSetting(CoreGlobalConstants.ResourceFolderKey, "Codefarts.Unity"), "Textures");
            texturesPath = texturesPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            var getPath = new Func<string, string>(x => Path.Combine(texturesPath, x).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

            this.deleteRuleContent = new GUIContent(Resources.Load(getPath("Delete"), typeof(Texture2D)) as Texture2D);

            this.isActive = true;
            this.editor.MarkerHasStartPosition = false;

            Tools.current = Tool.View;
            Tools.viewTool = ViewTool.FPS;
        }

        /// <summary>
        /// Called by <see cref="GridMapEditor"/> to update the tool on every call to OnSceneGUI.
        /// </summary>
        public void Update()
        {
            if (!this.isActive || !this.editor.IsMouseOverLayer())
            {
                return;
            }

            var current = Event.current;
            var settings = SettingsManager.Instance;

            if (this.isMouseDown && current.type == EventType.MouseUp)
            {
                this.isMouseDown = false;

                // we have stopped drawing so clean up objects that were replaced
                while (this.gameObjectsToRemove.Count > 0)
                {
                    Undo.DestroyObjectImmediate(this.gameObjectsToRemove[0]);
                    this.gameObjectsToRemove.RemoveAt(0);
                }

                // we can return as we have handled a mouse up event
                return;
            }

            // check if mouse is down or dragging and if so draw
            if ((!this.isMouseDown && current.type == EventType.MouseDown) || (this.isMouseDown && current.type == EventType.MouseDrag))
            {
                this.isMouseDown = true;

                // if middle mouse button just exit the user is trying to pan the camera
                if (current.button == 2)
                {
                    return;
                }

                if (current.type == EventType.MouseDown)
                {
                    this.drawRecord = new GenericImage<Color>(this.map.Columns, this.map.Rows);
                    this.gameObjectsToRemove.Clear();
                }

                // determine based on mouse button if we are drawing or erasing
                var previousState = this.isDrawing;
                this.isDrawing = current.button == 0;
                this.isDrawing = current.button == 1 ? false : this.isDrawing;
                if (previousState != this.isDrawing)
                {
                    this.lastPosition = new Point(-1, -1);
                }

                // get settings relating to drawing or erasing
                var holdShift = settings.GetSetting(this.isDrawing ? GlobalConstants.HoldShiftToDrawKey : GlobalConstants.HoldShiftToEraseKey, false);
                var holdAlt = settings.GetSetting(this.isDrawing ? GlobalConstants.HoldAltToDrawKey : GlobalConstants.HoldAltToEraseKey, false);
                var holdControl = settings.GetSetting(this.isDrawing ? GlobalConstants.HoldControlToDrawKey : GlobalConstants.HoldControlToEraseKey, false);

                // check settings and see of we can draw or erase. A value equal to 0 means it passed the test can occur
                var canDo = 0;
                canDo += holdShift && !current.shift ? 1 : 0;
                canDo += holdAlt && !current.alt ? 1 : 0;
                canDo += holdControl && !current.control ? 1 : 0;
                canDo += !holdAlt && current.alt ? 1 : 0; // maintains v1 behavior
                if (canDo == 0)
                {
                    if (this.isDrawing)
                    {
                        this.DrawThePrefab();
                    }
                    else
                    {
                        this.EraseThePrefab();
                    }

                    current.Use();
                }
            }
        }

        /// <summary>
        /// Adds a new drawing rule(s) by allowing the user to select a xml rule file.
        /// </summary>
        private void AddDrawingRule()
        {
            // get reference to localization manager
            var local = LocalizationManager.Instance;

            // prompt user to select a sync file
            var file = EditorUtility.OpenFilePanel(local.Get("SelectRuleFile"), string.Empty, "xml");
            if (file.Length == 0)
            {
                return;
            }

            // enforce directory separation characters (no alt chars)
            file = file.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            // check if already added rule
            if (this.drawingRules.ContainsKey(file))
            {
                EditorUtility.DisplayDialog(local.Get("Information"), local.Get("RuleSetAddedAlready"), local.Get("Close"));
                return;
            }

            // try to load rules from the file
            var rules = DrawingRulesEditor.LoadRulesFromFile(file, true);

            // if returned a result add the rules to the list
            if (rules != null)
            {
                this.drawingRules.Add(file, new DrawingRuleListItem { IsChecked = true, Rules = rules });
                this.map.DrawingRules.Add(file);
            }
        }

        /// <summary>
        /// Applies drawing rules to the prefab.
        /// </summary>
        /// <param name="prefab">The prefab to apply the drawing rules against.</param>
        /// <param name="gridPosition">The grid position where the prefab will be drawn at.</param>
        /// <param name="layer">The destination layer where the prefab will be drawn to.</param>
        /// <returns>Returns the original prefab or a new prefab that should be drawn.</returns>
        private GameObject ApplyDrawingRules(GameObject prefab, Point gridPosition, int layer)
        {
            // get reference to localization manager
            var local = LocalizationManager.Instance;

            var prefabToReturn = prefab;
            var random = new Random((int)DateTime.Now.Ticks);
            var settings = SettingsManager.Instance;
            var consoleOutput = settings.GetSetting(GlobalConstants.DrawingRulesOutputToConsoleKey, false);
            var consoleText = string.Empty;

            // for each enabled rule
            foreach (var pair in this.drawingRules)
            {
                if (!pair.Value.IsChecked)
                {
                    continue;
                }

                // check if xml rule file has changed since last read
                if (File.GetLastWriteTime(pair.Key) > pair.Value.LastLoaded)
                {
                    consoleText += local.Get("LoadingUpdatedRulesFromDisk") + "\r\n";

                    // reload the rules
                    var rules = DrawingRulesEditor.LoadRulesFromFile(pair.Key, false);

                    // if returned null there may have been an error reading the file
                    if (rules == null)
                    {
                        return null;
                    }

                    pair.Value.Rules = rules;
                    pair.Value.LastLoaded = DateTime.Now;
                }

                foreach (var rule in pair.Value.Rules)
                {
                    consoleText += string.Format(local.Get("PerformingRuleCheck") + "\r\n", rule.Name);

                    // skip if rule not enabled
                    if (!rule.Enabled)
                    {
                        consoleText += string.Format(local.Get("InitialRuleCheckFailedNotEnabled") + "\r\n", rule.Name);
                        continue;
                    }

                    // skip if there are no alternates and not allowed to use original
                    if (rule.Alternates == null || (rule.Alternates.Count == 0 && !rule.AllowOriginal))
                    {
                        consoleText += string.Format(local.Get("InitialRuleCheckFailedOriginalNotAllowed") + "\r\n", rule.Name);
                        continue;
                    }

                    // check if prefabs have a reference and there asset paths match
                    var sourceRulePrefab = GridMapping.Helpers.GetSourcePrefab(rule.Prefab);
                    var sourcePrefab = GridMapping.Helpers.GetSourcePrefab(prefab);
                    if (sourceRulePrefab != sourcePrefab)
                    {
                        var text = string.Format(local.Get("InitialRuleCheckFailedAssetPaths"), rule.Name);
                        text += "\r\n" + local.Get("RuleSource") + sourceRulePrefab;
                        text += "\r\n" + local.Get("PrefabSource") + sourcePrefab;
                        consoleText += text + "\r\n";
                        continue;
                    }

                    consoleText += local.Get("InitialCheckPassed") + "\r\n";

                    // callback for checking the existence of a prefab at a given grid location
                    var callback = new Func<int, int, int, GameObject>((column, row, layerIndex) => this.editor.GameObjects.Get(column, row, layerIndex));

                    // check if presence of neighbors match
                    var lower = !rule.NeighborsUpperEnabled || Helpers.CheckNeighborsPassed(gridPosition, layer - 1, false, rule, callback);
                    var upper = !rule.NeighborsUpperEnabled || Helpers.CheckNeighborsPassed(gridPosition, layer + 1, false, rule, callback);
                    var passed = Helpers.CheckNeighborsPassed(gridPosition, layer, true, rule, callback);

                    consoleText +=
                        string.Format(
                            local.Get("RulePassed") + "\r\n",
                            rule.Name,
                            passed & lower & upper,
                            passed,
                            lower,
                            upper);

                    // check if passed the neighbor tests
                    if (passed && lower && upper)
                    {
                        if ((rule.Alternates == null || rule.Alternates.Count == 0) && !rule.AllowOriginal)
                        {
                            if (consoleOutput)
                            {
                                consoleText += string.Format(local.Get("ReturningOriginal") + "\r\n", prefabToReturn.name);
                                Debug.Log(consoleText);
                            }

                            return prefabToReturn;
                        }

                        // select from a random alternative prefab
                        var index = random.Next(rule.Alternates.Count + (rule.AllowOriginal ? 1 : 0));

                        // return the alternate prefab 
                        var gameObject = index > rule.Alternates.Count - 1 ? prefabToReturn : rule.Alternates[index].Prefab;
                        if (gameObject == null)
                        {
                            if (consoleOutput)
                            {
                                consoleText += local.Get("ReturningNullResponseNoPrefabToReturn") + "\r\n";
                                Debug.Log(consoleText);
                            }

                            return null;
                        }

                        prefabToReturn = GridMapping.Helpers.PerformInstantiation(gameObject);
                        var prefabType = PrefabUtility.GetPrefabType(gameObject);
                        consoleText += string.Format(local.Get("ReturningAlternatePrefab") + "\r\n", index, gameObject.name, prefabType);
                        if (consoleOutput && !string.IsNullOrEmpty(consoleText.Trim()))
                        {
                            Debug.Log(consoleText);
                        }

                        return prefabToReturn;
                    }
                }
            }

            if (consoleOutput && !string.IsNullOrEmpty(consoleText.Trim()))
            {
                Debug.Log(consoleText);
            }

            return prefabToReturn;
        }

        /// <summary>
        /// Draws a prefab at the pre-calculated mouse hit position                         
        /// </summary>
        private void DrawThePrefab()
        {
#if PERFORMANCE
            var perf = PerformanceTesting<PerformanceID>.Instance;
            perf.Start(PerformanceID.Draw);
#endif

            // do not draw if not the view tool
            if (Tools.current != Tool.View)
            {
#if PERFORMANCE
                perf.Stop(PerformanceID.Draw);
#endif
                return;
            }

            // checked if layer locked or not visible
            var layerModel = this.map.Layers[this.editor.ActiveLayer];
            if (layerModel.Locked || !layerModel.Visible || !this.isDrawing)
            {
#if PERFORMANCE
                perf.Stop(PerformanceID.Draw);
#endif
                return;
            }

            // Calculate the position of the mouse over the grid layer
            var gridPosition = this.editor.GetMouseGridPosition();

            // if grid position is the same as the last just exit
            if (gridPosition == this.lastPosition || this.drawRecord[gridPosition.X, gridPosition.Y] == Color.White)
            {
#if PERFORMANCE
                perf.Stop(PerformanceID.Draw);
#endif
                return;
            }

            // record last position
            this.lastPosition = gridPosition;

            // create the prefab
            var prefab = Helpers.CreatePrimitivePrefab(this.editor);
            Undo.RegisterCreatedObjectUndo(prefab, prefab.name);

            // apply drawing rules to the prefab
            var rulePrefab = this.ApplyDrawingRules(prefab, gridPosition, this.editor.ActiveLayer);

            // get grid mapping service
            var gridMappingService = GridMappingService.Instance;

            // if rulePrefab is null we check if prefab is null and destroy it if necessary
            GameObject existing;
            if (rulePrefab == null)
            {
                if (prefab != null)
                {
                    UnityEngine.Object.DestroyImmediate(prefab);
                }

                // destroy existing prefab
                existing = this.editor.GameObjects.Get(gridPosition.X, gridPosition.Y, this.editor.ActiveLayer);
                if (existing != null)
                {
                    Undo.DestroyObjectImmediate(prefab);
                }

                // make sure to erase before exiting
                this.editor.GameObjects.Set(gridPosition.X, gridPosition.Y, this.editor.ActiveLayer, null);

                // records the erase action in the draw record
                this.drawRecord[gridPosition.X, gridPosition.Y] = Color.White;

                // we can now exit
#if PERFORMANCE
                perf.Stop(PerformanceID.Draw);
#endif
                return;
            }

            // get reference to the settings manager
            var settings = SettingsManager.Instance;

            // check if original prefab and returned rulePrefab are not the same reference
            if (prefab != null && rulePrefab.GetInstanceID() != prefab.GetInstanceID())
            {
                // check if users wants output rule information to console
                if (settings.GetSetting(GlobalConstants.DrawingRulesOutputToConsoleKey, false))
                {
                    Debug.Log(LocalizationManager.Instance.Get("DestroyingOriginallyCreatedPrefab"));
                }

                // destroy originally created prefab
                UnityEngine.Object.DestroyImmediate(prefab);
            }

            // assign prefab the reference to rulePrefab
            prefab = rulePrefab;

            // make sure to erase any existing prefab before drawing any new prefab
            existing = this.editor.GameObjects.Get(gridPosition.X, gridPosition.Y, this.editor.ActiveLayer);

            if (existing != null)
            {
                Undo.DestroyObjectImmediate(existing);
            }

            var nameFormat = settings.GetSetting(GlobalConstants.PrefabNameFormatKey, "{0}_l{1}_c{2}_r{3}");

            // give the prefab a name that represents it's location within the grid map
            prefab.name = string.Format(nameFormat, prefab.name, this.editor.ActiveLayer, gridPosition.X, gridPosition.Y);

            // set local rotation first so the GetBoundWithChildren calculates properly
            var transform = prefab.transform;
            transform.localEulerAngles = new Vector3(
                this.editor.GetSelectedRotationValue(this.editor.SelectedXRotationIndex),
                this.editor.GetSelectedRotationValue(this.editor.SelectedYRotationIndex),
                this.editor.GetSelectedRotationValue(this.editor.SelectedZRotationIndex));

            // position the prefab on the map
            this.map.PositionPrefabOnMap(this.editor.ActiveLayer, gridPosition.X, gridPosition.Y, prefab, this.editor.AutoCenterPrefab, this.editor.AutoScalePrefab);

            // update game object tracker
            this.editor.GameObjects.Set(gridPosition.X, gridPosition.Y, this.editor.ActiveLayer, prefab);

            // determine whether to hide the prefab in the hierarchy based on settings
            if (!SettingsManager.Instance.GetSetting(GlobalConstants.ShowPrefabsInHierarchyKey, true))
            {
                prefab.hideFlags |= HideFlags.HideInHierarchy;
                EditorApplication.RepaintHierarchyWindow();
            }

            EditorUtility.SetDirty(prefab);

            // get grid mapping service
            gridMappingService.OnPrefabDrawn(this.map, gridPosition.X, gridPosition.Y, this.editor.ActiveLayer, prefab);

#if PERFORMANCE
            perf.Stop(PerformanceID.Draw);
#endif
        }

        /// <summary>
        /// Shows the <see cref="DrawingRulesEditor"/> window and load the rules file for editing.
        /// </summary>
        /// <param name="filename">The xml rule file to be edited.</param>
        private void EditDrawingRule(string filename)
        {
            var window = DrawingRulesEditor.ShowAutoMaterialCreationWindow();
            window.SetFilename(filename);
            window.LoadData(false);
        }

        /// <summary>
        /// Erases a block at the pre-calculated mouse hit position
        /// </summary>
        private void EraseThePrefab()
        {
#if PERFORMANCE
            var perf = PerformanceTesting<PerformanceID>.Instance;
            perf.Start(PerformanceID.Erase);
#endif
            // checked if layer locked or not visible
            var layerModel = this.map.Layers[this.editor.ActiveLayer];
            if (layerModel.Locked || !layerModel.Visible || this.isDrawing)
            {
#if PERFORMANCE
                perf.Stop(PerformanceID.Erase);
#endif
                return;
            }

            // Calculate the position of the mouse over the grid layer
            var gridPosition = this.editor.GetMouseGridPosition();

            // if grid position is the same as the last just exit
            if (gridPosition == this.lastPosition)
            {
#if PERFORMANCE
                perf.Stop(PerformanceID.Erase);
#endif
                return;
            }

            // record last position
            this.lastPosition = gridPosition;

            // Given the grid position check to see if a prefab has already been created at that location
            var prefab = this.editor.GameObjects.Get(gridPosition.X, gridPosition.Y, this.editor.ActiveLayer);
            this.editor.GameObjects.Set(gridPosition.X, gridPosition.Y, this.editor.ActiveLayer, null);

            // if a game object was found with the same name and it is a child we just destroy it immediately
            if (prefab != null && prefab.transform.parent == this.map.transform)
            {
                // delete right away  
                Undo.DestroyObjectImmediate(prefab);
            }

            // get grid mapping service
            var gridMappingService = GridMappingService.Instance;
            gridMappingService.OnPrefabDrawn(this.map, gridPosition.X, gridPosition.Y, this.editor.ActiveLayer, null);

#if PERFORMANCE
            perf.Stop(PerformanceID.Erase);
#endif
        }
    }
}
