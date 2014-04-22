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
    /// Size structure.
    /// </summary>
    public struct SizeF
    {
        #region Constants and Fields

        /// <summary>
        /// The height.
        /// </summary>
        public float Height;

        /// <summary>
        /// The width.
        /// </summary>
        public float Width;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeF"/> structure.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public SizeF(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a Empty size.
        /// </summary>
        public static SizeF Empty
        {
            get
            {
                return new SizeF();
            }
        }

        #endregion

        #region Public Methods and Operators

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is SizeF)
            {
                var tmp = (SizeF)obj;
                return Math.Abs(tmp.Height - this.Height) < Mathf.Epsilon && Math.Abs(tmp.Width - this.Width) < Mathf.Epsilon;
            }

            return base.Equals(obj);
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
        public static SizeF operator +(SizeF value1, SizeF value2)
        {
            SizeF x;
            x.Width = value1.Width + value2.Width;
            x.Height = value1.Height + value2.Height;
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
        public static SizeF operator /(SizeF value1, SizeF value2)
        {
            SizeF x;
            x.Width = value1.Width / value2.Width;
            x.Height = value1.Height / value2.Height;
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
        public static SizeF operator /(SizeF value1, float divider)
        {
            float single = 1f / divider;
            SizeF x;
            x.Width = value1.Width * single;
            x.Height = value1.Height * single;
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
        public static bool operator ==(SizeF value1, SizeF value2)
        {
            if (Math.Abs(value1.Width - value2.Width) > Mathf.Epsilon)
            {
                return false;
            }

            return Math.Abs(value1.Height - value2.Height) < Mathf.Epsilon;
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
        public static bool operator !=(SizeF value1, SizeF value2)
        {
            if (Math.Abs(value1.Width - value2.Width) > Mathf.Epsilon)
            {
                return true;
            }

            return Math.Abs(value1.Height - value2.Height) > Mathf.Epsilon;
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
        public static SizeF operator *(SizeF value1, SizeF value2)
        {
            SizeF x;
            x.Width = value1.Width * value2.Width;
            x.Height = value1.Height * value2.Height;
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
        public static SizeF operator *(SizeF value, float scaleFactor)
        {
            SizeF x;
            x.Width = value.Width * scaleFactor;
            x.Height = value.Height * scaleFactor;
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
        public static SizeF operator *(float scaleFactor, SizeF value)
        {
            SizeF x;
            x.Width = value.Width * scaleFactor;
            x.Height = value.Height * scaleFactor;
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
        public static SizeF operator -(SizeF value1, SizeF value2)
        {
            SizeF x;
            x.Width = value1.Width - value2.Width;
            x.Height = value1.Height - value2.Height;
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
        public static SizeF operator -(SizeF value)
        {
            SizeF x;
            x.Width = -value.Width;
            x.Height = -value.Height;
            return x;
        }

        /// <summary>
        /// Returns a String that represents the current SizeF.
        /// </summary>
        /// <returns>
        /// The String that represents the current SizeF.
        /// </returns>
        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            var str = new string[2];
            str[0] = this.Width.ToString(currentCulture);
            str[1] = this.Height.ToString(currentCulture);
            return string.Format(currentCulture, "{{Width:{0} Height:{1}}}", str);
        }

        #endregion
    }
}