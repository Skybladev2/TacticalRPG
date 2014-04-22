// <copyright>
//   Copyright (c) 2012 Codefarts
//   All rights reserved.
//   contact@codefarts.com
//   http://www.codefarts.com
// </copyright>     
namespace Codefarts.GridMapping.Editor.Controls
{
    using System;
    using System.Collections.Generic;

    using Codefarts.Localization;

    using UnityEditor;

    using UnityEngine;

    using Object = UnityEngine.Object;

    /// <summary>
    /// Provides a generic control for material creation.
    /// </summary>
    public class GenericMaterialCreationControl : IDisposable
    {
        #region Fields

        /// <summary>
        /// Holds a predefined list of shader names.
        /// </summary>
        private readonly List<string> shaderNames;

        /// <summary>
        /// Holds a value indicating whether the object has been disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Holds a value indicating weather free form selection is being used.
        /// </summary>
        private bool freeForm;

        /// <summary>
        /// Holds a reference to a material reference.
        /// </summary>
        private Material material;

        /// <summary>
        /// Holds the value of the materials color.
        /// </summary>
        private Color materialColor;

        /// <summary>
        /// Holds the selection index for the specified shader.
        /// </summary>
        private int shaderSelectionIndex;

        /// <summary>
        /// Holds a Boolean value indicating whether the free form check box will be shown.
        /// </summary>
        private bool showFreeform = true;

        /// <summary>
        /// Holds the value for spacing between tiles.
        /// </summary>
        private int spacing = 1;

        /// <summary>
        /// Determines if the tile set starts with spaces.
        /// </summary>
        private bool startSpacing;

        /// <summary>
        /// Holds a reference to a texture asset.
        /// </summary>
        private Texture2D textureAsset;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericMaterialCreationControl" /> class.
        /// </summary>
        public GenericMaterialCreationControl()
        {
            // NOTE: Not sure if this will work if running under a different culture (Brazil & Asian regions etc) it should but hmmm...
            // wondering if localizing these strings is wise or unwise ...
            this.shaderNames = new List<string>(new[] { "Diffuse", "Transparent/Diffuse" });

            this.material = new Material(Shader.Find(this.shaderNames[this.shaderSelectionIndex]));
            this.materialColor = Color.white;
            this.material.color = this.materialColor;
            this.ShowColor = true;
            this.ShowShader = true;
            this.TileHeight = 32;
            this.TileWidth = 32;
            this.Inset = 0.0005f;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Provides an event that gets raised when <see cref="FreeForm" /> changes.
        /// </summary>
        public event EventHandler FreeformChanged;

        /// <summary>
        /// Provides an event that gets raised when <see cref="MaterialColor" /> changes.
        /// </summary>
        public event EventHandler MaterialColorChanged;

        /// <summary>
        /// Provides an event that gets raised when shader selection index changes.
        /// </summary>
        public event EventHandler ShaderChanged;

        /// <summary>
        /// Provides an event that gets raised when <see cref="Spacing" /> changes.
        /// </summary>
        public event EventHandler SpacingChanged;

        /// <summary>
        /// Provides an event that gets raised when <see cref="StartSpacing" /> changes.
        /// </summary>
        public event EventHandler StartSpacingChanged;

        /// <summary>
        /// Provides an event that gets raised when <see cref="TextureAsset" />  is selected or changes.
        /// </summary>
        public event EventHandler TextureChanged;

        /// <summary>
        /// Provides an event that gets raised when the tile size fields change.
        /// </summary>
        public event EventHandler TileSizeChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a callback after drawing the spacing and inset controls.
        /// </summary>
        public Action AfterSpacingDrawn { get; set; }

        /// <summary>
        /// Gets or sets a callback after drawing the texture controls.
        /// </summary>
        public Action AfterTextureDrawn { get; set; }

        /// <summary>
        /// Gets or sets a callback before drawing the spacing and inset controls.
        /// </summary>
        public Action BeforeSpacingDrawn { get; set; }

        /// <summary>
        /// Gets or sets a callback before drawing the texture controls.
        /// </summary>
        public Action BeforeTextureDrawn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether free form selection is being used.
        /// </summary>
        public bool FreeForm
        {
            get
            {
                return this.freeForm;
            }

            set
            {
                // if ShowFreeform is false or the values match just exit
                if (!this.showFreeform || this.freeForm == value)
                {
                    return;
                }

                this.freeForm = value;

                // if the value changed and there are event hooks raise the event
                if (this.FreeformChanged != null)
                {
                    this.FreeformChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the value for spacing between tiles.
        /// </summary>
        public float Inset { get; set; }

        /// <summary>
        /// Gets a reference to the material.
        /// </summary>
        public Material Material
        {
            get
            {
                return this.material;
            }
        }

        /// <summary>
        /// Gets or sets the value of the materials color.
        /// </summary>
        public Color MaterialColor
        {
            get
            {
                return this.materialColor;
            }

            set
            {
                // set material color and if material is set set the material color as well
                bool changed = value != this.materialColor;
                this.materialColor = value;

                if (this.material != null)
                {
                    this.material.color = this.materialColor;
                }

                // raise event if there are event hooks
                if (changed && this.MaterialColorChanged != null)
                {
                    this.MaterialColorChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets the name of the selected shader.
        /// </summary>
        public string SelectedShaderName
        {
            get
            {
                return this.shaderNames[this.shaderSelectionIndex];
            }
        }

        /// <summary>
        /// Gets a list of shader names that will be selectable from the shader drop down.
        /// </summary>
        public List<string> ShaderNames
        {
            get
            {
                return this.shaderNames;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the color selection control will be shown.
        /// </summary>
        public bool ShowColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to show the <see cref="freeForm" /> check box.
        /// </summary>
        public bool ShowFreeform
        {
            get
            {
                return this.showFreeform;
            }

            set
            {
                this.showFreeform = value;

                // if false ensure that the free form check box is unchecked
                if (!value)
                {
                    this.FreeForm = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the shader selection control is shown.
        /// </summary>
        public bool ShowShader { get; set; }

        /// <summary>
        /// Gets or sets the value for spacing between tiles.
        /// </summary>
        public int Spacing
        {
            get
            {
                return this.spacing;
            }

            set
            {
                bool changed = value != this.spacing;
                this.spacing = value;

                // if the value changed and there are event hooks raise the event
                if (changed && this.SpacingChanged != null)
                {
                    this.SpacingChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tile set starts with spaces.
        /// </summary>
        public bool StartSpacing
        {
            get
            {
                return this.startSpacing;
            }

            set
            {
                bool changed = value != this.startSpacing;
                this.startSpacing = value;

                // if the value changed and there are event hooks raise the event
                if (changed && this.StartSpacingChanged != null)
                {
                    this.StartSpacingChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets a reference to the texture asset.
        /// </summary>
        public Texture2D TextureAsset
        {
            get
            {
                return this.textureAsset;
            }

            set
            {
                bool changed = value != this.textureAsset;
                this.textureAsset = value;

                // if the value changed and there are event hooks raise the event
                if (changed && this.TextureChanged != null)
                {
                    this.TextureChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the value for the tile height.
        /// </summary>
        public int TileHeight { get; set; }

        /// <summary>
        /// Gets or sets a value for the tile width.
        /// </summary>
        public int TileWidth { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The dispose method for cleaning up variables.
        /// </summary>
        public void Dispose()
        {
            // create the material for drawing the preview tile
            if (this.material != null)
            {
                Object.DestroyImmediate(this.material);
                this.material = null;
            }

            this.disposed = true;
        }

        /// <summary>
        /// Draw the control.
        /// </summary>
        public void Draw()
        {
            if (this.disposed)
            {
                return;
            }

            // get reference to localization manager
            LocalizationManager local = LocalizationManager.Instance;

            GUILayout.BeginVertical(); // main vertical wrapper

            // draw tile size controls
            GUILayout.BeginHorizontal(); // tile size controls
            GUILayout.Label(local.Get("Width"));
            int width = EditorGUILayout.IntField(this.TileWidth);
            GUILayout.Label(local.Get("Height"));
            int height = EditorGUILayout.IntField(this.TileHeight);
            GUILayout.EndHorizontal(); // tile size controls

            // clamp tile dimensions
            width = width < 1 ? 1 : width;
            height = height < 1 ? 1 : height;
            width = width > 4096 ? 4096 : width;
            height = height > 4096 ? 4096 : height;

            if (width != this.TileWidth || height != this.TileHeight)
            {
                this.TileWidth = width;
                this.TileHeight = height;
                if (this.TileSizeChanged != null)
                {
                    this.TileSizeChanged(this, EventArgs.Empty);
                }
            }

            GUILayout.BeginHorizontal(); // color, spacing & texture

            GUILayout.BeginVertical(); // color & spacing controls

            if (this.ShowColor)
            {
                GUILayout.Label(local.Get("Color"));

                // have to wrap this in try/catch cause unity will throw exceptions in the console.
                // I suspect it may have something to do with the nested BeginHorizontal BeginVertical
                try
                {
                    this.MaterialColor = EditorGUILayout.ColorField(this.materialColor);
                }
                catch
                {
                }
            }

            // draw control for specifying if starts with spacing 
            this.StartSpacing = GUILayout.Toggle(this.startSpacing, local.Get("StartsWithSpacing"));
            if (this.ShowFreeform)
            {
                this.FreeForm = GUILayout.Toggle(this.freeForm, local.Get("Freeform"));
            }

            if (this.BeforeSpacingDrawn != null)
            {
                this.BeforeSpacingDrawn();
            }

            GUILayout.BeginHorizontal(); // spacing and inset

            GUILayout.BeginVertical(); // left column
            GUILayoutOption controlHeight = GUILayout.MinHeight(GUI.skin.textField.CalcHeight(GUIContent.none, 10));
            GUILayout.Label(local.Get("Spacing"), controlHeight);
            GUILayout.Label(local.Get("Inset"), controlHeight);
            GUILayout.EndVertical(); // left column

            GUILayout.BeginVertical(); // right column
            int value = EditorGUILayout.IntField(this.spacing);
            value = value < 0 ? 0 : value;
            this.Spacing = value;

            this.Inset = EditorGUILayout.FloatField(this.Inset);
            this.Inset = this.Inset < 0 ? 0 : this.Inset;
            GUILayout.EndVertical(); // right column

            GUILayout.EndHorizontal(); // spacing and inset

            if (this.AfterSpacingDrawn != null)
            {
                this.AfterSpacingDrawn();
            }

            GUILayout.EndVertical(); // color & spacing controls

            GUILayout.BeginVertical(); // texture controls
            if (this.BeforeTextureDrawn != null)
            {
                this.BeforeTextureDrawn();
            }

            GUILayout.Label(local.Get("Texture"));

            // have to wrap this in try/catch cause unity will throw exceptions in the console.
            // I suspect it may have something to do with the nested BeginHorizontal BeginVertical
            try
            {
                this.TextureAsset = EditorGUILayout.ObjectField(this.TextureAsset, typeof(Texture2D), false, GUILayout.Width(64), GUILayout.Height(64)) as Texture2D;
            }
            catch
            {
            }

            if (this.AfterTextureDrawn != null)
            {
                this.AfterTextureDrawn();
            }

            GUILayout.EndVertical(); // texture controls

            GUILayout.EndHorizontal(); // color, spacing & texture

            if (this.ShowShader)
            {
                // place some spacing of a few pixels
                GUILayout.Space(4);

                // draw a popup that allows for shader type selection
                GUILayout.Label(local.Get("ShaderType"));
                this.shaderSelectionIndex = this.shaderSelectionIndex > this.shaderNames.Count - 1 ? this.shaderNames.Count - 1 : this.shaderSelectionIndex;
                value = EditorGUILayout.Popup(this.shaderSelectionIndex, this.shaderNames.ToArray());
                if (value != this.shaderSelectionIndex)
                {
                    this.shaderSelectionIndex = value;

                    this.material.shader = Shader.Find(this.shaderNames[this.shaderSelectionIndex]);
                    this.material.color = this.materialColor;

                    if (this.ShaderChanged != null)
                    {
                        this.ShaderChanged(this, EventArgs.Empty);
                    }
                }
            }

            GUILayout.EndVertical(); // main vertical wrapper
        }

        #endregion
    }
}