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

    using UnityEngine;

    /// <summary>
    /// Provides arguments for the <see cref="GridMappingService.OnPrefabDrawn"/> method.
    /// </summary>
    public class DrawPrefabArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the column where the prefab was drawn.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Gets or sets the row where the prefab was drawn.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets the layer where the prefab was drawn.
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// Gets or sets a reference to the prefab that was drawn.
        /// </summary>
        public GameObject Prefab { get; set; }

        /// <summary>
        /// Gets or sets the map where the prefab was drawn.
        /// </summary>
        public GridMap Map { get; set; }
    }
}