/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.Map2D.Editor
{
    using System;
    using System.Collections.Generic;

    using Codefarts.GridMapping.Editor;
    using Codefarts.Map2D.Editor.Interfaces;

    /// <summary>
    /// Provides a singleton service for Map2D related functionality.
    /// </summary>
    public class Map2DService
    {
        /// <summary>
        /// Holds a singleton reference to a <see cref="Map2DService"/> type.
        /// </summary>
        private static Map2DService instance;

        /// <summary>
        /// Holds a list of open maps.
        /// </summary>
        private readonly List<Map2DModel> maps = new List<Map2DModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Map2DService"/> class. 
        /// </summary>
        public Map2DService()
        {
            this.ActiveMapIndex = -1;
        }

        /// <summary>
        /// Raised after the map has been drawn.
        /// </summary>
        public event EventHandler<MapEventArgs> AfterDrawingMap;

        /// <summary>
        /// Raised before the map is drawn.
        /// </summary>
        public event EventHandler<MapEventArgs> BeforeDrawingMap;

        /// <summary>
        /// Provides event notifications when the a drawing tool is selected.
        /// </summary>
        public event EventHandler<ToolChangedArgs<IMapEditor>> EditorDrawingToolSelected;

        /// <summary>
        /// Provides event notifications when the a drawing tool is selected.
        /// </summary>
        public event EventHandler<ToolChangedArgs<ILayerEditor>> LayerToolSelected;

        /// <summary>
        /// Provides event notifications when a new map has been started.
        /// </summary>
        public event EventHandler NewMapStarted;

        /// <summary>
        /// Provides event notifications when a map need repainting.
        /// </summary>
        public event EventHandler RepaintMap;

        /// <summary>
        /// Provides event notifications when the list changes for any of the drawing tools.
        /// </summary>
        public event EventHandler ToolsChanged;

        /// <summary>
        /// Provides event notifications when a new map layer is added.
        /// </summary>
        public event EventHandler<MapEventArgs> NewMapLayer;

        /// <summary>
        /// Gets a singleton to a <see cref="Map2DService"/>.
        /// </summary>
        public static Map2DService Instance
        {
            get
            {
                return instance ?? (instance = new Map2DService());
            }
        }

        /// <summary>
        /// Gets or sets the index to the currently active map.
        /// </summary>
        public int ActiveMapIndex { get; set; }

        /// <summary>
        /// Gets the number of open maps.
        /// </summary>
        public int MapCount
        {
            get
            {
                return this.maps.Count;
            }
        }

        /// <summary>
        /// Gets the active map.
        /// </summary>
        /// <returns>
        /// The <see cref="Map2DModel"/>.
        /// </returns>
        public Map2DModel GetActiveMap()
        {
            return this.maps[this.ActiveMapIndex];
        }

        /// <summary>
        /// Gets a reference to a map model.
        /// </summary>
        /// <param name="index">
        /// The index of the map.
        /// </param>
        /// <returns>
        /// Returns a reference to a <see cref="Map2DModel"/>.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"><see cref="index"/> is less then 0, greater then <see cref="MapCount"/> or there are not open maps.</exception>
        public Map2DModel GetMap(int index)
        {
            return this.maps[index];
        }

        /// <summary>
        /// Raises the <see cref="AfterDrawingMap"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        public void OnAfterDrawMap(object sender, MapEventArgs e)
        {
            if (this.AfterDrawingMap != null)
            {
                this.AfterDrawingMap(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="NewMapLayer"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        public void OnNewMapLayer(object sender, MapEventArgs e)
        {
            if (this.NewMapLayer != null)
            {
                this.NewMapLayer(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="BeforeDrawingMap"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        public void OnBeforeDrawMap(object sender, MapEventArgs e)
        {
            var handler = this.BeforeDrawingMap;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="EditorDrawingToolSelected"/> event.
        /// </summary>
        /// <param name="oldTool">A reference to the previously selected tool.</param>
        /// <param name="tool">A reference to the currently selected tool.</param>
        public void OnEditorDrawingToolSelected(IEditorTool<IMapEditor> oldTool, IEditorTool<IMapEditor> tool)
        {
            var handler = this.EditorDrawingToolSelected;
            if (handler != null)
            {
                handler(this, new ToolChangedArgs<IMapEditor> { OldTool = oldTool, Tool = tool });
            }
        }

        /// <summary>
        /// Raises the <see cref="LayerToolSelected"/> event.
        /// </summary>
        /// <param name="oldTool">A reference to the previously selected tool.</param>
        /// <param name="tool">A reference to the currently selected tool.</param>
        public void OnLayerToolSelected(IEditorTool<ILayerEditor> oldTool, IEditorTool<ILayerEditor> tool)
        {
            var handler = this.LayerToolSelected;
            if (handler != null)
            {
                handler(this, new ToolChangedArgs<ILayerEditor> { OldTool = oldTool, Tool = tool });
            }
        }

        /// <summary>
        /// Raises the <see cref="NewMapStarted"/> event.
        /// </summary>
        public virtual void OnNewMapStarted()
        {
            var handler = this.NewMapStarted;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="RepaintMap"/> event.
        /// </summary>
        public virtual void OnRepaintMap()
        {
            var handler = this.RepaintMap;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="ToolsChanged"/> event.
        /// </summary>
        public void OnToolsChanged()
        {
            var handler = this.ToolsChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Creates a new Map2D map.
        /// </summary>
        public void StartNewMap()
        {
            var map = new Map2DModel();
            this.maps.Add(map);
            if (this.ActiveMapIndex < 0)
            {
                this.ActiveMapIndex = this.maps.Count - 1;
            }

            this.OnNewMapStarted();
            var model = map.AddNewLayer(512, 256);
            model.Name = "Untitled";
        }
    }
}
