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
    /// Extension methods for the <see cref="Vector2"/> type.
    /// </summary>
    public partial struct Vector2 
    {
        /// <summary>
        /// Converts a <see cref="Point"/> type into a Vector2 without the need for explicit conversions.
        /// </summary>
        /// <param name="v">The <see cref="Point"/> to convert</param>
        /// <returns>Returns a new Vector2 type.</returns>
        public static implicit operator Vector2(Point v)
        {
            return new Vector2(v.X, v.Y);
        }

        /// <summary>
        /// Converts a Vector2 type into a <see cref="Point"/> without the need for explicit conversions.
        /// </summary>
        /// <param name="v">The Vector2 to convert</param>
        /// <returns>Returns a new Vector2 type.</returns>
        public static implicit operator Point(Vector2 v)
        {
            return new Point((int)v.X, (int)v.Y);
        }
    }
}