/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.CoreProjectCode.Settings.Xml
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements a generic equality comparer that uses a callback to perform the comparison.
    /// </summary>
    /// <typeparam name="T">The type we want to compare.</typeparam>
    public class EqualityComparerCallback<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Holds a reference to the callback that handles comparisons.
        /// </summary>
        private readonly Func<T, T, bool> function;

        /// <summary>
        /// Holds a reference to the callback used to calculate hash codes.
        /// </summary>
        private Func<T, int> hashFunction;
      
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerCallback{T}"/> class.
        /// </summary>
        /// <param name="func">
        /// The callback function that will do the comparing.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws a <see cref="ArgumentNullException"/> if the <see cref="func"/> parameter is null.
        /// </exception>
        public EqualityComparerCallback(Func<T, T, bool> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            this.function = func;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerCallback{T}"/> class.
        /// </summary>
        /// <param name="func">
        /// The callback function that will do the comparing.
        /// </param>
        /// <param name="hash">
        /// The callback function that will return the hash code.
        /// </param>  
        public EqualityComparerCallback(Func<T, T, bool> func, Func<T, int> hash)
            : this(func)
        {
            this.hashFunction = hash;
        }

        /// <summary>
        /// Gets or sets the callback function to use to compute the hash.
        /// </summary>
        public Func<T, int> HashFunction
        {
            get
            {
                return this.hashFunction;
            }

            set
            {
                this.hashFunction = value;
            }
        }
                  
        /// <summary>
        /// Provides a static method for returning a <see cref="EqualityComparerCallback{T}"/> type.
        /// </summary>
        /// <param name="func">
        /// The callback function that will do the comparing.
        /// </param>
        /// <returns>Returns a new instance of a <see cref="EqualityComparerCallback{T}"/> type.</returns>
        public static IEqualityComparer<T> Compare(Func<T, T, bool> func)
        {
            return new EqualityComparerCallback<T>(func);
        }

        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the objects are considered equal otherwise, false. If both objA and objB are null, the method returns true.</returns>
        public bool Equals(T x, T y)
        {
            return this.function(x, y);
        }    

        /// <summary>
        /// The get hash code for a object.
        /// </summary>
        /// <param name="obj">
        /// The object whose hash code will be computed.          
        /// </param>
        /// <returns>
        /// Returns a <see cref="int"/> value representing the hash code.
        /// </returns>
        /// <remarks>If no callback function was set for the <see cref="HashFunction"/> the object default <see cref="GetHashCode"/> method will be used.</remarks>
        public int GetHashCode(T obj)
        {
            if (this.hashFunction == null)
            {
                return obj.GetHashCode();
            }

            return this.hashFunction(obj);
        }
    }
}
