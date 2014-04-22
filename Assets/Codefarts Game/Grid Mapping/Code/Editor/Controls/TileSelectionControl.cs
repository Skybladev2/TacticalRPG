/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Editor.Controls
{
    using System;
    using System.Collections.Generic;

    using Codefarts.GridMapping.Common;
    using Codefarts.GridMapping.Utilities;

    using UnityEngine;

    using Color = Codefarts.GridMapping.Common.Color;
    using Vector2 = Codefarts.GridMapping.Common.Vector2;

    /// <summary>
    /// provides a control for selecting a tile for free form rectangle over a preview texture.
    /// </summary>
    public class TileSelectionControl
    {
        /// <summary>
        /// Holds a list of selected tile locations.
        /// </summary>
        private readonly List<Point> selectedTiles = new List<Point>();

        /// <summary>
        /// Stores the location of the first tile location where dragging began.
        /// </summary>
        private Point firstTileLocation;

        /// <summary>
        /// Used to determine if the user is dragging a selection rectangle.
        /// </summary>
        private bool isSelecting;

        /// <summary>
        /// Used to record when the last auto repaint occurred.
        /// </summary>
        private DateTime lastUpdateTime;

        /// <summary>
        /// Holds the scroll values for the main preview.
        /// </summary>
        private Vector2 mainPreviewScroll;

        /// <summary>
        /// Holds the maximum position of the selection rectangle (Bottom Right).
        /// </summary>
        private Vector2 max;

        /// <summary>
        /// Holds the minimum position of the selection rectangle (Top left).
        /// </summary>
        private Vector2 min;

        /// <summary>
        /// Used to hold the current state of the selection.
        /// </summary>
        private TileSelectionStatus selectionStatus;

        /// <summary>
        /// Holds a reference to a texture asset.
        /// </summary>
        private Texture2D textureAsset;

        /// <summary>
        /// Holds the value of the <see cref="Zoom"/> property.
        /// </summary>
        private int zoom = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileSelectionControl"/> class. 
        /// </summary>
        public TileSelectionControl()
        {
            this.TileWidth = 32;
            this.TileHeight = 32;
            this.Spacing = 1;
            this.Inset = 0.0005f;
            this.MultipleTileSelection = true;
            this.HoverRectangleColor = Color.Blue;
            this.SelectionRectangleColor = Color.Red;
            this.ShowHoverRectangle = true;
        }

        /// <summary>
        /// Provides an event that gets raised when free form selection is complete.
        /// </summary>
        public event EventHandler<FreeformSelectionEventArgs> FreeFormSelection;

        /// <summary>
        /// Provides an event that gets raised when a refresh is requested.
        /// </summary>
        public event EventHandler Refresh;

        /// <summary>
        /// Provides an event that gets raised when a different texture is selected.
        /// </summary>
        public event EventHandler TextureChanged;

        /// <summary>
        /// Provides an event that gets raised when tile selection is complete.
        /// </summary>
        public event EventHandler<TileSelectionEventArgs> TileSelection;

        /// <summary>
        /// Gets or sets a value indicating whether free form selection is being used.
        /// </summary>
        public bool FreeForm { get; set; }

        /// <summary>
        /// Gets or sets the color of the selection rectangle(s).
        /// </summary>
        public Color SelectionRectangleColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the hover rectangle.
        /// </summary>
        public UnityEngine.Color HoverRectangleColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not more then one tile can be selected.
        /// </summary>
        public bool MultipleTileSelection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the hover rectangle will be shown.
        /// </summary>
        public bool ShowHoverRectangle { get; set; }

        /// <summary>
        /// Gets or sets the zoom level.
        /// </summary>
        /// <remarks>Can not be less then 1.</remarks>
        public int Zoom
        {
            get
            {
                return this.zoom;
            }

            set
            {
                this.zoom = value < 1 ? 1 : value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the free form selection rectangle.
        /// </summary>
        public Rect FreeFormRectangle { get; set; }

        /// <summary>
        /// Gets or sets the value for spacing between tiles.
        /// </summary>
        public float Inset { get; set; }

        /// <summary>                  
        /// Gets the list of selected tile locations.
        /// </summary>
        public List<Point> SelectedTiles
        {
            get
            {
                return this.selectedTiles;
            }
        }

        /// <summary>
        /// Gets or sets the value for spacing between tiles.
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tile set starts with spaces.
        /// </summary>
        public bool StartSpacing { get; set; }

        /// <summary>
        /// Gets or sets a reference to the texture asset.
        /// </summary>
        public Texture2D TextureAsset
        {
            get
            {
                return this.textureAsset;
            }

            set
            {
                var changed = value != this.textureAsset;
                this.textureAsset = value;
                if (changed && this.TextureChanged != null)
                {
                    this.TextureChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the value for the tile height.
        /// </summary>
        public int TileHeight { get; set; }

        /// <summary>
        /// Gets or sets the value for the tile width.
        /// </summary>
        public int TileWidth { get; set; }

        /// <summary>
        /// Is used to draw the control.
        /// </summary>
        public void Draw()
        {
            // setup a scroll view so the user can scroll large textures
            this.mainPreviewScroll = GUILayout.BeginScrollView(this.mainPreviewScroll, false, false);

            // check if a texture asset is available
            if (this.textureAsset != null)
            {
                // we draw a label here with the same dimensions of the texture so scrolling will work
                var texture = this.textureAsset;
                GUILayout.Label(string.Empty, GUILayout.Width((texture.width * this.zoom) + 4), GUILayout.Height((texture.height * this.zoom) + 4));

                // draw the preview texture
                GUI.DrawTexture(new Rect(4, 4, texture.width * this.zoom, texture.height * this.zoom), texture, ScaleMode.StretchToFill, true, 1);

                // draw the selection rectangles
                this.DrawSelection(Event.current);
            }

            GUILayout.EndScrollView();
        }

        /// <summary>
        /// Raises the <see cref="FreeFormSelection" /> event.
        /// </summary>
        public void OnFreeFormSelection()
        {
            if (this.FreeFormSelection != null)
            {
                this.FreeFormSelection(this, new FreeformSelectionEventArgs { Status = this.selectionStatus, SelectionRectangle = this.FreeFormRectangle });
            }
        }

        /// <summary>
        /// Raises the <see cref="Refresh" /> event.
        /// </summary>
        public void OnRefresh()
        {
            if (this.Refresh != null)
            {
                this.Refresh(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="TileSelection" /> event.
        /// </summary>
        public void OnTileSelection()
        {
            if (this.TileSelection != null)
            {
                this.TileSelection(
                    this,
                    new TileSelectionEventArgs { Status = this.selectionStatus, TileLocations = this.selectedTiles, Min = this.min, Max = this.max });
            }
        }

        /// <summary>
        /// Selects tiles using selection rectangle.
        /// </summary>
        /// <param name="selectionRectangle">
        /// The selection rectangle in pixel co-ordinates.
        /// </param>
        public void SelectTiles(Rect selectionRectangle)
        {
            this.SelectTiles(new Vector2(selectionRectangle.xMin, selectionRectangle.yMin), new Vector2(selectionRectangle.xMax, selectionRectangle.yMax));
        }

        /// <summary>
        /// Selects tiles given two sets of points that define the min and max dimensions or a selection rectangle.
        /// </summary>
        /// <param name="minVector">
        /// The top left location in pixel co-ordinates of the selection rectangle.
        /// </param>
        /// <param name="maxVector">
        /// The bottom right location in pixel co-ordinates of the selection rectangle.
        /// </param>
        public void SelectTiles(Vector2 minVector, Vector2 maxVector)
        {
            // determine if the tile set starts with spacing
            var startOffset = this.StartSpacing ? this.Spacing : 0;

            Vector2 minTilePosition; // calculate min tile position;
            minTilePosition.X = (int)((minVector.X - startOffset) / (this.TileWidth + this.Spacing));
            minTilePosition.Y = (int)((minVector.Y - startOffset) / (this.TileHeight + this.Spacing));

            Vector2 maxTilePosition; // calculate max tile position;
            maxTilePosition.X = (int)((maxVector.X - startOffset) / (this.TileWidth + this.Spacing));
            maxTilePosition.Y = (int)((maxVector.Y - startOffset) / (this.TileHeight + this.Spacing));

            // clear any previous selection
            this.selectedTiles.Clear();
            this.firstTileLocation = minTilePosition;

            // add tile entries for the selection
            for (var idx = minTilePosition.X; idx < maxTilePosition.X; idx++)
            {
                for (var idy = minTilePosition.Y; idy < maxTilePosition.Y; idy++)
                {
                    this.selectedTiles.Add(new Vector2(idx, idy));
                }
            }
        }

        /// <summary>
        /// Used to draw the current free form selection rectangle.
        /// </summary>
        /// <param name="eventReference">
        /// A reference to the <see cref="Event"/>.
        /// </param>
        protected virtual void DrawFreeFormSelection(Event eventReference)
        {
            // get mouse position
            Point mousePosition = eventReference.mousePosition;

            // determine if user is selecting  
            if (this.isSelecting && eventReference.type == EventType.MouseDrag && eventReference.button == 0)
            {
                // this event has been used
                eventReference.Use();

                // clear any previous selection
                this.selectedTiles.Clear();

                // calculate the min and max positions based on the location of the mouse and the first position of mouse down
                var minVector = new Vector2(Math.Min(this.firstTileLocation.X, mousePosition.X), Math.Min(this.firstTileLocation.Y, mousePosition.Y)) / new Vector2(1f / this.zoom);
                var maxVector = new Vector2(Math.Max(this.firstTileLocation.X, mousePosition.X), Math.Max(this.firstTileLocation.Y, mousePosition.Y)) / new Vector2(1f / this.zoom);

                // set the free form rectangle
                this.FreeFormRectangle = new Rect(minVector.X, minVector.Y, maxVector.X - minVector.X, maxVector.Y - minVector.Y);

                // raise the selection changed event
                this.selectionStatus = TileSelectionStatus.Selecting;
                this.OnFreeFormSelection();
            }

            // draw the selected free form rectangle
            Helpers.DrawRect(
                new Rect(
                    this.FreeFormRectangle.x * this.zoom,
                    this.FreeFormRectangle.y * this.zoom,
                    this.FreeFormRectangle.width * this.zoom,
                    this.FreeFormRectangle.height * this.zoom),
                this.SelectionRectangleColor);

            // check if were beginning to select tiles with the left mouse button
            if (!this.isSelecting && eventReference.isMouse && eventReference.type == EventType.MouseDown && eventReference.button == 0)
            {
                this.isSelecting = true;
                this.firstTileLocation = mousePosition;

                // clear any previously selected tiles and add the first tile location
                this.selectedTiles.Clear();
                this.selectedTiles.Add(this.firstTileLocation);

                // raise the selection changed event
                this.selectionStatus = TileSelectionStatus.Begin;
                this.OnFreeFormSelection();

                // this event has been used
                eventReference.Use();
            }

            // check if were releasing left mouse button (IE stop selecting)
            if (this.isSelecting && eventReference.isMouse && eventReference.type == EventType.MouseUp && eventReference.button == 0)
            {
                // this event has been used
                eventReference.Use();

                this.isSelecting = false;
                this.selectionStatus = TileSelectionStatus.Complete;
                this.OnFreeFormSelection();
            }
        }

        /// <summary>
        /// Draw the selection rectangle in the main preview.
        /// </summary>
        /// <param name="eventReference">
        /// A reference to the <see cref="Event"/>.
        /// </param>
        protected virtual void DrawSelection(Event eventReference)
        {
            // draw selection rectangle depending on free form state
            if (this.FreeForm)
            {
                // free form state is true so allow the user to select a rectangle anywhere on the texture
                this.DrawFreeFormSelection(eventReference);
            }
            else
            {
                // free form selection state if false so act like we are selecting from a grid of tiles
                this.DrawTileSelection(eventReference);
            }

            // only repaint every so often to save cpu cycles
            if (DateTime.Now > this.lastUpdateTime + TimeSpan.FromMilliseconds(25))
            {
                // record the time that we are repainting
                this.lastUpdateTime = DateTime.Now;
                this.OnRefresh();
            }

            // check if the user has stopped selecting tiles
            if (eventReference.isMouse && eventReference.type == EventType.MouseUp && eventReference.button == 0)
            {
                this.isSelecting = false;
            }
        }

        /// <summary>
        /// Used to draw the currently selected tiles.
        /// </summary>
        /// <param name="eventReference">
        /// A reference to the <see cref="Event"/>.
        /// </param>
        protected virtual void DrawTileSelection(Event eventReference)
        {
            // if no event reference provided just exit
            if (eventReference == null)
            {
                return;
            }

            // used to store the top left tile coordinates of the tile position
            Point tilePosition;

            // determine if the tile set starts with spacing
            var startOffset = this.StartSpacing ? this.Spacing : 0;

            // convert the mouse position into a tile position
            var mousePosition = eventReference.mousePosition;

            tilePosition.X = (int)((mousePosition.x - startOffset) / ((this.TileWidth * this.Zoom) + this.Spacing));
            tilePosition.Y = (int)((mousePosition.y - startOffset) / ((this.TileHeight * this.Zoom) + this.Spacing));

            // calculate the min and max positions based on the location of the mouse and the first selected tile location
            this.min = new Vector2(Math.Min(this.firstTileLocation.X, tilePosition.X), Math.Min(this.firstTileLocation.Y, tilePosition.Y));
            if (this.MultipleTileSelection)
            {
                this.max = new Vector2(Math.Max(this.firstTileLocation.X, tilePosition.X), Math.Max(this.firstTileLocation.Y, tilePosition.Y));
            }
            else
            {
                this.max = this.min;
            }

            // cap the min max values to the dimensions of the texture
            this.min = this.CapValues(this.min);
            this.max = this.CapValues(this.max) + new Point(1, 1);

            // determine if user is selecting  
            if (this.isSelecting && eventReference.type == EventType.MouseDrag && eventReference.button == 0)
            {
                // this event has been used
                eventReference.Use();

                // clear any previous selection
                this.selectedTiles.Clear();

                // check if allowed to select multiple tiles
                if (this.MultipleTileSelection)
                {
                    // add tile entries for the selection
                    for (var idx = this.min.X; idx < this.max.X; idx++)
                    {
                        for (var idy = this.min.Y; idy < this.max.Y; idy++)
                        {
                            this.selectedTiles.Add(new Vector2(idx, idy));
                        }
                    }
                }
                else
                {
                    this.firstTileLocation = tilePosition;

                    // clear any previously selected tiles and add the first tile location
                    this.selectedTiles.Add(this.firstTileLocation);
                }

                // save last mouse position
                this.selectionStatus = TileSelectionStatus.Selecting;
                this.OnTileSelection();
            }

            // draw selected tiles
            float x;
            float y;
            foreach (var tile in this.selectedTiles)
            {
                // calculate rectangle Top Left for tile location
                x = startOffset + (tile.X * ((this.TileWidth * this.Zoom) + this.Spacing));
                y = startOffset + (tile.Y * ((this.TileHeight * this.Zoom) + this.Spacing));

                // draw the selected tile rectangle
                Helpers.DrawRect(
                    new Rect(
                        x,
                        y,
                        (this.TileWidth * this.Zoom) + this.Spacing - startOffset,
                        (this.TileHeight * this.Zoom) + this.Spacing - startOffset),
                    this.SelectionRectangleColor);
            }

            // draw blue rectangle indicating what tile the mouse is hovering over
            x = startOffset + (tilePosition.X * ((this.TileWidth * this.Zoom) + this.Spacing));
            y = startOffset + (tilePosition.Y * ((this.TileHeight * this.Zoom) + this.Spacing));

            if (this.ShowHoverRectangle)
            {
                Helpers.DrawRect(
                    new Rect(x, y, (this.TileWidth * this.Zoom) + this.Spacing - startOffset, (this.TileHeight * this.Zoom) + this.Spacing - startOffset),
                    this.HoverRectangleColor);
            }

            // check if were beginning to select tiles with the left mouse button
            if (!this.isSelecting && eventReference.isMouse && eventReference.type == EventType.MouseDown && eventReference.button == 0)
            {
                // this event has been used
                eventReference.Use();

                this.isSelecting = true;
                this.firstTileLocation = tilePosition;

                // clear any previously selected tiles and add the first tile location
                this.selectedTiles.Clear();
                this.selectedTiles.Add(this.firstTileLocation);

                // save last mouse position
                this.selectionStatus = TileSelectionStatus.Begin;
                this.OnTileSelection();
            }

            // check if were releasing left mouse button (IE stop selecting)
            if (this.isSelecting && eventReference.isMouse && eventReference.type == EventType.MouseUp && eventReference.button == 0)
            {
                // this event has been used
                eventReference.Use();

                this.isSelecting = false;
                this.selectionStatus = TileSelectionStatus.Complete;
                this.OnTileSelection();
            }
        }

        /// <summary>
        /// Restricts the values of a <see cref="Point"/> to the dimensions of the selected texture.
        /// </summary>
        /// <param name="value">
        /// The value to be restricted.
        /// </param>
        /// <returns>
        /// Returns the restricted <see cref="Point"/> value.
        /// </returns>
        private Point CapValues(Point value)
        {
            // prevent values less then 0
            value.X = value.X < 0 ? 0 : value.X;
            value.Y = value.Y < 0 ? 0 : value.Y;

            // prevent values greater then the texture dimensions
            value.X = value.X > (this.textureAsset.width / this.TileWidth) - 1 ? (this.textureAsset.width / this.TileWidth) - 1 : value.X;
            value.Y = value.Y > (this.textureAsset.height / this.TileHeight) - 1 ? (this.textureAsset.height / this.TileHeight) - 1 : value.Y;

            return value;
        }
    }
}