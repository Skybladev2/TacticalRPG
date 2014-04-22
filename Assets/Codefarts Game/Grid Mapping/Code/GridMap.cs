/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping
{
    using System.Collections.Generic;
    using System.Linq;

    using Codefarts.GridMapping.Models;

    using UnityEngine;

    /// <summary>
    /// Provides a component for grid mapping.
    /// </summary>
    public class GridMap : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets the number of rows of cells.
        /// </summary>
        public int Rows;

        /// <summary>
        /// Gets or sets the number of columns of cells.
        /// </summary>
        public int Columns;

        /// <summary>
        /// Gets or sets the layer information.
        /// </summary>
//        [SerializeField]
        public GridMapLayerModel[] Layers;

#if UNITY_EDITOR
        /// <summary>
        /// Gets or sets the list of xml drawing rule files.
        /// </summary>
        [SerializeField]
        private List<string> drawRules;
#endif

        /// <summary>
        /// Gets or sets the depth or thickness of the layers.
        /// </summary>
        public float Depth;

        /// <summary>
        /// Gets or sets the value of the cell width.
        /// </summary>
        public float CellWidth = 1;

        /// <summary>
        /// Gets or sets the value of the cell height.
        /// </summary>
        public float CellHeight = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridMap"/> class.
        /// </summary>
        public GridMap()
        {
            this.Columns = 20;
            this.Rows = 10;
            this.Depth = 1;
            this.Layers = new[] { new GridMapLayerModel { Visible = true, Locked = false } };
            this.drawRules = new List<string>();
        }

        ///// <summary>
        ///// Gets a <see cref="List{GridMapLayerModel}"/> of layer information.
        ///// </summary>
        //public GridMapLayerModel[] Layers
        //{
        //    get
        //    {
        //        return this.layers;
        //    }
        //}

        /// <summary>
        /// Gets a <see cref="List{String}"/> of xml drawing rule files.
        /// </summary>
        public List<string> DrawingRules
        {
            get
            {
                return this.drawRules;
            }
        }
    }
}
