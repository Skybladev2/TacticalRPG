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

    using UnityEngine;

    /// <summary>
    /// Provides event arguments for free form selection rectangles.
    /// </summary>
    public class FreeformSelectionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the status of the selection.
        /// </summary>
        public TileSelectionStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the selection rectangle.
        /// </summary>
        public Rect SelectionRectangle { get; set; }
    }
}