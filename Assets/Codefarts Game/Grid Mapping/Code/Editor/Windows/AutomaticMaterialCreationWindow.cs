/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.GridMapping.Windows
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.GridMapping.Common;
    using Codefarts.GridMapping.Editor;
    using Codefarts.GridMapping.Editor.Controls;
    using Codefarts.GridMapping.Models;
    using Codefarts.GridMapping.Utilities;

    using UnityEditor;

    using UnityEngine;

    using Color = UnityEngine.Color;
    using Vector2 = UnityEngine.Vector2;

    /// <summary>
    /// Provides a windows for handling automatic material creation.
    /// </summary>
    public class AutomaticMaterialCreationWindow : EditorWindow
    {
        /// <summary>
        /// Holds a series of images for each selection.
        /// </summary>
        private readonly Dictionary<string, GenericImage<Color>> images;

        /// <summary>
        /// Holds a reference to a control used to draw the main preview area.
        /// </summary>
        private readonly TileSelectionControl mainPreview;

        /// <summary>
        /// Holds a reference to a control used to draw the selection controls.
        /// </summary>
        private readonly GenericMaterialCreationControl materialControls;

        /// <summary>
        /// Holds a list of selection information that the user has made.
        /// </summary>
        private readonly Dictionary<string, List<TileSelectionModel>> tileMaterials;

        /// <summary>
        /// Holds the scroll value for the selected materials list.
        /// </summary>
        private Vector2 materialScroll;

        /// <summary>
        /// Holds the index for the currently selected material.
        /// </summary>
        private int materialSelectionIndex;

        /// <summary>
        /// Holds the Guid for the currently selected texture.
        /// </summary>
        private string selectedTextureId;

        /// <summary>
        /// Holds a value indicating whether the material list is displayed.
        /// </summary>
        private bool showMaterials;

        /// <summary>
        /// Holds a value indicating whether or not to show texture previews.
        /// </summary>
        private bool showTextures;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticMaterialCreationWindow"/> class.
        /// </summary>
        public AutomaticMaterialCreationWindow()
        {
            // setup controls and variables
            this.tileMaterials = new Dictionary<string, List<TileSelectionModel>>();
            this.images = new Dictionary<string, GenericImage<Color>>();
            this.materialControls = new GenericMaterialCreationControl();
            this.mainPreview = new TileSelectionControl();

            // hook into the before spacing drawn event to allow us to customize the look
            this.materialControls.BeforeSpacingDrawn = () =>
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                };

            // hook into the after spacing drawn event to allow us to customize the look
            this.materialControls.AfterSpacingDrawn = () =>
            {
                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            };

            // draw a show materials toggle button after the texture controls have been drawn
            this.materialControls.AfterTextureDrawn = () =>
            {
                this.showMaterials = GUILayout.Toggle(this.showMaterials, "Show\r\nMaterials", GUI.skin.button);
            };

            // hook into the tile selection event on the main preview control
            this.mainPreview.TileSelection += this.MainPreviewTileSelection;

            // Hook into the texture changed event and update the texture and selected texture id's
            this.materialControls.TextureChanged += (s, e) =>
            {
                this.mainPreview.TextureAsset = this.materialControls.TextureAsset;
                this.selectedTextureId = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetOrScenePath(this.materialControls.TextureAsset));
            };

            // ensure preview texture size is updated
            this.materialControls.TileSizeChanged += (s, e) =>
            {
                this.mainPreview.TileHeight = this.materialControls.TileHeight;
                this.mainPreview.TileWidth = this.materialControls.TileWidth;
            };

            // hook into events to sync control properties
            this.materialControls.StartSpacingChanged += (s, e) => { this.mainPreview.StartSpacing = this.materialControls.StartSpacing; };
            this.materialControls.SpacingChanged += (s, e) => { this.mainPreview.Spacing = this.materialControls.Spacing; };
            this.materialControls.FreeformChanged += (s, e) => { this.mainPreview.FreeForm = this.materialControls.FreeForm; };

            // sync control properties
            this.mainPreview.Refresh += (s, e) => this.Repaint();
            this.mainPreview.StartSpacing = this.materialControls.StartSpacing;
            this.mainPreview.Spacing = this.materialControls.Spacing;
            this.mainPreview.FreeForm = this.materialControls.FreeForm;

            // get grid mapping service and hook into the prefab drawn event
            var gridMappingService = GridMappingService.Instance;
            gridMappingService.PrefabDrawn += this.GridMappingService_PrefabDrawn;
        }

        /// <summary>
        /// Displays a <see cref="AutomaticMaterialCreationWindow"/> window.
        /// </summary>
        [MenuItem("Window/Codefarts/Grid Mapping/*BETA* Automatic Material Creation")]
        public static void ShowAutoMaterialCreationWindow()
        {
            var local = Localization.LocalizationManager.Instance;
            var window = GetWindow<AutomaticMaterialCreationWindow>();
            window.title = local.Get("AutomaticMaterialCreation");
            window.Show();
        }

        /// <summary>
        /// Called by unity when the windows is enabled.
        /// </summary>
        public void OnEnable()
        {
            // load available shaders for shader popup list
            var settings = SettingsManager.Instance;
            this.materialControls.ShaderNames.Clear();

            // setup a default shader array to use if unable to retrieve the setting
            var defaultShaderList = new[] { "Diffuse", "Transparent/Diffuse", "Transparent/Cutout/Diffuse", "Transparent/Cutout/Soft Edge Unlit" };

            // try to get the shader array from settings
            var strings = settings.GetSetting(GlobalConstants.AutoMaterialCreationShadersKey, string.Join("\r\n", defaultShaderList));

            // get value on how the shader list looks
            var listType = settings.GetSetting(GlobalConstants.AutoMaterialCreationShaderListStyleKey, 0);

            // set the shaders that will be available in the shader pop-up
            this.materialControls.ShaderNames.AddRange(strings.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => (listType == 0 ? line : line.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)).Trim()));
        }

        /// <summary>
        /// Called by unity for drawing the graphical user interface.
        /// </summary>
        public void OnGUI()
        {
            try
            {
                GUILayout.BeginVertical();
                this.materialControls.Draw();

                if (this.showMaterials)
                {
                    this.DrawMaterials();
                }

                this.mainPreview.Draw();
                GUILayout.EndVertical();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// Draws controls for the materials that were auto generated.
        /// </summary>
        private void DrawMaterials()
        {
            // setup button style to use for each item in the list
            var buttonStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.UpperCenter, imagePosition = ImagePosition.ImageAbove, fixedWidth = 64, fixedHeight = 64 };

            // get reference to the selected texture if any
           // var texture = EditorUtility.InstanceIDToObject(this.selectedTextureId) as Texture2D;
            var texture = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(this.selectedTextureId)) as Texture2D;

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();  // save delete buttons

            // if a texture is specified then show the save button
            if (texture != null && this.tileMaterials.ContainsKey(this.selectedTextureId) && GUILayout.Button("Save"))
            {
                // save the selected material is the save button was clicked
                this.SaveMaterial();
            }

            // draw selected texture name
            GUILayout.FlexibleSpace();
            GUILayout.Label(texture == null ? string.Empty : texture.name);
            GUILayout.FlexibleSpace();

            // provide a delete button
            if (GUILayout.Button("Delete"))
            {
            }

            GUILayout.EndHorizontal(); // save delete buttons

            GUILayout.BeginHorizontal(GUILayout.MinHeight(85), GUILayout.MaxHeight(85));
            GUILayout.Space(4);
            this.showTextures = GUILayout.Toggle(this.showTextures, ">", GUI.skin.button, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true));
            this.materialScroll = GUILayout.BeginScrollView(this.materialScroll, false, false);
            GUILayout.BeginHorizontal();

            // check if the user is wanting to show the textures
            if (this.showTextures)
            {
                foreach (var pair in this.tileMaterials)
                {
                   // texture = EditorUtility.InstanceIDToObject(pair.Key) as Texture2D;
                    texture = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(pair.Key)) as Texture2D;
                    if (texture == null)
                    {
                        continue;
                    }

                    var assetPreview = AssetPreview.GetAssetPreview(texture) != null ? AssetPreview.GetAssetPreview(texture) : AssetPreview.GetMiniTypeThumbnail(typeof(Texture2D));
                    if (GUILayout.Button(new GUIContent(texture.name, assetPreview), buttonStyle))
                    {
                        this.selectedTextureId = pair.Key;
                        this.showTextures = false;
                        this.materialControls.TextureAsset = texture;
                    }
                }
            }
            else
            {
                if (this.tileMaterials.ContainsKey(this.selectedTextureId))
                {
                    var list = this.tileMaterials[this.selectedTextureId];
                    for (var i = 0; i < list.Count; i++)
                    {
                        var tile = list[i];
                        var result = GUILayout.Toggle(this.materialSelectionIndex == i, new GUIContent(tile.Material.name, tile.TexturePreview), buttonStyle);
                        if (!result)
                        {
                            continue;
                        }

                        // update material selection index
                        this.materialSelectionIndex = i;

                        // set active material
                        GridMappingService.Instance.CurrentMaterial = tile.Material;

                        // set main preview information
                        this.mainPreview.TileWidth = tile.TileSize.Width;
                        this.mainPreview.TileHeight = tile.TileSize.Height;
                        this.mainPreview.SelectTiles(new Rect(tile.Min.X, tile.Min.Y, tile.TileSize.Width, tile.TileSize.Height));
                    }
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Used to handle the <see cref="GridMappingService.PrefabDrawn"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="DrawPrefabArgs"/> that contains the event data.</param>
        private void GridMappingService_PrefabDrawn(object sender, DrawPrefabArgs e)
        {
        }

        /// <summary>
        /// Handles the event for tile selection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        private void MainPreviewTileSelection(object sender, TileSelectionEventArgs e)
        {
            // do nothing until selection has completed or if no texture selected 
            if (e.Status != TileSelectionStatus.Complete || this.materialControls.TextureAsset == null)
            {
                return;
            }

            // check if already has entry for a texture
            // var textureId = this.materialControls.TextureAsset.GetInstanceID();
            var textureId = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetOrScenePath(this.materialControls.TextureAsset));
            if (!this.tileMaterials.ContainsKey(textureId))
            {
                // no entry so add one
                this.tileMaterials.Add(textureId, new List<TileSelectionModel>());

                // generate a generic image for the texture so it can be used for previews
                this.images.Add(textureId, this.materialControls.TextureAsset.CreateGenericImage());
            }

            // get entry list
            var list = this.tileMaterials[textureId];

            // get tile size, shader name & color
            var size = new Size(this.mainPreview.TileWidth, this.mainPreview.TileHeight);
            var shaderName = this.materialControls.SelectedShaderName;
            var color = this.materialControls.MaterialColor;

            var settings = SettingsManager.Instance;
            var shaderListStyle = settings.GetSetting(GlobalConstants.AutoMaterialCreationShaderListStyleKey, 0);

            // check for matching top/left bottom/right, size, shader name & color values
            var result = list.FirstOrDefault(x => e.Min == x.Min &&
                                                  e.Max == x.Max &&
                                                  size == x.TileSize &&
                                                  shaderName == x.ShaderName &&
                                                  color == x.Color);

            var local = Localization.LocalizationManager.Instance;
            if (result == null)
            {
                // attempt to create a a new material 
                var model = new TileSelectionModel();
                try
                {
                    shaderName = shaderListStyle == 1 ? shaderName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) : shaderName;
                    var shader = Shader.Find(shaderName);
                    model.Material = new Material(shader);
                }
                catch (Exception ex)
                {
                    // report error
                    EditorUtility.DisplayDialog(local.Get("Warning"), ex.Message, local.Get("Close"));
                    return;
                }

                // get texture co-ordinates for the material
                var textureCoordinates = this.mainPreview.GetTextureCoordinates(this.materialControls, 0);
                model.Material.mainTextureOffset = new Vector2(textureCoordinates.xMin, textureCoordinates.yMin);
                model.Material.mainTextureScale = new Vector2(textureCoordinates.width, textureCoordinates.height);
                model.Material.mainTexture = this.materialControls.TextureAsset;
                model.Material.color = color;

                model.Min = e.Min;
                model.Max = e.Max;
                model.IsDirty = true;
                model.TileSize = size;
                model.ShaderName = shaderName;

                // set model preview texture
                var image = model.Material.ToGenericImage();
                image.Tint(color, TintType.Alpha);
                model.TexturePreview = image.ToTexture2D();

                list.Add(model);
                result = model;

                // save material selections to settings to keep a record of selections
                this.SaveToSettings(result);
            }

            // set current material
            GridMappingService.Instance.CurrentMaterial = result.Material;

#if DEBUG
            Debug.Log(string.Format("{0} textures {1} unique", this.tileMaterials.Count, this.tileMaterials.Sum(x => x.Value.Count)));
#endif
        }

        /// <summary>
        /// Saves the currently selected material.
        /// </summary>
        private void SaveMaterial()
        {
            var settings = SettingsManager.Instance;

            // get reference to the tile material list
            var list = this.tileMaterials[this.selectedTextureId];

            // check if there are materials and the selected material index is in range
            if (list.Count == 0 || this.materialSelectionIndex >= list.Count)
            {
                return;
            }

            // get path to the folder where the materials should be saved to
            var path = Path.Combine("Assets", settings.GetSetting(GlobalConstants.AutoMaterialCreationFolderKey, string.Empty));

            // get reference to the material selection
            var model = list[this.materialSelectionIndex];

            // setup the path to include the material filename
            path = Path.Combine(path, Path.ChangeExtension(model.Material.name.Replace("/", "-"), ".mat"));

            // ensure that the path does not contain Path.DirectorySeparatorChar characters
            path = path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // Ensure the path is unique
            path = AssetDatabase.GenerateUniqueAssetPath(path);

            // save the material file
            AssetDatabase.CreateAsset(model.Material, path);

            // remove the material from the list
            list.RemoveAt(this.materialSelectionIndex);

            // add the material to the recently use material list
            GridMappingService.Instance.RecentlyUsedMaterials.Add(model.Material);
        }

        /// <summary>
        /// Saves a tile selection model to a settings repository.
        /// </summary>
        /// <param name="model">
        /// The tile selection model to save.
        /// </param>
        private void SaveToSettings(TileSelectionModel model)
        {
            if (model == null)
            {
                return;
            }
        }
    }
}