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
    using System.Globalization;

    using UnityEngine;

    /// <summary>
    /// The vector 3.
    /// </summary>
    [Serializable]
    public partial struct Vector3
    {
        #region Constants and Fields

        /// <summary>
        /// The x.
        /// </summary>
        public float X;

        /// <summary>
        /// The y.
        /// </summary>
        public float Y;

        /// <summary>
        /// The z.
        /// </summary>
        public float Z;

        /// <summary>
        /// The _backward.
        /// </summary>
        private static readonly Vector3 _backward;

        /// <summary>
        /// The _down.
        /// </summary>
        private static readonly Vector3 _down;

        /// <summary>
        /// The _forward.
        /// </summary>
        private static readonly Vector3 _forward;

        /// <summary>
        /// The _left.
        /// </summary>
        private static readonly Vector3 _left;

        /// <summary>
        /// The _one.
        /// </summary>
        private static readonly Vector3 _one;

        /// <summary>
        /// The _right.
        /// </summary>
        private static readonly Vector3 _right;

        /// <summary>
        /// The _unit x.
        /// </summary>
        private static readonly Vector3 _unitX;

        /// <summary>
        /// The _unit y.
        /// </summary>
        private static readonly Vector3 _unitY;

        /// <summary>
        /// The _unit z.
        /// </summary>
        private static readonly Vector3 _unitZ;

        /// <summary>
        /// The _up.
        /// </summary>
        private static readonly Vector3 _up;

        /// <summary>
        /// The _zero.
        /// </summary>
        private static readonly Vector3 _zero;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="Vector3"/> struct.
        /// </summary>
        static Vector3()
        {
            _zero = new Vector3();
            _one = new Vector3(1f, 1f, 1f);
            _unitX = new Vector3(1f, 0f, 0f);
            _unitY = new Vector3(0f, 1f, 0f);
            _unitZ = new Vector3(0f, 0f, 1f);
            _up = new Vector3(0f, 1f, 0f);
            _down = new Vector3(0f, -1f, 0f);
            _right = new Vector3(1f, 0f, 0f);
            _left = new Vector3(-1f, 0f, 0f);
            _forward = new Vector3(0f, 0f, -1f);
            _backward = new Vector3(0f, 0f, 1f);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> struct.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <param name="z">
        /// The z.
        /// </param>
        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> struct.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public Vector3(float value)
        {
            float single = value;
            float single1 = single;
            this.Z = single;
            float single2 = single1;
            float single3 = single2;
            this.Y = single2;
            this.X = single3;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> struct.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="z">
        /// The z.
        /// </param>
        public Vector3(Vector2 value, float z)
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = z;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Backward.
        /// </summary>
        public static Vector3 Backward
        {
            get
            {
                return _backward;
            }
        }

        /// <summary>
        /// Gets Down.
        /// </summary>
        public static Vector3 Down
        {
            get
            {
                return _down;
            }
        }

        /// <summary>
        /// Gets Forward.
        /// </summary>
        public static Vector3 Forward
        {
            get
            {
                return _forward;
            }
        }

        /// <summary>
        /// Gets Left.
        /// </summary>
        public static Vector3 Left
        {
            get
            {
                return _left;
            }
        }

        /// <summary>
        /// Gets One.
        /// </summary>
        public static Vector3 One
        {
            get
            {
                return _one;
            }
        }

        /// <summary>
        /// Gets Right.
        /// </summary>
        public static Vector3 Right
        {
            get
            {
                return _right;
            }
        }

        /// <summary>
        /// Gets UnitX.
        /// </summary>
        public static Vector3 UnitX
        {
            get
            {
                return _unitX;
            }
        }

        /// <summary>
        /// Gets UnitY.
        /// </summary>
        public static Vector3 UnitY
        {
            get
            {
                return _unitY;
            }
        }

        /// <summary>
        /// Gets UnitZ.
        /// </summary>
        public static Vector3 UnitZ
        {
            get
            {
                return _unitZ;
            }
        }

        /// <summary>
        /// Gets Up.
        /// </summary>
        public static Vector3 Up
        {
            get
            {
                return _up;
            }
        }

        /// <summary>
        /// Gets Zero.
        /// </summary>
        public static Vector3 Zero
        {
            get
            {
                return _zero;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Add(Vector3 value1, Vector3 value2)
        {
            Vector3 x;
            x.X = value1.X + value2.X;
            x.Y = value1.Y + value2.Y;
            x.Z = value1.Z + value2.Z;
            return x;
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Add(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
        }

        /// <summary>
        /// The barycentric.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="value3">
        /// The value 3.
        /// </param>
        /// <param name="amount1">
        /// The amount 1.
        /// </param>
        /// <param name="amount2">
        /// The amount 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Barycentric(Vector3 value1, Vector3 value2, Vector3 value3, float amount1, float amount2)
        {
            Vector3 x;
            x.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
            x.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
            x.Z = value1.Z + amount1 * (value2.Z - value1.Z) + amount2 * (value3.Z - value1.Z);
            return x;
        }

        /// <summary>
        /// The barycentric.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="value3">
        /// The value 3.
        /// </param>
        /// <param name="amount1">
        /// The amount 1.
        /// </param>
        /// <param name="amount2">
        /// The amount 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Barycentric(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, float amount1, float amount2, out Vector3 result)
        {
            result.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
            result.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
            result.Z = value1.Z + amount1 * (value2.Z - value1.Z) + amount2 * (value3.Z - value1.Z);
        }

        /// <summary>
        /// The catmull rom.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="value3">
        /// The value 3.
        /// </param>
        /// <param name="value4">
        /// The value 4.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 CatmullRom(Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount)
        {
            float single = amount * amount;
            float single1 = amount * single;
            Vector3 x;
            x.X = 0.5f * (2f * value2.X + (-value1.X + value3.X) * amount + (2f * value1.X - 5f * value2.X + 4f * value3.X - value4.X) * single + (-value1.X + 3f * value2.X - 3f * value3.X + value4.X) * single1);
            x.Y = 0.5f * (2f * value2.Y + (-value1.Y + value3.Y) * amount + (2f * value1.Y - 5f * value2.Y + 4f * value3.Y - value4.Y) * single + (-value1.Y + 3f * value2.Y - 3f * value3.Y + value4.Y) * single1);
            x.Z = 0.5f * (2f * value2.Z + (-value1.Z + value3.Z) * amount + (2f * value1.Z - 5f * value2.Z + 4f * value3.Z - value4.Z) * single + (-value1.Z + 3f * value2.Z - 3f * value3.Z + value4.Z) * single1);
            return x;
        }

        /// <summary>
        /// The catmull rom.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="value3">
        /// The value 3.
        /// </param>
        /// <param name="value4">
        /// The value 4.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void CatmullRom(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, float amount, out Vector3 result)
        {
            float single = amount * amount;
            float single1 = amount * single;
            result.X = 0.5f * (2f * value2.X + (-value1.X + value3.X) * amount + (2f * value1.X - 5f * value2.X + 4f * value3.X - value4.X) * single + (-value1.X + 3f * value2.X - 3f * value3.X + value4.X) * single1);
            result.Y = 0.5f * (2f * value2.Y + (-value1.Y + value3.Y) * amount + (2f * value1.Y - 5f * value2.Y + 4f * value3.Y - value4.Y) * single + (-value1.Y + 3f * value2.Y - 3f * value3.Y + value4.Y) * single1);
            result.Z = 0.5f * (2f * value2.Z + (-value1.Z + value3.Z) * amount + (2f * value1.Z - 5f * value2.Z + 4f * value3.Z - value4.Z) * single + (-value1.Z + 3f * value2.Z - 3f * value3.Z + value4.Z) * single1);
        }

        /// <summary>
        /// The clamp.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="min">
        /// The min.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
        {
            float x;
            float single;
            float y;
            float y1;
            float z;
            float z1;
            var x1 = value1.X;
            x = x1 > max.X ? max.X : x1;

            x1 = x;
            single = x1 < min.X ? min.X : x1;

            x1 = single;
            var single1 = value1.Y;
            y = single1 > max.Y ? max.Y : single1;

            single1 = y;
            y1 = single1 < min.Y ? min.Y : single1;

            single1 = y1;
            var z2 = value1.Z;
            z = z2 > max.Z ? max.Z : z2;

            z2 = z;
            z1 = z2 < min.Z ? min.Z : z2;

            z2 = z1;
            Vector3 vector3;
            vector3.X = x1;
            vector3.Y = single1;
            vector3.Z = z2;
            return vector3;
        }

        /// <summary>
        /// The clamp.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="min">
        /// The min.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Clamp(ref Vector3 value1, ref Vector3 min, ref Vector3 max, out Vector3 result)
        {
            float x;
            float single;
            float y;
            float y1;
            float z;
            float z1;
            var x1 = value1.X;
            x = x1 > max.X ? max.X : x1;

            x1 = x;
            single = x1 < min.X ? min.X : x1;

            x1 = single;
            var single1 = value1.Y;
            y = single1 > max.Y ? max.Y : single1;

            single1 = y;
            y1 = single1 < min.Y ? min.Y : single1;

            single1 = y1;
            var z2 = value1.Z;
            z = z2 > max.Z ? max.Z : z2;

            z2 = z;
            z1 = z2 < min.Z ? min.Z : z2;

            z2 = z1;
            result.X = x1;
            result.Y = single1;
            result.Z = z2;
        }

        /// <summary>
        /// The cross.
        /// </summary>
        /// <param name="vector1">
        /// The vector 1.
        /// </param>
        /// <param name="vector2">
        /// The vector 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            Vector3 y;
            y.X = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            y.Y = vector1.Z * vector2.X - vector1.X * vector2.Z;
            y.Z = vector1.X * vector2.Y - vector1.Y * vector2.X;
            return y;
        }

        /// <summary>
        /// The cross.
        /// </summary>
        /// <param name="vector1">
        /// The vector 1.
        /// </param>
        /// <param name="vector2">
        /// The vector 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            float y = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            float z = vector1.Z * vector2.X - vector1.X * vector2.Z;
            float x = vector1.X * vector2.Y - vector1.Y * vector2.X;
            result.X = y;
            result.Y = z;
            result.Z = x;
        }

        /// <summary>
        /// The distance.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// The distance.
        /// </returns>
        public static float Distance(Vector3 value1, Vector3 value2)
        {
            float x = value1.X - value2.X;
            float y = value1.Y - value2.Y;
            float z = value1.Z - value2.Z;
            float single = x * x + y * y + z * z;
            return (float)Math.Sqrt(single);
        }

        /// <summary>
        /// The distance.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Distance(ref Vector3 value1, ref Vector3 value2, out float result)
        {
            float x = value1.X - value2.X;
            float y = value1.Y - value2.Y;
            float z = value1.Z - value2.Z;
            float single = x * x + y * y + z * z;
            result = (float)Math.Sqrt(single);
        }

        /// <summary>
        /// The distance squared.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// The distance squared.
        /// </returns>
        public static float DistanceSquared(Vector3 value1, Vector3 value2)
        {
            float x = value1.X - value2.X;
            float y = value1.Y - value2.Y;
            float z = value1.Z - value2.Z;
            return x * x + y * y + z * z;
        }

        /// <summary>
        /// The distance squared.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void DistanceSquared(ref Vector3 value1, ref Vector3 value2, out float result)
        {
            float x = value1.X - value2.X;
            float y = value1.Y - value2.Y;
            float z = value1.Z - value2.Z;
            result = x * x + y * y + z * z;
        }

        /// <summary>
        /// The divide.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Divide(Vector3 value1, Vector3 value2)
        {
            Vector3 x;
            x.X = value1.X / value2.X;
            x.Y = value1.Y / value2.Y;
            x.Z = value1.Z / value2.Z;
            return x;
        }

        /// <summary>
        /// The divide.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Divide(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            result.Z = value1.Z / value2.Z;
        }

        /// <summary>
        /// The divide.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Divide(Vector3 value1, float value2)
        {
            float single = 1f / value2;
            Vector3 x;
            x.X = value1.X * single;
            x.Y = value1.Y * single;
            x.Z = value1.Z * single;
            return x;
        }

        /// <summary>
        /// The divide.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Divide(ref Vector3 value1, float value2, out Vector3 result)
        {
            float single = 1f / value2;
            result.X = value1.X * single;
            result.Y = value1.Y * single;
            result.Z = value1.Z * single;
        }

        /// <summary>
        /// The dot.
        /// </summary>
        /// <param name="vector1">
        /// The vector 1.
        /// </param>
        /// <param name="vector2">
        /// The vector 2.
        /// </param>
        /// <returns>
        /// The dot.
        /// </returns>
        public static float Dot(Vector3 vector1, Vector3 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        /// <summary>
        /// The dot.
        /// </summary>
        /// <param name="vector1">
        /// The vector 1.
        /// </param>
        /// <param name="vector2">
        /// The vector 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Dot(ref Vector3 vector1, ref Vector3 vector2, out float result)
        {
            result = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        /// <summary>
        /// The hermite.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="tangent1">
        /// The tangent 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="tangent2">
        /// The tangent 2.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount)
        {
            float single = amount * amount;
            float single1 = amount * single;
            float single2 = 2f * single1 - 3f * single + 1f;
            float single3 = -2f * single1 + 3f * single;
            float single4 = single1 - 2f * single + amount;
            float single5 = single1 - single;
            Vector3 x;
            x.X = value1.X * single2 + value2.X * single3 + tangent1.X * single4 + tangent2.X * single5;
            x.Y = value1.Y * single2 + value2.Y * single3 + tangent1.Y * single4 + tangent2.Y * single5;
            x.Z = value1.Z * single2 + value2.Z * single3 + tangent1.Z * single4 + tangent2.Z * single5;
            return x;
        }

        /// <summary>
        /// The hermite.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="tangent1">
        /// The tangent 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="tangent2">
        /// The tangent 2.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Hermite(ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount, out Vector3 result)
        {
            float single = amount * amount;
            float single1 = amount * single;
            float single2 = 2f * single1 - 3f * single + 1f;
            float single3 = -2f * single1 + 3f * single;
            float single4 = single1 - 2f * single + amount;
            float single5 = single1 - single;
            result.X = value1.X * single2 + value2.X * single3 + tangent1.X * single4 + tangent2.X * single5;
            result.Y = value1.Y * single2 + value2.Y * single3 + tangent1.Y * single4 + tangent2.Y * single5;
            result.Z = value1.Z * single2 + value2.Z * single3 + tangent1.Z * single4 + tangent2.Z * single5;
        }

        /// <summary>
        /// The lerp.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
        {
            Vector3 x;
            x.X = value1.X + (value2.X - value1.X) * amount;
            x.Y = value1.Y + (value2.Y - value1.Y) * amount;
            x.Z = value1.Z + (value2.Z - value1.Z) * amount;
            return x;
        }

        /// <summary>
        /// The lerp.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Lerp(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
        {
            result.X = value1.X + (value2.X - value1.X) * amount;
            result.Y = value1.Y + (value2.Y - value1.Y) * amount;
            result.Z = value1.Z + (value2.Z - value1.Z) * amount;
        }

        /// <summary>
        /// The max.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Max(Vector3 value1, Vector3 value2)
        {
            float x;
            float y;
            float z;

            x = value1.X > value2.X ? value1.X : value2.X;

            y = value1.Y > value2.Y ? value1.Y : value2.Y;

            z = value1.Z > value2.Z ? value1.Z : value2.Z;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// The max.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Max(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            float x;
            float y;
            float z;

            x = value1.X > value2.X ? value1.X : value2.X;

            y = value1.Y > value2.Y ? value1.Y : value2.Y;

            z = value1.Z > value2.Z ? value1.Z : value2.Z;

            result = new Vector3(x, y, z);
        }

        /// <summary>
        /// The min.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Min(Vector3 value1, Vector3 value2)
        {
            float x;
            float y;
            float z;

            x = value1.X < value2.X ? value1.X : value2.X;

            y = value1.Y < value2.Y ? value1.Y : value2.Y;

            z = value1.Z < value2.Z ? value1.Z : value2.Z;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// The min.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Min(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            float x;
            float y;
            float z;

            x = value1.X < value2.X ? value1.X : value2.X;

            y = value1.Y < value2.Y ? value1.Y : value2.Y;

            z = value1.Z < value2.Z ? value1.Z : value2.Z;

            result = new Vector3(x, y, z);
        }

        /// <summary>
        /// The multiply.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Multiply(Vector3 value1, Vector3 value2)
        {
            Vector3 x;
            x.X = value1.X * value2.X;
            x.Y = value1.Y * value2.Y;
            x.Z = value1.Z * value2.Z;
            return x;
        }

        /// <summary>
        /// The multiply.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Multiply(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            result.Z = value1.Z * value2.Z;
        }

        /// <summary>
        /// The multiply.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="scaleFactor">
        /// The scale factor.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Multiply(Vector3 value1, float scaleFactor)
        {
            Vector3 x;
            x.X = value1.X * scaleFactor;
            x.Y = value1.Y * scaleFactor;
            x.Z = value1.Z * scaleFactor;
            return x;
        }

        /// <summary>
        /// The multiply.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="scaleFactor">
        /// The scale factor.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Multiply(ref Vector3 value1, float scaleFactor, out Vector3 result)
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
            result.Z = value1.Z * scaleFactor;
        }

        /// <summary>
        /// The negate.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Negate(Vector3 value)
        {
            Vector3 x;
            x.X = -value.X;
            x.Y = -value.Y;
            x.Z = -value.Z;
            return x;
        }

        /// <summary>
        /// The negate.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Negate(ref Vector3 value, out Vector3 result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
        }

        /// <summary>
        /// The normalize.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Normalize(Vector3 value)
        {
            float x = value.X * value.X + value.Y * value.Y + value.Z * value.Z;
            float single = 1f / (float)Math.Sqrt(x);
            Vector3 y;
            y.X = value.X * single;
            y.Y = value.Y * single;
            y.Z = value.Z * single;
            return y;
        }

        /// <summary>
        /// The normalize.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Normalize(ref Vector3 value, out Vector3 result)
        {
            float x = value.X * value.X + value.Y * value.Y + value.Z * value.Z;
            float single = 1f / (float)Math.Sqrt(x);
            result.X = value.X * single;
            result.Y = value.Y * single;
            result.Z = value.Z * single;
        }

        /// <summary>
        /// The reflect.
        /// </summary>
        /// <param name="vector">
        /// The vector.
        /// </param>
        /// <param name="normal">
        /// The normal.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        {
            float x = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
            Vector3 y;
            y.X = vector.X - 2f * x * normal.X;
            y.Y = vector.Y - 2f * x * normal.Y;
            y.Z = vector.Z - 2f * x * normal.Z;
            return y;
        }

        /// <summary>
        /// The reflect.
        /// </summary>
        /// <param name="vector">
        /// The vector.
        /// </param>
        /// <param name="normal">
        /// The normal.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
        {
            float x = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
            result.X = vector.X - 2f * x * normal.X;
            result.Y = vector.Y - 2f * x * normal.Y;
            result.Z = vector.Z - 2f * x * normal.Z;
        }

        /// <summary>
        /// The smooth step.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 SmoothStep(Vector3 value1, Vector3 value2, float amount)
        {
            float single;
            if (amount > 1f)
            {
                single = 1f;
            }
            else
            {
                single = amount < 0f ? 0f : amount;
            }

            amount = single;
            amount = amount * amount * (3f - 2f * amount);
            Vector3 x;
            x.X = value1.X + (value2.X - value1.X) * amount;
            x.Y = value1.Y + (value2.Y - value1.Y) * amount;
            x.Z = value1.Z + (value2.Z - value1.Z) * amount;
            return x;
        }

        /// <summary>
        /// The smooth step.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void SmoothStep(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
        {
            float single;
            if (amount > 1f)
            {
                single = 1f;
            }
            else
            {
                single = amount < 0f ? 0f : amount;
            }

            amount = single;
            amount = amount * amount * (3f - 2f * amount);
            result.X = value1.X + (value2.X - value1.X) * amount;
            result.Y = value1.Y + (value2.Y - value1.Y) * amount;
            result.Z = value1.Z + (value2.Z - value1.Z) * amount;
        }

        /// <summary>
        /// The subtract.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Subtract(Vector3 value1, Vector3 value2)
        {
            Vector3 x;
            x.X = value1.X - value2.X;
            x.Y = value1.Y - value2.Y;
            x.Z = value1.Z - value2.Z;
            return x;
        }

        /// <summary>
        /// The subtract.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Subtract(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            result.Z = value1.Z - value2.Z;
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
        public static Vector3 operator +(Vector3 value1, Vector3 value2)
        {
            Vector3 x;
            x.X = value1.X + value2.X;
            x.Y = value1.Y + value2.Y;
            x.Z = value1.Z + value2.Z;
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
        public static Vector3 operator /(Vector3 value1, Vector3 value2)
        {
            Vector3 x;
            x.X = value1.X / value2.X;
            x.Y = value1.Y / value2.Y;
            x.Z = value1.Z / value2.Z;
            return x;
        }

        /// <summary>
        /// The /.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="divider">
        /// The divider.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 operator /(Vector3 value, float divider)
        {
            float single = 1f / divider;
            Vector3 x;
            x.X = value.X * single;
            x.Y = value.Y * single;
            x.Z = value.Z * single;
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
        public static bool operator ==(Vector3 value1, Vector3 value2)
        {
            if (Math.Abs(value1.X - value2.X) > Mathf.Epsilon || Math.Abs(value1.Y - value2.Y) > Mathf.Epsilon)
            {
                return false;
            }

            return Math.Abs(value1.Z - value2.Z) < Mathf.Epsilon;
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
        public static bool operator !=(Vector3 value1, Vector3 value2)
        {
            if (Math.Abs(value1.X - value2.X) > Mathf.Epsilon || Math.Abs(value1.Y - value2.Y) > Mathf.Epsilon)
            {
                return true;
            }

            return Math.Abs(value1.Z - value2.Z) > Mathf.Epsilon;
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
        public static Vector3 operator *(Vector3 value1, Vector3 value2)
        {
            Vector3 x;
            x.X = value1.X * value2.X;
            x.Y = value1.Y * value2.Y;
            x.Z = value1.Z * value2.Z;
            return x;
        }

        /// <summary>
        /// The *.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="scaleFactor">
        /// The scale factor.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 operator *(Vector3 value, float scaleFactor)
        {
            Vector3 x;
            x.X = value.X * scaleFactor;
            x.Y = value.Y * scaleFactor;
            x.Z = value.Z * scaleFactor;
            return x;
        }

        /// <summary>
        /// The *.
        /// </summary>
        /// <param name="scaleFactor">
        /// The scale factor.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 operator *(float scaleFactor, Vector3 value)
        {
            Vector3 x;
            x.X = value.X * scaleFactor;
            x.Y = value.Y * scaleFactor;
            x.Z = value.Z * scaleFactor;
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
        public static Vector3 operator -(Vector3 value1, Vector3 value2)
        {
            Vector3 x;
            x.X = value1.X - value2.X;
            x.Y = value1.Y - value2.Y;
            x.Z = value1.Z - value2.Z;
            return x;
        }

        /// <summary>
        /// The -.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 operator -(Vector3 value)
        {
            Vector3 x;
            x.X = -value.X;
            x.Y = -value.Y;
            x.Z = -value.Z;
            return x;
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The equals.
        /// </returns>
        public bool Equals(Vector3 other)
        {
            if (Math.Abs(this.X - other.X) > Mathf.Epsilon || Math.Abs(this.Y - other.Y) > Mathf.Epsilon)
            {
                return false;
            }

            return Math.Abs(this.Z - other.Z) < Mathf.Epsilon;
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The equals.
        /// </returns>
        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Vector3)
            {
                flag = this.Equals((Vector3)obj);
            }

            return flag;
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The get hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode();
        }

        /// <summary>
        /// The length.
        /// </summary>
        /// <returns>
        /// The length.
        /// </returns>
        public float Length()
        {
            float x = this.X * this.X + this.Y * this.Y + this.Z * this.Z;
            return (float)Math.Sqrt(x);
        }

        /// <summary>
        /// The length squared.
        /// </summary>
        /// <returns>
        /// The length squared.
        /// </returns>
        public float LengthSquared()
        {
            return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
        }

        /// <summary>
        /// The normalize.
        /// </summary>
        public void Normalize()
        {
            float x = this.X * this.X + this.Y * this.Y + this.Z * this.Z;
            float single = 1f / (float)Math.Sqrt(x);
            Vector3 vector3 = this;
            vector3.X = vector3.X * single;
            Vector3 y = this;
            y.Y = y.Y * single;
            Vector3 z = this;
            z.Z = z.Z * single;
        }

        /// <summary>
        /// Returns a String that represents the current Vector3.
        /// </summary>
        /// <returns>
        /// The String that represents the current Vector3.
        /// </returns>  
        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            var str = new object[3];
            str[0] = this.X.ToString(currentCulture);
            str[1] = this.Y.ToString(currentCulture);
            str[2] = this.Z.ToString(currentCulture);
            return string.Format(currentCulture, "{{X:{0} Y:{1} Z:{2}}}", str);
        }

        #endregion

        // public static Vector3 Transform(Vector3 position, Matrix matrix)
        // {
        // float x = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
        // float single = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
        // float x1 = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;
        // Vector3 vector3.X = x;
        // vector3.Y = single;
        // vector3.Z = x1;
        // return vector3;
        // }

        // public static void Transform(ref Vector3 position, ref Matrix matrix, out Vector3 result)
        // {
        // float x = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
        // float single = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
        // float x1 = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;
        // result.X = x;
        // result.Y = single;
        // result.Z = x1;
        // }

        // public static Vector3 Transform(Vector3 value, Quaternion rotation)
        // {
        // float x = rotation.X + rotation.X;
        // float y = rotation.Y + rotation.Y;
        // float z = rotation.Z + rotation.Z;
        // float w = rotation.W * x;
        // float single = rotation.W * y;
        // float w1 = rotation.W * z;
        // float x1 = rotation.X * x;
        // float single1 = rotation.X * y;
        // float x2 = rotation.X * z;
        // float y1 = rotation.Y * y;
        // float y2 = rotation.Y * z;
        // float z1 = rotation.Z * z;
        // float single2 = value.X * (1f - y1 - z1) + value.Y * (single1 - w1) + value.Z * (x2 + single);
        // float x3 = value.X * (single1 + w1) + value.Y * (1f - x1 - z1) + value.Z * (y2 - w);
        // float single3 = value.X * (x2 - single) + value.Y * (y2 + w) + value.Z * (1f - x1 - y1);
        // Vector3 vector3.X = single2;
        // vector3.Y = x3;
        // vector3.Z = single3;
        // return vector3;
        // }

        // public static void Transform(ref Vector3 value, ref Quaternion rotation, out Vector3 result)
        // {
        // float x = rotation.X + rotation.X;
        // float y = rotation.Y + rotation.Y;
        // float z = rotation.Z + rotation.Z;
        // float w = rotation.W * x;
        // float single = rotation.W * y;
        // float w1 = rotation.W * z;
        // float x1 = rotation.X * x;
        // float single1 = rotation.X * y;
        // float x2 = rotation.X * z;
        // float y1 = rotation.Y * y;
        // float y2 = rotation.Y * z;
        // float z1 = rotation.Z * z;
        // float single2 = value.X * (1f - y1 - z1) + value.Y * (single1 - w1) + value.Z * (x2 + single);
        // float x3 = value.X * (single1 + w1) + value.Y * (1f - x1 - z1) + value.Z * (y2 - w);
        // float single3 = value.X * (x2 - single) + value.Y * (y2 + w) + value.Z * (1f - x1 - y1);
        // result.X = single2;
        // result.Y = x3;
        // result.Z = single3;
        // }

        // public static void Transform(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
        // {
        // if (sourceArray != null)
        // {
        // if (destinationArray != null)
        // {
        // if ((int)destinationArray.Length >= (int)sourceArray.Length)
        // {
        // for (int i = 0; i < (int)sourceArray.Length; i++)
        // {
        // float x = sourceArray[i].X;
        // float y = sourceArray[i].Y;
        // float z = sourceArray[i].Z;
        // destinationArray[i].X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + matrix.M41;
        // destinationArray[i].Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + matrix.M42;
        // destinationArray[i].Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + matrix.M43;
        // }
        // return;
        // }
        // else
        // {
        // throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
        // }
        // }
        // else
        // {
        // throw new ArgumentNullException("destinationArray");
        // }
        // }
        // else
        // {
        // throw new ArgumentNullException("sourceArray");
        // }
        // }

        // public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Matrix matrix, Vector3[] destinationArray, int destinationIndex, int length)
        // {
        // if (sourceArray != null)
        // {
        // if (destinationArray != null)
        // {
        // if ((long)((int)sourceArray.Length) >= (long)sourceIndex + (long)length)
        // {
        // if ((long)((int)destinationArray.Length) >= (long)destinationIndex + (long)length)
        // {
        // while (length > 0)
        // {
        // float x = sourceArray[sourceIndex].X;
        // float y = sourceArray[sourceIndex].Y;
        // float z = sourceArray[sourceIndex].Z;
        // destinationArray[destinationIndex].X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + matrix.M41;
        // destinationArray[destinationIndex].Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + matrix.M42;
        // destinationArray[destinationIndex].Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + matrix.M43;
        // sourceIndex++;
        // destinationIndex++;
        // length--;
        // }
        // return;
        // }
        // else
        // {
        // throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
        // }
        // }
        // else
        // {
        // throw new ArgumentException(FrameworkResources.NotEnoughSourceSize);
        // }
        // }
        // else
        // {
        // throw new ArgumentNullException("destinationArray");
        // }
        // }
        // else
        // {
        // throw new ArgumentNullException("sourceArray");
        // }
        // }

        // public static void Transform(Vector3[] sourceArray, ref Quaternion rotation, Vector3[] destinationArray)
        // {
        // if (sourceArray != null)
        // {
        // if (destinationArray != null)
        // {
        // if ((int)destinationArray.Length >= (int)sourceArray.Length)
        // {
        // float x = rotation.X + rotation.X;
        // float y = rotation.Y + rotation.Y;
        // float z = rotation.Z + rotation.Z;
        // float w = rotation.W * x;
        // float single = rotation.W * y;
        // float w1 = rotation.W * z;
        // float x1 = rotation.X * x;
        // float single1 = rotation.X * y;
        // float x2 = rotation.X * z;
        // float y1 = rotation.Y * y;
        // float y2 = rotation.Y * z;
        // float z1 = rotation.Z * z;
        // float single2 = 1f - y1 - z1;
        // float single3 = single1 - w1;
        // float single4 = x2 + single;
        // float single5 = single1 + w1;
        // float single6 = 1f - x1 - z1;
        // float single7 = y2 - w;
        // float single8 = x2 - single;
        // float single9 = y2 + w;
        // float single10 = 1f - x1 - y1;
        // for (int i = 0; i < (int)sourceArray.Length; i++)
        // {
        // float x3 = sourceArray[i].X;
        // float y3 = sourceArray[i].Y;
        // float z2 = sourceArray[i].Z;
        // destinationArray[i].X = x3 * single2 + y3 * single3 + z2 * single4;
        // destinationArray[i].Y = x3 * single5 + y3 * single6 + z2 * single7;
        // destinationArray[i].Z = x3 * single8 + y3 * single9 + z2 * single10;
        // }
        // return;
        // }
        // else
        // {
        // throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
        // }
        // }
        // else
        // {
        // throw new ArgumentNullException("destinationArray");
        // }
        // }
        // else
        // {
        // throw new ArgumentNullException("sourceArray");
        // }
        // }

        // public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector3[] destinationArray, int destinationIndex, int length)
        // {
        // if (sourceArray != null)
        // {
        // if (destinationArray != null)
        // {
        // if ((long)((int)sourceArray.Length) >= (long)sourceIndex + (long)length)
        // {
        // if ((long)((int)destinationArray.Length) >= (long)destinationIndex + (long)length)
        // {
        // float x = rotation.X + rotation.X;
        // float y = rotation.Y + rotation.Y;
        // float z = rotation.Z + rotation.Z;
        // float w = rotation.W * x;
        // float single = rotation.W * y;
        // float w1 = rotation.W * z;
        // float x1 = rotation.X * x;
        // float single1 = rotation.X * y;
        // float x2 = rotation.X * z;
        // float y1 = rotation.Y * y;
        // float y2 = rotation.Y * z;
        // float z1 = rotation.Z * z;
        // float single2 = 1f - y1 - z1;
        // float single3 = single1 - w1;
        // float single4 = x2 + single;
        // float single5 = single1 + w1;
        // float single6 = 1f - x1 - z1;
        // float single7 = y2 - w;
        // float single8 = x2 - single;
        // float single9 = y2 + w;
        // float single10 = 1f - x1 - y1;
        // while (length > 0)
        // {
        // float x3 = sourceArray[sourceIndex].X;
        // float y3 = sourceArray[sourceIndex].Y;
        // float z2 = sourceArray[sourceIndex].Z;
        // destinationArray[destinationIndex].X = x3 * single2 + y3 * single3 + z2 * single4;
        // destinationArray[destinationIndex].Y = x3 * single5 + y3 * single6 + z2 * single7;
        // destinationArray[destinationIndex].Z = x3 * single8 + y3 * single9 + z2 * single10;
        // sourceIndex++;
        // destinationIndex++;
        // length--;
        // }
        // return;
        // }
        // else
        // {
        // throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
        // }
        // }
        // else
        // {
        // throw new ArgumentException(FrameworkResources.NotEnoughSourceSize);
        // }
        // }
        // else
        // {
        // throw new ArgumentNullException("destinationArray");
        // }
        // }
        // else
        // {
        // throw new ArgumentNullException("sourceArray");
        // }
        // }

        // public static Vector3 TransformNormal(Vector3 normal, Matrix matrix)
        // {
        // float x = normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31;
        // float single = normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32;
        // float x1 = normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33;
        // Vector3 vector3.X = x;
        // vector3.Y = single;
        // vector3.Z = x1;
        // return vector3;
        // }

        // public static void TransformNormal(ref Vector3 normal, ref Matrix matrix, out Vector3 result)
        // {
        // float x = normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31;
        // float single = normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32;
        // float x1 = normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33;
        // result.X = x;
        // result.Y = single;
        // result.Z = x1;
        // }

        // public static void TransformNormal(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
        // {
        // if (sourceArray != null)
        // {
        // if (destinationArray != null)
        // {
        // if ((int)destinationArray.Length >= (int)sourceArray.Length)
        // {
        // for (int i = 0; i < (int)sourceArray.Length; i++)
        // {
        // float x = sourceArray[i].X;
        // float y = sourceArray[i].Y;
        // float z = sourceArray[i].Z;
        // destinationArray[i].X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31;
        // destinationArray[i].Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32;
        // destinationArray[i].Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33;
        // }
        // return;
        // }
        // else
        // {
        // throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
        // }
        // }
        // else
        // {
        // throw new ArgumentNullException("destinationArray");
        // }
        // }
        // else
        // {
        // throw new ArgumentNullException("sourceArray");
        // }
        // }

        // public static void TransformNormal(Vector3[] sourceArray, int sourceIndex, ref Matrix matrix, Vector3[] destinationArray, int destinationIndex, int length)
        // {
        // if (sourceArray != null)
        // {
        // if (destinationArray != null)
        // {
        // if ((long)((int)sourceArray.Length) >= (long)sourceIndex + (long)length)
        // {
        // if ((long)((int)destinationArray.Length) >= (long)destinationIndex + (long)length)
        // {
        // while (length > 0)
        // {
        // float x = sourceArray[sourceIndex].X;
        // float y = sourceArray[sourceIndex].Y;
        // float z = sourceArray[sourceIndex].Z;
        // destinationArray[destinationIndex].X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31;
        // destinationArray[destinationIndex].Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32;
        // destinationArray[destinationIndex].Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33;
        // sourceIndex++;
        // destinationIndex++;
        // length--;
        // }
        // return;
        // }
        // else
        // {
        // throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
        // }
        // }
        // else
        // {
        // throw new ArgumentException(FrameworkResources.NotEnoughSourceSize);
        // }
        // }
        // else
        // {
        // throw new ArgumentNullException("destinationArray");
        // }
        // }
        // else
        // {
        // throw new ArgumentNullException("sourceArray");
        // }
        // }
    }
}