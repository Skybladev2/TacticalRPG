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
    using System.Collections.Generic;

    /// <summary>
    /// Provides a node for the <see cref="TreeView"/> control.
    /// </summary>
    public class TreeViewNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewNode"/> class.
        /// </summary>
        public TreeViewNode()
        {
            this.Nodes = new List<TreeViewNode>();
            this.IsVisible = true;
            this.IsExpanded = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the node is expanded to show it's children or not.
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the node is visible in the tree view.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the list of child nodes.
        /// </summary>
        public List<TreeViewNode> Nodes { get; set; }

        /// <summary>
        /// Gets or sets a tag for the node.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets the value associated with the node.
        /// </summary>
        public object Value { get; set; }
    }
}