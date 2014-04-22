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
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Codefarts.Localization;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Used to draw controls that allow the user to select a folder
    /// </summary>
    public class SelectOutputFolderControl
    {
        #region Fields

        /// <summary>
        /// Used to hold a last of asset folders
        /// </summary>
        private readonly FolderCache folderCache;

        /// <summary>
        /// Holds a reference to the output path where materials will be generated.
        /// </summary>
        [SerializeField]
        private string outputPath = string.Empty;

        /// <summary>
        /// Stores the current output folder selection index is <see cref="ShowAsList"/> is true.
        /// </summary>
        [SerializeField]
        private int selectedFolderIndex;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectOutputFolderControl"/> class. 
        /// </summary>
        public SelectOutputFolderControl()
        {
            // setup folder cache
            this.folderCache = new FolderCache { RootFolder = "Assets/", Seconds = 2 };
            this.ShowAsListCheckBox = true;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Provides an event for when the output path changes.
        /// </summary>
        public event EventHandler OutputPathChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a path to the folder that has been specified.
        /// </summary>
        public string OutputPath
        {
            get
            {
                return this.outputPath;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to show the output.
        /// </summary>
        public bool ShowAsList;

        /// <summary>
        /// Gets or sets a value indicating whether or not the "AsList" check box will be visible.
        /// </summary>
        public bool ShowAsListCheckBox;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Used to setup the output folder controls.
        /// </summary>
        public void Draw()
        {
            // get reference to localization manager
            var local = LocalizationManager.Instance;

            // provide controls for selecting a output path
            GUILayout.BeginHorizontal();
            GUILayout.Label(local.Get("OutputPath"));
            GUILayout.FlexibleSpace();
            var asList = false;
            if (this.ShowAsListCheckBox)
            {
                asList = GUILayout.Toggle(this.ShowAsList, local.Get("AsList"));
            }

            GUILayout.EndHorizontal();

            // check is wanting to display as list or text field
            if (asList)
            {
                // get list of project asset folders for popup list
                var folders = this.folderCache.GetFolders().OrderBy(s => s).ToArray();

                // if previous state was text field attempt to find the specified folder name in the list and set the selection index to match
                if (!this.ShowAsList)
                {
                    // check if output path is specified
                    this.outputPath = this.outputPath.Trim();
                    if (string.IsNullOrEmpty(this.outputPath))
                    {
                        // reset selected folder index and attempt to set output path from the array of available folders
                        this.selectedFolderIndex = 0;
                        if (folders.Length != 0)
                        {
                            this.SetOutputPath(folders[this.selectedFolderIndex]);
                        }
                    }
                    else
                    {
                        // try to find user specified folder from the output path field in the popup list of folders and set the index to it
                        for (int i = 0; i < folders.Length; i++)
                        {
                            if (string.Compare(this.outputPath, folders[i], true, CultureInfo.InvariantCulture) != 0)
                            {
                                continue;
                            }

                            this.selectedFolderIndex = i;
                            break;
                        }
                    }
                }

                // draw popup list  of project folders
                var index = EditorGUILayout.Popup(this.selectedFolderIndex, folders);
                if (index != this.selectedFolderIndex)
                {
                    this.selectedFolderIndex = index;
                    this.SetOutputPath(folders[this.selectedFolderIndex]);
                }
            }
            else
            {
                // check if user wants to show text field and previous state was to show list
                if (this.ShowAsList)
                {
                    // set output folder based on list selection
                    var folders = this.folderCache.GetFolders().OrderBy(x => x).ToArray();
                    this.SetOutputPath(folders[this.selectedFolderIndex]);
                }

                GUILayout.BeginHorizontal();

                // display output folder controls
                var tempPath = EditorGUILayout.TextField(this.outputPath);
                this.SetOutputPath(tempPath);

                if (GUILayout.Button("...", GUILayout.MaxWidth(40)))
                {
                    this.DoSelectOutputPath();
                }

                GUILayout.EndHorizontal();
            }

            // save as list state
            this.ShowAsList = asList;
        }

        /// <summary>
        /// Sets the output path.
        /// </summary>
        /// <param name="path">
        /// The value to set the output path to.
        /// </param>
        /// <remarks>
        /// Invalid characters in <see cref="path"/> will be removed and will also prevent rooted paths, and paths that are 
        /// outside of the projects "Assets" folder.
        /// </remarks>
        public void SetOutputPath(string path)
        {
            path = path == null ? string.Empty : path.Trim();
            var invalidPathChars = Path.GetInvalidPathChars();
            var index = path.IndexOfAny(invalidPathChars);
            while (index != -1)
            {
                path = path.Remove(index, 1);
                index = path.IndexOfAny(invalidPathChars);
            }

            if (Path.IsPathRooted(path))
            {
                var assetsFolder = Path.Combine(Environment.CurrentDirectory, "Assets" + Path.AltDirectorySeparatorChar);
                path = path.Replace(assetsFolder.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), string.Empty);
            }

            // get assets full path
            if (!string.IsNullOrEmpty(path))
            {
                var rootPath = Path.GetFullPath(this.folderCache.RootFolder);
                var specifiedPath = Path.GetFullPath(Path.Combine("Assets", path));
                if (!specifiedPath.StartsWith(rootPath))
                {
                    path = string.Empty;
                }
            }

            if (path == this.outputPath)
            {
                return;
            }

            this.outputPath = path;
            if (this.OutputPathChanged != null)
            {
                this.OutputPathChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Used to select a output path where materials will be saved to.
        /// </summary>
        private void DoSelectOutputPath()
        {
            var local = LocalizationManager.Instance;

            // prompt the user to select a path
            var tempPath = EditorUtility.OpenFolderPanel(local.Get("SelectOutputPath"), this.outputPath, string.Empty);
            this.SetOutputPath(tempPath);
        }

        #endregion
    }
}