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
    using Codefarts.GridMapping.Editor;

    using UnityEngine;

    using Color = Common.Color;

    /// <summary>
    /// Contains extension methods for the <see cref="Material"/> type.
    /// </summary>
    public static class MaterialExtensionMethods
    {
        /// <summary>
        /// Converts a materials <see cref="Material.mainTexture"/> property into a <see cref="GenericImage{T}"/> type takes into account the 
        /// materials <see cref="Material.mainTextureOffset"/> and <see cref="Material.mainTextureScale"/> properties. See remarks.
        /// </summary>
        /// <param name="material">The source material to get the <see cref="Material.mainTexture"/> reference from.</param>
        /// <returns>Returns a <see cref="GenericImage{T}"/> type representing the materials texture co-ordinates.</returns>
        /// <remarks>This method does not return the original texture specified by the <see cref="Material.mainTexture"/> property but rather
        /// takes into account the materials <see cref="Material.mainTextureOffset"/> and <see cref="Material.mainTextureScale"/> properties
        /// in order to generate a <see cref="GenericImage{T}"/> type.</remarks>
        /// <exception cref="InvalidCastException">If <see cref="Material.mainTexture"/> cannot be cast to a <see cref="Texture2D"/> type.</exception>
        /// <exception cref="ArgumentNullException">If the <see cref="material"/> parameter is null.</exception>
        public static GenericImage<Color> ToGenericImage(this Material material)
        {
            // ensure material specified
            if (material == null)
            {
                throw new ArgumentNullException("material");
            }

            // if no main texture then also return null
            if (material.mainTexture == null)
            {
                return null;
            }

            var texture = material.mainTexture as Texture2D;

            // ensure material texture is a Texture2D type
            if (texture == null)
            {
                throw new InvalidCastException("Material texture is not a Texture2D!");
            }

            // create a generic image.
            var image = texture.CreateGenericImage();
            var data = image.ToUnityColor32Array();
            var tempImage = new GenericImage<Color>(image.Width, image.Height);
            tempImage.SetPixels(Array.ConvertAll(data, input => new Color(input.r, input.g, input.b, input.a)));

            var offset = material.mainTextureOffset;
            var position = new Point(
                (int)((offset.x * texture.width) % texture.width),
                (int)(texture.height - ((offset.y * texture.height) % texture.height)));

            var scale = material.mainTextureScale;
            var size = new Size((int)(scale.x * texture.width), (int)(scale.y * texture.height));

            var subImage = new GenericImage<Color>(size.Width, size.Height);
            subImage.Draw(tempImage, 0, 0, position.X, position.Y - size.Height, size.Width, size.Height, (source, blendWith) => blendWith);

            // convert and return a generic image
            return subImage;
        }
    }
}