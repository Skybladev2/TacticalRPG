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
    using System.Collections.Generic;
    using System.Linq;

    using Codefarts.Localization;

    /// <summary>
    /// Provides a manager for drawing tools.
    /// </summary>
    public class DrawingToolManager
    {
        /// <summary>
        /// Holds a reference to the singleton instance of the <see cref="DrawingToolManager"/> type.
        /// </summary>
        private static DrawingToolManager singleton;

        /// <summary>
        /// Holds the registered tool references.
        /// </summary>
        private readonly List<IEditorTool<GridMapEditor>> tools = new List<IEditorTool<GridMapEditor>>();

        /// <summary>
        /// Gets a singleton instance of the <see cref="DrawingToolManager"/> type.
        /// </summary>
        public static DrawingToolManager Instance
        {
            get
            {
                return singleton ?? (singleton = new DrawingToolManager());
            }
        }

        /// <summary>
        /// Gets the number of tools registered.
        /// </summary>
        public int Count
        {
            get
            {
                return this.tools.Count;
            }
        }

        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> that contains the registered tools. 
        /// </summary>
        public IEnumerable<IEditorTool<GridMapEditor>> Tools
        {
            get
            {
                foreach (var tool in this.tools)
                {
                    yield return tool;
                }
            }
        }

        /// <summary>
        /// Removes all registered tools.
        /// </summary>
        public void Clear()
        {
            this.tools.Clear();           
        }

        /// <summary>
        /// Gets a reference to a registered tool.
        /// </summary>
        /// <param name="uid">THe <see cref="Guid"/> that uniquely identifies the tool.</param>
        /// <returns>Returns a <see cref="IEditorTool{T}"/> instance.</returns>
        public IEditorTool<GridMapEditor> GetTool(Guid uid)
        {
            return this.tools.FirstOrDefault(x => x.Uid == uid);
        }

        /// <summary>
        /// Checks to see weather or not a tool as been registered with the <see cref="DrawingToolManager"/>.
        /// </summary>
        /// <param name="guid">The UID of the drawing tool to check for.</param>
        /// <returns>Returns true if found, otherwise false.</returns>
        public bool HasTool(Guid guid)
        {
            return this.tools.Any(x => x.Uid == guid);
        }

        /// <summary>
        /// Registers a tool with the <see cref="DrawingToolManager"/>.
        /// </summary>
        /// <param name="tool">A reference to a tool that implements the <see cref="IEditorTool{T}"/> interface.</param>
        public void Register(IEditorTool<GridMapEditor> tool)
        {
            // if tool is null throw exception
            if (tool == null)
            {
                throw new ArgumentNullException("tool");
            }

            // throw exception if tool already added
            if (this.tools.Any(x => x.Uid == tool.Uid))
            {
                throw new ArgumentException(LocalizationManager.Instance.Get("ERR_ToolWithSameUidAlreadyRegistered"));
            }

            // add the tool
            this.tools.Add(tool);
        }
    }
}