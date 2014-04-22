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

    using UnityEngine;

    /// <summary>
    /// Provides a tile drawing tool for Map2D.
    /// </summary>
    public class ToggleMapBorder : IEditorTool<IMapEditor>
    {
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
        /// Gets a value representing the content to be used for the tools button.
        /// </summary>
        public GUIContent ButtonContent
        {
            get
            {
                if (this.content == null)
                {
                    var image = new GenericImage<Common.Color>(64, 64);
                    image.DrawRectangle(0, 0, image.Width, image.Height, Color.black);
                    this.content = new GUIContent(image.ToTexture2D(), this.Title);
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
                return LocalizationManager.Instance.Get("MapBorder");
            }
        }

        /// <summary>
        /// Gets a <see cref="Guid"/> value that uniquely identifies the tool.
        /// </summary>
        public Guid Uid
        {
            get
            {
                return new Guid("CEF9C660-05E7-4879-A1B3-3BC3BB445682");
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
        /// Called by the editor to allow the tool to draw it self on the tool bar.
        /// </summary>
        /// <returns>If true the tool has drawn it self and no further drawing is necessary, otherwise false.</returns>
        public bool DrawTool()
        {
            var settings = SettingsManager.Instance;
            var size = settings.GetSetting(GlobalConstants.Map2DDrawingModeButtonSizeKey, 64.0f);

            var result = GUILayout.Toggle(this.state, this.ButtonContent, GUI.skin.button, GUILayout.Width(size), GUILayout.Height(size));
            if (result != this.state)
            {
                if (!this.state && result)
                {
                    // state enabled
                    Map2DService.Instance.AfterDrawingMap += this.DrawBorder;
                }

                if (this.state && !result)
                {
                    Map2DService.Instance.AfterDrawingMap -= this.DrawBorder;
                }

                // state change occurred
                this.state = result;
            }

            return true;
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
        /// Used to draw the map border.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        private void DrawBorder(object sender, EventArgs e)
        {
            var service = Map2DService.Instance;
            if (service.MapCount < 1)
            {
                return;
            }

            var map = service.GetActiveMap();
            if (map.Layers == null)
            {
                return;
            }

            if (map.ActiveLayer < 0)
            {
                return;
            }

            var layer = map.Layers[map.ActiveLayer];
            int width;
            int height;
            if (layer.Texture != null)
            {
                width = layer.Texture.width;
                height = layer.Texture.height;
            }
            else
            {
                if (layer.Image == null)
                {
                    return;
                }

                width = layer.Image.Width;
                height = layer.Image.Height;
            }

            var rectangle = new Rect(-1 + layer.Offset.x, -1 + layer.Offset.y, width + 2, height + 2);
            Helpers.DrawRect(rectangle, Color.red);
        }
    }
}