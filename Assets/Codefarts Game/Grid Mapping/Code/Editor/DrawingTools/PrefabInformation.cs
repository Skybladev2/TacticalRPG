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
    /// The prefab information tool.
    /// </summary>
    public class PrefabInformation : IEditorTool<GridMapEditor>
    {
        /// <summary>
        /// Holds a reference to the editor associated with the map.
        /// </summary>
        private GridMapEditor editor;

        /// <summary>
        /// Holds a value to indicate weather or not the tool is the active tool.
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
                return new GUIContent(Resources.Load(getPath("PrefabInformation"), typeof(Texture2D)) as Texture2D, this.Title);
            }
        }

        /// <summary>
        /// Gets a value representing the title or name of the tool.
        /// </summary>
        public string Title
        {
            get
            {
                return LocalizationManager.Instance.Get("PrefabInformation");
            }
        }

        /// <summary>
        /// Gets a <see cref="Guid"/> value that uniquely identifies the tool.
        /// </summary>
        public Guid Uid
        {
            get
            {
                return new Guid("1FCCF81F-A440-4068-BCD3-BB226D98396F");
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
            if (!this.isActive)
            {
                return;
            }

            // do not draw if not the view tool
            if (Tools.current != Tool.View)
            {
                return;
            }

            var current = Event.current;

            // check if mouse is up and button is 0 and if so exit
            if (!(current.type == EventType.MouseUp & current.button == 0))
            {
                return;
            }

            current.Use();

            // Calculate the position of the mouse over the grid layer
            var gridPosition = this.editor.GetMouseGridPosition();

            var item = this.editor.GameObjects.Get(gridPosition.X, gridPosition.Y, this.editor.ActiveLayer);
            if (item == null)
            {
                return;
            }

            // generate output data
            var data = string.Format(
                "Name: \"{0}\"\r\n" +
                "PrefabType: {1}\r\n" +
                "Source Path: \"{2}\"\r\n" +
                "Shared Material: \"{3}\"\r\n" +
                "Material: \"{4}\"\r\n",
                item.name,
                Enum.GetName(typeof(PrefabType), PrefabUtility.GetPrefabType(item)),
                Helpers.GetSourcePrefab(item),
                item.renderer != null && item.renderer.sharedMaterial != null ? item.renderer.sharedMaterial.name : string.Empty,
                item.renderer != null && item.renderer.material != null ? item.renderer.material.name : string.Empty);
            Debug.Log(data);
        }
    }
}