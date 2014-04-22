/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.CoreProjectCode.Settings
{
    /// <summary>
    /// Provides event arguments for the <see cref="IValues{TKey}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type used for the <see cref="Name"/> property.</typeparam>
    public class ValueChangedEventArgs<T> : System.EventArgs
    {
        /// <summary>
        /// Gets the key of the setting that changed.
        /// </summary>
        public T Name { get; internal set; }

        /// <summary>
        /// Gets the value of the setting.
        /// </summary>
        public object Value { get; internal set; }
    }
}