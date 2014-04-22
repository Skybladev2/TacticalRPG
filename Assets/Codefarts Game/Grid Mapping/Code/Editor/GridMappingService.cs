/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Editor
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// The grid mapping service.
    /// </summary>
    public class GridMappingService
    {
        /// <summary>
        /// Holds a reference to a singleton instance of <see cref="GridMappingService"/>.
        /// </summary>
        private static GridMappingService service;

        /// <summary>
        /// The current material.
        /// </summary>
        private Material currentMaterial;

        /// <summary>
        /// The current prefab.
        /// </summary>
        private GameObject currentPrefab;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridMappingService"/> class.
        /// </summary>
        public GridMappingService()
        {
            this.RecentlyUsedMaterials = new List<Material>();
            this.RecentlyUsedPrefabs = new List<GameObject>();
        }

        /// <summary>
        /// Provides event notifications when a prefab is drawn on a map.
        /// </summary>
        public event EventHandler<DrawPrefabArgs> PrefabDrawn;

        /// <summary>
        /// Provides event notifications when the <see cref="CurrentMaterial"/> property changes.
        /// </summary>
        public event EventHandler CurrentMaterialChanged;

        /// <summary>
        /// Provides event notifications when the <see cref="CurrentPrefab"/> property changes.
        /// </summary>
        public event EventHandler CurrentPrefabChanged;

        /// <summary>
        /// Provides event notifications when the a drawing tool is selected.
        /// </summary>
        public event EventHandler<ToolChangedArgs<GridMapEditor>> DrawingToolSelected;

        /// <summary>
        /// Provides event notifications when the drawing tool list changes.
        /// </summary>
        public event EventHandler DrawingToolChanged;

        /// <summary>
        /// Gets a singleton instance of a <see cref="GridMappingService"/>.
        /// </summary>
        public static GridMappingService Instance
        {
            get
            {
                return service ?? (service = new GridMappingService());
            }
        }

        /// <summary>
        /// Gets or sets the recently used materials list.
        /// </summary>
        public List<Material> RecentlyUsedMaterials { get; set; }

        /// <summary>
        /// Gets or sets the recently used materials list.
        /// </summary>
        public List<GameObject> RecentlyUsedPrefabs { get; set; }

        /// <summary>
        /// Gets or sets the current prefab.
        /// </summary>
        public GameObject CurrentPrefab
        {
            get
            {
                return this.currentPrefab;
            }

            set
            {
                this.currentPrefab = value;
                this.OnCurrentPrefabChanged();
            }
        }

        /// <summary>
        /// Gets or sets the current material.
        /// </summary>
        public Material CurrentMaterial
        {
            get
            {
                return this.currentMaterial;
            }

            set
            {
                this.currentMaterial = value;
                this.OnCurrentMaterialChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="CurrentMaterialChanged"/> event.
        /// </summary>
        public void OnCurrentMaterialChanged()
        {
            if (this.CurrentMaterialChanged != null)
            {
                this.CurrentMaterialChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="CurrentPrefabChanged"/> event.
        /// </summary>
        public void OnCurrentPrefabChanged()
        {
            if (this.CurrentPrefabChanged != null)
            {
                this.CurrentPrefabChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Used to raise the <see cref="PrefabDrawn"/> event.
        /// </summary>
        /// <param name="map">A reference to the map associated with the action.</param>
        /// <param name="column">The column where the prefab was drawn.</param>
        /// <param name="row">The row where the prefab was drawn.</param>
        /// <param name="layer">The layer where the prefab was drawn.</param>
        /// <param name="prefab">A reference to the prefab.</param>
        public void OnPrefabDrawn(GridMap map, int column, int row, int layer, GameObject prefab)
        {
            if (this.PrefabDrawn != null)
            {
                this.PrefabDrawn(this, new DrawPrefabArgs { Map = map, Column = column, Row = row, Layer = layer, Prefab = prefab });
            }
        }

        /// <summary>
        /// Raises the <see cref="DrawingToolSelected"/> event.
        /// </summary>
        /// <param name="oldTool">A reference to the previously selected tool.</param>
        /// <param name="tool">A reference to the currently selected tool.</param>
        /// <param name="map">A reference to the <see cref="GridMap"/> associated with the tool change.</param>
        public void OnDrawingToolSelected(IEditorTool<GridMapEditor> oldTool, IEditorTool<GridMapEditor> tool, GridMap map)
        {
            if (this.DrawingToolSelected != null)
            {
                this.DrawingToolSelected(this, new ToolChangedArgs<GridMapEditor> { OldTool = oldTool, Tool = tool, Map = map });
            }
        }

        /// <summary>
        /// Raises the <see cref="DrawingToolChanged"/> event.
        /// </summary>
        public void OnDrawingToolChanged()
        {
            if (this.DrawingToolChanged != null)
            {
                this.DrawingToolChanged(this, EventArgs.Empty);
            }
        }
    }
}
