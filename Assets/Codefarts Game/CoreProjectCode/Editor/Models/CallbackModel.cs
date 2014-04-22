/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.CoreProjectCode.Models
{
    using System;

    using Codefarts.CoreProjectCode.Interfaces;

    /// <summary>
    /// Provides a modal for callbacks.
    /// </summary>
    /// <typeparam name="T">The generic type used to represent the data type.</typeparam>
    public class CallbackModel<T> : IRun
    {
        /// <summary>
        /// Gets or sets a reference to a callback method.
        /// </summary>
        public Action<T> Callback { get; set; }

        /// <summary>
        /// Gets or sets a reference to some data that will be passed to the callback method.
        /// </summary>
        public T Data { get; set; } 

        /// <summary>
        /// Gets a value indicating the priority.
        /// </summary>
        public int Priority { get; internal set; }

        /// <summary>
        /// Implements <see cref="IRun.Run"/> to run the callback. 
        /// </summary>
        public void Run()
        {
            this.Callback(this.Data);
        }
    }
}