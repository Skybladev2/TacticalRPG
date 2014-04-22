/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.GridMapping.Editor.DrawingTools.Map2D
{
    using System;

    using Codefarts.Map2D.Editor.Interfaces;

    using UnityEngine;

    /// <summary>
    /// Provides a tile drawing tool for Map2D.
    /// </summary>
    public class FlexibleSpacing : IEditorTool<IMapEditor>, IEditorTool<ILayerEditor>
    {
        /// <summary>
        /// Gets a value representing the content to be used for the tools button.
        /// </summary>
        public GUIContent ButtonContent
        {
            get
            {
                return GUIContent.none;
            }
        }

        /// <summary>
        /// Gets a value representing the title or name of the tool.
        /// </summary>
        public string Title
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets a <see cref="Guid"/> value that uniquely identifies the tool.
        /// </summary>
        public Guid Uid
        {
            get
            {
                return new Guid("73795F68-8981-4496-9229-8BC48EB5DFDC");
            }
        }

        /// <summary>
        /// Called by the editor to notify the tool to draw something.
        /// </summary>
        public void Draw()
        {
            // does nothing
        }

        /// <summary>
        /// Called by the editor to give the tool the ability to include additional tools in the inspector.
        /// </summary>
        public void OnInspectorGUI()
        {
            // does nothing
        }

        /// <summary>
        /// Called by the editor to allow the tool to draw it self on the tool bar.
        /// </summary>
        /// <returns>If true the tool has drawn it self and no further drawing is necessary, otherwise false.</returns>
        public bool DrawTool()
        {
            GUILayout.FlexibleSpace();
            return true;
        }

        /// <summary>
        /// Method will be called when the tool is no longer of use by the system or a new tool is being selected.
        /// </summary>
        public void Shutdown()
        {
        }

        /// <summary>
        /// Called when the tool is set as the active tool.
        /// </summary>
        /// <param name="editor">A reference to the editor that this tool should work against.</param>
        public void Startup(ILayerEditor editor)
        {
        }

        /// <summary>
        /// Called when the tool is set as the active tool.
        /// </summary>
        /// <param name="mapEditor">A reference to the editor that this tool should work against.</param>
        public void Startup(IMapEditor mapEditor)
        {
        }

        /// <summary>
        /// Called by the editor to update the tool on every call to OnSceneGUI.
        /// </summary>
        public void Update()
        {
        }
    }
}