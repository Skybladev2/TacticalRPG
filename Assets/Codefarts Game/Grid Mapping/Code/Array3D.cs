// <copyright>
//   Copyright (c) 2012 Codefarts
//   All rights reserved.
//   contact@codefarts.com
//   http://www.codefarts.com
// </copyright>

namespace Codefarts.GridMapping
{
    using System;

    /// <summary>
    /// Provides a 3D array class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Array3D<T>
    {
        /// <summary>
        /// The array that holds the items.
        /// </summary>
        private T[] data;

        /// <summary>
        /// Initializes a new instance of the <see cref="Array3D{T}"/> class.
        /// </summary>
        /// <param name="width">
        /// The width of the array.
        /// </param>
        /// <param name="height">
        /// The height of the array.
        /// </param>
        /// <param name="depth">
        /// The depth of the array.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Is thrown if width, height or depth are less then 1.
        /// </exception>
        public Array3D(int width, int height, int depth)
        {
            if (width < 1)
            {
                throw new ArgumentOutOfRangeException("width");
            }

            if (height < 1)
            {
                throw new ArgumentOutOfRangeException("height");
            }

            if (depth < 1)
            {
                throw new ArgumentOutOfRangeException("depth");
            }

            this.Width = width;
            this.Height = height;
            this.Depth = depth;

            this.data = new T[this.Depth * (this.Width * this.Height)];
        }

        /// <summary>
        /// Gets or sets the width of the array.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets or sets the height of the array.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets or sets the depth of the array.
        /// </summary>
        public int Depth { get; private set; }


        public void CopyTo(Array3D<T> array, int z)
        {

        }

        public void Set(int x, int y, int z, T value)
        {
            this.data[(z * (this.Width * this.Height)) + ((y * this.Width) + x)] = value;
        }

        public T Get(int x, int y, int z)
        {
            return this.data[(z * (this.Width * this.Height)) + ((y * this.Width) + x)];
        }
    }
}
