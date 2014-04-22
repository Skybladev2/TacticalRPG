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
    using System.IO;

    using Codefarts.CoreProjectCode;
    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.Localization;

    using UnityEditor;

    using UnityEngine;

    using Helpers = Codefarts.GridMapping.Helpers;

    /// <summary>
    /// The prefab picker tool used to pick a prefab and make it the current drawing prefab.
    /// </summary>
    public class PrefabPicker : IEditorTool<GridMapEditor>
    {
        /// <summary>
        /// Holds a reference to the <see cref="GridMapEditor"/> that this tool is associated with.
        /// </summary>
        private GridMapEditor editor;

        /// <summary>
        /// Used to determine if the tool is active.
        /// </summary>
        private bool isActive;

        /// <summary>
        /// Gets a value representing the content to be used for the tools button.
        /// </summary>
        public GUIContent ButtonContent
        {
            get
            {
                var settings = SettingsManager.Instance;
                var texturesPath = Path.Combine(settings.GetSetting(CoreGlobalConstants.ResourceFolderKey, "Codefarts.Unity"), "Textures");
                texturesPath = texturesPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                var getPath = new Func<string, string>(x => Path.Combine(texturesPath, x).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                return new GUIContent(Resources.Load(getPath("Eyedropper"), typeof(Texture2D)) as Texture2D, this.Title);
            }
        }

        /// <summary>
        /// Gets a value representing the title or name of the tool.
        /// </summary>
        public string Title
        {
            get
            {
                return LocalizationManager.Instance.Get("PrefabPicker");
            }
        }

        /// <summary>
        /// Gets a <see cref="Guid"/> value that uniquely identifies the tool.
        /// </summary>
        public Guid Uid
        {
            get
            {
                return new Guid("B6C7367A-A064-4C5A-BBA0-99A1300585AB");
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
            // does nothing
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
            if (this.editor != null)
            {
                this.editor.MarkerHasStartPosition = false;
            }

            this.editor = null;
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

            this.editor = editor;
            this.editor.MarkerHasStartPosition = false;
            this.isActive = true;
        }

        /// <summary>
        /// Called by <see cref="GridMapEditor"/> to update the tool on every call to OnSceneGUI.
        /// </summary>
        public void Update()
        {
            // do not pick if not the view tool or is not active
            if (!this.isActive || Tools.current != Tool.View)
            {
                return;
            }

            var current = Event.current;

            // check if mouse is down or dragging and if so draw
            if (current.type == EventType.MouseUp && current.button == 0)
            {
                // eat the event
                current.Use();

                // Calculate the position of the mouse over the grid layer
                var gridPosition = this.editor.GetMouseGridPosition();

                var item = this.editor.GameObjects.Get(gridPosition.X, gridPosition.Y, this.editor.ActiveLayer);

                var sourcePrefab = Helpers.GetSourcePrefab(item);
                var instance = GridMappingService.Instance;
                if (!string.IsNullOrEmpty(sourcePrefab) && File.Exists(sourcePrefab.Trim()))
                {
                    // load a reference to the source prefab
                    var temp = AssetDatabase.LoadAssetAtPath(sourcePrefab, typeof(GameObject)) as GameObject;
                    if (item != null && item.renderer != null)
                    {
                        instance.CurrentMaterial = item.renderer.sharedMaterial;
                    }

                    item = temp;
                }

                // set current prefab
                instance.CurrentPrefab = item;
                this.editor.SetPrefabSelectionToCustom();

                this.editor.Repaint();
            }
        }
    }
}
