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

    /// <summary>
    /// Provides <see cref="EventArgs"/> for when the map.
    /// </summary>
    public class MapEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapEventArgs"/> class. 
        /// </summary>
        /// <param name="map">
        /// Sets the initial value of the <see cref="Map"/> property.
        /// </param>
        public MapEventArgs(Map2DModel map)
        {
            this.Map = map;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapEventArgs"/> class.
        /// </summary>
        public MapEventArgs()
        {
        }

        /// <summary>
        /// Gets or sets a reference to a map model.
        /// </summary>
        public Map2DModel Map { get; set; }
    }
}