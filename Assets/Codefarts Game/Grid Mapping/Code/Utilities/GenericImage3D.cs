/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.GridMapping.Utilities
{
    using System;

    /// <summary>
    /// A <see cref="GenericImage{T}"/> that supports layering.
    /// </summary>
    /// <typeparam name="T">
    /// Specifies the type used for the pixel data.
    /// </typeparam>
    public class GenericImage3D<T> : GenericImage<T>
    {
        /// <summary>
        /// Holds the value for the <see cref="LayerCount"/> property.
        /// </summary>
        private int layerCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage3D{T}"/> class. 
        /// </summary>
        /// <param name="width">
        /// The width of the image.
        /// </param>
        /// <param name="height">
        /// The height of the image.
        /// </param>
        /// <param name="layerCount">
        /// The number of layers the image will start with.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="width"/>, <see cref="height"/> or <see cref="layerCount"/> can not be less then 1.
        /// </exception>
        public GenericImage3D(int width, int height, int layerCount)
            : base(width, height)
        {
            this.LayerCount = layerCount;
        }

        /// <summary>
        /// Gets or sets the number of image layers.
        /// </summary>
        public int LayerCount
        {
            get
            {
                return this.layerCount;
            }

            set
            {
                if (this.layerCount == value)
                {
                    return;
                }

                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                this.layerCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the pixel at a specific pixel position.
        /// </summary>
        /// <param name="x">
        /// The x position of the pixel.
        /// </param>
        /// <param name="y">
        /// The y position of the pixel.
        /// </param>
        /// <returns>Returns the value at the specified pixel location.</returns>
        /// <exception cref="IndexOutOfRangeException">When retrieving a pixel value that is not within the bounds of the image.</exception>
        /// <remarks>When setting pixels invokes a internal range check to prevent <see cref="IndexOutOfRangeException"/> exceptions being thrown.</remarks>
        public new T this[int x, int y]
        {
            get
            {
                return this.PixelGrid[(y * this.Width) + x];
            }

            set
            {
                // perform range check before setting
                var index = (y * this.Width) + x;
                if (index > -1 && index < this.PixelGrid.Length)
                {
                    this.PixelGrid[index] = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the pixel at a specific pixel position.
        /// </summary>
        /// <param name="x">
        /// The x position of the pixel.
        /// </param>
        /// <param name="y">
        /// The y position of the pixel.
        /// </param>
        /// <param name="layer">The later index.</param>
        /// <returns>Returns the value at the specified pixel location.</returns>
        /// <exception cref="IndexOutOfRangeException">When retrieving a pixel value that is not within the bounds of the image.</exception>
        /// <remarks>When setting pixels invokes a internal range check to prevent <see cref="IndexOutOfRangeException"/> exceptions being thrown.</remarks>
        public T this[int x, int y, int layer]
        {
            get
            {
                return this.PixelGrid[((y * this.Width) + x) * layer];
            }

            set
            {
                // perform range check before setting
                var index = ((y * this.Width) + x) * layer;
                if (index > -1 && index < this.PixelGrid.Length)
                {
                    this.PixelGrid[index] = value;
                }
            }
        }
    }
}
