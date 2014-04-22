namespace Codefarts.GridMapping.Windows
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.CoreProjectCode.Settings.Xml;
    using Codefarts.GridMapping.Editor;
    using Codefarts.GridMapping.Editor.Controls;
    using Codefarts.GridMapping.Editor.Utilities;
    using Codefarts.GridMapping.Models;

    using UnityEditor;
    using UnityEngine;

    using Helpers = Codefarts.GridMapping.Helpers;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Provides a editor window for editing drawing rules.
    /// </summary>
    public class DrawingRulesEditor : EditorWindow
    {
        /// <summary>
        /// Holds a list of rules.
        /// </summary>
        private readonly List<DrawRuleModel> rules = new List<DrawRuleModel>();

        /// <summary>
        /// Holds the value of the alternate list filter.
        /// </summary>
        private string alternateListFilter;

        /// <summary>
        /// Holds the current alternate list prefab selection.
        /// </summary>
        private GameObject alternatePrefab;

        /// <summary>
        /// Holds the scroll values for the alternate prefab list.
        /// </summary>
        private Vector2 alternatePrefabListScroll;

        /// <summary>
        /// Holds a value indicating whether or not prefabs are added to the alternate prefab list
        /// automatically simply by selecting them in the asset selection window.
        /// </summary>
        private bool autoAddAlternate;

        /// <summary>
        /// Holds a value indicating whether or not rules are added to the rule list
        /// automatically simply by selecting them in the asset selection window.
        /// </summary>
        private bool autoAddRule;

        /// <summary>
        /// Holds the scroll values for the rule description window.
        /// </summary>
        private Vector2 descriptionScroll;

        /// <summary>
        /// Holds the current working filename.
        /// </summary>
        private string fileName;

        /// <summary>
        /// Holds a value indicating whether or not changes have been made to the rule set.
        /// </summary>
        private bool isDirty;

        /// <summary>
        /// Used to determine how much area there is for drawing alternate prefabs.
        /// </summary>
        private Rect lastRect;

        /// <summary>
        /// Holds a reference to the currently selected prefab used for the rule.
        /// </summary>
        private GameObject prefab;

        /// <summary>
        /// Holds a reference to a <see cref="RecompileClass"/> in order to determine if unity recompiled scripts.
        /// </summary>
        private RecompileClass recompiled;

        /// <summary>
        /// Holds a cached array of user specified categories.
        /// </summary>
        private string[] ruleCategories;

        /// <summary>
        /// Holds the value of the rule list filter.
        /// </summary>
        private string ruleListFilter;

        /// <summary>
        /// Holds the scroll values for the rule list.
        /// </summary>
        private Vector2 ruleListScroll;

        /// <summary>
        /// Holds the selected index for the alternate prefab list.
        /// </summary>
        private int selectedAlternateIndex;

        /// <summary>
        /// Holds the current category selection.
        /// </summary>
        private int selectedCategoryIndex;

        /// <summary>
        /// Holds the selected index for the rule list.
        /// </summary>
        private int selectedRuleIndex = -1;

        /// <summary>
        /// Used to determine how the rule category is displayed.
        /// </summary>
        private bool showCategoryPopup;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingRulesEditor"/> class. 
        /// </summary>
        public DrawingRulesEditor()
        {
            this.fileName = null;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a rules entries have changed.
        /// </summary>
        private bool IsDirty
        {
            get
            {
                return this.isDirty;
            }

            set
            {
                // set is dirty flag
                this.isDirty = value;
#if TESTING
                if (this.autoSaveChanges && File.Exists(this.fileName))
                {
                    this.SaveData(false);
                } 
#endif
            }
        }
       
        /// <summary>
        /// Builds an array of boolean values representing neighbor selections.
        /// </summary>
        /// <param name="value">
        /// A string containing 1 and 0 characters indicating selection.
        /// </param>
        /// <returns>
        /// Returns an array of boolean values representing neighbor selections.
        /// </returns>
        public static bool[] BuildNeighborArray(string value)
        {
            var result = new bool[value.Length];
            for (var i = 0; i < value.Length; i++)
            {
                result[i] = value[i] == '1';
            }

            return result;
        }

        /// <summary>
        /// Loads rules from a file.
        /// </summary>
        /// <param name="file">The xml file to load the rules from.</param>
        /// <param name="reportErrors">If true will display a dialog with an error message if something went wrong.</param>
        /// <returns>Returns an array of <see cref="DrawRuleModel"/> types.</returns>
        public static DrawRuleModel[] LoadRulesFromFile(string file, bool reportErrors)
        {
            // Attempt to load the xml document
            var doc = new XmlDocument();
            try
            {
                doc.Load(file);
            }
            catch (Exception ex)
            {
                // report to user that could not read file
                if (reportErrors)
                {
                    EditorUtility.DisplayDialog("Error", "There was a problem reading the selected file.\r\n" + ex.Message, "Close");
                }

                return null;
            }

            // check to see if the root document name is what we expect
            if (doc.DocumentElement == null || doc.DocumentElement.LocalName != "rules")
            {
                if (reportErrors)
                {
                    EditorUtility.DisplayDialog("Error", "No root node or root element did not have the name \"rules\".", "Close");
                }
                
                return null;
            }

            var getObject = new Func<GameObject, XmlNode, GameObject>((go, node) =>
                {
                    if (go != null)
                    {
                        return go;
                    }

                    if (node == null || string.IsNullOrEmpty(node.InnerText))
                    {
                        return null;
                    }

                    var assetPath = node.InnerText.Trim();
                  
                    if (!File.Exists(assetPath))
                    {
                        return null;
                    }

                    var item = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
                    return item;
                });

            var buildAlternates = new Func<XmlNode, IEnumerable<PrefabListItem>>(node =>
                {
                    if (node == null)
                    {
                        return new PrefabListItem[0];
                    }

                    return from child in node.SelectNodes("alternate").OfType<XmlNode>()
                           let id = child.Attributes == null ? null : child.Attributes["id"]
                           where id != null && !string.IsNullOrEmpty(id.Value.Trim())
                           select new PrefabListItem
                                      {
                                          Prefab = getObject(AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(id.Value.Trim()), typeof(GameObject)) as GameObject, child.SelectSingleNode("prefabsource"))
                                      };
                });

            // query the xml elements and attempt to generate a model fo each entry
            var results = from item in doc.DocumentElement.ChildNodes.OfType<XmlNode>()
                          where item.LocalName == "rule"
                          let name = item.SelectSingleNode("name")
                          let enabled = item.SelectSingleNode("enabled")
                          let prefab = item.SelectSingleNode("prefab")
                          let upperneighbors = item.SelectSingleNode("upperneighbors")
                          let neighbors = item.SelectSingleNode("neighbors")
                          let lowerneighbors = item.SelectSingleNode("lowerneighbors")
                          let alternates = item.SelectSingleNode("alternates")
                          let alloworiginal = item.SelectSingleNode("alloworiginal")
                          let description = item.SelectSingleNode("description")
                          let category = item.SelectSingleNode("category")
                          select new DrawRuleModel
                          {
                              Name = name.InnerText,
                              Enabled = bool.Parse(enabled.InnerText.Trim()),
                              Prefab = string.IsNullOrEmpty(prefab.InnerText.Trim()) ? null : (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(prefab.InnerText.Trim()), typeof(GameObject)),
                              // ReSharper disable PossibleNullReferenceException
                              NeighborsUpperEnabled = bool.Parse(upperneighbors.Attributes["enabled"].InnerText.Trim()),
                              NeighborsUpper = BuildNeighborArray(upperneighbors.Attributes["states"].InnerText.Trim()),
                              Neighbors = BuildNeighborArray(neighbors.Attributes["states"].InnerText.Trim()),
                              NeighborsLowerEnabled = bool.Parse(lowerneighbors.Attributes["enabled"].InnerText.Trim()),
                              NeighborsLower = BuildNeighborArray(lowerneighbors.Attributes["states"].InnerText.Trim()),
                              // ReSharper restore PossibleNullReferenceException
                              Alternates = buildAlternates(alternates).ToList(),

                              AllowOriginal = bool.Parse(alloworiginal.InnerText.Trim()),
                              Description = description == null ? string.Empty : description.InnerText.Trim(),
                              Category = category == null ? string.Empty : category.InnerText.Trim()
                          };

            // attempt to process the linq query
            DrawRuleModel[] ruleModels;
            try
            {
                ruleModels = results.ToArray();
            }
            catch (Exception)
            {
                if (reportErrors)
                {
                    EditorUtility.DisplayDialog("Error", "The xml file may contain errors, missing values or values that can not be understood.", "Close");
                }

                return null;
            }

            return ruleModels;
        }

        /// <summary>
        /// Saves a series of rule models to a xml file.
        /// </summary>
        /// <param name="file">The file where the rule data will be saved to.</param>
        /// <param name="models">The rules that are to be saved.</param>
        public static void SaveRulesToFile(string file, IEnumerable<DrawRuleModel> models)
        {
            var data = "<?xml version=\"1.0\"?>\r\n<rules>\r\n{0}\r\n</rules>";
            var entries = string.Join("\r\n", models.Select(x => x.ToXml()).ToArray());
            data = string.Format(data, entries);

            // save data to a xml file
            File.WriteAllText(file, data);
        }

        /// <summary>
        /// Shows the <see cref="DrawingRulesEditor"/> window.
        /// </summary>
        /// <returns>Returns a reference to a <see cref="DrawingRulesEditor"/>.</returns>
        [MenuItem("Window/Codefarts/Grid Mapping/*BETA* Drawing Rules Editor")]
        public static DrawingRulesEditor ShowAutoMaterialCreationWindow()
        {
            var local = Localization.LocalizationManager.Instance;

            // get the window, show it, and hand it focus
            var window = GetWindow<DrawingRulesEditor>(local.Get("DrawingRulesTitle"));
            window.Show();

            return window;
        }

        /// <summary>
        /// Loads rule entries from a xml file.
        /// </summary>
        /// <param name="showDialog">
        /// If true will prompt the user to select a file to load the rule set from.
        /// </param>
        public void LoadData(bool showDialog)
        {
            string file;
            if (showDialog)
            {
                // prompt user to select a sync file
                file = EditorUtility.OpenFilePanel("Load rule data", string.Empty, "xml");
                if (string.IsNullOrEmpty(file))
                {
                    return;
                }

                // enforce directory separation characters (no alt chars)
                file = file.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }
            else
            {
                file = this.fileName;
            }

            // load the rule data from a xml file
            var ruleModels = LoadRulesFromFile(file, showDialog);

            // if null there may have been a problem reading the file       
            if (ruleModels == null)
            {
                return;
            }

            // start a new sync list. If changes have been made a dialog will prompt the user
            if (!this.StartNewList(showDialog))
            {
                return;
            }

            // add models to the table model list
            foreach (var model in ruleModels)
            {
                this.rules.Add(model);
            }

            // set the file name 
            this.fileName = file;
            this.selectedRuleIndex = 0;
            this.selectedAlternateIndex = 0;
            this.IsDirty = false;
        }

        /// <summary>
        /// Called by unity to draw the graphical user interface.
        /// </summary>
        public void OnGUI()
        {
            switch (Event.current.type)
            {
                case EventType.Ignore:
                    return;
            }

            // check if recompile occurred
            if (this.recompiled == null && !string.IsNullOrEmpty(this.fileName) && File.Exists(this.fileName))
            {
                this.LoadData(false);
                this.recompiled = new RecompileClass();
            }

            GUILayout.BeginVertical(); // main wrapper

            // draw the header buttons
            this.DrawHeaderButtons();

            // put some spacing between controls
            GUILayout.Space(8);

            GUILayout.BeginHorizontal(); // rule list and rule info

            // draw the rule list
            this.DrawRulesList();

            // put some spacing between controls
            GUILayout.Space(4);

            // draw information for the rule
            this.DrawRuleInfo();

            GUILayout.EndHorizontal(); // rule list and rule info

            GUILayout.EndVertical(); // main wrapper
        }

        /// <summary>
        /// Sets the current working filename for the rule editor.
        /// </summary>
        /// <param name="file">The file name where the rule set will be saved to.</param>
        public void SetFilename(string file)
        {
            this.fileName = file;
            this.IsDirty = true;
        }

        /// <summary>
        /// Displays a context menu containing the rule categories.
        /// </summary>
        public void ShowCategoryPopup()
        {
            if (this.ruleCategories == null || this.ruleCategories.Length == 0)
            {
                return;
            }

            var menu = new GenericMenu();
            foreach (var category in this.ruleCategories)
            {
                menu.AddItem(new GUIContent(category), false, cb => { this.ruleListFilter = (string)cb; }, category);
            }
            
            menu.ShowAsContext();
        }

        /// <summary>
        /// Adds a alternate prefab for the selected rule.
        /// </summary>
        /// <param name="rule">A reference to the rule where the alternate prefab will be added to.</param>
        private void AddAlternatePrefab(DrawRuleModel rule)
        {
            // do not allow null references to be added
            if (this.alternatePrefab == null)
            {
                return;
            }

            // if alternate prefab selection is not null and not already added then add it
            if (this.alternatePrefab != null)
            {
                rule.Alternates.Add(new PrefabListItem { Prefab = this.alternatePrefab });
            }

            // ensure that there is a alternate prefab selected
            this.selectedAlternateIndex = rule.Alternates.Count == 1 ? 0 : this.selectedAlternateIndex;

            // ensure the alternate prefab list is scrolled to view the added rule at the bottom of the list
            this.alternatePrefabListScroll.y = float.MaxValue;

            // we added a alternate prefab so record the rule change
            this.IsDirty = true;
        }

        /// <summary>
        /// Adds a new rule to the rule list.
        /// </summary>
        private void AddNewRule()
        {
            // add new rule to the list
            this.rules.Add(new DrawRuleModel { Prefab = this.prefab });

            // update selected index
            this.selectedRuleIndex = this.rules.Count - 1;

            // ensure the rule list is scrolled to view the added rule at the bottom of the list
            this.ruleListScroll.y = float.MaxValue;

            // we added a alternate prefab so record the rule change
            this.IsDirty = true;
        }

        /// <summary>
        /// Clones the currently selected rule from the rule list.
        /// </summary>
        private void CloneRule()
        {
            // ensure rules field is available
            if (this.rules == null || this.rules.Count == 0)
            {
                return;
            }

            // ensure that the selected rule index is still in range
            this.selectedRuleIndex = this.selectedRuleIndex >= this.rules.Count ? this.rules.Count - 1 : this.selectedRuleIndex;

            // if no selection exit
            if (this.selectedRuleIndex == -1)
            {
                return;
            }

            // get the selected rule
            var rule = this.rules[this.selectedRuleIndex];

            // add a cloned rule
            this.rules.Add(rule.Clone());

            // update selected index
            this.selectedRuleIndex = this.rules.Count - 1;

            // ensure the rule list is scrolled to view the added rule at the bottom of the list
            this.ruleListScroll.y = float.MaxValue;

            // the rules set has changed so set IsDirty
            this.IsDirty = true;
        }

        /// <summary>
        /// Used to draw a single alternate prefab item in the alternate prefab list.
        /// </summary>
        /// <param name="alternate">The reference to the alternate prefab.</param>
        private void DrawAlternateItem(PrefabListItem alternate)
        {
            // if alternate parameter is null do nothing
            if (alternate == null)
            {
                return;
            }

            // setup button style to use for each item in the list
            var buttonStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.UpperCenter, imagePosition = ImagePosition.ImageAbove };

            // get asset preview
            var assetPreview = alternate.Prefab == null ? null : AssetPreview.GetAssetPreview(alternate.Prefab);
            assetPreview = assetPreview == null ? AssetPreview.GetMiniTypeThumbnail(typeof(GameObject)) : assetPreview;

            GUILayout.BeginVertical(GUI.skin.box, GUILayout.MinWidth(154), GUILayout.MaxWidth(154)); // box

            // if there is something to show then show it
            if (!string.IsNullOrEmpty(alternate.Name) || alternate.Prefab != this.alternatePrefab)
            {
                GUILayout.BeginHorizontal(); // top name and update button   

                // show the rule name above
                // GUILayout.Label(alternate.name);
                alternate.IsChecked = GUILayout.Toggle(alternate.IsChecked, new GUIContent(alternate.Name));

                // provide a button to update the rule prefab to the currently selected prefab
                if (GUILayout.Button("S", GUILayout.ExpandWidth(false)))
                {
                    Selection.objects = new Object[] { alternate.Prefab };
                    EditorGUIUtility.PingObject(alternate.Prefab);
                }

                // provide a button to update the rule prefab to the currently selected prefab
                if (this.alternatePrefab != null && alternate.Prefab != this.alternatePrefab && GUILayout.Button("U", GUILayout.ExpandWidth(false)))
                {
                    alternate.Prefab = this.alternatePrefab;

                    // rule.Alternates[index] = alternate;
                    this.IsDirty = true;
                }

                GUILayout.EndHorizontal(); // top name and update button
            }

            // draw asset preview
            alternate.IsChecked = GUILayout.Toggle(alternate.IsChecked, new GUIContent(assetPreview), buttonStyle);

            GUILayout.EndVertical(); // box
        }

        /// <summary>
        /// Draw header controls for the alternate list.
        /// </summary>
        /// <param name="rule">A reference to the currently selected rule.</param>
        private void DrawAlternateListHeaderControls(DrawRuleModel rule)
        {
            // Draw controls for the alternate prefab list
            GUILayout.BeginHorizontal(); // control header bar
            var selectedPrefab = EditorGUILayout.ObjectField(this.alternatePrefab, typeof(GameObject), true, GUILayout.ExpandWidth(false)) as GameObject;
            if (selectedPrefab != this.alternatePrefab)
            {
                this.alternatePrefab = selectedPrefab;
                if (this.autoAddAlternate)
                {
                    this.AddAlternatePrefab(rule);
                }
            }

            // provides a add button to add the currently selected alternate prefab to the list
            if (GUILayout.Button("Add"))
            {
                this.AddAlternatePrefab(rule);
            }

            // provide a check box indicating whether or not alternate prefabs are added to the list simply by 
            // selecting them in the asset selection window
            this.autoAddAlternate = GUILayout.Toggle(this.autoAddAlternate, "Automatically add");
            GUILayout.FlexibleSpace();

            // provides a button for generating a preview of the selected alternate prefab in the off
            // chance unity is unable to generate a preview for us
            if (GUILayout.Button("Generate Preview"))
            {
                // not yet implemented!
                EditorUtility.DisplayDialog("Information", "Not implemented yet!", "Close");
            }

            // provides a remove button for removing alternate prefabs from the list
            if (GUILayout.Button("Remove"))
            {
                this.RemoveAlternatePrefab(rule);
            }

            GUILayout.EndHorizontal(); // control header bar

            // provide some spacing between the upper and lower controls
            GUILayout.Space(4);

            // draw a filter controls for filtering the list of displayed alternate prefabs
            GUILayout.BeginHorizontal(); // filtering controls
            GUILayout.FlexibleSpace();

            // draw filter label and text field
            GUILayout.Label("Filter", GUILayout.ExpandWidth(false));
            this.alternateListFilter = GUILayout.TextField(this.alternateListFilter ?? string.Empty, GUILayout.MinWidth(128));

            // provides a clear button to clear the filter
            if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
            {
                this.alternateListFilter = string.Empty;
            }

            // put some spacing between controls
            GUILayout.Space(8);

            // provide a none button to check all alternates
            if (GUILayout.Button("All"))
            {
                foreach (var alt in rule.Alternates)
                {
                    alt.IsChecked = true;
                }
            }

            // put some spacing between controls     
            GUILayout.Space(2);

            // provide a none button to uncheck all alternates
            if (GUILayout.Button("None"))
            {
                foreach (var alt in rule.Alternates)
                {
                    alt.IsChecked = false;
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal(); // filtering controls
        }

        /// <summary>
        /// Draws the alternate prefab list for a rule.
        /// </summary>
        /// <param name="rule">The source rule whose alternate prefabs will be displayed in the list.</param>
        private void DrawAlternatePrefabsList(DrawRuleModel rule)
        {
            GUILayout.BeginVertical();    // top encapsulation

            // draw the header
            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.FlexibleSpace();
            GUILayout.Label("Alternate prefabs to draw");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // draw alternate list controls
            this.DrawAlternateListHeaderControls(rule);

            // provide some spacing between the filter controls and the alternate prefab list
            GUILayout.Space(4);

            // draw alternate prefab list
            this.alternatePrefabListScroll = GUILayout.BeginScrollView(this.alternatePrefabListScroll, false, false, GUILayout.ExpandWidth(true));  // scroll view

            // calculate the number of columns to show
            var columns = (int)(this.lastRect.width / 154) - 1;
            columns = columns < 2 ? 2 : columns;

            GUILayout.BeginVertical();   // items in the list wrapper
            GUILayout.BeginHorizontal();  // rows
            var index = 0;
            var currentColumn = 0;
            while (index < rule.Alternates.Count)
            {
                // get reference to alternate prefab
                var alternate = rule.Alternates[index];

                // ensure alternate is not null and if it is remove it and continue
                if (alternate == null)
                {
                    rule.Alternates.RemoveAt(index);
                    continue;
                }

                // ensure that the reference is not null and that the name of the alternate prefab contains 
                // portions of the filter text if specified
                if (string.IsNullOrEmpty(this.alternateListFilter) || (!string.IsNullOrEmpty(alternate.Name) && alternate.Name.ToLower().Contains(this.alternateListFilter)))
                {
                    // draw the alternate item
                    this.DrawAlternateItem(alternate);

                    // determine if a different alternate prefab was selected
                    if (alternate.IsChecked && this.selectedAlternateIndex != index)
                    {
                        this.selectedAlternateIndex = index;
                    }
                }

                // check to see if we need to move to next row
                currentColumn++;
                if (currentColumn > columns)
                {
                    currentColumn = 0;
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal(); // end row
                    GUILayout.BeginHorizontal(); // start a new row     
                }

                // move to next item
                index++;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();   // rows
            GUILayout.EndVertical();   // items in the list wrapper

            GUILayout.EndScrollView();  // scroll view

            // calculate the layout width that we have to work with
            if (Event.current.type == EventType.Repaint)
            {
                this.lastRect = GUILayoutUtility.GetLastRect();
            }

            GUILayout.EndVertical();    // top encapsulation
        }

        /// <summary>
        /// Draws a grid of check boxes.
        /// </summary>
        /// <param name="neighbors">A array of neighbors states.</param>
        /// <param name="content">The content to use for each check box.</param>
        /// <returns>Returns the array of checked neighbor states.</returns>
        private bool[] DrawGrid(bool[] neighbors, GUIContent[] content)
        {
            // draw check box grid
            var values = ControlGrid.DrawCheckBoxGrid(neighbors, content, 3, GUI.skin.button, GUILayout.MaxWidth(32), GUILayout.MaxHeight(32));

            // determine if state changed and if so set IsDirty
            for (var i = 0; i < values.Length; i++)
            {
                if (values[i] != neighbors[i])
                {
                    this.IsDirty = true;
                }
            }

            return values;
        }

        /// <summary>
        /// Draws action buttons at the top of the window.
        /// </summary>
        private void DrawHeaderButtons()
        {
            GUILayout.BeginVertical();

            // draw the path and filename along the top
            GUILayout.BeginHorizontal();
            GUILayout.Label("File Name: ");
            GUI.SetNextControlName("txtFilename");
            GUILayout.TextArea(this.fileName ?? string.Empty, this.IsDirty ? "ErrorLabel" : GUI.skin.label);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();  // button bar
            GUILayout.FlexibleSpace();

            // draw new button for creating a new rule set
            GUI.SetNextControlName("btnNew");
            if (GUILayout.Button("New", GUILayout.MinHeight(32)))
            {
                this.StartNewList(true);
            }

            // provide a load button to load a existing rule set from a xml file
            GUI.SetNextControlName("btnload");
            if (GUILayout.Button("Load", GUILayout.MinHeight(32)))
            {
                this.LoadData(true);
            }

            // if there have been changes drew the save button otherwise keep it hidden
            GUI.SetNextControlName("btnSave");
            if (this.IsDirty && GUILayout.Button("Save", GUILayout.MinHeight(32)))
            {
                this.SaveData(false);
            }

            // provide a save as button to save the rule set as a different xml file
            GUI.SetNextControlName("btnSaveAs");
            if (GUILayout.Button("Save As", GUILayout.MinHeight(32)))
            {
                this.SaveData(true);
            }

            GUILayout.FlexibleSpace();

#if TESTING
            this.autoSaveChanges = GUILayout.Toggle(this.autoSaveChanges, "Auto save changes", GUILayout.ExpandWidth(false));
#endif
            GUILayout.EndHorizontal();  // button bar

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws the neighbor grids for the upper, lower and same layer neighbors.
        /// </summary>
        /// <param name="rule">The source rule to get the neighbor info from.</param>
        private void DrawNeighborGrids(DrawRuleModel rule)
        {
            GUILayout.BeginVertical();

            // draws a header for the neighbor grids
            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.FlexibleSpace();
            GUILayout.Label("Surrounding neighbors");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.MinHeight(125));
            GUILayout.FlexibleSpace();

            // setup default content for the upper and lower neighbor grids
            var emptyContent = new[]
                {
                    GUIContent.none, GUIContent.none, GUIContent.none, GUIContent.none, GUIContent.none, GUIContent.none,
                    GUIContent.none, GUIContent.none, GUIContent.none
                };

            GUILayout.BeginVertical(GUILayout.MinWidth(128));

            // provides a check box to indicate whether this rule takes into account the prefabs on the layer above
            GUI.SetNextControlName("chkUpperNeighbors");
            var enabled = GUILayout.Toggle(rule.NeighborsUpperEnabled, "Upper Neighbors");

            // determine if the state changed and if so set IsDirty
            if (enabled != rule.NeighborsUpperEnabled)
            {
                rule.NeighborsUpperEnabled = enabled;
                this.IsDirty = true;
            }

            // draw the upper neighbor grid
            rule.NeighborsUpper = this.DrawGrid(rule.NeighborsUpper, emptyContent);
            GUILayout.EndVertical();

            // put some spacing between grids
            GUILayout.Space(8);

            GUILayout.BeginVertical(GUILayout.MinWidth(128));
            GUILayout.Label("Neighbors", GUILayout.MinHeight(GUI.skin.toggle.CalcHeight(GUIContent.none, 100)));

            // setup neighbor grid content with center grid indicating the prefab that is drawn
            var image = AssetPreview.GetMiniTypeThumbnail(typeof(GameObject));
            var content = new[]
                {
                    GUIContent.none, GUIContent.none, GUIContent.none, GUIContent.none, new GUIContent(image),
                    GUIContent.none, GUIContent.none, GUIContent.none, GUIContent.none
                };

            // draw neighbor grid
            rule.Neighbors = this.DrawGrid(rule.Neighbors, content);
            rule.Neighbors[4] = false;
            GUILayout.EndVertical();

            // put some spacing between grids
            GUILayout.Space(8);

            // provides a check box to indicate whether this rule takes into account the prefabs on the layer below
            GUILayout.BeginVertical(GUILayout.MinWidth(128));
            GUI.SetNextControlName("chkLowerNeighbors");
            enabled = GUILayout.Toggle(rule.NeighborsLowerEnabled, "Lower Neighbors");

            // determine if the state changed and if so set IsDirty
            if (enabled != rule.NeighborsLowerEnabled)
            {
                rule.NeighborsLowerEnabled = enabled;
                this.IsDirty = true;
            }

            // draw lower neighbor grid
            rule.NeighborsLower = this.DrawGrid(rule.NeighborsLower, emptyContent);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Responsible for drawing rule name, description, asset path, enabled state etc.
        /// </summary>
        /// <param name="rule">The rule to draw the information for.</param>
        private void DrawRuleData(DrawRuleModel rule)
        {
            if (rule == null)
            {
                return;
            }

            GUILayout.BeginVertical();   // top wrapper

            // draws a header for the neighbor grids
            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.FlexibleSpace();
            GUILayout.Label("Rule information");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal(); // top check boxes
            GUILayout.FlexibleSpace();

            // provide a enabled check box and determine if its state changed and if so set IsDirty
            GUI.SetNextControlName("chkEnabled");
            var enabledState = GUILayout.Toggle(rule.Enabled, "Enabled", GUILayout.ExpandWidth(false));
            if (enabledState != rule.Enabled)
            {
                rule.Enabled = enabledState;
                this.IsDirty = true;
            }

            // put some spacing between controls
            GUILayout.Space(16);

            // provide check box indicating whether or not the prefab being drawn is also allowed to be considered for drawing
            GUI.SetNextControlName("chkAllowOriginal");
            enabledState = GUILayout.Toggle(rule.AllowOriginal, "Allow Original", GUILayout.ExpandWidth(false));

            // if its state changed set IsDirty
            if (enabledState != rule.AllowOriginal)
            {
                rule.AllowOriginal = enabledState;
                this.IsDirty = true;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal(); // top check boxes

            // show path to asset
            var sourcePrefab = Helpers.GetSourcePrefab(rule.Prefab);
            sourcePrefab = string.IsNullOrEmpty(sourcePrefab) ? string.Empty : sourcePrefab;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Asset Path:", GUILayout.ExpandWidth(false));
           
            GUI.SetNextControlName("txtAssetPath");
            GUILayout.TextArea(sourcePrefab);
            EditorGUIUtility.LookLikeControls();
            GUILayout.EndHorizontal();

            // show category controls
            GUILayout.BeginHorizontal();   // category

            // provide button for toggling category display style
            GUI.SetNextControlName("chkCategory");
            var categoryPopup = GUILayout.Toggle(this.showCategoryPopup, "Category", GUI.skin.button, GUILayout.ExpandWidth(false));
            if (categoryPopup != this.showCategoryPopup)
            {
                this.showCategoryPopup = categoryPopup;

                // try to find index
                var index = Array.IndexOf(this.ruleCategories, rule.Category, 0);
                if (index != -1)
                {
                    this.selectedCategoryIndex = index;
                }
            }

            // get list of categories
            if (this.showCategoryPopup)
            {
                GUI.SetNextControlName("CategoryPopup");
                var index = EditorGUILayout.Popup(this.selectedCategoryIndex, this.ruleCategories);

                // detect category selection change and if so update rule category
                if (index != this.selectedCategoryIndex)
                {
                    this.selectedCategoryIndex = index;
                    rule.Category = this.ruleCategories[index];
                    this.IsDirty = true;
                }
            }
            else
            {
                GUI.SetNextControlName("txtCategory");
                var value = GUILayout.TextField(rule.Category ?? string.Empty);
                if (value != rule.Category)
                {
                    rule.Category = value;
                    this.IsDirty = true;
                }
            }

            GUILayout.EndHorizontal();  // category

            // show rule name controls
            GUILayout.BeginHorizontal();
            GUILayout.Label("Rule Name:", GUILayout.ExpandWidth(false));
            GUI.SetNextControlName("txtRuleName");
            var ruleName = GUILayout.TextField(rule.Name ?? string.Empty);
            GUILayout.EndHorizontal();

            // if its state changed set IsDirty
            if (ruleName != rule.Name)
            {
                rule.Name = ruleName;
                this.IsDirty = true;
            }

            // provide controls to set the rule description and allow for scrolling
            GUILayout.Label("Description:");
            this.descriptionScroll = GUILayout.BeginScrollView(this.descriptionScroll, false, false);
            GUI.SetNextControlName("txtDescription");
            var description = GUILayout.TextArea(rule.Description ?? string.Empty);
            GUILayout.EndScrollView();

            // if its state changed set IsDirty
            if (description != rule.Description)
            {
                rule.Description = description;
                this.IsDirty = true;
            }

            GUILayout.EndVertical(); // top wrapper
        }

        /// <summary>
        /// Draws controls for editing the rule information.
        /// </summary>
        private void DrawRuleInfo()
        {
            // if no rules draw nothing and just exit
            if (this.rules == null || this.rules.Count == 0)
            {
                return;
            }

            // get selected rule
            var rule = this.rules[this.selectedRuleIndex];

            // draw rule data
            this.DrawRuleData(rule);

            // put some spacing between controls
            GUILayout.Space(8);

            // draw the neighbor grids
            this.DrawNeighborGrids(rule);

            // put some spacing between controls
            GUILayout.Space(8);

            // draw the alternate prefab list
            this.DrawAlternatePrefabsList(rule);

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Used to draw the controls at the top of the rule list.
        /// </summary>
        private void DrawRuleListHeaderControls()
        {
            GUILayout.BeginVertical();

            // draw an prefab selection control for the rule to be added and if auto adding is enabled then auto add a new rule
            // when a different prefab is selected from the asset selection window
            var prefabSelection = EditorGUILayout.ObjectField(this.prefab, typeof(GameObject), true) as GameObject;
            if (prefabSelection != this.prefab || (prefabSelection != null && this.prefab != null && prefabSelection.GetInstanceID() != this.prefab.GetInstanceID()))
            {
                this.prefab = prefabSelection;
                if (this.autoAddRule)
                {
                    this.AddNewRule();
                }
            }

            GUILayout.BeginHorizontal(); // rule list buttons

            // add button to ad new rules
            if (GUILayout.Button("Add"))
            {
                this.AddNewRule();
            }

            GUILayout.FlexibleSpace();

            if (this.rules != null && this.rules.Count > 0 && GUILayout.Button("Clone"))
            {
                this.CloneRule();
            }

            GUILayout.FlexibleSpace();

            // for removing rules from the list
            if (this.rules != null && this.rules.Count > 0 && GUILayout.Button("Remove"))
            {
                this.RemoveRule();
            }

            GUILayout.EndHorizontal(); // rule list buttons

            // check box for setting auto add 
            this.autoAddRule = GUILayout.Toggle(this.autoAddRule, "Auto add");

            // put some spacing between controls
            GUILayout.Space(4);

            // a generate preview button for generating a preview of a prefab is unity is unable to do so
            if (GUILayout.Button("Generate Preview"))
            {
                EditorUtility.DisplayDialog("Information", "Not implemented yet!", "Close");
            }

            // put some spacing between controls
            GUILayout.Space(4);

            // Provide a filter control where the user can find the rule they are looking for
            GUILayout.BeginHorizontal();
            GUILayout.Label("Filter", GUILayout.ExpandWidth(false));
            this.ruleListFilter = GUILayout.TextField(this.ruleListFilter ?? string.Empty);
            if (GUILayout.Button("C", GUILayout.ExpandWidth(false)))
            {
                this.ShowCategoryPopup();
            }

            if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
            {
                this.ruleListFilter = string.Empty;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws a rule item in the rule list.
        /// </summary>
        /// <param name="rule">A reference to the rule to be drawn.</param>
        /// <param name="index">The index of the rule being drawn.</param>      
        /// <returns>Returns true if the rule has been checked.</returns>
        private bool DrawRuleListItem(DrawRuleModel rule, int index)
        {
            // setup button style to use
            var buttonStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.UpperCenter, imagePosition = ImagePosition.ImageAbove };

            // get asset preview
            var assetPreview = rule.Prefab == null ? null : AssetPreview.GetAssetPreview(rule.Prefab);
            assetPreview = assetPreview == null ? AssetPreview.GetMiniTypeThumbnail(typeof(GameObject)) : assetPreview;

            // draw asset preview
            GUILayout.BeginVertical(GUI.skin.box); // entry list item

            // if there is something to show then show it
            if (!string.IsNullOrEmpty(rule.Name) || rule.Prefab != this.prefab)
            {
                GUILayout.BeginHorizontal(); // top name and update button   

                // show the rule name above
                GUILayout.Label(rule.Name);

                // provide a button to update the rule prefab to the currently selected prefab
                if (rule.Prefab != null && GUILayout.Button("S", GUILayout.ExpandWidth(false)))
                {
                    Selection.objects = new Object[] { rule.Prefab };
                    EditorGUIUtility.PingObject(rule.Prefab);
                }

                // provide a button to update the rule prefab to the currently selected prefab
                if (Helpers.GetSourcePrefab(rule.Prefab) != Helpers.GetSourcePrefab(this.prefab) && GUILayout.Button("U", GUILayout.ExpandWidth(false)))
                {
                    rule.Prefab = this.prefab;
                    this.IsDirty = true;
                }

                GUILayout.EndHorizontal(); // top name and update button
            }

            // draw the actual toggle button for the entry
            var isChecked = GUILayout.Toggle(this.selectedRuleIndex == index, new GUIContent(rule.Prefab == null ? string.Empty : rule.Prefab.name, assetPreview), buttonStyle);

            GUILayout.EndVertical(); // entry list item
            return isChecked;
        }

        /// <summary>
        /// Draws the rule list.
        /// </summary>
        private void DrawRulesList()
        {
            GUILayout.BeginVertical(GUILayout.MinWidth(175), GUILayout.MaxWidth(175));

            // draw controls for the rule list
            this.DrawRuleListHeaderControls();

            // put some spacing between controls
            GUILayout.Space(4);

            // setup comparer
            var comparer =
                new EqualityComparerCallback<string>(
                    (a, b) =>
                    string.Compare(
                        a == null ? string.Empty : a.Trim(),
                        b == null ? string.Empty : b.Trim(),
                        StringComparison.OrdinalIgnoreCase) == 0);

            // draw prefabs
            this.ruleListScroll = GUILayout.BeginScrollView(this.ruleListScroll, false, false);
            var index = 0;
            this.ruleCategories = new string[1000];
            var categoryCount = 0;
            while (this.rules != null && index < this.rules.Count)
            {
                // get a reference to the rule
                var rule = this.rules[index];

                // before even determining whether to filter the rule be sure to update rule category list with the rules category
                if (!string.IsNullOrEmpty(rule.Category ?? string.Empty))
                {
                    // check if already exists
                    if (!this.ruleCategories.Contains(rule.Category, comparer))
                    {
                        this.ruleCategories[categoryCount++] = rule.Category;

                        // ensure there is room to store more categories in the list
                        if (categoryCount > this.ruleCategories.Length - 1)
                        {
                            Array.Resize(ref this.ruleCategories, this.ruleCategories.Length + 1000);
                        }
                    }
                }

                // if a filter is specified then filter out the rule
                if (string.IsNullOrEmpty(this.ruleListFilter) ||
                    (!string.IsNullOrEmpty(rule.Category) && rule.Category.ToLower().Contains(this.ruleListFilter.ToLower())) ||
                    (!string.IsNullOrEmpty(rule.Name) && rule.Name.ToLower().Contains(this.ruleListFilter.ToLower())) ||
                    (rule.Prefab != null && rule.Prefab.name.ToLower().Contains(this.ruleListFilter.ToLower())))
                {
                    // draw the rule
                    var isChecked = this.DrawRuleListItem(rule, index);

                    // if the item was checked and was not currently the selected item update the selected index
                    if (isChecked && this.selectedRuleIndex != index)
                    {
                        this.selectedRuleIndex = index;

                        // try to find category index
                        var catIndex = Array.IndexOf(this.ruleCategories, rule.Category, 0);
                        if (catIndex != -1)
                        {
                            this.selectedCategoryIndex = catIndex;
                        }
                    }
                }

                // move to next item
                index++;
            }

            // trim off any excess category slots
            Array.Resize(ref this.ruleCategories, categoryCount);

            // make sure selected category index is still in range
            this.selectedCategoryIndex = this.selectedCategoryIndex > this.ruleCategories.Length - 1 ? this.ruleCategories.Length - 1 : this.selectedCategoryIndex;

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }

        /// <summary>
        /// Removes the currently selected alternate prefab.
        /// </summary>
        /// <param name="rule">The rule whose alternate prefab will be removed.</param>
        private void RemoveAlternatePrefab(DrawRuleModel rule)
        {
            if (rule.Alternates == null)
            {
                return;
            }

            var index = 0;
            while (index < rule.Alternates.Count)
            {
                if (rule.Alternates[index].IsChecked)
                {
                    rule.Alternates.RemoveAt(index);

                    // the rules set has changed so set IsDirty       
                    this.IsDirty = true;

                    continue;
                }

                index++;
            }
        }

        /// <summary>
        /// Removes a rule from the rule list.
        /// </summary>
        private void RemoveRule()
        {
            // if the selected rule index is in range remove it from the list
            if (this.rules.Count > 0 && this.selectedRuleIndex < this.rules.Count)
            {
                var settings = SettingsManager.Instance;
                var local = Localization.LocalizationManager.Instance;
                if (settings.GetSetting(GlobalConstants.DrawingRulesConfirmationKey, true) &&
                    !EditorUtility.DisplayDialog(local.Get("Warning"), local.Get("AreYouSure"), local.Get("Accept"), local.Get("Cancel")))
                {
                    return;
                }

                this.rules.RemoveAt(this.selectedRuleIndex);
            }

            // ensure that the selected rule index is still in range
            this.selectedRuleIndex = this.selectedRuleIndex >= this.rules.Count ? this.rules.Count - 1 : this.selectedRuleIndex;

            // the rules set has changed so set IsDirty
            this.IsDirty = true;
        }

        /// <summary>
        /// Save the current entries to disk.
        /// </summary>
        /// <param name="saveNew">
        /// If true will prompt the user to save the rule set to a different filename..
        /// </param>
        private void SaveData(bool saveNew)
        {
            // if filename not set or saveNew is true then prompt user to specify a save location
            if (string.IsNullOrEmpty(this.fileName) || saveNew)
            {
                // prompt user
                var local = Localization.LocalizationManager.Instance;
                var file = EditorUtility.SaveFilePanel(local.Get("SaveRuleData"), string.Empty, "Rules.xml", "xml");
                if (file.Length == 0)
                {
                    // user canceled so just exit
                    return;
                }

                // enforce directory separation characters (no alt chars)
                file = file.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

                // save file name
                this.fileName = file;
            }

            // generate xml markup from the rule data
            SaveRulesToFile(this.fileName, this.rules);

            // clear the isdirty flags  
            this.IsDirty = false;

            // refresh the asset data base to ensure any changes are picked up right away
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Starts a new rule set.
        /// </summary>
        /// <param name="showDialog">If true will prompt the user if they are sure.</param>
        /// <returns>Returns true if a new list has been started.</returns>
        private bool StartNewList(bool showDialog)
        {
            var settings = SettingsManager.Instance;
            var local = Localization.LocalizationManager.Instance;
            var showConfirmation = settings.GetSetting(GlobalConstants.DrawingRulesConfirmationKey, true);

            // prompt the user is they are sure they want to start a new rule set but only if there have been changes
            if (this.IsDirty && showDialog && showConfirmation && !EditorUtility.DisplayDialog(local.Get("Warning"), local.Get("AreYouSure"), local.Get("Accept"), local.Get("Cancel")))
            {
                return false;
            }

            // clear variables
            this.rules.Clear();
            this.fileName = null;
            this.IsDirty = false;
            return true;
        }

        /// <summary>
        /// provides a class used for detecting when unity recompiles
        /// </summary>
        private class RecompileClass
        {
        }

#if TESTING
        /// <summary>
        /// Holds a value that determines whether or not changes are automatically saved to the current working filename.
        /// </summary>
        private bool autoSaveChanges;
#endif
    }
}