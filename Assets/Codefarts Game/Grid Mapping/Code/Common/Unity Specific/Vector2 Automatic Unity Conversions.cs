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
    using System;

    using UnityEngine;

    /// <summary>
    /// The vector 2.
    /// </summary>
    public partial struct Vector2
    {
        /// <summary>
        /// Converts a <see cref="Vector2"/> type into a unity Vector2 without the need for explicit conversions.
        /// </summary>
        /// <param name="v">The <see cref="Vector2"/> to convert</param>
        /// <returns>Returns a new unity Vector2 type.</returns>
        public static implicit operator UnityEngine.Vector2(Vector2 v)
        {
            return new UnityEngine.Vector2(v.X, v.Y);
        }

        /// <summary>
        /// Converts a unity Vector2 type into a <see cref="Vector2"/> without the need for explicit conversions.
        /// </summary>
        /// <param name="v">The unity Vector2 to convert</param>
        /// <returns>Returns a new unity Vector2 type.</returns>
        public static implicit operator Vector2(UnityEngine.Vector2 v)
        {
            return new Vector2(v.x, v.y);
        }

        /// <summary>
        /// The +.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator +(Vector2 value1, UnityEngine.Vector2 value2)
        {
            UnityEngine.Vector2 x;
            x.x = value1.X + value2.x;
            x.y = value1.Y + value2.y;
            return x;
        }
       
        /// <summary>
        /// The +.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator +(UnityEngine.Vector2 value2, Vector2 value1)
        {
            UnityEngine.Vector2 x;
            x.x = value1.X + value2.x;
            x.y = value1.Y + value2.y;
            return x;
        }

        /// <summary>
        /// The /.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator /(Vector2 value1, UnityEngine.Vector2 value2)
        {
            UnityEngine.Vector2 x;
            x.x = value1.X / value2.x;
            x.y = value1.Y / value2.y;
            return x;
        }

        /// <summary>
        /// The /.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator /(UnityEngine.Vector2 value2, Vector2 value1)
        {
            UnityEngine.Vector2 x;
            x.x = value1.X / value2.x;
            x.y = value1.Y / value2.y;
            return x;
        }

        /// <summary>
        /// The ==.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static bool operator ==(Vector2 value1, UnityEngine.Vector2 value2)
        {
            return Math.Abs(value1.X - value2.x) < Mathf.Epsilon && Math.Abs(value1.Y - value2.y) < Mathf.Epsilon;
        }

        /// <summary>
        /// The !=.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static bool operator !=(Vector2 value1, UnityEngine.Vector2 value2)
        {
            return Math.Abs(value1.X - value2.x) > Mathf.Epsilon || Math.Abs(value1.Y - value2.y) > Mathf.Epsilon; 
        }

        /// <summary>
        /// The *.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator *(Vector2 value1, UnityEngine.Vector2 value2)
        {
            UnityEngine.Vector2 x;
            x.x = value1.X * value2.x;
            x.y = value1.Y * value2.y;
            return x;
        }

        /// <summary>
        /// The -.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator -(Vector2 value1, UnityEngine.Vector2 value2)
        {
            UnityEngine.Vector2 x;
            x.x = -value2.x;
            x.y = -value2.y;
            return x;
        }

        /// <summary>
        /// The -.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static UnityEngine.Vector2 operator -(UnityEngine.Vector2 value2, Vector2 value1)
        {
            UnityEngine.Vector2 x;
            x.x = -value2.x;
            x.y = -value2.y;
            return x;
        }       
    }
}