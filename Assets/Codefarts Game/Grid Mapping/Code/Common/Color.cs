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

    /// <summary>
    /// The color.
    /// </summary>
    [Serializable]
    public partial struct Color
    {
        public bool Equals(Color other)
        {
            return this.A == other.A && this.B == other.B && this.G == other.G && this.R == other.R;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is Color && Equals((Color)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.A.GetHashCode();
                hashCode = (hashCode * 397) ^ this.B.GetHashCode();
                hashCode = (hashCode * 397) ^ this.G.GetHashCode();
                hashCode = (hashCode * 397) ^ this.R.GetHashCode();
                return hashCode;
            }
        }

        #region Constants and Fields

        /// <summary>
        /// Alpha component.
        /// </summary>
        public byte A;

        /// <summary>
        /// Blue component.
        /// </summary>
        public byte B;

        /// <summary>
        /// Green component.
        /// </summary>
        public byte G;

        /// <summary>
        /// Red component.
        /// </summary>
        public byte R;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct. 
        /// Initializes a new instance of the<see cref="Color"/>struct.
        /// </summary>
        /// <param name="r">
        /// Red component.
        /// </param>
        /// <param name="g">
        /// Green component.
        /// </param>
        /// <param name="b">
        /// Blue component.
        /// </param>
        /// <param name="a">
        /// Alpha component.
        /// </param>
        public Color(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a Black color.
        /// </summary>
        public static Color Black
        {
            get
            {
                return new Color(0, 0, 0, 255);
            }
        }

        /// <summary>
        /// Gets a Blue color.
        /// </summary>
        public static Color Blue
        {
            get
            {
                return new Color(0, 0, 255, 255);
            }
        }

        /// <summary>
        /// Gets a Green color.
        /// </summary>
        public static Color Green
        {
            get
            {
                return new Color(0, 255, 0, 255);
            }
        }

        /// <summary>
        /// Gets a Red color.
        /// </summary>
        public static Color Red
        {
            get
            {
                return new Color(255, 0, 0, 255);
            }
        }

        /// <summary>
        /// Gets a Transparent color.
        /// </summary>
        public static Color Transparent
        {
            get
            {
                return new Color(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Gets a White Color.
        /// </summary>
        public static Color White
        {
            get
            {
                return new Color(255, 255, 255, 255);
            }
        }

        /// <summary>
        /// Gets a Gray Color.
        /// </summary>
        public static Color Gray
        {
            get
            {
                return new Color(128, 128, 128, 255);
            }
        }

        /// <summary>
        /// Gets a yellow Color.
        /// </summary>
        public static Color Yellow
        {
            get
            {
                return new Color(255, 255, 0, 255);
            }
        }

        /// <summary>
        /// Returns a new color from the specified color components.
        /// </summary>
        /// <param name="a">
        /// The alpha value.
        /// </param>
        /// <param name="r">
        /// The red value.
        /// </param>
        /// <param name="g">
        /// The green value.
        /// </param>
        /// <param name="b">
        /// The blue value.
        /// </param>
        /// <returns>
        /// Return the new color.
        /// </returns>
        public static Color FromArgb(byte a, byte r, byte g, byte b)
        {
            return new Color(r, g, b, a);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The color components as a string.
        /// </summary>
        /// <returns>
        /// Returns a string containing the color components.
        /// </returns>
        public override string ToString()
        {
            return string.Format("A:{0} R:{1} G:{2} B:{3}", this.A, this.R, this.G, this.B);
        }

        /// <summary>
        /// Compares two colors for equality.
        /// </summary>
        /// <param name="left">
        /// The Point on the left side of the sign.
        /// </param>
        /// <param name="right">
        /// The Point on the right side of the sign.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="Point"/>.
        /// </returns>
        public static bool operator ==(Color left, Color right)
        {
            return (left.A == right.A && left.R == right.R && left.B == right.B && left.G == right.G);
        }

        /// <summary>
        /// Compares two colors for inequality.
        /// </summary>
        /// <param name="left">
        /// The Point on the left side of the sign.
        /// </param>
        /// <param name="right">
        /// The Point on the right side of the sign.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="Point"/>.
        /// </returns>
        public static bool operator !=(Color left, Color right)
        {
            return !(left == right);
        }

        #endregion
    }
}