/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.GridMapping.Editor.DrawingTools.Map2D
{
    using System;

    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.GridMapping.Utilities;
    using Codefarts.Localization;
    using Codefarts.Map2D.Editor;
    using Codefarts.Map2D.Editor.Interfaces;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Provides a tile drawing tool for Map2D.
    /// </summary>
    public class ToggleCheckerboardBackground : IEditorTool<IMapEditor>
    {
        /// <summary>
        /// Holds a <see cref="GenericImage{T}"/> reference of the <see cref="checkerboardTexture"/> field.
        /// </summary>
        private GenericImage<Common.Color> checkerboardImage;

        /// <summary>
        /// Holds a reference to a checkerboard texture used as the map background.
        /// </summary>
        private Texture2D checkerboardTexture;

        /// <summary>
        /// Holds the content for the tool button.
        /// </summary>
        private GUIContent content;

        /// <summary>
        /// Holds a reference to the editor that this tool is associated with.
        /// </summary>
        private IMapEditor editor;

        /// <summary>
        /// Used to determine if the tool is active.
        /// </summary>
        private bool isActive;

        /// <summary>
        /// Holds the state of the tool button.
        /// </summary>
        private bool state;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleCheckerboardBackground"/> class.
        /// </summary>
        public ToggleCheckerboardBackground()
        {
            var settings = SettingsManager.Instance;
            var service = Map2DService.Instance;
            this.state = settings.GetSetting(GlobalConstants.Map2DShowCheckerboardBackground, false);
            service.BeforeDrawingMap += this.DrawBackground;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ToggleCheckerboardBackground"/> class. 
        /// </summary>
        ~ToggleCheckerboardBackground()
        {
            this.OnDestroy();
        }

        /// <summary>
        /// Gets a value representing the content to be used for the tools button.
        /// </summary>
        public GUIContent ButtonContent
        {
            get
            {
                if (this.content == null)
                {
                    var image = new GenericImage<Common.Color>(64, 64);
                    image.Checkerboard(0, 0, image.Width, image.Height, Color.white, Color.black, 16);
                    this.content = new GUIContent(image.ToTexture2D(), this.Title);

                    this.checkerboardImage = new GenericImage<Common.Color>(256, 256);
                    this.checkerboardImage.Checkerboard(0, 0, this.checkerboardImage.Width, this.checkerboardImage.Height, Common.Color.White, Common.Color.Black, 16);
                    this.checkerboardTexture = this.checkerboardImage.ToTexture2D();
                }

                return this.content;
            }
        }

        /// <summary>
        /// Gets a value representing the title or name of the tool.
        /// </summary>
        public string Title
        {
            get
            {
                return LocalizationManager.Instance.Get("CheckerboardBackground");
            }
        }

        /// <summary>
        /// Gets a <see cref="Guid"/> value that uniquely identifies the tool.
        /// </summary>
        public Guid Uid
        {
            get
            {
                return new Guid("0315A955-F3D5-4AB4-B759-1FED16776EE9");
            }
        }

        /// <summary>
        /// Called by the editor to notify the tool to draw something.
        /// </summary>
        public void Draw()
        {
        }

        /// <summary>
        /// Called by the editor to allow the tool to draw it self on the tool bar.
        /// </summary>
        /// <returns>If true the tool has drawn it self and no further drawing is necessary, otherwise false.</returns>
        public bool DrawTool()
        {
            var settings = SettingsManager.Instance;
            var size = settings.GetSetting(GlobalConstants.Map2DDrawingModeButtonSizeKey, 64.0f);

            var value = GUILayout.Toggle(this.state, this.ButtonContent, GUI.skin.button, GUILayout.Width(size), GUILayout.Height(size));
            if (value != this.state)
            {
            }

            this.state = value;
            return true;
        }

        /// <summary>
        /// OnDestroy is called when the EditorWindow is closed.
        /// </summary>
        public void OnDestroy()
        {
            var service = Map2DService.Instance;
            service.BeforeDrawingMap -= this.DrawBackground;
        }
    
        /// <summary>
        /// Called by the editor to give the tool the ability to include additional tools in the inspector.
        /// </summary>
        public void OnInspectorGUI()
        {
            // does nothing
        }

        /// <summary>
        /// Method will be called when the tool is no longer of use by the system or a new tool is being selected.
        /// </summary>
        public void Shutdown()
        {
            this.isActive = false;
            if (this.editor != null)
            {
            }

            this.editor = null;
            this.OnDestroy();
        }

        /// <summary>
        /// Called when the tool is set as the active tool.
        /// </summary>
        /// <param name="mapEditor">A reference to the editor that this tool should work against.</param>
        public void Startup(IMapEditor mapEditor)
        {
            if (this.isActive)
            {
                return;
            }

            this.editor = mapEditor;
            this.isActive = true;
        }

        /// <summary>
        /// Called by the editor to update the tool on every call to OnSceneGUI.
        /// </summary>
        public void Update()
        {
        }

        /// <summary>
        /// Used to draw the checkerboard background.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        private void DrawBackground(object sender, EventArgs e)
        {
            if (!this.state)
            {
                return;
            }

            var window = (EditorWindow)sender;
            for (var y = 0; y < window.position.height; y += this.checkerboardTexture.height)
            {
                for (var x = 0; x < window.position.width; x += this.checkerboardTexture.width)
                {
                    GUI.DrawTexture(new Rect(x + 4, y + 4, this.checkerboardTexture.width, this.checkerboardTexture.height), this.checkerboardTexture);
                }
            }
        }
    }
}