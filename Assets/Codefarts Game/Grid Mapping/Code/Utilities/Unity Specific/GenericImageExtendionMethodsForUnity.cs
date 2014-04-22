/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Utilities
{
    using System;

    using Codefarts.GridMapping.Common;

    using UnityEngine;

    using Color = Codefarts.GridMapping.Common.Color;
    using Vector2 = UnityEngine.Vector2;

    /// <summary>
    /// <see cref="GenericImage{T}"/> extension methods for unity.
    /// </summary>
    public static class GenericImageExtensionMethodsForUnity
    {
        /// <summary>
        /// Converts a <see cref="GenericImage{T}"/> to a <see cref="Texture2D"/> type.
        /// </summary>
        /// <param name="image">The image to be converted.</param>
        /// <returns>Returns a new <see cref="Texture2D"/> type.</returns>   
        public static Texture2D ToTexture2D(this GenericImage<Color> image)
        {
            var texture = new Texture2D(image.Width, image.Height, TextureFormat.ARGB32, false);
            var flippedImage = image.Clone();
            flippedImage.FlipVertically();
            texture.SetPixels32(flippedImage.ToUnityColor32Array());
            texture.Apply();
            return texture;
        }

        /// <summary>
        /// Converts a <see cref="GenericImage{T}"/> to a <see cref="Texture2D"/> type.
        /// </summary>
        /// <param name="image">The image to be converted.</param>
        /// <returns>Returns a new <see cref="Texture2D"/> type.</returns>   
        public static Texture2D ToTexture2D(this GenericImage<UnityEngine.Color> image)
        {
            var texture = new Texture2D(image.Width, image.Height, TextureFormat.ARGB32, false);
            var flippedImage = image.Clone();
            flippedImage.FlipVertically();
            texture.SetPixels32(flippedImage.ToUnityColor32Array());
            texture.Apply();
            return texture;
        }

        /// <summary>
        /// Draws a string onto a <see cref="GenericImage{T}"/>.
        /// </summary>
        /// <param name="image">The image where the string will be drawn to.</param>
        /// <param name="font">The font to use when drawing the string.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="x">The x position where the string will be drawn at.</param>
        /// <param name="y">The y position where the string will be drawn at.</param>
        public static void DrawString(this GenericImage<Color> image, Font font, string text, float x, float y)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            if (font == null)
            {
                throw new ArgumentNullException("font");
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a <see cref="Texture2D"/> to a <see cref="GenericImage{T}"/> type.
        /// </summary>
        /// <param name="image">The image to be converted.</param>
        /// <returns>Returns a new <see cref="GenericImage{T}"/> type.</returns>
        /// <remarks>The source Texture2D image must be readable.</remarks>
        public static GenericImage<Color> ToGenericImage(this Texture2D image)
        {
            var colors = image.GetPixels32(0);
            var texture = new GenericImage<Color>(image.width, image.height);
            var index = 0;
            for (var y = texture.Height - 1; y >= 0; y--)
            {
                for (var x = 0; x < texture.Width; x++)
                {
                    texture[x, y] = colors[index++];
                }
            }

            return texture;
        }        

        /// <summary>
        /// Converts a <see cref="GenericImage{T}"/> to an array of <see cref="Color32"/>.
        /// </summary>
        /// <param name="image">The image to be converted.</param>
        /// <returns>Returns a new array of <see cref="Color32"/>.</returns>   
        public static Color32[] ToUnityColor32Array(this GenericImage<Color> image)
        {
            var destination = new Color32[image.Width * image.Height];
            var index = 0;
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    destination[index++] = image[x, y];
                }
            }

            return destination;
        }

        /// <summary>
        /// Converts a <see cref="GenericImage{T}"/> to an array of <see cref="Color32"/>.
        /// </summary>
        /// <param name="image">The image to be converted.</param>
        /// <returns>Returns a new array of <see cref="Color32"/>.</returns>   
        public static Color32[] ToUnityColor32Array(this GenericImage<UnityEngine.Color> image)
        {
            var destination = new Color32[image.Width * image.Height];
            var index = 0;
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    destination[index++] = image[x, y];
                }
            }

            return destination;
        }

        /// <summary>
        /// Converts a <see cref="GenericImage{T}"/> to an array of <see cref="UnityEngine.Color"/>.
        /// </summary>
        /// <param name="image">The image to be converted.</param>
        /// <returns>Returns a new array of <see cref="UnityEngine.Color"/>.</returns>   
        public static UnityEngine.Color[] ToUnityColorArray(this GenericImage<Color> image)
        {
            var colorArray = new UnityEngine.Color[image.Width * image.Height];
            var position = 0;
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    colorArray[position++] = image[x, y];
                }
            }

            return colorArray;
        }

        /// <summary>
        /// Draws a <see cref="GenericImage{T}"/> on to a <see cref="Texture2D"/> texture.
        /// </summary>
        /// <param name="texture">A reference to a <see cref="Texture2D"/> type.</param>
        /// <param name="sourceImage">A reference to the <see cref="GenericImage{T}"/> that will be drawn.</param>
        /// <param name="sourceRectangle">The source rectangle within the <see cref="sourceImage"/> that will be drawn.</param>
        /// <param name="position">The position within <see cref="texture"/> where the <see cref="sourceImage"/> will be drawn at.</param>
        public static void Draw(this Texture2D texture, GenericImage<Color> sourceImage, Rect sourceRectangle, Vector2 position)
        {
            Draw(texture, sourceImage, (int)position.x, (int)position.y, (int)sourceRectangle.x, (int)sourceRectangle.y, (int)sourceRectangle.width, (int)sourceRectangle.height, false, false);
        }

        /// <summary>
        /// Draws a <see cref="GenericImage{T}"/> on to a <see cref="Texture2D"/> texture.
        /// </summary>
        /// <param name="texture">A reference to a <see cref="Texture2D"/> type.</param>
        /// <param name="sourceImage">A reference to the <see cref="GenericImage{T}"/> that will be drawn.</param>
        /// <param name="sourceRectangle">The source rectangle within the <see cref="sourceImage"/> that will be drawn.</param>
        /// <param name="position">The position within <see cref="texture"/> where the <see cref="sourceImage"/> will be drawn at.</param>
        /// <param name="flipHorizontally">If true will flip the <see cref="sourceImage"/> horizontally before drawing.</param>
        /// <param name="flipVertically">If true will flip the <see cref="sourceImage"/> vertically before drawing.</param>
        public static void Draw(this Texture2D texture, GenericImage<Color> sourceImage, Rect sourceRectangle, Vector2 position, bool flipHorizontally, bool flipVertically)
        {
            Draw(texture, sourceImage, (int)position.x, (int)position.y, (int)sourceRectangle.x, (int)sourceRectangle.y, (int)sourceRectangle.width, (int)sourceRectangle.height, flipHorizontally, flipVertically);
        }

        /// <summary>
        /// Draws a <see cref="GenericImage{T}"/> on to a <see cref="Texture2D"/> texture.
        /// </summary>
        /// <param name="texture">A reference to a <see cref="Texture2D"/> type.</param>
        /// <param name="sourceImage">A reference to the <see cref="GenericImage{T}"/> that will be drawn.</param>
        /// <param name="x">The x position where the <see cref="sourceImage"/> will be drawn.</param>
        /// <param name="y">The y position where the <see cref="sourceImage"/> will be drawn.</param>
        /// <param name="sourceX">The source x position within <see cref="sourceImage"/>.</param>
        /// <param name="sourceY">The source y position within <see cref="sourceImage"/>.</param>
        /// <param name="sourceWidth">The source width within the <see cref="sourceImage"/>.</param>
        /// <param name="sourceHeight">The source height within the <see cref="sourceImage"/>.</param>
        /// <param name="flipHorizontally">If true will flip the <see cref="sourceImage"/> horizontally before drawing.</param>
        /// <param name="flipVertically">If true will flip the <see cref="sourceImage"/> vertically before drawing.</param>
        public static void Draw(this Texture2D texture, GenericImage<Color> sourceImage, int x, int y, int sourceX, int sourceY, int sourceWidth, int sourceHeight, bool flipHorizontally, bool flipVertically)
        {
            var textureRectangle = new Rect(0, 0, texture.width, texture.height);
            var sourceRectangle = new Rect(x, y, sourceWidth, sourceHeight);
            var intersect = textureRectangle.Intersect(sourceRectangle);

            if (!intersect.Intersects(new Rect(0, 0, sourceImage.Width, sourceImage.Height)))
            {
                return;
            }

            var tempImage = new GenericImage<Color>((int)intersect.width, (int)intersect.height);
            tempImage.Draw(sourceImage, 0, 0, sourceX, sourceY, tempImage.Width, tempImage.Height, (source, blendWith) => blendWith);

            if (flipHorizontally)
            {
                tempImage.FlipHorizontally();
            }

            if (flipVertically)
            {
                tempImage.FlipVertically();
            }

            var colors = tempImage.ToUnityColorArray();
            texture.SetPixels(x, y, (int)intersect.width, (int)intersect.height, colors);
            texture.Apply();
        }
    }
}
