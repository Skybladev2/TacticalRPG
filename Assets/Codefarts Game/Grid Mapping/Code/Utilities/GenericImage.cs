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
    /// Provides a class for a platform independent image.
    /// </summary>  
    /// <typeparam name="T">
    /// Used to define the pixel data type.
    /// </typeparam>
    public class GenericImage<T>
    {
        #region Constants and Fields

        /// <summary>
        /// Holds the image height.
        /// </summary>
        private int height;

        /// <summary>
        /// Holds the actual pixel data.
        /// </summary>
        private T[] pixelGrid;

        /// <summary>
        /// Holds the image width.
        /// </summary>
        private int width;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage{T}"/> class. 
        /// </summary>
        /// <param name="width">
        /// The width of the image.
        /// </param>
        /// <param name="height">
        /// The height of the image.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Width or Height can not be less then 1.
        /// </exception>
        public GenericImage(int width, int height)
        {
            if (width < 1)
            {
                throw new ArgumentOutOfRangeException("width");
            }

            if (height < 1)
            {
                throw new ArgumentOutOfRangeException("height");
            }

            this.width = width;
            this.height = height;
            this.pixelGrid = new T[width * height];
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Height can not be less than 1.</exception>
        public int Height
        {
            get
            {
                return this.height;
            }

            set
            {
                if (this.height == value)
                {
                    return;
                }

                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                this.height = value;
                var image = new GenericImage<T>(this.width, value);
                image.Draw(this, 0, 0, (sourceValue, blendWithValue) => sourceValue);
                this.pixelGrid = image.PixelGrid;
            }
        }

        /// <summary>
        /// Gets a array of <see cref="T"/> types.
        /// </summary>
        public T[] PixelGrid
        {
            get
            {
                return this.pixelGrid;
            }
        }

        /// <summary>
        /// Gets or sets the width of the image.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Width can not be less than 1.</exception>
        public int Width
        {
            get
            {
                return this.width;
            }

            set
            {
                if (this.width == value)
                {
                    return;
                }

                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                this.width = value;
                var image = new GenericImage<T>(value, this.height);
                image.Draw(this, 0, 0, (source, blendWithValue) => source);
                this.pixelGrid = image.PixelGrid;
            }
        }

        #endregion

        #region Public Indexers

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
        public T this[int x, int y]
        {
            get
            {
                return this.PixelGrid[(y * this.width) + x];
            }

            set
            {
                // perform range check before setting
                var index = (y * this.width) + x;
                if (index > -1 && index < this.pixelGrid.Length)
                {
                    this.PixelGrid[(y * this.width) + x] = value;
                }
            }
        }

        #endregion
    }
}