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
    using UnityEngine;

    /// <summary>
    /// Contains extension methods for the <see cref="Texture2D"/> type.
    /// </summary>
    public static class Texture2DExtensionMethods
    {
        /// <summary>
        /// Draws pixels from the <see cref="src"/> texture.
        /// </summary>
        /// <param name="texture">The destination texture where the pixels will be copied to.</param>
        /// <param name="src">The source texture where the pixels will be copied from.</param>
        /// <param name="x">The x destination in <see cref="texture"/> where pixel copying will occur.</param>
        /// <param name="y">The y destination in <see cref="texture"/> where pixel copying will occur.</param>
        public static void Blit(this Texture2D texture, Texture2D src, int x, int y)
        {
            var colors = src.GetPixels(0, 0, src.width, src.height);
            texture.SetPixels(x, texture.height - src.height - y, src.width, src.height, colors);
            texture.Apply();
        }

        public static void Blit(this Texture2D texture, Texture2D src, Rect srcRect, Vector2 pos)
        {
            Blit(texture, src, (int)pos.x, (int)pos.y, (int)srcRect.x, (int)srcRect.y, (int)srcRect.width, (int)srcRect.height);
        }

        public static void Blit(this Texture2D texture, Texture2D src, int x, int y, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            var colors = src.GetPixels(srcX, srcY, srcWidth, srcHeight);
            texture.SetPixels(x, y, srcWidth, srcHeight, colors);
            texture.Apply();
        }   
    }
}
