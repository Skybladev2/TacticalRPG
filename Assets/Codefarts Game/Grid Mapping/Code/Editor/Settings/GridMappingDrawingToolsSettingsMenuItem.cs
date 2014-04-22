/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.GridMapping.Editor.Settings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.Localization;

    using UnityEngine;

    /// <summary>
    /// Provides a menu for grid map drawing tools.
    /// </summary>
    public class GridMappingDrawingToolsSettingsMenuItem
    {
        /// <summary>
        /// Stores the tools that will appear in the inspector window.
        /// </summary>
        private static List<SelectionModel<string>> addedTools;

        /// <summary>
        /// Holds the scroll position for the <see cref="availableTools"/> list.
        /// </summary>
        private static Vector2 availableList;

        /// <summary>
        /// Stores the list of tools that are available for adding the the <see cref="addedTools"/> list.
        /// </summary>
        private static List<SelectionModel<string>> availableTools;

        /// <summary>
        /// Holds the scroll position for the <see cref="addedTools"/> list.
        /// </summary>
        private static Vector2 toolList;

        /// <summary>
        /// Draws the <see cref="GridMappingDrawingToolsSettingsMenuItem"/> class.
        /// </summary>
        public static void Draw()
        {
            var local = LocalizationManager.Instance;

            GUILayout.BeginVertical();

            SettingHelpers.DrawSettingsFloatField(GlobalConstants.DrawingModeButtonSizeKey, local.Get("SETT_DrawingToolButtonSize"), 64, 1, 100, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.DrawModeButtonsPerRowKey, local.Get("SETT_DrawingToolButtonsPerRow"), 3, 1, 100, Helpers.RedrawInspector);

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
                addedTools.AddRange(availableTools.Where(x => x.Selected).Select(x => new SelectionModel<string>() { Tag = x.Tag, Value = x.Value }));
                availableTools.ForEach(x => x.Selected = false);
                settings.SetValue(GlobalConstants.DrawingToolListKey, string.Join(",", addedTools.Select(x => x.Tag.ToString()).ToArray()));
                GridMappingService.Instance.OnDrawingToolChanged();
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

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws controls for moving selected items in the added tool list up or down.
        /// </summary>
        private static void DrawIndexPositionControls()
        {
            var local = LocalizationManager.Instance;
            var settings = SettingsManager.Instance;

            // draw buttons for moving selected added tools up or down the list
            GUILayout.BeginVertical(GUILayout.MaxWidth(25));
            GUILayout.FlexibleSpace();

            // draw buttons for moving selected entries up in the list
            if (GUILayout.Button(local.Get("Up")))
            {
                for (var i = 0; i < addedTools.Count; i++)
                {
                    var index = i + 1;
                    if (index > addedTools.Count - 1)
                    {
                        continue;
                    }

                    // swap indexes
                    if (addedTools[index].Selected)
                    {
                        var temp = addedTools[index];
                        addedTools[index] = addedTools[i];
                        addedTools[i] = temp;
                    }
                }

                // update the settings and raise a event 
                settings.SetValue(GlobalConstants.DrawingToolListKey, string.Join(",", addedTools.Select(x => x.Tag.ToString()).ToArray()));
                GridMappingService.Instance.OnDrawingToolChanged();
            }

            // draw buttons for moving selected entries down in the list
            if (GUILayout.Button(local.Get("Down")))
            {
                for (var i = addedTools.Count - 1; i >= 0; i--)
                {
                    var index = i - 1;
                    if (index < 0)
                    {
                        continue;
                    }

                    // swap indexes
                    if (addedTools[index].Selected)
                    {
                        var temp = addedTools[index];
                        addedTools[index] = addedTools[i];
                        addedTools[i] = temp;
                    }
                }

                // update the settings and raise a event 
                settings.SetValue(GlobalConstants.DrawingToolListKey, string.Join(",", addedTools.Select(x => x.Tag.ToString()).ToArray()));
                GridMappingService.Instance.OnDrawingToolChanged();
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

            // if no added tools yet add them from settings
            if (addedTools == null)
            {
                // setup variable containing the default list of built in tools
                var defaultTools = new[]
                                       {
                                           typeof(DrawingTools.Pencil).FullName,
                                           typeof(DrawingTools.PrefabInformation).FullName,
                                           typeof(DrawingTools.PrefabPicker).FullName, 
                                           typeof(DrawingTools.Rectangle).FullName
                                       };

                // get fully qualified tool names from settings
                var drawToolSetting = settings.GetSetting(GlobalConstants.DrawingToolListKey, string.Join(",", defaultTools));
                var strings = drawToolSetting.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // set the added tools list
                addedTools = strings.Select(item =>
                    {
                        var extension = Path.GetExtension(item);
                        return extension != null ? new SelectionModel<string>() { Selected = false, Tag = item, Value = extension.Substring(1) } : null;
                    }).ToList();
            }

            // draw the actual list of added tools
            GUILayout.BeginVertical(GUILayout.MaxWidth(150));
            var local = LocalizationManager.Instance;
            GUILayout.Label(local.Get("SETT_DrawingTools"));
            GUILayout.BeginHorizontal();

            // provide a remove button to remove selected items from the added tools list
            if (GUILayout.Button(local.Get("Remove")))
            {
                foreach (var tool in addedTools.Where(x => x.Selected).ToArray())
                {
                    addedTools.Remove(tool);
                }

                settings.SetValue(GlobalConstants.DrawingToolListKey, string.Join(",", addedTools.Select(x => x.Tag.ToString()).ToArray()));
                GridMappingService.Instance.OnDrawingToolChanged();
            }

            // provides a button for selecting all tools in the added tool list
            if (GUILayout.Button(local.Get("All")))
            {
                addedTools.ForEach(x => x.Selected = true);
            }

            // provides a button for deselecting all tools in the added tool list
            if (GUILayout.Button(local.Get("None")))
            {
                addedTools.ForEach(x => x.Selected = false);
            }

            GUILayout.EndHorizontal();

            toolList = GUILayout.BeginScrollView(toolList, false, false);
            GUILayout.BeginVertical();
            foreach (var item in addedTools)
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
            var fullName = typeof(IEditorTool<GridMapEditor>).FullName;
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