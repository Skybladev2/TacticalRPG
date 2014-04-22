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
    using UnityEngine;                            

    /// <summary>
    /// The color.
    /// </summary>
    public partial struct Color
    {
        #region Public Methods and Operators

        ///// <summary>
        ///// Converts a <see cref="Color"/> array into a unity <see cref="UnityEngine.Color32"/> array without the need for explicit conversions.
        ///// </summary>
        ///// <param name="value">The <see cref="Color"/> array to convert</param>
        ///// <returns>Returns a new unity <see cref="UnityEngine.Color32"/> array.</returns>
        //public static implicit operator Color32[](Color[] value)
        //{
        //    var colors = new Color32[value.Length];
        //    for (int i = 0; i < colors.Length; i++)
        //    {
        //        var c = value[i];
        //        colors[i] = new Color32(c.R, c.G, c.B, c.A);
        //    }

        //    return colors;
        //}

        /// <summary>
        /// Converts a <see cref="Color"/> type into a unity <see cref="UnityEngine.Color32"/> without the need for explicit conversions.
        /// </summary>
        /// <param name="value">The <see cref="Color"/> to convert</param>
        /// <returns>Returns a new unity <see cref="UnityEngine.Color32"/> type.</returns>
        public static implicit operator Color32(Color value)
        {
            return new Color32(value.R, value.G, value.B, value.A);
        }

        /// <summary>
        /// Converts a <see cref="Color"/> type into a unity <see cref="UnityEngine.Color"/> without the need for explicit conversions.
        /// </summary>
        /// <param name="value">The <see cref="Color"/> to convert</param>
        /// <returns>Returns a new unity <see cref="UnityEngine.Color"/> type.</returns>
        public static implicit operator UnityEngine.Color(Color value)
        {
            return new Color32(value.R, value.G, value.B, value.A);
        }

        /// <summary>
        /// Converts a <see cref="UnityEngine.Color32"/> type into a unity <see cref="Color"/> without the need for explicit conversions.
        /// </summary>
        /// <param name="value">The <see cref="UnityEngine.Color32"/> to convert</param>
        /// <returns>Returns a new <see cref="Color"/> type.</returns>
        public static implicit operator Color(Color32 value)
        {
            return new Color(value.r, value.g, value.b, value.a);
        }

        /// <summary>
        /// Converts a <see cref="UnityEngine.Color"/> type into a unity <see cref="Color"/> without the need for explicit conversions.
        /// </summary>
        /// <param name="value">The <see cref="UnityEngine.Color"/> to convert</param>
        /// <returns>Returns a new <see cref="Color"/> type.</returns>
        public static implicit operator Color(UnityEngine.Color value)
        {
            Color32 col = value;
            return col;// new Color((byte)(value.r * byte.MaxValue), (byte)(value.g * byte.MaxValue), (byte)(value.b * byte.MaxValue), (byte)(value.a * byte.MaxValue));
        }

        #endregion
    }
}