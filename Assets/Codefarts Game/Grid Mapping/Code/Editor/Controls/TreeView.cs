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

    using UnityEngine;

    /// <summary>
    /// Provides a tree view node control.
    /// </summary>
    public class TreeView
    {
        /// <summary>
        /// Holds a reference to the selected node.
        /// </summary>
        private TreeViewNode selectedNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeView"/> class.
        /// </summary>
        public TreeView()
        {
            this.Inset = 8;
            this.Nodes = new List<TreeViewNode>();
        }

        /// <summary>
        /// Provides an event for when the selection changes.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Gets or sets a value that determines how much each child node gets inset.
        /// </summary>
        public float Inset { get; set; }

        /// <summary>
        /// Gets or sets the nodes for the tree view.
        /// </summary>
        public List<TreeViewNode> Nodes { get; set; }

        /// <summary>
        /// Gets the currently selected tree node.
        /// </summary>
        public TreeViewNode SelectedNode
        {
            get
            {
                return this.selectedNode;
            }

            private set
            {
                this.selectedNode = value;
                if (this.SelectionChanged != null)
                {
                    this.SelectionChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the expansion indicator will be drawn.
        /// </summary>
        public bool ShowExpansion { get; set; }

        /// <summary>
        /// Draw the tree view.
        /// </summary>
        public void Draw()
        {
            this.Draw(null);
        }

        /// <summary>
        /// Draws the tree view.
        /// </summary>
        /// <param name="drawCallback">
        /// The draw callback that will be called that will allow the developer to draw there own custom tree view node templates.
        /// </param>
        public void Draw(Func<TreeViewNode, bool, bool> drawCallback)
        {
            var stack = new Stack<TreeViewNode>();
            GUILayout.BeginVertical();
            foreach (var node in this.Nodes)
            {
                this.DrawNode(stack, node, drawCallback);
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Used to draw a tree view node.
        /// </summary>
        /// <param name="stack">
        /// The current node hierarchy stack.
        /// </param>
        /// <param name="node">
        /// The node to be drawn.
        /// </param>
        /// <param name="drawCallback">
        /// The draw callback that will be called that will allow the developer to draw there own custom tree view node template.
        /// </param>
        private void DrawNode(Stack<TreeViewNode> stack, TreeViewNode node, Func<TreeViewNode, bool, bool> drawCallback)
        {
            // if the node is not visible skip it
            if (node == null || !node.IsVisible)
            {
                return;
            }

            GUILayout.BeginHorizontal();  // node

            var insetPixels = this.Inset * stack.Count;
            GUILayout.Space(insetPixels);
            stack.Push(node);

            if (this.ShowExpansion && GUILayout.Button(node.IsExpanded ? "-" : "+", GUI.skin.label))
            {
                node.IsExpanded = !node.IsExpanded;
            }

            if (drawCallback != null)
            {
                var selected = drawCallback(node, this.SelectedNode == node);
                this.SelectedNode = selected ? node : this.SelectedNode;
            }
            else
            {
                var label = this.SelectedNode == node ? "SelectionRect" : "label";
                var button = GUILayout.Button(node.Value.ToString(), label);
                this.SelectedNode = button ? node : this.SelectedNode;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();  // node

            // if the node is expanded draw it's children
            if (node.IsExpanded)
            {
                foreach (var child in node.Nodes)
                {
                    this.DrawNode(stack, child, drawCallback);
                }
            }

            stack.Pop();
        }
    }
}