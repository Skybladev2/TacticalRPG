/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Common
{
    /// <summary>
    /// Extension methods for the <see cref="Color"/> type.
    /// </summary>
    public static class ColorExtensionMethods
    {
        #region Public Methods and Operators

        /// <summary>
        /// Blends two colors together.
        /// </summary>
        /// <param name="color">
        /// The source color.
        /// </param>
        /// <param name="src">
        /// The blend color.
        /// </param>
        /// <returns>
        /// Returns the result as a <see cref="Color"/> type.
        /// </returns>
        /// <remarks>
        /// From Wikipedia https://en.wikipedia.org/wiki/Alpha_compositing -> "Alpha blending" 
        /// </remarks>
        public static Color Blend(this Color color, Color src)
        {
            float sr = src.R / 255f;
            float sg = src.G / 255f;
            float sb = src.B / 255f;
            float sa = src.A / 255f;
            float dr = color.R / 255f;
            float dg = color.G / 255f;
            float db = color.B / 255f;
            float da = color.A / 255f;

            float oa = sa + (da * (1 - sa));
            float r = ((sr * sa) + ((dr * da) * (1 - sa))) / oa;
            float g = ((sg * sa) + ((dg * da) * (1 - sa))) / oa;
            float b = ((sb * sa) + ((db * da) * (1 - sa))) / oa;
            float a = oa;

            return new Color((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), (byte)(a * 255));
        }

        /// <summary>
        /// Packs color components into a <see cref="uint"/> type.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        /// <returns>Returns a uint representing the color.</returns>
        public static uint Pack(int r, int g, int b, int a)
        {
            return (((uint)r & 0xff)) | (((uint)g & 0xff) << 8) | (((uint)b & 0xff) << 16) | (((uint)a & 0xff) << 24);
        }

        /// <summary>
        /// Unpacks a <see cref="uint"/> type into a color type.
        /// </summary>
        /// <param name="value">The packed color value to unpack.</param>
        /// <returns>Returns a color type from the unpacked values.</returns>
        public static Color Unpack(uint value)
        {
            var r = (byte)(value);
            var g = (byte)(value >> 8);
            var b = (byte)(value >> 16);
            var a = (byte)(value >> 24);

            return new Color(r, g, b, a);
        }
        #endregion
    }
}