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
    public class Map2DLayersWindow : EditorWindow, ILayerEditor
    {
        /// <summary>
        /// Holds a list of tools that have been registered.
        /// </summary>
        private readonly List<IEditorTool<ILayerEditor>> tools;

        /// <summary>
        /// Holds a reference to the current drawing tool.
        /// </summary>
        private IEditorTool<ILayerEditor> currentTool;

        /// <summary>
        /// Holds the id for the current drawing tool.
        /// </summary>
        private Guid drawingToolIndex;

        /// <summary>
        /// Holds the scroll value for the layer list.
        /// </summary>
        private Vector2 scrollValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Map2DLayersWindow"/> class.
        /// </summary>
        public Map2DLayersWindow()
        {
            this.tools = new List<IEditorTool<ILayerEditor>>();
            var service = Map2DService.Instance;
            service.ToolsChanged += this.UpdateToolList;
            service.NewMapStarted += (sender, args) => this.Repaint();
            service.NewMapLayer += (sender, args) => this.Repaint();
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
        public IEnumerable<IEditorTool<ILayerEditor>> Tools
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
        [MenuItem("Window/Codefarts/Map2D/*BETA* Layers Window")]
        public static void ShowWindow()
        {
            var local = LocalizationManager.Instance;
            var window = GetWindow<Map2DLayersWindow>(local.Get("Map2DLayers"));
            window.Show();
        }

        /// <summary>
        /// Gets a reference to a registered tool.
        /// </summary>
        /// <param name="uid">THe <see cref="Guid"/> that uniquely identifies the tool.</param>
        /// <returns>Returns a <see cref="IEditorTool{T}"/> instance.</returns>
        public IEditorTool<ILayerEditor> GetTool(Guid uid)
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
            GUILayout.BeginVertical();

            if (Map2DService.Instance.ActiveMapIndex < 0)
            {
                GUILayout.Label(LocalizationManager.Instance.Get("NoMap"));
                return;
            }

            this.DrawToolBar();

            this.DrawLayerList();

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Registers a tool with the <see cref="Map2DLayersWindow"/> window.
        /// </summary>
        /// <param name="tool">A reference to a tool that implements the <see cref="IEditorTool{T}"/> interface.</param>
        public void Register(IEditorTool<ILayerEditor> tool)
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
        /// Draws the list of layers.
        /// </summary>
        private void DrawLayerList()
        {
            var service = Map2DService.Instance;
            if (service.ActiveMapIndex < 0)
            {
                return;
            }

            var map = service.GetActiveMap();
            if (map.Layers == null)
            {
                return;
            }

            var layers = new Map2DLayerModel[map.Layers.Count];
            map.Layers.CopyTo(layers, 0);

            this.scrollValue = GUILayout.BeginScrollView(this.scrollValue, false, false);
            GUILayout.BeginVertical();

            ControlGrid.DrawGenericGrid(
                (data, index, style, options) =>
                {
                    //GUILayout.BeginHorizontal(GUI.skin.box, GUILayout.Height(64), GUILayout.ExpandWidth(true));

                    //GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

                    var layerModel = data[index];

                    var nameText = layerModel.Name;
                    nameText = nameText ?? string.Empty;

                    if (index == map.ActiveLayer)
                    {
                        GUI.SetNextControlName("txtLayerName" + index);
                        layerModel.Name = GUILayout.TextField(nameText, GUILayout.MinWidth(32));
                    }
                    else
                    {
                        if (GUILayout.Button(nameText, GUILayout.MinWidth(32)))
                        {
                            map.ActiveLayer = index;
                            GUI.FocusControl("txtLayerName" + index);
                            service.OnRepaintMap();
                        }
                    }

                    //GUILayout.BeginHorizontal();

                    //GUILayout.EndHorizontal();

                    //GUILayout.EndVertical();

                    //GUILayout.FlexibleSpace();

                    //var texture = layerModel.Texture;
                    //if (texture != null)
                    //{
                    //    if (GUILayout.Button(string.Empty, GUI.skin.label, GUILayout.MaxWidth(64), GUILayout.Height(64)))
                    //    {
                    //        Debug.Log("here");
                    //    }

                    //    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture, ScaleMode.ScaleToFit, false);
                    //}

                    //GUILayout.EndHorizontal();

                    return layerModel;
                },
                layers,
                1,
                null);

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndScrollView();
        }

        /// <summary>
        /// Draws the layer toolbar.
        /// </summary>
        private void DrawToolBar()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            var values = new IEditorTool<ILayerEditor>[this.tools.Count];
            var i = 0;
            foreach (var editorTool in this.tools)
            {
                values[i++] = editorTool;
            }

            var settings = SettingsManager.Instance;
            var size = settings.GetSetting(GlobalConstants.Map2DLayerButtonSizeKey, 64.0f);
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
                        settings.SetValue(GlobalConstants.Map2DLastLayerDrawingToolUidKey, this.currentTool.Uid.ToString());

                        // notify service so code hooks can receive notifications
                        Map2DService.Instance.OnLayerToolSelected(oldTool, tool);
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
            var drawToolSetting = settings.GetSetting(GlobalConstants.Map2DLayerToolListKey, string.Empty);
            var addedTools = drawToolSetting.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // attempt to create each tool
            string errors = null;
            var list = new List<IEditorTool<ILayerEditor>>();
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
                    var temp = type.Assembly.CreateInstance(tool) as IEditorTool<ILayerEditor>;
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
                service.OnLayerToolSelected(null, null);
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
                    service.OnLayerToolSelected(null, this.currentTool);
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