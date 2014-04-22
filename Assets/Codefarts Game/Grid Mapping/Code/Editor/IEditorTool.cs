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

    using UnityEngine;

    /// <summary>
    /// Provides a interface for drawing tools to implement.
    /// </summary>
    /// <typeparam name="T">
    /// Specifies the type that manages the tool.
    /// </typeparam>
    public interface IEditorTool<T>
    {
        /// <summary>
        /// Gets a <see cref="Guid"/> value that uniquely identifies the tool.
        /// </summary>
        Guid Uid { get; }

        /// <summary>
        /// Gets a value representing the title or name of the tool.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets a value representing the content to be used for the tools button.
        /// </summary>
        GUIContent ButtonContent { get; }

        /// <summary>
        /// Method will be called when the tool is no longer of use by the system or a new tool is being selected.
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Called when the tool is set as the active tool.
        /// </summary>
        /// <param name="editor">A reference to the editor that this tool should work against.</param>
        void Startup(T editor);

        /// <summary>
        /// Called by the editor to update the tool.
        /// </summary>
        void Update();

        /// <summary>
        /// Called by the editor to notify the tool to draw something.
        /// </summary>
        void Draw();

        /// <summary>
        /// Called by the editor to give the tool the ability to include additional tools in the inspector.
        /// </summary>
        void OnInspectorGUI();

        /// <summary>
        /// Called by the editor to allow the tool to draw it self on the tool bar.
        /// </summary>
        /// <returns>If true the tool has drawn it self and no further drawing is necessary, otherwise false.</returns>
        bool DrawTool();
    }
}
