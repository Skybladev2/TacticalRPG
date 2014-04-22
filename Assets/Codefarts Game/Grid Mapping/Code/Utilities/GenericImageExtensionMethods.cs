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
    using System.IO;

    using Codefarts.GridMapping.Common;

    /// <summary>
    /// Contains extension methods for the <see cref="GenericImage{T}"/> type.
    /// </summary>
    public static class GenericImageExtensionMethods
    {
        #region Public Methods and Operators

        /// <summary>
        /// Fills an area with a checkerboard pattern.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">The source image.</param>
        /// <param name="x">The x position within the <see cref="image"/>.</param>
        /// <param name="y">The y position within the <see cref="image"/>.</param>
        /// <param name="width">The width of the checkerboard.</param>
        /// <param name="height">The height of the checkerboard.</param>
        /// <param name="colorA">The first checkerboard fill value.</param>
        /// <param name="colorB">The second checkerboard fill value.</param>
        /// <param name="size">The size of the checkerboard pattern.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <see cref="size"/> is less then 1.</exception>
        public static void Checkerboard<T>(this GenericImage<T> image, int x, int y, int width, int height, T colorA, T colorB, int size)
        {
            if (size < 1)
            {
                throw new ArgumentOutOfRangeException("size");
            }

            var half = size / 2;
            for (var indexY = 0; indexY < height; indexY += size)
            {
                for (var indexX = 0; indexX < width; indexX += size)
                {
                    image.FillRectangle(x + indexX, y + indexY, half, half, colorB);
                    image.FillRectangle(x + indexX + half, y + indexY, half, half, colorA);
                    image.FillRectangle(x + indexX + half, y + indexY + half, half, half, colorB);
                    image.FillRectangle(x + indexX, y + indexY + half, half, half, colorA);
                }
            }
        }

        /// <summary>
        /// Clears a image.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The source image.
        /// </param>
        /// <param name="value">
        /// The value to use for the clear.
        /// </param>
        public static void Clear<T>(this GenericImage<T> image, T value)
        {
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    image[x, y] = value;
                }
            }
        }
       
        /// <summary>
        /// Clones an image.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The source image.
        /// </param>
        /// <returns>
        /// Returns a new image containing the same data.
        /// </returns>
        public static GenericImage<T> Clone<T>(this GenericImage<T> image)
        {
            var newImage = new GenericImage<T>(image.Width, image.Height);
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    newImage[x, y] = image[x, y];
                }
            }

            return newImage;
        }

        /// <summary>
        /// Draws a image within another image.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="source">
        /// The source image.
        /// </param>
        /// <param name="x">
        /// The x position in the destination image.
        /// </param>
        /// <param name="y">
        /// The y position in the destination image.
        /// </param>
        /// <param name="blendCallback">
        /// A <see cref="Func{TResult}"/> callback that is used to perform pixel blending.
        /// </param>
        /// <typeparamref name="T">
        /// The type representing the pixel data.</typeparamref>
        public static void Draw<T>(this GenericImage<T> image, GenericImage<T> source, int x, int y, Func<T, T, T> blendCallback)
        {
            Draw(image, source, x, y, source.Width, source.Height, 0, 0, source.Width, source.Height, blendCallback);
        }

        /// <summary>
        /// Draws a image within another image.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="source">
        /// The source image.
        /// </param>
        /// <param name="x">
        /// The x position in the destination image.
        /// </param>
        /// <param name="y">
        /// The y position in the destination image.
        /// </param>
        /// <param name="width">
        /// The destination width of the drawn image.
        /// </param>
        /// <param name="height">
        /// The destination height of the drawn image.
        /// </param>
        /// <param name="blendCallback">
        /// A <see cref="Func{TResult}"/> callback that is used to perform pixel blending.
        /// </param>
        /// <typeparamref name="T">
        /// The type representing the pixel data.</typeparamref>
        public static void Draw<T>(this GenericImage<T> image, GenericImage<T> source, int x, int y, int width, int height, Func<T, T, T> blendCallback)
        {
            Draw(image, source, x, y, width, height, 0, 0, source.Width, source.Height, blendCallback);
        }

        /// <summary>
        /// Draws a image within another image.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="source">
        /// The source image.
        /// </param>
        /// <param name="x">
        /// The x position in the destination image.
        /// </param>
        /// <param name="y">
        /// The y position in the destination image.
        /// </param>
        /// <param name="sourceX">
        /// The x position in the source image.
        /// </param>
        /// <param name="sourceY">
        /// The y position in the source image.
        /// </param>
        /// <param name="sourceWidth">
        /// The source width.
        /// </param>
        /// <param name="sourceHeight">
        /// The source height.
        /// </param>
        /// <param name="blendCallback">
        /// A <see cref="Func{TResult}"/> callback that is used to perform pixel blending.
        /// </param>
        /// <typeparamref name="T">
        /// The type representing the pixel data.</typeparamref>
        public static void Draw<T>(this GenericImage<T> image, GenericImage<T> source, int x, int y, int sourceX, int sourceY, int sourceWidth, int sourceHeight, Func<T, T, T> blendCallback)
        {
            Draw(image, source, x, y, sourceWidth, sourceHeight, sourceX, sourceY, sourceWidth, sourceHeight, blendCallback);
        }

        /// <summary>
        /// Draws a image within another image.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="source">
        /// The source image.
        /// </param>
        /// <param name="x">
        /// The x position in the destination image.
        /// </param>
        /// <param name="y">
        /// The y position in the destination image.
        /// </param>
        /// <param name="width">
        /// The destination width of the drawn image.
        /// </param>
        /// <param name="height">
        /// The destination height of the drawn image.
        /// </param>
        /// <param name="sourceX">
        /// The x position in the source image.
        /// </param>
        /// <param name="sourceY">
        /// The y position in the source image.
        /// </param>
        /// <param name="sourceWidth">
        /// The source width.
        /// </param>
        /// <param name="sourceHeight">
        /// The source height.
        /// </param>
        /// <param name="blendCallback">
        /// A <see cref="Func{TResult}"/> callback that is used to perform pixel blending.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <see cref="source"/> or <see cref="blendCallback"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If width, height, sourceWidth or sourceHeight are less then 1.
        /// </exception>
        /// <typeparamref name="T">
        /// The type representing the pixel data.</typeparamref>
        public static void Draw<T>(this GenericImage<T> image, GenericImage<T> source, int x, int y, int width, int height, int sourceX, int sourceY, int sourceWidth, int sourceHeight, Func<T, T, T> blendCallback)
        {
            // perform input validation
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (blendCallback == null)
            {
                throw new ArgumentNullException("blendCallback");
            }

            if (sourceWidth < 1)
            {
                throw new ArgumentOutOfRangeException("sourceWidth");
            }

            if (sourceHeight < 1)
            {
                throw new ArgumentOutOfRangeException("sourceHeight");
            }

            if (width < 1)
            {
                throw new ArgumentOutOfRangeException("width");
            }

            if (height < 1)
            {
                throw new ArgumentOutOfRangeException("height");
            }

            var scaledWidth = (float)width / sourceWidth;
            var scaledHeight = (float)height / sourceHeight;

            if (x > image.Width - 1 || y > image.Height - 1 || sourceX > source.Width - 1 || sourceY > source.Height - 1)
            {
                return;
            }

            // copy source data to temp image
            var temporaryImage = new GenericImage<T>(sourceWidth, sourceHeight);

            for (var indexY = 0; indexY < sourceHeight; indexY++)
            {
                for (var indexX = 0; indexX < sourceWidth; indexX++)
                {
                    var positionX = indexX + sourceX;
                    var positionY = indexY + sourceY;

                    // TODO: this needs to be better optimized. We are processing all pixels even though some of 
                    // the pixels could be cropped off. Need to calculate the rectangle the copy pixels and process only that rectangle
                    if (positionX > source.Width - 1 || positionY > source.Height - 1 || positionX < 0 || positionY < 0)
                    {
                        continue;
                    }

                    temporaryImage[indexX, indexY] = source[positionX, positionY];
                }
            }

            var scaled = temporaryImage.Scale(scaledWidth, scaledHeight);

            if (x < -scaled.Width - 1 || y < -scaled.Height - 1)
            {
                return;
            }

            for (var indexY = 0; indexY < scaled.Height; indexY++)
            {
                for (var indexX = 0; indexX < scaled.Width; indexX++)
                {
                    var destinationX = indexX + x;
                    var destinationY = indexY + y;

                    // TODO: this needs to be better optimized. We are processing all pixels even though some of 
                    // the pixels could be cropped off. Need to calculate the rectangle the copy pixels and process only that rectangle
                    if (destinationX > image.Width - 1 || destinationY > image.Height - 1 || destinationX < 0 || destinationY < 0)
                    {
                        continue;
                    }

                    var value = scaled[indexX, indexY];

                    image[destinationX, destinationY] = blendCallback(image[destinationX, destinationY], value);
                }
            }
        }

        /// <summary>
        /// Draws a circle.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="centerX">
        /// The x center position of the circle.
        /// </param>
        /// <param name="centerY">
        /// The y center position of the circle.
        /// </param>
        /// <param name="radius">
        /// The radius of the circle.
        /// </param>
        /// <param name="color">
        /// The value to use.
        /// </param>
        public static void DrawCircle<T>(this GenericImage<T> image, int centerX, int centerY, int radius, T color)
        {
            // source (converted from Java) -> http://rosettacode.org/wiki/Bitmap/Midpoint_circle_algorithm#Java
            var d = (5 - radius * 4) / 4;
            var x = 0;
            var y = radius;

            do
            {
                // ensure index is in range before setting (depends on your image implementation)
                // in this case we check if the pixel location is within the bounds of the image before setting the pixel
                image[centerX + x, centerY + y] = color;
                image[centerX + x, centerY - y] = color;
                image[centerX - x, centerY + y] = color;
                image[centerX - x, centerY - y] = color;
                image[centerX + y, centerY + x] = color;
                image[centerX + y, centerY - x] = color;
                image[centerX - y, centerY + x] = color;
                image[centerX - y, centerY - x] = color;
              
                if (d < 0)
                {
                    d += 2 * x + 1;
                }
                else
                {
                    d += 2 * (x - y) + 1;
                    y--;
                }

                x++;
            }
            while (x <= y);
        }

        /// <summary>
        /// Draws a Ellipse.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="centerX">
        /// The x center position of the circle.
        /// </param>
        /// <param name="centerY">
        /// The y center position of the circle.
        /// </param>
        /// <param name="width">
        /// The width of the Ellipse.
        /// </param>
        /// <param name="height">
        /// The height of the Ellipse.
        /// </param>
        /// <param name="color">
        /// The value to use.
        /// </param>
        public static void DrawEllipse<T>(this GenericImage<T> image, int centerX, int centerY, int width, int height, T color)
        {
            // source (converted from C++) -> http://www.dailyfreecode.com/Code/draw-ellipse-midpoint-ellipse-algorithm-714.aspx
            float aa = width * width;
            float bb = height * height;
            float aa2 = aa * 2;
            float bb2 = bb * 2;

            float x = 0;
            float y = height;

            float fx = 0;
            float fy = aa2 * height;

            float p = (int)(bb - (aa * height) + (0.25 * aa) + 0.5);

            image[(int)(centerX + x), (int)(centerY + y)] = color;
            image[(int)(centerX + x), (int)(centerY - y)] = color;
            image[(int)(centerX - x), (int)(centerY - y)] = color;
            image[(int)(centerX - x), (int)(centerY + y)] = color;

            while (fx < fy)
            {
                x++;
                fx += bb2;

                if (p < 0)
                {
                    p += fx + bb;
                }      
                else
                {
                    y--;
                    fy -= aa2;
                    p += fx + bb - fy;
                }

                image[(int)(centerX + x), (int)(centerY + y)] = color;
                image[(int)(centerX + x), (int)(centerY - y)] = color;
                image[(int)(centerX - x), (int)(centerY - y)] = color;
                image[(int)(centerX - x), (int)(centerY + y)] = color;
            }

            p = (int)((bb * (x + 0.5) * (x + 0.5)) + (aa * (y - 1) * (y - 1)) - (aa * bb) + 0.5);

            while (y > 0)
            {
                y--;
                fy -= aa2;

                if (p >= 0)
                {
                    p += aa - fy;
                }     
                else
                {
                    x++;
                    fx += bb2;
                    p += fx + aa - fy;
                }

                image[(int)(centerX + x), (int)(centerY + y)] = color;
                image[(int)(centerX + x), (int)(centerY - y)] = color;
                image[(int)(centerX - x), (int)(centerY - y)] = color;
                image[(int)(centerX - x), (int)(centerY + y)] = color;
            }
        }

        /// <summary>
        /// Draws a line on an <see cref="GenericImage{T}"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="x1">
        /// The x position for the start of the line.
        /// </param>
        /// <param name="y1">
        /// The y position for the start of the line.
        /// </param>
        /// <param name="x2">
        /// The x position for the end of the line.
        /// </param>
        /// <param name="y2">
        /// The y position for the end of the line.
        /// </param>
        /// <param name="value">
        /// The value that the line will be drawn with.
        /// </param>
        /// <typeparamref name="T">
        /// The type representing the pixel data.</typeparamref>
        public static void DrawLine<T>(this GenericImage<T> image, int x1, int y1, int x2, int y2, T value)
        {
            // source (converted from C) -> http://rosettacode.org/wiki/Bitmap/Bresenham%27s_line_algorithm#C
            int dx = Math.Abs(x2 - x1), sx = x1 < x2 ? 1 : -1;
            int dy = Math.Abs(y2 - y1), sy = y1 < y2 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2;

            while (true)
            {
                image[x1, y1] = value;
                if (x1 == x2 && y1 == y2)
                {
                    break;
                }

                int e2 = err;
                if (e2 > -dx)
                {
                    err -= dy;
                    x1 += sx;
                }

                if (e2 < dy)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="x">
        /// The x position of the left side of the rectangle.
        /// </param>
        /// <param name="y">
        /// The y position of the top side of the rectangle.
        /// </param>
        /// <param name="width">
        /// The width of the rectangle.
        /// </param>
        /// <param name="height">
        /// The height of the rectangle.
        /// </param>
        /// <param name="value">
        /// The value to use.
        /// </param>   
        public static void DrawRectangle(this GenericImage<Color> image, int x, int y, int width, int height, Color value)
        {
            DrawRectangle(image, x, y, width, height, value, (source, blendWith) => source.Blend(blendWith));
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="x">
        /// The x position of the left side of the rectangle.
        /// </param>
        /// <param name="y">
        /// The y position of the top side of the rectangle.
        /// </param>
        /// <param name="width">
        /// The width of the rectangle.
        /// </param>
        /// <param name="height">
        /// The height of the rectangle.
        /// </param>
        /// <param name="value">
        /// The value to use.
        /// </param>
        /// <param name="blendCallback">
        /// A <see cref="Func{TResult}"/> callback that is used to perform pixel blending.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <see cref="blendCallback"/> is null.
        /// </exception>
        public static void DrawRectangle<T>(this GenericImage<T> image, int x, int y, int width, int height, T value, Func<T, T, T> blendCallback)
        {
            if (blendCallback == null)
            {
                throw new ArgumentNullException("blendCallback");
            }

            var bottom = y + height - 1;
            var right = x + width - 1;

            // allows negative widths
            if (x > right)
            {
                var temp = right;
                right = x;
                x = temp;
            }

            // allows negative heights
            if (y > bottom)
            {
                var temp = bottom;
                bottom = y;
                y = temp;
            }

            // top 
            if (y > -1 && y < image.Height)
            {
                var leftValue = x < 0 ? 0 : x;
                var rightValue = right > image.Width - 1 ? image.Width - 1 : right;
                for (var i = leftValue; i <= rightValue; i++)
                {
                    image[i, y] = blendCallback(image[i, y], value);
                }
            }

            // bottom 
            if (bottom > -1 && bottom < image.Height)
            {
                var leftValue = x < 0 ? 0 : x;
                var rightValue = right > image.Width - 1 ? image.Width - 1 : right;
                for (var i = leftValue; i <= rightValue; i++)
                {
                    image[i, bottom] = blendCallback(image[i, bottom], value);
                }
            }

            // left 
            if (x > -1 && x < image.Width)
            {
                var topValue = y < 0 ? 0 : y;
                var bottomValue = bottom > image.Height - 1 ? image.Height - 1 : bottom;
                for (var i = topValue; i <= bottomValue; i++)
                {
                    image[x, i] = blendCallback(image[x, i], value);
                }
            }

            // right 
            if (right > -1 && right < image.Width)
            {
                var topValue = y < 0 ? 0 : y;
                var bottomValue = bottom > image.Height - 1 ? image.Height - 1 : bottom;
                for (var i = topValue; i <= bottomValue; i++)
                {
                    image[right, i] = blendCallback(image[right, i], value);
                }
            }
        }

        /// <summary>
        /// Fills a circle.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="centerX">
        /// The x center position of the circle.
        /// </param>
        /// <param name="centerY">
        /// The y center position of the circle.
        /// </param>
        /// <param name="radius">
        /// The radius of the circle.
        /// </param>
        /// <param name="color">
        /// The value to use.
        /// </param>
        /// <typeparamref name="T">
        /// The type representing the pixel data.</typeparamref>
        public static void FillCircle<T>(this GenericImage<T> image, int centerX, int centerY, int radius, T color)
        {
            // source -> http://www.dailyfreecode.com/code/fill-circle-scan-line-circle-fill-675.aspx
            var counter = centerY + radius;

            for (var count = centerY - radius; count <= counter; count++)
            {
                var x1 = (int)(centerX + Math.Sqrt((radius * radius) - ((count - centerY) * (count - centerY))) + 0.5);
                var x2 = (int)(centerX - Math.Sqrt((radius * radius) - ((count - centerY) * (count - centerY))) + 0.5);

                image.DrawLine(x1, count, x2, count, color);
            }
        }

        /// <summary>
        /// Draws a filled Ellipse.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="x">
        /// The x left most position of the ellipse.
        /// </param>
        /// <param name="y">
        /// The y top most position of the ellipse.
        /// </param>
        /// <param name="width">
        /// The width of the ellipse.
        /// </param>
        /// <param name="height">
        /// The height of the ellipse.
        /// </param>
        /// <param name="color">
        /// The value to use.
        /// </param>
        /// <typeparamref name="T">
        /// The type representing the pixel data.</typeparamref>
        public static void FillEllipse<T>(this GenericImage<T> image, int x, int y, int width, int height, T color)
        {
            width = width / 2;
            height = height / 2;
            var centerX = x + width;
            var centerY = y + height;

            // source -> http://stackoverflow.com/questions/10322341/simple-algorithm-for-drawing-filled-ellipse-in-c-c
            for (var indexY = -height; indexY <= height; indexY++)
            {
                for (var indexX = -width; indexX <= width; indexX++)
                {
                    var dx = indexX / (double)width;
                    var dy = indexY / (double)height;
                    if ((dx * dx) + (dy * dy) <= 1)
                    {
                        image[centerX + indexX, centerY + indexY] = color;
                    }
                }
            }
        }

        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="x">
        /// The x position of the left side of the rectangle.
        /// </param>
        /// <param name="y">
        /// The y position of the top side of the rectangle.
        /// </param>
        /// <param name="width">
        /// The width of the rectangle.
        /// </param>
        /// <param name="height">
        /// The height of the rectangle.
        /// </param>
        /// <param name="color">
        /// The value to use.
        /// </param>                                                 
        public static void FillRectangle(this GenericImage<Color> image, int x, int y, int width, int height, Color color)
        {
            for (var indexY = 0; indexY < width; indexY++)
            {
                for (var indexX = 0; indexX < height; indexX++)
                {
                    if (indexY + x > image.Width - 1 || indexX + y > image.Height - 1 || indexY + x < -width || indexX + y < -height)
                    {
                        continue;
                    }

                    image[indexY + x, indexX + y] = image[indexY + x, indexX + y].Blend(color);
                }
            }
        }

        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="x">
        /// The x position of the left side of the rectangle.
        /// </param>
        /// <param name="y">
        /// The y position of the top side of the rectangle.
        /// </param>
        /// <param name="width">
        /// The width of the rectangle.
        /// </param>
        /// <param name="height">
        /// The height of the rectangle.
        /// </param>
        /// <param name="value">
        /// The value to use.
        /// </param>
        /// <typeparamref name="T">
        /// The type representing the pixel data.</typeparamref>
        public static void FillRectangle<T>(this GenericImage<T> image, int x, int y, int width, int height, T value)
        {
            for (var indexY = 0; indexY < width; indexY++)
            {
                for (var indexX = 0; indexX < height; indexX++)
                {
                    if (indexY + x > image.Width - 1 || indexX + y > image.Height - 1 || indexY + x < -width || indexX + y < -height)
                    {
                        continue;
                    }

                    image[indexY + x, indexX + y] = value;
                }
            }
        }

        /// <summary>
        /// Flips a image horizontally.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// A reference to the image to be flipped.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// If the <see cref="image"/> parameter is null.
        /// </exception>
        public static void FlipHorizontally<T>(this GenericImage<T> image)
        {
            // TODO: Cloning is easy but memory intensive replace with proper code
            var pixels = image.Clone();
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    image[x, y] = pixels[pixels.Width - 1 - x, y];
                }
            }
        }

        /// <summary>
        /// Flips a image vertically.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// A reference to the image to be flipped.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// If the <see cref="image"/> parameter is null.
        /// </exception>
        public static void FlipVertically<T>(this GenericImage<T> image)
        {
            // TODO: Cloning is easy but memory intensive replace with proper code
            var pixels = image.Clone();
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    image[x, y] = pixels[x, pixels.Height - 1 - y];
                }
            }
        }

        /// <summary>
        /// Generates a normal map.
        /// </summary>
        /// <param name="image">
        /// The source image to generate the normal map from.
        /// </param>
        /// <returns>
        /// Returns a new <see cref="GenericImage{T}"/> containing the normal map.
        /// </returns>   
        public static GenericImage<Color> NormalMap(this GenericImage<Color> image)
        {
            var newImage = new GenericImage<Color>(image.Width, image.Height);
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; ++x)
                {
                    var tempPixel = image[x, y];
                    var tempVectorX = tempPixel.R / 255.0f;
                    var tempVectorY = tempPixel.R / 255.0f;
                    var tempVectorZ = 1.0f;
                    Normalize(tempVectorX, tempVectorY, tempVectorZ, out tempVectorX, out tempVectorY, out tempVectorZ);
                    tempVectorX = ((tempVectorX + 1.0f) / 2.0f) * 255.0f;
                    tempVectorY = ((tempVectorY + 1.0f) / 2.0f) * 255.0f;
                    tempVectorZ = ((tempVectorZ + 1.0f) / 2.0f) * 255.0f;
                    newImage[x, y] = new Color((byte)tempVectorX, (byte)tempVectorY, (byte)tempVectorZ, 255);
                }
            }

            return newImage;
        }

        /// <summary>
        /// Rotates the image.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The source image to rotate.
        /// </param>
        /// <param name="angle">
        /// The angle in degrees to rotate the image.
        /// </param>
        /// <returns>
        /// Returns a new rotated image.
        /// </returns>
        /// <typeparamref name="T">
        /// The type representing the pixel data.</typeparamref>
        public static GenericImage<T> Rotate<T>(this GenericImage<T> image, float angle)
        {
            return Rotate(image, image.Width / 2, image.Height / 2, angle);
        }

        /// <summary>
        /// Rotates the image.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The source image to rotate.
        /// </param>
        /// <param name="centerX">
        /// The x position of the rotation axis.
        /// </param>
        /// <param name="centerY">
        /// The y position of the rotation axis.
        /// </param>
        /// <param name="angle">
        /// The angle in degrees to rotate the image.
        /// </param>
        /// <returns>
        /// Returns a new rotated image.
        /// </returns>
        /// <typeparamref name="T">
        /// The type representing the pixel data.</typeparamref>
        public static GenericImage<T> Rotate<T>(this GenericImage<T> image, int centerX, int centerY, float angle)
        {
            // source (adapted for GenericImage) -> http://stackoverflow.com/a/11176961/341706
            var radians = (Math.PI / 180) * angle;
            var cos = Math.Cos(radians);
            var sin = Math.Sin(radians);
            var newImage = new GenericImage<T>(image.Width, image.Height);

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var m = x - centerX;
                    var n = y - centerY;
                    var j = ((int)((m * cos) + (n * sin))) + centerX;
                    var k = ((int)((n * cos) - (m * sin))) + centerY;
                    if (j >= 0 && j < image.Width && k >= 0 && k < image.Height)
                    {
                        newImage[x, y] = image[j, k];
                    }
                }
            }

            return newImage;
        }

        /// <summary>
        /// Saves the image data to a stream.
        /// </summary>
        /// <param name="image">
        /// The source image.
        /// </param>
        /// <param name="stream">
        /// The stream that will be written to.
        /// </param>
        /// <remarks>Only writes the raw pixel data and does not include any width, height, color depth info etc.</remarks>
        public static void Save(this GenericImage<Color> image, Stream stream)
        {
            var data = image.ToByteArray();
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Scales a image.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type use for pixel data.
        /// </typeparam>
        /// <param name="image">
        /// The source image.
        /// </param>
        /// <param name="x">
        /// The horizontal scale.
        /// </param>
        /// <param name="y">
        /// The vertical scale.
        /// </param>
        /// <returns>
        /// Returns a new scaled image.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// x and y values must be greater then 0.
        /// </exception>
        public static GenericImage<T> Scale<T>(this GenericImage<T> image, float x, float y)
        {
            if (x <= 0)
            {
                throw new ArgumentOutOfRangeException("x");
            }

            if (y <= 0)
            {
                throw new ArgumentOutOfRangeException("y");
            }

            if (Math.Abs(x - 1.0f) < 0 && Math.Abs(y - 1.0f) < 0)
            {
                return image.Clone();
            }

            var width = (int)(image.Width * x);
            var height = (int)(image.Height * y);
            if (width < 1)
            {
                width = 1;
            }

            if (height < 1)
            {
                height = 1;
            }

            var scaled = new GenericImage<T>(width, height);

            for (var indexY = 0; indexY < scaled.Height; indexY++)
            {
                for (var indexX = 0; indexX < scaled.Width; indexX++)
                {
                    var u = scaled.Width == 1 ? 1 : indexX / (float)(scaled.Width - 1);
                    var v = scaled.Height == 1 ? 1 : indexY / (float)(scaled.Height - 1);
                    scaled[indexX, indexY] = image[(int)Math.Round(u * (image.Width - 1)), (int)Math.Round(v * (image.Height - 1))];
                }
            }

            return scaled;
        }

        /// <summary>
        /// Sets the pixels of a <see cref="GenericImage{T}"/> type.
        /// </summary>
        /// <param name="image">
        /// The image whose pixel data will be set.
        /// </param>
        /// <param name="pixelData">
        /// The source pixel data.
        /// </param>
        /// <typeparam name="T">
        /// The type of pixel data.
        /// </typeparam>
        /// <exception cref="ArgumentException">
        /// If the pixel count for <see cref="pixelData"/> is not the same as the <see cref="image"/> pixel count.
        /// </exception>
        public static void SetPixels<T>(this GenericImage<T> image, T[] pixelData)
        {
            if (image.PixelGrid.Length != pixelData.Length)
            {
                throw new ArgumentException("pixelData");
            }

            for (var i = 0; i < image.PixelGrid.Length; i++)
            {
                image.PixelGrid[i] = pixelData[i];
            }
        }

        /// <summary>
        /// Color tints an image.
        /// </summary>
        /// <param name="image">
        /// The image to color tint.
        /// </param>
        /// <param name="color">
        /// The color to use as the tint.
        /// </param>
        /// <param name="type">
        /// The type to tint to perform.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Unsupported type specified.
        /// </exception>
        public static void Tint(this GenericImage<Color> image, Color color, TintType type)
        {
            for (var indexX = 0; indexX < image.Width; indexX++)
            {
                for (var indexY = 0; indexY < image.Height; indexY++)
                {
                    var sourceColor = image[indexX, indexY];

                    var r = sourceColor.R;
                    var g = sourceColor.G;
                    var b = sourceColor.B;
                    var a = sourceColor.A;

                    switch (type)
                    {
                        case TintType.Alpha:

                            ////////////////////// ALPHA ////////////////////////
                            sourceColor.R = (byte)((r * color.R) / 255);
                            sourceColor.G = (byte)((g * color.G) / 255);
                            sourceColor.B = (byte)((b * color.B) / 255);
                            sourceColor.A = (byte)((a * color.A) / 255);
                            break;

                        case TintType.Multiply:

                            /////////////////////// MULTIPLY //////////////////////////////
                            // Faster than a division like (s * d) / 255 are 2 shifts and 2 adds
                            var ta = (a * color.A) + 128;
                            var tr = (r * color.R) + 128;
                            var tg = (g * color.G) + 128;
                            var tb = (b * color.B) + 128;

                            var ba = ((ta >> 8) + ta) >> 8;
                            var br = ((tr >> 8) + tr) >> 8;
                            var bg = ((tg >> 8) + tg) >> 8;
                            var bb = ((tb >> 8) + tb) >> 8;

                            sourceColor.R = (byte)ba;
                            sourceColor.G = (byte)(ba <= br ? ba : br);
                            sourceColor.B = (byte)(ba <= bg ? ba : bg);
                            sourceColor.A = (byte)(ba <= bb ? ba : bb);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("type");
                    }
                }
            }
        }

        /// <summary>
        /// Converts image pixel data to a byte array.
        /// </summary>
        /// <param name="image">
        /// The source image.
        /// </param>
        /// <returns>
        /// Returns an array of bytes containing the pixel data.
        /// </returns>
        public static byte[] ToByteArray(this GenericImage<Color> image)
        {
            return image.ToByteArray(0, 0, image.Width, image.Height);
        }

        /// <summary>
        /// Converts image pixel data to a byte array.
        /// </summary>
        /// <param name="image">
        /// The source image.
        /// </param>
        /// <param name="x">
        /// The x position of the left side of the rectangle.
        /// </param>
        /// <param name="y">
        /// The y position of the top side of the rectangle.
        /// </param>
        /// <param name="width">
        /// The width of the rectangle.
        /// </param>
        /// <param name="height">
        /// The height of the rectangle.
        /// </param>
        /// <returns>
        /// Returns an array of bytes containing the pixel data.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If x, y is out of bounds of the images dimensions. The width or height is less then 1.
        /// </exception>       
        /// <exception cref="ArgumentException">
        /// Clipped width or height is less then 1.
        /// </exception>                        
        public static byte[] ToByteArray(this GenericImage<Color> image, int x, int y, int width, int height)
        {
            if (x > image.Width - 1)
            {
                throw new ArgumentOutOfRangeException("x");
            }

            if (y > image.Height - 1)
            {
                throw new ArgumentOutOfRangeException("y");
            }

            if (width < 1)
            {
                throw new ArgumentOutOfRangeException("width");
            }

            if (height < 1)
            {
                throw new ArgumentOutOfRangeException("height");
            }

            if (x + width - 1 > image.Width - 1)
            {
                width = image.Width - 1 - x;
            }

            if (y + height - 1 > image.Height - 1)
            {
                height = image.Height - 1 - y;
            }

            if (width < 1)
            {
                throw new ArgumentException();
            }

            if (height < 1)
            {
                throw new ArgumentException();
            }

            if (x < 0)
            {
                width = x + width;
                x = 0;
            }

            if (y < 0)
            {
                height = y + height;
                y = 0;
            }

            if (width < 1)
            {
                throw new ArgumentException();
            }

            if (height < 1)
            {
                throw new ArgumentException();
            }

            var data = new byte[width * height * 4];
            var position = 0;

            for (var indexY = 0; indexY < height; indexY++)
            {
                for (var indexX = 0; indexX < width; indexX++)
                {
                    var positionY = indexY + y;
                    var positionX = indexX + x;

                    var color = image[positionX, positionY];
                    data[position++] = color.R;
                    data[position++] = color.G;
                    data[position++] = color.B;
                    data[position++] = color.A;
                }
            }

            return data;
        }

        /// <summary>
        /// Normalizes a 3 point vector.
        /// </summary>
        /// <param name="valueX">The X value of the input vector.</param>
        /// <param name="valueY">The Y value of the input vector.</param>
        /// <param name="valueZ">The Z value of the input vector.</param>
        /// <param name="resultX">The X result of the normalized input vector.</param>
        /// <param name="resultY">The Y result of the normalized input vector.</param>
        /// <param name="resultZ">The Z result of the normalized input vector.</param>
        private static void Normalize(float valueX, float valueY, float valueZ, out float resultX, out float resultY, out float resultZ)
        {
            var x = (valueX * valueX) + (valueY * valueY) + (valueZ * valueZ);
            var single = 1f / (float)Math.Sqrt(x);

            resultX = valueX * single;
            resultY = valueY * single;
            resultZ = valueZ * single;
        }

        #endregion
    }
}