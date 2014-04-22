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
    using System.Globalization;

    /// <summary>
    /// Used to represent a location in 2D space using whole numbers.
    /// </summary>
    public partial struct Point
    {
        #region Constants and Fields

        /// <summary>
        /// The x component.
        /// </summary>
        public int X;

        /// <summary>
        /// The y component.
        /// </summary>
        public int Y;

        /// <summary>
        /// Used by the <see cref="Empty"/> property.
        /// </summary>
        private static readonly Point empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="Point"/> struct.
        /// </summary>
        static Point()
        {
            empty = new Point(0, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> struct.
        /// </summary>
        /// <param name="x">
        /// The x component.
        /// </param>
        /// <param name="y">
        /// The y component.
        /// </param>
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets an empty point.
        /// </summary>
        public static Point Empty
        {
            get
            {
                return empty;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds to points together.
        /// </summary>
        /// <param name="a">
        /// The Point on the left side of the sign.
        /// </param>
        /// <param name="b">
        /// The Point on the right side of the sign.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="Point"/>.
        /// </returns>
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Divides two points.
        /// </summary>
        /// <param name="a">
        /// The Point on the left side of the sign.
        /// </param>
        /// <param name="b">
        /// The Point on the right side of the sign.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="Point"/>.
        /// </returns>
        public static Point operator /(Point a, Point b)
        {
            return new Point(a.X / b.X, a.Y / b.Y);
        }

        /// <summary>
        /// Compares two points for equality.
        /// </summary>
        /// <param name="a">
        /// The Point on the left side of the sign.
        /// </param>
        /// <param name="b">
        /// The Point on the right side of the sign.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="Point"/>.
        /// </returns>
        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        /// <summary>
        /// Compares two points for inequality.
        /// </summary>
        /// <param name="a">
        /// The Point on the left side of the sign.
        /// </param>
        /// <param name="b">
        /// The Point on the right side of the sign.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="Point"/>.
        /// </returns>
        public static bool operator !=(Point a, Point b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        /// <summary>
        /// Multiplies two points;
        /// </summary>
        /// <param name="a">
        /// The Point on the left side of the sign.
        /// </param>
        /// <param name="b">
        /// The Point on the right side of the sign.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="Point"/>.
        /// </returns>
        public static Point operator *(Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }

        /// <summary>
        /// Subtracts two points.
        /// </summary>
        /// <param name="a">
        /// The Point on the left side of the sign.
        /// </param>
        /// <param name="b">
        /// The Point on the right side of the sign.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="Point"/>.
        /// </returns>
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Checks for equality with another point.
        /// </summary>
        /// <param name="other">
        /// The other to compare against.
        /// </param>
        /// <returns>
        /// Return true of the point is equal to other.
        /// </returns>
        public bool Equals(Point other)
        {
            return other.X == this.X && other.Y == this.Y;
        }

        /// <summary>
        /// Checks for equality with another object.
        /// </summary>
        /// <param name="obj">
        /// The obj to compare against.
        /// </param>
        /// <returns>
        /// Return true of the point is equal to obj.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (obj.GetType() != typeof(Point))
            {
                return false;
            }

            return this.Equals((Point)obj);
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// Returns the hash code.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.X * 397) ^ this.Y;
            }
        }

        /// <summary>
        /// Returns a String that represents the current Point.
        /// </summary>
        /// <returns>
        /// The String that represents the current Point.
        /// </returns>
        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            var str = new string[2];
            str[0] = this.X.ToString(currentCulture);
            str[1] = this.Y.ToString(currentCulture);
            return string.Format(currentCulture, "{{X:{0} Y:{1}}}", str);
        }

        #endregion
    }
}