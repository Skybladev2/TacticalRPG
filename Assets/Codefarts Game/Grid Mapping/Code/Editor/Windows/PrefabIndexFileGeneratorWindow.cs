// <copyright>
//   Copyright (c) 2012 Codefarts
//   All rights reserved.
//   contact@codefarts.com
//   http://www.codefarts.com
// </copyright>

namespace Codefarts.GridMapping.Editor
{
    using System;
    using System.IO;
    using System.Linq;

    using Codefarts.GridMapping.Editor.Controls;
    using Codefarts.Utilities;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    /// <summary>
    ///     Provides a editor window for quickly selecting rotation values.
    /// </summary>
    public class PrefabIndexFileGeneratorWindow : EditorWindow
    {
        private readonly SelectOutputFolderControl output = new SelectOutputFolderControl();

        private string category = string.Empty;

        private string data = string.Empty;

        private Vector2 scroll;

        private string separator = ", ";

        private bool detectResourcesFolder = true;

        private string fileName = string.Empty;

        private bool appendResults = false;

        #region Public Methods and Operators

        /// <summary>
        ///     Called by unity to draw the user interface.
        /// </summary>
        public void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();

            GUILayout.BeginVertical(GUILayout.ExpandWidth(false));
            this.output.Draw();
            GUILayout.EndVertical();

            this.DrawFilename();
            this.DrawButtonControls();
            GUILayout.EndVertical();

            this.DrawCategoryAndSeparatorControls();
            GUILayout.EndHorizontal();

            // draw the generated data
            this.scroll = GUILayout.BeginScrollView(this.scroll, true, true);
            this.data = EditorGUILayout.TextArea(this.data);  // this works as expected IE no word wrap
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }

        private void DrawFilename()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(Localization.LocalizationManager.Instance.Get("Filename"), GUILayout.ExpandWidth(false));
            this.fileName = EditorGUILayout.TextField(this.fileName);
            GUILayout.EndHorizontal();
        }

        private void DrawButtonControls()
        {
            var local = Localization.LocalizationManager.Instance;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(local.Get("Generate"), GUILayout.Height(32)))
            {
                if (!this.appendResults)
                {
                    this.data = local.Get("PrefabIndexFileDescriptor") + "\r\n";
                }

                if (!string.IsNullOrEmpty(this.separator.Trim()))
                {
                    this.data += local.Get("SeparatorComment") + " " + this.separator + "\r\n";
                }

                if (!string.IsNullOrEmpty(this.category.Trim()))
                {
                    this.data += local.Get("CategoryComment") + " " + this.category + "\r\n";
                }

                // attempt to generate the data
                try
                {
                    var selectedAsset = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
                    foreach (var file in selectedAsset.Select(path => AssetDatabase.GetAssetPath(path.GetInstanceID())).Where(file => File.Exists(file)))
                    {
                        var properPath = this.GetProperPath(file);
                        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(properPath);
                        var correctedPath = Path.Combine(Path.GetDirectoryName(properPath), fileNameWithoutExtension).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        this.data += correctedPath + this.separator + this.separator + Path.GetFileNameWithoutExtension(file) + "\r\n";
                    }
                }
                catch (Exception ex)
                {
                    EditorUtility.DisplayDialog(local.Get("Error"), local.Get("ERR_ErrorGeneratingPrefabIndexData"), local.Get("Close"));
                    Debug.LogError(ex);
                }
            }

            if (GUILayout.Button(local.Get("Save"), GUILayout.Height(32)))
            {
                // if a file name was specified check it
                if (this.fileName.Trim() != string.Empty)
                {
                    var filePath = Path.Combine(Path.Combine("Assets", this.output.OutputPath), this.fileName);
                    filePath = Path.ChangeExtension(filePath, ".txt");
                    File.WriteAllText(filePath, this.data);
                    AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceUpdate);
                    Debug.Log(string.Format(local.Get("GeneratedPrefabIndexFileAt"), filePath));
                }
                else
                {
                    Debug.Log(local.Get("ERR_NoPrefabIndexFileGeneratedNoFilename"));
                }
            }

            if (GUILayout.Button(local.Get("Copy"), GUILayout.Height(32)))
            {
                Clipboard.SetText(this.data);
            }

            GUILayout.EndHorizontal();
        }

        private string GetProperPath(string path)
        {
            if (!this.detectResourcesFolder)
            {
                return path;
            }

            var parts = path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            var i = -1;
            while (i + 1 < parts.Length)
            {
                if (parts[i + 1].Trim().ToLower() == "resources")
                {
                    return i + 2 > parts.Length - 1 ? string.Empty : string.Join(Path.DirectorySeparatorChar.ToString(), parts, i + 2, parts.Length - (i + 2));
                }

                i++;
            }

            return path;
        }

        private void DrawCategoryAndSeparatorControls()
        {
            var local = Localization.LocalizationManager.Instance;

            GUILayout.BeginVertical(GUILayout.MinWidth(this.position.width / 2));

            GUILayout.BeginVertical();
            GUILayout.Label(local.Get("Category"));
            this.category = GUILayout.TextField(this.category);
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label(local.Get("Separator"));
            this.separator = GUILayout.TextField(this.separator);
            this.separator = string.IsNullOrEmpty(this.separator.Trim()) ? ", " : this.separator;
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            this.detectResourcesFolder = EditorGUILayout.Toggle(local.Get("StripResourcesFolder"), this.detectResourcesFolder, GUILayout.ExpandWidth(false));
            this.appendResults = EditorGUILayout.Toggle(local.Get("AppendGeneratedPrefabIndexData"), this.appendResults, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        #endregion

        /// <summary>
        /// Displays a <see cref="PrefabIndexFileGeneratorWindow"/> window.
        /// </summary>
        [MenuItem("Window/Codefarts/Grid Mapping/Prefab Index File Generator")]
        public static void ShowWindow()
        {
            var local = Localization.LocalizationManager.Instance;
            var window = GetWindow<PrefabIndexFileGeneratorWindow>();
            window.title = local.Get("PrefabIndexFileGeneratorTitle");
            window.Show();
        }
    }
}