/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.Map2D.Editor.Settings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.GridMapping.Editor;
    using Codefarts.Localization;
    using Codefarts.Map2D.Editor;
    using Codefarts.Map2D.Editor.Interfaces;

    using UnityEngine;

    /// <summary>
    /// Provides a menu for grid map drawing tools.
    /// </summary>
    public class Map2DDrawingToolsSettingsMenuItem
    {
        /// <summary>
        /// Stores the tools that will appear in the editor window.
        /// </summary>
        private static readonly List<SelectionModel<string>> AddedEditorTools = new List<SelectionModel<string>>();

        /// <summary>
        /// Stores the tools that will appear in the layer window.
        /// </summary>
        private static readonly List<SelectionModel<string>> AddedLayerTools = new List<SelectionModel<string>>();

        /// <summary>
        /// Holds the scroll position for the <see cref="availableTools"/> list.
        /// </summary>
        private static Vector2 availableList;

        /// <summary>
        /// Stores the list of tools that are available for adding to the the <see cref="AddedEditorTools"/> list.
        /// </summary>
        private static List<SelectionModel<string>> availableTools;

        /// <summary>
        /// Holds the index of the tool bar that is being edited.
        /// </summary>
        private static int toolBarIndex;

        /// <summary>
        /// Holds the scroll position for the <see cref="AddedEditorTools"/> list.
        /// </summary>
        private static Vector2 toolList;

        /// <summary>
        /// Initializes static members of the <see cref="Map2DDrawingToolsSettingsMenuItem"/> class. 
        /// </summary>
        static Map2DDrawingToolsSettingsMenuItem()
        {
            var settings = SettingsManager.Instance;
            var keys = new[] { GlobalConstants.Map2DDrawingToolListKey, GlobalConstants.Map2DLayerToolListKey };
            var toolLists = new[] { AddedEditorTools, AddedLayerTools };

            for (var i = 0; i < keys.Length; i++)
            {
                var list = toolLists[i];
                var settingsKey = keys[i];

                // get fully qualified tool names from settings
                var drawToolSetting = settings.GetSetting(settingsKey, string.Empty);
                var strings = drawToolSetting.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // set the added tools list
                list.AddRange(strings.Select(item =>
                {
                    var extension = Path.GetExtension(item);
                    return extension != null ? new SelectionModel<string>() { Selected = false, Tag = item, Value = extension.Substring(1) } : null;
                }));
            }
        }

        /// <summary>
        /// Draws the <see cref="Map2DDrawingToolsSettingsMenuItem"/> class.
        /// </summary>
        public static void Draw()
        {
            var local = LocalizationManager.Instance;

            GUILayout.BeginVertical();

            SettingHelpers.DrawSettingsFloatField(GlobalConstants.DrawingModeButtonSizeKey, local.Get("SETT_DrawingToolButtonSize"), 64, 1, 100, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.DrawModeButtonsPerRowKey, local.Get("SETT_DrawingToolButtonsPerRow"), 20, 1, 100, Helpers.RedrawInspector);

            DrawHeaderTools();

            GUILayout.BeginHorizontal();

            DrawAvailableToolList();

            DrawToolList();

            DrawIndexPositionControls();

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws the list of available tools.
        /// </summary>
        private static void DrawAvailableToolList()
        {
            var settings = SettingsManager.Instance;

            // be sure to update the available tools list
            GetAvailableTools(false);

            GUILayout.BeginVertical(GUILayout.MaxWidth(175));
            var local = LocalizationManager.Instance;
            GUILayout.Label(local.Get("SETT_AvailableTools"));

            GUILayout.BeginHorizontal();

            // provide a add button to add tools the the added tools list
            if (GUILayout.Button(local.Get("Add")))
            {
                List<SelectionModel<string>> list = null;
                var settingsKey = string.Empty;
                switch (toolBarIndex)
                {
                    case 0:
                        list = AddedEditorTools;
                        settingsKey = GlobalConstants.Map2DDrawingToolListKey;
                        break;

                    case 1:
                        list = AddedLayerTools;
                        settingsKey = GlobalConstants.Map2DLayerToolListKey;
                        break;
                }

                list.AddRange(availableTools.Where(x => x.Selected).Select(x => new SelectionModel<string>() { Tag = x.Tag, Value = x.Value }));
                availableTools.ForEach(x => x.Selected = false);
                settings.SetValue(settingsKey, string.Join(",", list.Select(x => x.Tag.ToString()).ToArray()));
                Map2DService.Instance.OnToolsChanged();
            }

            // provides a button for selecting all tools in the available tool list
            if (GUILayout.Button(local.Get("All")))
            {
                availableTools.ForEach(x => x.Selected = true);
            }

            // provides a button for deselecting all tools in the available tool list
            if (GUILayout.Button(local.Get("None")))
            {
                availableTools.ForEach(x => x.Selected = false);
            }

            GUILayout.EndHorizontal();

            availableList = GUILayout.BeginScrollView(availableList, false, false);
            GUILayout.BeginVertical();

            if (availableTools != null)
            {
                foreach (var item in availableTools)
                {
                    item.Selected = GUILayout.Toggle(item.Selected, item.Value, GUI.skin.button);
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws the header buttons controls for adding and removing tools.
        /// </summary>
        private static void DrawHeaderTools()
        {
            var local = LocalizationManager.Instance;

            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            // provide a search button to perform an force a update of the available tools list
            if (GUILayout.Button(local.Get("Search"), GUILayout.MinWidth(64), GUILayout.MinHeight(32)))
            {
                GetAvailableTools(true);
            }

            GUILayout.FlexibleSpace();

            var options = new[]
                              {
                                  local.Get("EditorToolbar"),
                                  local.Get("LayersToolbar") 
                              };

            var result = GUILayout.SelectionGrid(toolBarIndex, options, 2, GUILayout.MinHeight(32));
            if (result != toolBarIndex)
            {
                toolBarIndex = result;
                GetAvailableTools(true);
            }

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws controls for moving selected items in the added tool list up or down.
        /// </summary>
        private static void DrawIndexPositionControls()
        {
            var local = LocalizationManager.Instance;
            var settings = SettingsManager.Instance;

            List<SelectionModel<string>> list;
            var settingsKey = string.Empty;
            switch (toolBarIndex)
            {
                case 0:
                    list = AddedEditorTools;
                    settingsKey = GlobalConstants.Map2DDrawingToolListKey;
                    break;

                case 1:
                    list = AddedLayerTools;
                    settingsKey = GlobalConstants.Map2DLayerToolListKey;
                    break;

                default:
                    return;
            }

            // draw buttons for moving selected added tools up or down the list
            GUILayout.BeginVertical(GUILayout.MaxWidth(25));
            GUILayout.FlexibleSpace();

            // draw buttons for moving selected entries up in the list
            if (GUILayout.Button(local.Get("Up")))
            {
                for (var i = 0; i < list.Count; i++)
                {
                    var index = i + 1;
                    if (index > list.Count - 1)
                    {
                        continue;
                    }

                    // swap indexes
                    if (list[index].Selected)
                    {
                        var temp = list[index];
                        list[index] = list[i];
                        list[i] = temp;
                    }
                }

                // update the settings and raise a event 
                settings.SetValue(settingsKey, string.Join(",", list.Select(x => x.Tag.ToString()).ToArray()));
                Map2DService.Instance.OnToolsChanged();
            }

            // draw buttons for moving selected entries down in the list
            if (GUILayout.Button(local.Get("Down")))
            {
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    var index = i - 1;
                    if (index < 0)
                    {
                        continue;
                    }

                    // swap indexes
                    if (list[index].Selected)
                    {
                        var temp = list[index];
                        list[index] = list[i];
                        list[i] = temp;
                    }
                }

                // update the settings and raise a event 
                settings.SetValue(settingsKey, string.Join(",", list.Select(x => x.Tag.ToString()).ToArray()));
                Map2DService.Instance.OnToolsChanged();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws the list of added tools.
        /// </summary>
        /// <remarks>
        /// These are tools that will appear in the inspector.</remarks>
        private static void DrawToolList()
        {
            var settings = SettingsManager.Instance;
            List<SelectionModel<string>> list;
            var settingsKey = string.Empty;
            switch (toolBarIndex)
            {
                case 0:
                    list = AddedEditorTools;
                    settingsKey = GlobalConstants.Map2DDrawingToolListKey;
                    break;

                case 1:
                    list = AddedLayerTools;
                    settingsKey = GlobalConstants.Map2DLayerToolListKey;
                    break;

                default:
                    return;
            }

            // draw the actual list of added tools
            GUILayout.BeginVertical(GUILayout.MaxWidth(150));
            var local = LocalizationManager.Instance;
            GUILayout.Label(local.Get("SETT_DrawingTools"));
            GUILayout.BeginHorizontal();

            // provide a remove button to remove selected items from the added tools list
            if (GUILayout.Button(local.Get("Remove")))
            {
                list.RemoveAll(x => x.Selected);
                settings.SetValue(settingsKey, string.Join(",", list.Select(x => x.Tag.ToString()).ToArray()));
                Map2DService.Instance.OnToolsChanged();
            }

            // provides a button for selecting all tools in the added tool list
            if (GUILayout.Button(local.Get("All")))
            {
                list.ForEach(x => x.Selected = true);
            }

            // provides a button for deselecting all tools in the added tool list
            if (GUILayout.Button(local.Get("None")))
            {
                list.ForEach(x => x.Selected = false);
            }

            GUILayout.EndHorizontal();

            toolList = GUILayout.BeginScrollView(toolList, false, false);
            GUILayout.BeginVertical();
            foreach (var item in list)
            {
                item.Selected = GUILayout.Toggle(item.Selected, item.Value, GUI.skin.button);
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Searches through assemblies and finds types that implement the <see cref="IEditorTool{T}"/> interface.
        /// </summary>
        /// <param name="force">true is you want to force a rebuild of the available tools list.</param>
        private static void GetAvailableTools(bool force)
        {
            // if not forcing and there are already available tools just exit
            if (!force && availableTools != null)
            {
                return;
            }

            // search for types in each assembly that implement the IEditorTool interface
            var list = new List<SelectionModel<string>>();
            var fullName = string.Empty;
            switch (toolBarIndex)
            {
                case 0:
                    fullName = typeof(IEditorTool<IMapEditor>).FullName;
                    break;

                case 1:
                    fullName = typeof(IEditorTool<ILayerEditor>).FullName;
                    break;
            }

            foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()))
            {
                try
                {
                    var interfaceType = type.GetInterfaces().FirstOrDefault(x => x.FullName == fullName);
                    if (interfaceType != null)
                    {
                        list.Add(
                            new SelectionModel<string>()
                                {
                                    Selected = false,
                                    Tag = type.FullName,
                                    Value = type.Name
                                });
                    }
                }
                catch
                {
                    // ignore error
                }
            }

            // set the available tools list
            availableTools = list.OrderBy(x => x.Value).ToList();
        }

        /// <summary>
        /// Provides a model for list selections.
        /// </summary>
        /// <typeparam name="T">Specifies the type for the selection.</typeparam>
        private class SelectionModel<T>
        {
            /// <summary>
            /// Gets or sets a value indicating whether the model is selected in the list.
            /// </summary>
            public bool Selected { get; set; }

            /// <summary>
            /// Gets or sets a generic tag value for storing arbitrary data.
            /// </summary>
            public object Tag { get; set; }

            /// <summary>
            /// Gets or sets the value associated with the selection.
            /// </summary>
            public T Value { get; set; }
        }
    }
}