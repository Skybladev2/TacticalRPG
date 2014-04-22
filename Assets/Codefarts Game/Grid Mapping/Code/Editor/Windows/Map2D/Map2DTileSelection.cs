/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.Map2D.Editor.Windows
{
    using System;
    using System.Globalization;

    using Codefarts.GridMapping.Editor;
    using Codefarts.GridMapping.Editor.Controls;
    using Codefarts.GridMapping.Utilities;
    using Codefarts.Localization;

    using UnityEditor;

    using UnityEngine;

     /// <summary>
    /// Provides a tile selection window.
    /// </summary>
    public class Map2DTileSelection : EditorWindow
    {
        /// <summary>
        /// Holds a reference to a control used to draw the main preview area.
        /// </summary>
        private readonly TileSelectionControl mainPreview;

        /// <summary>
        /// Holds a reference to a control used to draw the selection controls.
        /// </summary>
        private readonly GenericMaterialCreationControl materialControls;

        /// <summary>
        /// Initializes a new instance of the <see cref="Map2DTileSelection"/> class.
        /// </summary>
        public Map2DTileSelection()
        {
            // setup controls and variables
            this.materialControls = new GenericMaterialCreationControl();
            this.mainPreview = new TileSelectionControl();
            this.materialControls.ShowFreeform = false;
            this.materialControls.ShowColor = false;
            this.materialControls.ShowShader = false;
            this.mainPreview.MultipleTileSelection = false;
            this.mainPreview.ShowHoverRectangle = false;

            // hook into the before spacing drawn event to allow us to customize the look
            this.materialControls.BeforeSpacingDrawn = () => GUILayout.BeginVertical();

            // hook into the after spacing drawn event to allow us to customize the look
            this.materialControls.AfterSpacingDrawn = () =>
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

                    var items = new bool[4];
                    items[this.mainPreview.Zoom - 1] = true;
                    ControlGrid.DrawGenericGrid(
                        (data, index, style, options) =>
                        {
                            var result = GUILayout.Toggle(data[index], (index + 1).ToString(CultureInfo.InvariantCulture), style, options);
                            if (result != data[index])
                            {
                                this.mainPreview.Zoom = index + 1;
                            }

                            return result;
                        },
                        items,
                        items.Length,
                        GUI.skin.button);

                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                };

            // hook into the tile selection event on the main preview control
            this.mainPreview.TileSelection += this.MainPreviewTileSelection;

            // hook into the main preview texture changed so we can generate a generic image for the selected texture
            this.mainPreview.TextureChanged += this.MainPreviewTextureChanged;

            // Hook into the texture changed event and update the texture and selected texture id's
            this.materialControls.TextureChanged += (s, e) =>
            {
                this.mainPreview.TextureAsset = this.materialControls.TextureAsset;
            };

            // ensure preview texture size is updated
            this.materialControls.TileSizeChanged += (s, e) =>
            {
                this.mainPreview.TileHeight = this.materialControls.TileHeight;
                this.mainPreview.TileWidth = this.materialControls.TileWidth;
            };

            // hook into events to sync control properties
            this.materialControls.StartSpacingChanged += (s, e) => { this.mainPreview.StartSpacing = this.materialControls.StartSpacing; };
            this.materialControls.SpacingChanged += (s, e) => { this.mainPreview.Spacing = this.materialControls.Spacing; };
            this.materialControls.FreeformChanged += (s, e) => { this.mainPreview.FreeForm = this.materialControls.FreeForm; };

            // sync control properties
            this.mainPreview.Refresh += (s, e) => this.Repaint();
            this.mainPreview.StartSpacing = this.materialControls.StartSpacing;
            this.mainPreview.Spacing = this.materialControls.Spacing;
            this.mainPreview.FreeForm = this.materialControls.FreeForm;
        }

         private void MainPreviewTextureChanged(object sender, EventArgs e)
         {
             
         }

         /// <summary>
        /// Gets or sets the <see cref="GenericImage{T}"/> that contains the tile selection image.
        /// </summary>
        public GenericImage<Color> SelectedTile { get; set; }

        /// <summary>
        /// Shows the <see cref="Map2DTileSelection"/> window.
        /// </summary>
        [MenuItem("Window/Codefarts/Map2D/*BETA* 2D Map Tile Selection")]
        public static void ShowWindow()
        {
            var local = LocalizationManager.Instance;
            var window = GetWindow<Map2DTileSelection>(local.Get("Map2DTiles"));
            window.Show();
        }

        /// <summary>
        /// Called by unity for drawing the graphical user interface.
        /// </summary>
        public void OnGUI()
        {
            try
            {
                GUILayout.BeginVertical();
                this.materialControls.Draw();

                this.mainPreview.Draw();
                GUILayout.EndVertical();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// Handles the event for tile selection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        private void MainPreviewTileSelection(object sender, TileSelectionEventArgs e)
        {
            // do nothing until selection has completed or if no texture selected 
            if (e.Status != TileSelectionStatus.Complete || this.materialControls.TextureAsset == null)
            {
                return;
            }

            var image = new GenericImage<Color>(this.mainPreview.TileWidth, this.mainPreview.TileHeight);
            image.Draw(
                this.materialControls.TextureAsset.CreateGenericImage(),
                0,
                0,
                e.Min.X,
                e.Min.Y,
                this.mainPreview.TileWidth,
                this.mainPreview.TileHeight,
                (source, blendWith) => blendWith);

            this.SelectedTile = image;
        }
    }
}
