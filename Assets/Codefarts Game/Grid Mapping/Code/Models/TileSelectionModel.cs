/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.GridMapping.Models
{
    using System;

    using Codefarts.GridMapping.Common;

    using UnityEngine;

    using Color = UnityEngine.Color;

    /// <summary>
    /// Provides a model for tile selections.
    /// </summary>
    public class TileSelectionModel
    {
        /// <summary>
        /// Holds a unique id for the model.
        /// </summary>
        private readonly Guid uid;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileSelectionModel"/> class.
        /// </summary>
        public TileSelectionModel()
        {
            this.uid = Guid.NewGuid();
        }

        /// <summary>
        /// Gets a unique <see cref="Guid"/> that represents the selection model.
        /// </summary>
        public Guid Uid
        {
            get
            {
                return this.uid;
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="Color"/> associated with the selection.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets a texture preview of the tile selection.
        /// </summary>
        public Texture TexturePreview { get; set; }

        /// <summary>
        /// Gets or sets the minimum pixel coordinates of the selection.
        /// </summary>
        public Point Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum pixel coordinates of the selection.
        /// </summary>
        public Point Max { get; set; }

        /// <summary>
        /// Gets or sets the material associated with the selection.
        /// </summary>
        public Material Material { get; set; }

        /// <summary>
        /// Gets or sets the source texture filename of the texture asset.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the tile size of the selection.
        /// </summary>
        public Size TileSize { get; set; }

        /// <summary>
        /// Gets or sets the name of the shader associated with the selection.
        /// </summary>
        public string ShaderName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the model has changed.
        /// </summary>
        public bool IsDirty { get; set; }
    }
}