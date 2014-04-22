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
    using Codefarts.GridMapping.Utilities;

    using UnityEngine;

    /// <summary>
    /// Provides a model for a Map2D map layer.
    /// </summary>
    public class Map2DLayerModel
    {
        /// <summary>
        /// Gets or sets a <see cref="Texture2D"/> containing the layer image.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="GenericImage{T}"/> that holds the map layer image.
        /// </summary>
        public GenericImage<GridMapping.Common.Color> Image { get; set; }

        /// <summary>
        /// Gets or sets the layer offset where the layer will be drawn.
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Gets or sets the layer name.
        /// </summary>
        public string Name { get; set; }
    }
}