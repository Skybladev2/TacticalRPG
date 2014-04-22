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
    /// The vector 2.
    /// </summary>
    [Serializable]
    public partial struct Vector2
    {
        #region Constants and Fields

        /// <summary>
        /// The x value.
        /// </summary>
        public float X;

        /// <summary>
        /// The y value.
        /// </summary>
        public float Y;

        /// <summary>
        /// Static <see cref="Vector2"/> that equals one.
        /// </summary>
        /// <remarks>The <see cref="One"/> property will return this value.</remarks>
        private static readonly Vector2 _one;

        /// <summary>
        /// Static <see cref="Vector2"/> that points in the X direction.
        /// </summary>
        /// <remarks>The <see cref="UnitX"/> property will return this value.</remarks>
        private static readonly Vector2 _unitX;

        /// <summary>
        /// Static <see cref="Vector2"/> that points in the Y direction.
        /// </summary>
        /// <remarks>The <see cref="UnitY"/> property will return this value.</remarks>
        private static readonly Vector2 _unitY;

        /// <summary>
        /// Static <see cref="Vector2"/> that equals zero.
        /// </summary>
        /// <remarks>The <see cref="Zero"/> property will return this value.</remarks>
        private static readonly Vector2 _zero;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="Vector2"/> struct.
        /// </summary>
        static Vector2()
        {
            _zero = new Vector2();
            _one = new Vector2(1f, 1f);
            _unitX = new Vector2(1f, 0f);
            _unitY = new Vector2(0f, 1f);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> struct.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> struct.
        /// </summary>
        /// <param name="value">
        /// The value for the X and Y components.
        /// </param>
        public Vector2(float value)
        {
             this.Y = value;
            this.X = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Static <see cref="Vector2"/> that equals one.
        /// </summary>
        public static Vector2 One
        {
            get
            {
                return _one;
            }
        }

        /// <summary>
        /// Static <see cref="Vector2"/> that points in the X direction.
        /// </summary>
        public static Vector2 UnitX
        {
            get
            {
                return _unitX;
            }
        }

        /// <summary>
        /// Static <see cref="Vector2"/> that points in the Y direction.
        /// </summary>
        public static Vector2 UnitY
        {
            get
            {
                return _unitY;
            }
        }

        /// <summary>
        /// Static <see cref="Vector2"/> that equals zero.
        /// </summary>
        public static Vector2 Zero
        {
            get
            {
                return _zero;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// Returns the value of the two vectors added together.
        /// </returns>
        public static Vector2 Add(Vector2 value1, Vector2 value2)
        {
            Vector2 x;
            x.X = value1.X + value2.X;
            x.Y = value1.Y + value2.Y;
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
        /// The result of from adding the two vectors together.
        /// </param>
        public static void Add(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
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
        public static Vector2 Barycentric(Vector2 value1, Vector2 value2, Vector2 value3, float amount1, float amount2)
        {
            Vector2 x;
            x.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
            x.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
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
        public static void Barycentric(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, float amount1, float amount2, out Vector2 result)
        {
            result.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
            result.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
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
        public static Vector2 CatmullRom(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount)
        {
            float single = amount * amount;
            float single1 = amount * single;
            Vector2 x;
            x.X = 0.5f * (2f * value2.X + (-value1.X + value3.X) * amount + (2f * value1.X - 5f * value2.X + 4f * value3.X - value4.X) * single + (-value1.X + 3f * value2.X - 3f * value3.X + value4.X) * single1);
            x.Y = 0.5f * (2f * value2.Y + (-value1.Y + value3.Y) * amount + (2f * value1.Y - 5f * value2.Y + 4f * value3.Y - value4.Y) * single + (-value1.Y + 3f * value2.Y - 3f * value3.Y + value4.Y) * single1);
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
        public static void CatmullRom(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4, float amount, out Vector2 result)
        {
            float single = amount * amount;
            float single1 = amount * single;
            result.X = 0.5f * (2f * value2.X + (-value1.X + value3.X) * amount + (2f * value1.X - 5f * value2.X + 4f * value3.X - value4.X) * single + (-value1.X + 3f * value2.X - 3f * value3.X + value4.X) * single1);
            result.Y = 0.5f * (2f * value2.Y + (-value1.Y + value3.Y) * amount + (2f * value1.Y - 5f * value2.Y + 4f * value3.Y - value4.Y) * single + (-value1.Y + 3f * value2.Y - 3f * value3.Y + value4.Y) * single1);
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
        public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
        {
            float x;
            float single;
            float y;
            float y1;
            float x1 = value1.X;
            x = x1 > max.X ? max.X : x1;

            x1 = x;
            single = x1 < min.X ? min.X : x1;

            x1 = single;
            float single1 = value1.Y;
            y = single1 > max.Y ? max.Y : single1;

            single1 = y;
            y1 = single1 < min.Y ? min.Y : single1;

            single1 = y1;
            Vector2 vector2;
            vector2.X = x1;
            vector2.Y = single1;
            return vector2;
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
        public static void Clamp(ref Vector2 value1, ref Vector2 min, ref Vector2 max, out Vector2 result)
        {
            float x;
            float single;
            float y;
            float y1;
            float x1 = value1.X;
            x = x1 > max.X ? max.X : x1;

            x1 = x;
            single = x1 < min.X ? min.X : x1;

            x1 = single;
            float single1 = value1.Y;
            y = single1 > max.Y ? max.Y : single1;

            single1 = y;
            y1 = single1 < min.Y ? min.Y : single1;

            single1 = y1;
            result.X = x1;
            result.Y = single1;
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
        public static float Distance(Vector2 value1, Vector2 value2)
        {
            float x = value1.X - value2.X;
            float y = value1.Y - value2.Y;
            float single = x * x + y * y;
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
        public static void Distance(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            float x = value1.X - value2.X;
            float y = value1.Y - value2.Y;
            float single = x * x + y * y;
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
        public static float DistanceSquared(Vector2 value1, Vector2 value2)
        {
            float x = value1.X - value2.X;
            float y = value1.Y - value2.Y;
            return x * x + y * y;
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
        public static void DistanceSquared(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            float x = value1.X - value2.X;
            float y = value1.Y - value2.Y;
            result = x * x + y * y;
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
        public static Vector2 Divide(Vector2 value1, Vector2 value2)
        {
            Vector2 x;
            x.X = value1.X / value2.X;
            x.Y = value1.Y / value2.Y;
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
        public static void Divide(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
        }

        /// <summary>
        /// The divide.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="divider">
        /// The divider.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector2 Divide(Vector2 value1, float divider)
        {
            float single = 1f / divider;
            Vector2 x;
            x.X = value1.X * single;
            x.Y = value1.Y * single;
            return x;
        }

        /// <summary>
        /// The divide.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="divider">
        /// The divider.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public static void Divide(ref Vector2 value1, float divider, out Vector2 result)
        {
            float single = 1f / divider;
            result.X = value1.X * single;
            result.Y = value1.Y * single;
        }

        /// <summary>
        /// The dot.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="value2">
        /// The value 2.
        /// </param>
        /// <returns>
        /// The dot.
        /// </returns>
        public static float Dot(Vector2 value1, Vector2 value2)
        {
            return value1.X * value2.X + value1.Y * value2.Y;
        }

        /// <summary>
        /// The dot.
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
        public static void Dot(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            result = value1.X * value2.X + value1.Y * value2.Y;
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
        public static Vector2 Hermite(Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount)
        {
            float single = amount * amount;
            float single1 = amount * single;
            float single2 = 2f * single1 - 3f * single + 1f;
            float single3 = -2f * single1 + 3f * single;
            float single4 = single1 - 2f * single + amount;
            float single5 = single1 - single;
            Vector2 x;
            x.X = value1.X * single2 + value2.X * single3 + tangent1.X * single4 + tangent2.X * single5;
            x.Y = value1.Y * single2 + value2.Y * single3 + tangent1.Y * single4 + tangent2.Y * single5;
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
        public static void Hermite(ref Vector2 value1, ref Vector2 tangent1, ref Vector2 value2, ref Vector2 tangent2, float amount, out Vector2 result)
        {
            float single = amount * amount;
            float single1 = amount * single;
            float single2 = 2f * single1 - 3f * single + 1f;
            float single3 = -2f * single1 + 3f * single;
            float single4 = single1 - 2f * single + amount;
            float single5 = single1 - single;
            result.X = value1.X * single2 + value2.X * single3 + tangent1.X * single4 + tangent2.X * single5;
            result.Y = value1.Y * single2 + value2.Y * single3 + tangent1.Y * single4 + tangent2.Y * single5;
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
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
        {
            Vector2 x;
            x.X = value1.X + (value2.X - value1.X) * amount;
            x.Y = value1.Y + (value2.Y - value1.Y) * amount;
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
        public static void Lerp(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
        {
            result.X = value1.X + (value2.X - value1.X) * amount;
            result.Y = value1.Y + (value2.Y - value1.Y) * amount;
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
        public static Vector2 Max(Vector2 value1, Vector2 value2)
        {
            float x;
            float y;
            x = value1.X > value2.X ? value1.X : value2.X;

            y = value1.Y > value2.Y ? value1.Y : value2.Y;

            return new Vector2(x, y);
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
        public static void Max(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            float x;
            float y;
            x = value1.X > value2.X ? value1.X : value2.X;

            y = value1.Y > value2.Y ? value1.Y : value2.Y;

            result = new Vector2(x, y);
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
        public static Vector2 Min(Vector2 value1, Vector2 value2)
        {
            float x;
            float y;
            x = value1.X < value2.X ? value1.X : value2.X;

            y = value1.Y < value2.Y ? value1.Y : value2.Y;

            return new Vector2(x, y);
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
        public static void Min(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            float x;
            float y;
            x = value1.X < value2.X ? value1.X : value2.X;

            y = value1.Y < value2.Y ? value1.Y : value2.Y;

            result = new Vector2(x, y);
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
        public static Vector2 Multiply(Vector2 value1, Vector2 value2)
        {
            Vector2 x;
            x.X = value1.X * value2.X;
            x.Y = value1.Y * value2.Y;
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
        public static void Multiply(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
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
        public static Vector2 Multiply(Vector2 value1, float scaleFactor)
        {
            Vector2 x;
            x.X = value1.X * scaleFactor;
            x.Y = value1.Y * scaleFactor;
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
        public static void Multiply(ref Vector2 value1, float scaleFactor, out Vector2 result)
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
        }

        /// <summary>
        /// The negate.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector2 Negate(Vector2 value)
        {
            Vector2 x;
            x.X = -value.X;
            x.Y = -value.Y;
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
        public static void Negate(ref Vector2 value, out Vector2 result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
        }

        /// <summary>
        /// The normalize.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector2 Normalize(Vector2 value)
        {
            float x = value.X * value.X + value.Y * value.Y;
            float single = 1f / (float)Math.Sqrt(x);
            Vector2 y;
            y.X = value.X * single;
            y.Y = value.Y * single;
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
        public static void Normalize(ref Vector2 value, out Vector2 result)
        {
            float x = value.X * value.X + value.Y * value.Y;
            float single = 1f / (float)Math.Sqrt(x);
            result.X = value.X * single;
            result.Y = value.Y * single;
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
        public static Vector2 Reflect(Vector2 vector, Vector2 normal)
        {
            float x = vector.X * normal.X + vector.Y * normal.Y;
            Vector2 y;
            y.X = vector.X - 2f * x * normal.X;
            y.Y = vector.Y - 2f * x * normal.Y;
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
        public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
        {
            float x = vector.X * normal.X + vector.Y * normal.Y;
            result.X = vector.X - 2f * x * normal.X;
            result.Y = vector.Y - 2f * x * normal.Y;
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
        public static Vector2 SmoothStep(Vector2 value1, Vector2 value2, float amount)
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
            Vector2 x;
            x.X = value1.X + (value2.X - value1.X) * amount;
            x.Y = value1.Y + (value2.Y - value1.Y) * amount;
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
        public static void SmoothStep(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
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
        public static Vector2 Subtract(Vector2 value1, Vector2 value2)
        {
            Vector2 x;
            x.X = value1.X - value2.X;
            x.Y = value1.Y - value2.Y;
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
        public static void Subtract(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
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
        public static Vector2 operator +(Vector2 value1, Vector2 value2)
        {
            Vector2 x;
            x.X = value1.X + value2.X;
            x.Y = value1.Y + value2.Y;
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
        public static Vector2 operator /(Vector2 value1, Vector2 value2)
        {
            Vector2 x;
            x.X = value1.X / value2.X;
            x.Y = value1.Y / value2.Y;
            return x;
        }

        /// <summary>
        /// The /.
        /// </summary>
        /// <param name="value1">
        /// The value 1.
        /// </param>
        /// <param name="divider">
        /// The divider.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector2 operator /(Vector2 value1, float divider)
        {
            float single = 1f / divider;
            Vector2 x;
            x.X = value1.X * single;
            x.Y = value1.Y * single;
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
        public static bool operator ==(Vector2 value1, Vector2 value2)
        {
            if (Math.Abs(value1.X - value2.X) > Mathf.Epsilon)
            {
                return false;
            }

            return Math.Abs(value1.Y - value2.Y) < Mathf.Epsilon;
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
        public static bool operator !=(Vector2 value1, Vector2 value2)
        {
            if (Math.Abs(value1.X - value2.X) > Mathf.Epsilon)
            {
                return true;
            }

            return Math.Abs(value1.Y - value2.Y) > Mathf.Epsilon;
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
        public static Vector2 operator *(Vector2 value1, Vector2 value2)
        {
            Vector2 x;
            x.X = value1.X * value2.X;
            x.Y = value1.Y * value2.Y;
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
        public static Vector2 operator *(Vector2 value, float scaleFactor)
        {
            Vector2 x;
            x.X = value.X * scaleFactor;
            x.Y = value.Y * scaleFactor;
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
        public static Vector2 operator *(float scaleFactor, Vector2 value)
        {
            Vector2 x;
            x.X = value.X * scaleFactor;
            x.Y = value.Y * scaleFactor;
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
        public static Vector2 operator -(Vector2 value1, Vector2 value2)
        {
            Vector2 x;
            x.X = value1.X - value2.X;
            x.Y = value1.Y - value2.Y;
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
        public static Vector2 operator -(Vector2 value)
        {
            Vector2 x;
            x.X = -value.X;
            x.Y = -value.Y;
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
        public bool Equals(Vector2 other)
        {
            if (Math.Abs(this.X - other.X) > Mathf.Epsilon)
            {
                return false;
            }

            return Math.Abs(this.Y - other.Y) < Mathf.Epsilon;
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
            if (obj is Vector2)
            {
                flag = this.Equals((Vector2)obj);
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
            return this.X.GetHashCode() + this.Y.GetHashCode();
        }

        /// <summary>
        /// The length.
        /// </summary>
        /// <returns>
        /// The length.
        /// </returns>
        public float Length()
        {
            float x = this.X * this.X + this.Y * this.Y;
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
            return this.X * this.X + this.Y * this.Y;
        }

        /// <summary>
        /// The normalize.
        /// </summary>
        public void Normalize()
        {
            float x = this.X * this.X + this.Y * this.Y;
            float single = 1f / (float)Math.Sqrt(x);
            Vector2 vector2 = this;
            vector2.X = vector2.X * single;
            Vector2 y = this;
            y.Y = y.Y * single;
        }

        /// <summary>
        /// Returns a String that represents the current Vector2.
        /// </summary>
        /// <returns>
        /// The String that represents the current Vector2.
        /// </returns>
        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            var str = new object[2];
            str[0] = this.X.ToString(currentCulture);
            str[1] = this.Y.ToString(currentCulture);
            return string.Format(currentCulture, "{{X:{0} Y:{1}}}", str);
        }

        #endregion
    }
}