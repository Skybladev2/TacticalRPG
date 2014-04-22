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

    using Codefarts.Localization;

    /// <summary>
    /// Provides a model for a Map2D  map.
    /// </summary>
    public class Map2DModel
    {
        /// <summary>
        /// Holds the value for the <see cref="ActiveLayer"/> property.
        /// </summary>
        private int activeLayer;

        /// <summary>
        /// Gets or sets the index for the active map layer.
        /// </summary>
        public int ActiveLayer
        {
            get
            {
                return this.activeLayer;
            }

            set
            {
                if (this.Layers == null)
                {
                    throw new NullReferenceException(LocalizationManager.Instance.Get("ERR_LayersPropertyNull"));
                }

                if (value < 0 || value > this.Layers.Count - 1)
                {
                    throw new IndexOutOfRangeException("value");
                }

                this.activeLayer = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of map layers.
        /// </summary>
        public List<Map2DLayerModel> Layers { get; set; }
    }
}