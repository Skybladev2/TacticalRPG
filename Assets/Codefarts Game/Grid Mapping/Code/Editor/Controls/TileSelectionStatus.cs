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
    /// <summary>
    /// Provides a enumerator that represents the tile selection status.
    /// </summary>
    public enum TileSelectionStatus
    {
        /// <summary>
        /// User has begun selecting.
        /// </summary>
        Begin,

        /// <summary>
        /// User is in the process of selecting (dragging or holding the mouse)
        /// </summary>
        Selecting,

        /// <summary>
        /// The user has finished making there selection.
        /// </summary>
        Complete
    }
}