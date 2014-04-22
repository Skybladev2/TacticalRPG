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

    /// <summary>
    /// Provides event arguments for tile selections.
    /// </summary>
    public class TileSelectionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the selection status.
        /// </summary>
        public TileSelectionStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the list of tile locations in tile coordinates.
        /// </summary>
        public List<Point> TileLocations { get; set; }

        /// <summary>
        /// Gets or sets the minimum point (Top Left) of the selection rectangle.
        /// </summary>
        public Point Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum point (Bottom Right) of the selection rectangle.
        /// </summary>
        public Point Max { get; set; }
    }
}