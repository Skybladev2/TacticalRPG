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

    using UnityEngine;
    using Vector2 = Codefarts.GridMapping.Common.Vector2;

    /// <summary>
    /// Provides extension methods for the <see cref="TileSelectionControl"/> type.
    /// </summary>
    public static class TileSelectionControlExtensionMethods
    {
        /// <summary>
        /// Calculates the texture co-ordinates for the selection.
        /// </summary>
        /// <param name="preview">
        /// A reference to a <see cref="TileSelectionControl"/> type.
        /// </param>
        /// <param name="material">
        /// A reference to a <see cref="GenericMaterialCreationControl"/> type.
        /// </param>
        /// <param name="orientationIndex">
        /// <p>The orientation Index.</p>
        /// <p>
        /// <list type="number">
        ///     <item>
        ///         <term>0 Zero</term>
        ///         <description>None.</description>
        ///     </item>
        ///     <item>
        ///         <term>1 Vertically</term>
        ///         <description>Coordinates will be flipped vertically.</description>
        ///     </item>
        ///     <item>
        ///         <term>2 Horizontally</term>
        ///         <description>Coordinates will be flipped horizontally.</description>
        ///     </item>
        ///     <item>
        ///         <term>3 Both</term>
        ///         <description>Coordinates will be flipped both horizontally and vertically.</description>
        ///     </item>
        /// </list>
        /// </p> 
        /// </param>
        /// <returns>
        /// Returns the textures co-ordinates for the selection in UV space.
        /// </returns>
        /// <exception cref="ArgumentNullException">If the <see cref="preview"/> or <see cref="material"/> parameter is null.</exception>
        /// <exception cref="ArgumentNullException">If the <see cref="TileSelectionControl.TextureAsset"/> or <see cref="GenericMaterialCreationControl.TextureAsset"/> properties are null.</exception>
        public static Rect GetTextureCoordinates(this TileSelectionControl preview, GenericMaterialCreationControl material, int orientationIndex)
        {    
            var startOffset = material.StartSpacing ? material.Spacing : 0;

            var tileWidth = material.TileWidth;
            var tileHeight = material.TileHeight;
            var spacing = material.Spacing;
            var textureAsset = material.TextureAsset;
            var inset = material.Inset;
            var textureCoordinates = new Rect();

            // if in free form mode return the freeform selection rectangle
            if (material.FreeForm)
            {
                var rect = preview.FreeFormRectangle;
                textureCoordinates.xMin = rect.xMin / preview.TextureAsset.width;
                textureCoordinates.yMin = rect.yMin / preview.TextureAsset.height;
                textureCoordinates.width = rect.width / preview.TextureAsset.width;
                textureCoordinates.height = rect.height / preview.TextureAsset.height;
            }
            else
            {
                // calculate selection rectangle size
                var size = preview.GetSelectionSize();

                // get min X/Y position of the selection rectangle
                var minX = float.MaxValue;
                var minY = float.MaxValue;

                foreach (var tile in preview.SelectedTiles)
                {
                    if (tile.X < minX)
                    {
                        minX = tile.X;
                    }

                    if (tile.Y < minY)
                    {
                        minY = tile.Y;
                    }
                }

                // setup a rect containing the US co-ordinates
                textureCoordinates = new Rect(
                    (startOffset + (minX * (tileWidth + spacing))) / textureAsset.width,
                    (startOffset + (minY * (tileHeight + spacing))) / textureAsset.height,
                    ((size.X * (tileWidth + spacing)) - spacing - startOffset) / textureAsset.width,
                    ((size.Y * (tileHeight + spacing)) - spacing - startOffset) / textureAsset.height);

                // apply inset
                textureCoordinates.x += inset;
                textureCoordinates.y += inset;
                textureCoordinates.width -= inset * 2;
                textureCoordinates.height -= inset * 2;
            }

            // textures co-ordinates originate from the lower left corner of the texture so adjust y to accommodate
            textureCoordinates.y = 1 - (textureCoordinates.y / 1) - textureCoordinates.height;

            // check to flip vertically
            if (orientationIndex == 1 || orientationIndex == 3)
            {
                var min = textureCoordinates.yMin;
                textureCoordinates.yMin = textureCoordinates.yMax;
                textureCoordinates.yMax = min;
            }

            // check to flip horizontally
            if (orientationIndex == 2 || orientationIndex == 3)
            {
                var min = textureCoordinates.xMin;
                textureCoordinates.xMin = textureCoordinates.xMax;
                textureCoordinates.xMax = min;
            }

            return textureCoordinates;
        }

        /// <summary>
        /// Calculates the size of the selection rectangle in tile coordinates.
        /// </summary>
        /// <param name="preview">
        /// A reference to a <see cref="TileSelectionControl"/> type.
        /// </param>
        /// <returns>
        /// Returns the size of the selection rectangle in tile co-ordinates as a <see cref="Vector2"/> type.
        /// </returns>
        /// <exception cref="ArgumentNullException">If the <see cref="preview"/> parameter is null.</exception>
        public static Vector2 GetSelectionSize(this TileSelectionControl preview)
        {
            // calculate the min and max values of the selection rectangle
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;
          
            foreach (var tile in preview.SelectedTiles)
            {
                if (tile.X < minX)
                {
                    minX = tile.X;
                }

                if (tile.X > maxX)
                {
                    maxX = tile.X;
                }

                if (tile.Y < minY)
                {
                    minY = tile.Y;
                }

                if (tile.Y > maxY)
                {
                    maxY = tile.Y;
                }
            }

            // store the width and height
            var selectionWidth = maxX - minX + 1;
            var selectionHeight = maxY - minY + 1;

            // return the size
            return new Vector2(selectionWidth, selectionHeight);
        }
    }
}
