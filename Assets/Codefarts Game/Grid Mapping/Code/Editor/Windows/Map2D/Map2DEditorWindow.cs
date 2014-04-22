/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.Map2D.Editor.Windows
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Codefarts.CoreProjectCode;
    using Codefarts.CoreProjectCode.Services;
    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.GridMapping.Editor;
    using Codefarts.GridMapping.Editor.Controls;
    using Codefarts.Localization;
    using Codefarts.Map2D.Editor.Interfaces;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// A image based 2D map editor utility.
    /// </summary>
    public class Map2DEditorWindow : EditorWindow, IMapEditor
    {
        /// <summary>
        /// Holds a list of tools that have been registered.
        /// </summary>
        private readonly List<IEditorTool<IMapEditor>> tools;

        /// <summary>
        /// Holds a reference to the current drawing tool.
        /// </summary>
        private IEditorTool<IMapEditor> currentTool;

        /// <summary>
        /// Holds the id for the current drawing tool.
        /// </summary>
        private Guid drawingToolIndex;

        /// <summary>
        /// Holds the scroll position for the map.
        /// </summary>
        private Vector2 mapScroll;

        /// <summary>
        /// Holds content for when there is no map.
        /// </summary>
        private GUIContent[] noMapButtonContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Map2DEditorWindow"/> class.
        /// </summary>
        public Map2DEditorWindow()
        {
            this.tools = new List<IEditorTool<IMapEditor>>();
            this.LoadContent();
            var service = Map2DService.Instance;
            service.ToolsChanged += this.UpdateToolList;
            service.NewMapLayer += (sender, args) => this.Repaint();
            service.RepaintMap += (sender, args) => this.Repaint();
        }

        /// <summary>
        /// Gets the number of tools registered.
        /// </summary>
        public int ToolCount
        {
            get
            {
                return this.tools.Count;
            }
        }

        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> that contains the registered tools. 
        /// </summary>
        public IEnumerable<IEditorTool<IMapEditor>> Tools
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
        /// Shows the editor window.
        /// </summary>
        [MenuItem("Window/Codefarts/Map2D/*BETA* 2D Map Editor")]
        public static void ShowWindow()
        {
            var local = LocalizationManager.Instance;
            var window = GetWindow<Map2DEditorWindow>(local.Get("Map2DEditor"));
            window.Show();
        }

        /// <summary>
        /// Gets a reference to a registered tool.
        /// </summary>
        /// <param name="uid">THe <see cref="Guid"/> that uniquely identifies the tool.</param>
        /// <returns>Returns a <see cref="IEditorTool{T}"/> instance.</returns>
        public IEditorTool<IMapEditor> GetTool(Guid uid)
        {
            foreach (var tool in this.tools)
            {
                if (tool.Uid.Equals(uid))
                {
                    return tool;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks to see weather or not a tool as been registered with the <see cref="DrawingToolManager"/>.
        /// </summary>
        /// <param name="guid">The UID of the drawing tool to check for.</param>
        /// <returns>Returns true if found, otherwise false.</returns>
        public bool HasTool(Guid guid)
        {
            foreach (var tool in this.tools)
            {
                if (tool.Uid.Equals(guid))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Loads in any required content.
        /// </summary>
        public void LoadContent()
        {
            var local = LocalizationManager.Instance;
            var settings = SettingsManager.Instance;
            var texturesPath = Path.Combine(settings.GetSetting(CoreGlobalConstants.ResourceFolderKey, "Codefarts.Unity"), "Textures");
            texturesPath = texturesPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            var getPath = new Func<string, string>(x => Path.Combine(texturesPath, x).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

            this.noMapButtonContent = new[]
                {
                    new GUIContent(Resources.Load(getPath("BlueGlobe"), typeof(Texture2D)) as Texture2D, local.Get("New")), 
                    new GUIContent(Resources.Load(getPath("OpenMap2D"), typeof(Texture2D)) as Texture2D, local.Get("Open"))     
                };
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.    
        /// </summary>
        public void OnEnable()
        {
            EditorCallbackService.Instance.Register(() => this.UpdateToolList(this, EventArgs.Empty));
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        public void OnGUI()
        {
            if (Map2DService.Instance.ActiveMapIndex < 0)
            {
                this.DrawNoMapImage();
                return;
            }

            GUILayout.BeginVertical();

            this.DrawToolBar();
            this.DrawMap();

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Registers a tool with the <see cref="Map2DService"/> service.
        /// </summary>
        /// <param name="tool">A reference to a tool that implements the <see cref="IEditorTool{T}"/> interface.</param>
        public void Register(IEditorTool<IMapEditor> tool)
        {
            // if tool is null throw exception
            if (tool == null)
            {
                throw new ArgumentNullException("tool");
            }

            // throw exception if tool already added
            if (this.HasTool(tool.Uid))
            {
                throw new ArgumentException(LocalizationManager.Instance.Get("ERR_ToolWithSameUidAlreadyRegistered"));
            }

            // add the tool
            this.tools.Add(tool);
        }

        /// <summary>
        /// Draws the map.
        /// </summary>
        private void DrawMap()
        {
            var service = Map2DService.Instance;

            // check for proper map index
            if (service.ActiveMapIndex < 0)
            {
                return;
            }

            // get the active map and ensure that one exists
            var map = service.GetActiveMap();
            if (map == null)
            {
                return;
            }

            this.mapScroll = GUILayout.BeginScrollView(this.mapScroll, false, false);
            GUILayout.BeginVertical();

            // raise the before map draw event
            var args = new MapEventArgs(map);
            service.OnBeforeDrawMap(this, args);

            // ensure that the map has layers
            if (map.Layers != null)
            {
                // draw each map layer
                for (var i = 0; i < map.Layers.Count; i++)
                {
                    var layer = map.Layers[i];
                    if (layer.Texture == null)
                    {
                        continue;
                    }

                    var rectangle = new Rect(4 + layer.Offset.x, 4 + layer.Offset.y, layer.Texture.width, layer.Texture.height);
                    GUILayout.Label(string.Empty, GUILayout.Width(rectangle.width), GUILayout.Height(rectangle.height + 4));
                    GUI.DrawTexture(rectangle, layer.Texture);
                }
            }

            args.Map = map;
            service.OnAfterDrawMap(this, args);

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        /// <summary>
        /// Draws the no map image controls.
        /// </summary>
        private void DrawNoMapImage()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            GUILayout.FlexibleSpace();

            ControlGrid.DrawGenericGrid(
                (data, index, style, options) =>
                {
                    if (GUILayout.Button(data[index], style, options))
                    {
                        switch (index)
                        {
                            case 0:
                                Map2DService.Instance.StartNewMap();
                                break;
                        }
                    }

                    return null;
                },
                this.noMapButtonContent,
                this.noMapButtonContent.Length,
                GUI.skin.button,
                GUILayout.Width(128),
                GUILayout.Height(128));

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws the editor tool bar.
        /// </summary>
        private void DrawToolBar()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            var values = new IEditorTool<IMapEditor>[this.tools.Count];
            var i = 0;
            foreach (var editorTool in this.tools)
            {
                values[i++] = editorTool;
            }

            var settings = SettingsManager.Instance;
            var size = settings.GetSetting(GlobalConstants.Map2DDrawingModeButtonSizeKey, 64.0f);
            var buttonsPerRow = settings.GetSetting(GlobalConstants.Map2DDrawModeButtonsPerRowKey, 20);

            ControlGrid.DrawGenericGrid(
                (toolArray, index, style, options) =>
                {
                    var tool = toolArray[index];
                    if (tool.DrawTool())
                    {
                        return tool;
                    }

                    var result = GUILayout.Toggle(this.drawingToolIndex == tool.Uid, tool.ButtonContent, style, options);

                    // detect a tool change
                    if (result && this.drawingToolIndex != tool.Uid)
                    {
                        // if there is a current tool shut it down
                        if (this.currentTool != null)
                        {
                            this.currentTool.Shutdown();
                        }

                        // store reference to the old tool
                        var oldTool = this.currentTool;

                        // save index to tool
                        this.drawingToolIndex = tool.Uid;

                        // set current tool
                        this.currentTool = tool;
                        this.currentTool.Startup(this);

                        // save tool UID as the last tool used
                        settings.SetValue(GlobalConstants.Map2DLastEditorDrawingToolUidKey, this.currentTool.Uid.ToString());

                        // notify service so code hooks can receive notifications
                        Map2DService.Instance.OnEditorDrawingToolSelected(oldTool, tool);
                    }

                    // return same tool reference
                    return tool;
                },
                values,
                buttonsPerRow,
                GUI.skin.button,
                GUILayout.Height(size),
                GUILayout.Width(size));

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Handles the <see cref="Map2DService.ToolsChanged"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        private void UpdateToolList(object sender, EventArgs e)
        {
            // rebuild the tool list  
            var settings = SettingsManager.Instance;

            // get added tools from settings
            var drawToolSetting = settings.GetSetting(GlobalConstants.Map2DDrawingToolListKey, string.Empty);
            var addedTools = drawToolSetting.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // attempt to create each tool
            string errors = null;
            var list = new List<IEditorTool<IMapEditor>>();
            foreach (var tool in addedTools)
            {
                try
                {
                    // get the type
                    var type = Type.GetType(tool, true);
                    if (type == null)
                    {
                        continue;
                    }

                    // attempt to create the type
                    var temp = type.Assembly.CreateInstance(tool) as IEditorTool<IMapEditor>;
                    if (temp != null)
                    {
                        list.Add(temp);
                    }
                }
                catch (Exception ex)
                {
                    // log the error message
                    errors += string.Format("{0}\r\n", ex.Message);
                }
            }

            // get map and service references
            var service = Map2DService.Instance;

            // shut down and remove reference to the current drawing tool if present
            if (this.currentTool != null)
            {
                this.currentTool.Shutdown();
                this.currentTool = null;
                service.OnEditorDrawingToolSelected(null, null);
            }

            // get tool manager service & remove all existing tools and register new ones
            // based of the tools that were just created
            this.tools.Clear();
            foreach (var x in list)
            {
                if (!this.HasTool(x.Uid))
                {
                    this.Register(x);
                }
            }

            // if there was a drawing tool specified before try to make it the current tool again
            if (this.drawingToolIndex != Guid.Empty)
            {
                var toolReference = this.HasTool(this.drawingToolIndex) ? this.GetTool(this.drawingToolIndex) : null;
                if (toolReference != null)
                {
                    this.currentTool = toolReference;
                    this.currentTool.Startup(this);
                    service.OnEditorDrawingToolSelected(null, this.currentTool);
                }
            }

            // remember to update the window for immediate results
            this.Repaint();

            // report any errors as warning in the console
            if (errors != null)
            {
                Debug.LogWarning(string.Format(LocalizationManager.Instance.Get("ERR_ErrorsOccurredUpdatingToolList"), errors));
            }
        }
    }
}