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

    /// <summary>Stores an ordered pair of integers, which specify a <see cref="Size.Height" /> and <see cref="Size.Width" />.</summary>
    public struct Size
    {
        #region Static Fields

        /// <summary>Gets a <see cref="Size" /> structure that has a <see cref="Size.Height" /> and <see cref="Size.Width" /> value of 0. </summary>
        /// <returns>A <see cref="Size" /> that has a <see cref="Size.Height" /> and <see cref="Size.Width" /> value of 0.</returns>
        public static readonly Size Empty;

        #endregion

        #region Fields

        /// <summary>
        /// The height.
        /// </summary>
        private int height;

        /// <summary>
        /// The width.
        /// </summary>
        private int width;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="Size"/> struct.
        /// </summary>
        static Size()
        {
            Empty = new Size();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> struct. 
        /// Initializes a new instance of the <see cref="Size"/> structure from the specified <see cref="Point"/> structure.
        /// </summary>
        /// <param name="pt">
        /// The <see cref="Point"/> structure from which to initialize this <see cref="Size"/> structure. 
        /// </param>
        public Size(Point pt)
        {
            this.width = pt.X;
            this.height = pt.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> struct. 
        /// Initializes a new instance of the <see cref="Size"/> structure from the specified dimensions.
        /// </summary>
        /// <param name="width">
        /// The width component of the new <see cref="Size"/>. 
        /// </param>
        /// <param name="height">
        /// The height component of the new <see cref="Size"/>. 
        /// </param>
        public Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the vertical component of this <see cref="Size" /> structure.</summary>
        /// <returns>The vertical component of this <see cref="Size" /> structure, typically measured in pixels.</returns>
        public int Height
        {
            get
            {
                return this.height;
            }

            set
            {
                this.height = value;
            }
        }

        /// <summary>Tests whether this <see cref="Size" /> structure has width and height of 0.</summary>
        /// <returns>This property returns true when this <see cref="Size" /> structure has both a width and height of 0; otherwise, false.</returns>
        public bool IsEmpty
        {
            get
            {
                if (this.width != 0)
                {
                    return false;
                }
                
                return this.height == 0;
            }
        }

        /// <summary>Gets or sets the horizontal component of this <see cref="Size" /> structure.</summary>
        /// <returns>The horizontal component of this <see cref="Size" /> structure, typically measured in pixels.</returns>
        public int Width
        {
            get
            {
                return this.width;
            }

            set
            {
                this.width = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the width and height of one <see cref="Size"/> structure to the width and height of another <see cref="Size"/> structure.
        /// </summary>
        /// <returns>
        /// A <see cref="Size"/> structure that is the result of the addition operation.
        /// </returns>
        /// <param name="a">
        /// The first <see cref="Size"/> structure to add.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Size"/> structure to add.
        /// </param>
        public static Size Add(Size a, Size b)
        {
            return new Size(a.Width + b.Width, a.Height + b.Height);
        }

        /// <summary>
        /// Converts the specified <see cref="SizeF"/> structure to a <see cref="Size"/> structure by rounding the values of the <see cref="Size"/> structure to the next higher integer values.
        /// </summary>
        /// <returns>
        /// The <see cref="Size"/> structure this method converts to.
        /// </returns>
        /// <param name="value">
        /// The <see cref="SizeF"/> structure to convert. 
        /// </param>
        public static Size Ceiling(SizeF value)
        {
            return new Size((int)Math.Ceiling(value.Width), (int)Math.Ceiling(value.Height));
        }

        /// <summary>
        /// Converts the specified <see cref="SizeF"/> structure to a <see cref="Size"/> structure by rounding the values of the <see cref="SizeF"/> structure to the nearest integer values.
        /// </summary>
        /// <returns>
        /// The <see cref="Size"/> structure this method converts to.
        /// </returns>
        /// <param name="value">
        /// The <see cref="SizeF"/> structure to convert. 
        /// </param>
        public static Size Round(SizeF value)
        {
            return new Size((int)Math.Round(value.Width), (int)Math.Round(value.Height));
        }

        /// <summary>
        /// Subtracts the width and height of one <see cref="Size"/> structure from the width and height of another <see cref="Size"/> structure.
        /// </summary>
        /// <returns>
        /// A <see cref="Size"/> structure that is a result of the subtraction operation.
        /// </returns>
        /// <param name="a">
        /// The <see cref="Size"/> structure on the left side of the subtraction operator. 
        /// </param>
        /// <param name="b">
        /// The <see cref="Size"/> structure on the right side of the subtraction operator. 
        /// </param>
        public static Size Subtract(Size a, Size b)
        {
            return new Size(a.Width - b.Width, a.Height - b.Height);
        }

        /// <summary>
        /// Converts the specified <see cref="SizeF"/> structure to a <see cref="Size"/> structure by truncating the values of the <see cref="SizeF"/> structure to the next lower integer values.
        /// </summary>
        /// <returns>
        /// The <see cref="Size"/> structure this method converts to.
        /// </returns>
        /// <param name="value">
        /// The <see cref="SizeF"/> structure to convert. 
        /// </param>
        public static Size Truncate(SizeF value)
        {
            return new Size((int)value.Width, (int)value.Height);
        }

        /// <summary>Adds the width and height of one <see cref="Size" /> structure to the width and height of another <see cref="Size" /> structure.</summary>
        /// <returns>A <see cref="Size" /> structure that is the result of the addition operation.</returns>
        /// <param name="a">The first <see cref="Size" /> to add. </param>
        /// <param name="b">The second <see cref="Size" /> to add. </param>
        public static Size operator +(Size a, Size b)
        {
            return Add(a, b);
        }

        /// <summary>Tests whether two <see cref="Size" /> structures are equal.</summary>
        /// <returns>true if <paramref name="a" /> and <paramref name="b" /> have equal width and height; otherwise, false.</returns>
        /// <param name="a">The <see cref="Size" /> structure on the left side of the equality operator. </param>
        /// <param name="b">The <see cref="Size" /> structure on the right of the equality operator. </param>
        public static bool operator ==(Size a, Size b)
        {
            if (a.Width != b.Width)
            {
                return false;
            }

            return a.Height == b.Height;
        }

        /// <summary>
        /// The op_ explicit.
        /// </summary>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <returns>
        /// </returns>
        public static explicit operator Point(Size size)
        {
            return new Point(size.Width, size.Height);
        }

        /// <summary>
        /// The op_ implicit.
        /// </summary>
        /// <param name="p">
        /// The p.
        /// </param>
        /// <returns>
        /// </returns>
        public static implicit operator SizeF(Size p)
        {
            return new SizeF(p.Width, p.Height);
        }

        /// <summary>Tests whether two <see cref="Size" /> structures are different.</summary>
        /// <returns>true if <paramref name="a" /> and <paramref name="b" /> differ either in width or height; false if <paramref name="a" /> and <paramref name="b" /> are equal.</returns>
        /// <param name="a">The <see cref="Size" /> structure on the left of the inequality operator. </param>
        /// <param name="b">The <see cref="Size" /> structure on the right of the inequality operator. </param>
        public static bool operator !=(Size a, Size b)
        {
            return !(a == b);
        }

        /// <summary>Subtracts the width and height of one <see cref="Size" /> structure from the width and height of another <see cref="Size" /> structure.</summary>
        /// <returns>A <see cref="Size" /> structure that is the result of the subtraction operation.</returns>
        /// <param name="a">The <see cref="Size" /> structure on the left side of the subtraction operator. </param>
        /// <param name="b">The <see cref="Size" /> structure on the right side of the subtraction operator. </param>
        public static Size operator -(Size a, Size b)
        {
            return Subtract(a, b);
        }

        /// <summary>
        /// Tests to see whether the specified object is a <see cref="Size"/> structure with the same dimensions as this <see cref="Size"/> structure.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> is a <see cref="Size"/> and has the same width and height as this <see cref="Size"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">
        /// The <see cref="T:System.Object"/> to test. 
        /// </param>
        public override bool Equals(object obj)
        {
            if (obj is Size)
            {
                var size = (Size)obj;
                if (size.width != this.width)
                {
                    return false;
                }

                return size.height == this.height;
            }

            return false;
        }

        /// <summary>Returns a hash code for this <see cref="Size" /> structure.</summary>
        /// <returns>An integer value that specifies a hash value for this <see cref="Size" /> structure.</returns>
        public override int GetHashCode()
        {
            return this.width ^ this.height;
        }

        /// <summary>Creates a human-readable string that represents this <see cref="Size" /> structure.</summary>
        /// <returns>A string that represents this <see cref="Size" />.</returns>
        public override string ToString()
        {
            var str = new string[5];
            str[0] = "{Width=";
            str[1] = this.width.ToString(CultureInfo.CurrentCulture);
            str[2] = ", Height=";
            str[3] = this.height.ToString(CultureInfo.CurrentCulture);
            str[4] = "}";
            return string.Concat(str);
        }

        #endregion
    }
}