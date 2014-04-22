/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Editor
{
    /// <summary>
    /// Provides extension methods for array types.
    /// </summary>
    public class Array3DExtensionMethods
    {
        /// <summary>
        /// Treats a single dimensional array like a 3D array and stores a value.
        /// </summary>
        /// <typeparam name="T">Specifies the data type for the array.</typeparam>
        /// <param name="array">A reference to the array.</param>
        /// <param name="x">The X dimension within the <see cref="array"/> parameter.</param>
        /// <param name="y">The Y dimension within the <see cref="array"/> parameter.</param>
        /// <param name="z">The Z dimension within the <see cref="array"/> parameter.</param>
        /// <param name="width">Specifies the X dimension width.</param>
        /// <param name="height">Specifies the Y dimension height.</param>
        /// <param name="value">The value to be assigned to the array index.</param>
        public void Set<T>(T[] array, int x, int y, int z, int width, int height, T value)
        {
            array[(z * (width * height)) + ((y * width) + x)] = value;
        }

        /// <summary>
        /// Treats a single dimensional array like a 3D array and retrieves a value.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the data type for the array.
        /// </typeparam>
        /// <param name="array">
        /// A reference to the array.
        /// </param>
        /// <param name="x">
        /// The X dimension within the <see cref="array"/> parameter.
        /// </param>
        /// <param name="y">
        /// The Y dimension within the <see cref="array"/> parameter.
        /// </param>
        /// <param name="z">
        /// The Z dimension within the <see cref="array"/> parameter.
        /// </param>
        /// <param name="width">
        /// Specifies the X dimension width.
        /// </param>
        /// <param name="height">
        /// Specifies the Y dimension height.
        /// </param>
        /// <returns>
        /// Will return a <see cref="T"/> type from the array.
        /// </returns>
        public T Get<T>(T[] array, int x, int y, int z, int width, int height)
        {
            return array[(z * (width * height)) + ((y * width) + x)];
        }
    }
}
