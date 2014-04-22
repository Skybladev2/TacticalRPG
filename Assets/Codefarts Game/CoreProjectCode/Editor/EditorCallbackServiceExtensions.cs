/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.CoreProjectCode.Services
{
    using System;

    /// <summary>
    /// Provides extension methods for the <see cref="EditorCallbackService"/> type.
    /// </summary>
    public static class EditorCallbackServiceExtensions
    {
        /// <summary>
        /// Registers a <see cref="Action"/> callback.
        /// </summary>
        /// <param name="service">A reference to a <see cref="EditorCallbackService"/> object.</param>
        /// <param name="callback">A reference to a callback.</param>
        /// <param name="priority">A value indicating the execution priority.</param>
        /// <exception cref="ArgumentNullException">If service parameter is null.</exception>
        public static void Register(this EditorCallbackService service, Action callback, int priority)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            service.Register<object>(x => callback(), null, priority);
        }

        /// <summary>
        /// Registers a <see cref="Action{T}"/> callback.
        /// </summary>
        /// <typeparam name="T">The type of data that the <see cref="callback"/> takes as a parameter.</typeparam>
        /// <param name="service">A reference to a <see cref="EditorCallbackService"/> object.</param>
        /// <param name="callback">A reference to a callback.</param>
        /// <param name="priority">A value indicating the execution priority.</param>
        /// <exception cref="ArgumentNullException">If service parameter is null.</exception>
        public static void Register<T>(this EditorCallbackService service, Action<T> callback, int priority)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            service.Register(callback, default(T), priority);
        }

        /// <summary>
        /// Registers a <see cref="Action"/> callback.
        /// </summary>
        /// <param name="service">A reference to a <see cref="EditorCallbackService"/> object.</param>
        /// <param name="callback">A reference to a callback.</param>
        /// <exception cref="ArgumentNullException">If service parameter is null.</exception>
        /// <remarks>Priority will be set to 0.</remarks>
        public static void Register(this EditorCallbackService service, Action callback)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            service.Register<object>(x => callback(), null, 0);
        }

        /// <summary>
        /// Registers a <see cref="Action{T}"/> callback.
        /// </summary>
        /// <typeparam name="T">The type of data that the <see cref="callback"/> takes as a parameter.</typeparam>
        /// <param name="service">A reference to a <see cref="EditorCallbackService"/> object.</param>
        /// <param name="callback">A reference to a callback.</param>
        /// <exception cref="ArgumentNullException">If service parameter is null.</exception>
        /// <remarks>Priority will be set to 0.</remarks>
        public static void Register<T>(this EditorCallbackService service, Action<T> callback)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            service.Register(callback, default(T), 0);
        }

        /// <summary>
        /// Registers a <see cref="Action{T}"/> callback.
        /// </summary>
        /// <typeparam name="T">
        /// The type of data that the <see cref="callback"/> takes as a parameter.
        /// </typeparam>
        /// <param name="service">A reference to a <see cref="EditorCallbackService"/> object.</param>
        /// <param name="callback">
        /// A reference to a callback.
        /// </param>
        /// <param name="data">
        /// The data that will be passed as a parameter when the <see cref="callback"/> is invoked.
        /// </param>
        /// <exception cref="ArgumentNullException">If service parameter is null.</exception>
        /// <remarks>Priority will be set to 0.</remarks>
        public static void Register<T>(this EditorCallbackService service, Action<T> callback, T data)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            service.Register(callback, default(T), 0);
        }
    }
}