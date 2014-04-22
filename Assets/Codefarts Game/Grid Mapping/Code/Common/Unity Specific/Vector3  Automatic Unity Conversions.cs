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
    /// The vector 3.
    /// </summary>
    public partial struct Vector3
    {
        /// <summary>
        /// Converts a <see cref="Vector3"/> type into a unity Vector3 without the need for explicit conversions.
        /// </summary>
        /// <param name="v">The <see cref="Vector3"/> to convert</param>
        /// <returns>Returns a new unity Vector3 type.</returns>
        public static implicit operator UnityEngine.Vector3(Vector3 v)
        {
            return new UnityEngine.Vector3(v.X, v.Y, v.Z);
        }

        /// <summary>
        /// Converts a unity Vector3 type into a <see cref="Vector3"/> without the need for explicit conversions.
        /// </summary>
        /// <param name="v">The unity Vector3 to convert</param>
        /// <returns>Returns a new unity Vector3 type.</returns>
        public static implicit operator Vector3(UnityEngine.Vector3 v)
        {
            return new Vector3(v.x, v.y, v.z);
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
        public static UnityEngine.Vector3 operator +(Vector3 value1, UnityEngine.Vector3 value2)
        {
            UnityEngine.Vector3 x;
            x.x = value1.X + value2.x;
            x.y = value1.Y + value2.y;
            x.z = value1.Z + value2.z;
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
        public static UnityEngine.Vector3 operator +(UnityEngine.Vector3 value2, Vector3 value1)
        {
            UnityEngine.Vector3 x;
            x.x = value1.X + value2.x;
            x.y = value1.Y + value2.y;
            x.z = value1.Z + value2.z;
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
        public static UnityEngine.Vector3 operator /(Vector3 value1, UnityEngine.Vector3 value2)
        {
            UnityEngine.Vector3 x;
            x.x = value1.X / value2.x;
            x.y = value1.Y / value2.y;
            x.z = value1.Z / value2.z;
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
        public static UnityEngine.Vector3 operator /(UnityEngine.Vector3 value2, Vector3 value1)
        {
            UnityEngine.Vector3 x;
            x.x = value1.X / value2.x;
            x.y = value1.Y / value2.y;
            x.z = value1.Z / value2.z;
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
        public static bool operator ==(Vector3 value1, UnityEngine.Vector3 value2)
        {
            return Math.Abs(value1.X - value2.x) < Mathf.Epsilon && Math.Abs(value1.Y - value2.y) < Mathf.Epsilon && Math.Abs(value1.Z - value2.z) < Mathf.Epsilon;
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
        public static bool operator ==(UnityEngine.Vector3 value2, Vector3 value1)
        {
            return Math.Abs(value1.X - value2.x) < Mathf.Epsilon && Math.Abs(value1.Y - value2.y) < Mathf.Epsilon && Math.Abs(value1.Z - value2.z) < Mathf.Epsilon;
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
        public static bool operator !=(Vector3 value1, UnityEngine.Vector3 value2)
        {
            return Math.Abs(value1.X - value2.x) > Mathf.Epsilon || Math.Abs(value1.Y - value2.y) > Mathf.Epsilon || Math.Abs(value1.Z - value2.z) > Mathf.Epsilon;
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
        public static bool operator !=(UnityEngine.Vector3 value2, Vector3 value1)
        {
            return Math.Abs(value1.X - value2.x) > Mathf.Epsilon || Math.Abs(value1.Y - value2.y) > Mathf.Epsilon || Math.Abs(value1.Z - value2.z) > Mathf.Epsilon;
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
        public static UnityEngine.Vector3 operator *(Vector3 value1, UnityEngine.Vector3 value2)
        {
            UnityEngine.Vector3 x;
            x.x = value1.X * value2.x;
            x.y = value1.Y * value2.y;
            x.z = value1.Z * value2.z;
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
        public static UnityEngine.Vector3 operator -(Vector3 value1, UnityEngine.Vector3 value2)
        {
            UnityEngine.Vector3 x;
            x.x = -value2.x;
            x.y = -value2.y;
            x.z = -value2.z;
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
        public static UnityEngine.Vector3 operator -(UnityEngine.Vector3 value2, Vector3 value1)
        {
            UnityEngine.Vector3 x;
            x.x = -value2.x;
            x.y = -value2.y;
            x.z = -value2.z;
            return x;
        }
    }
}