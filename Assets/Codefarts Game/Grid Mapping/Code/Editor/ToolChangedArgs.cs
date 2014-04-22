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

    /// <summary>
    /// Provides event arguments for the <see cref="GridMappingService.DrawingToolSelected"/> event.
    /// </summary>
    public class ToolChangedArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets or sets a reference to the previous tool after it has been shut down.
        /// </summary>
        public IEditorTool<T> OldTool { get; set; }

        /// <summary>
        /// Gets or sets the current drawing tool after it has been started.
        /// </summary>
        public IEditorTool<T> Tool { get; set; }

        /// <summary>
        /// Gets or sets a reference to a <see cref="GridMap"/> associated with the tool change.
        /// </summary>
        public GridMap Map { get; set; }
    }
}