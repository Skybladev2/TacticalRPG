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
    /// Provides an interface for storing and retrieving values.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type to use as the access key.
    /// </typeparam>
    public interface IValues<TKey>
    {
        /// <summary>
        /// Event for notifying when a value has changed.
        /// </summary>
        event System.EventHandler<ValueChangedEventArgs<TKey>> ValueChanged;

        /// <summary>
        /// Gets a value from the data store.
        /// </summary>
        /// <param name="key">
        /// The key of the value to get.
        /// </param>                     
        /// <typeparam name="T">
        /// The type of value to be returned.
        /// </typeparam>
        /// <returns>
        /// Returns a <see cref="T"/> type containing the value.
        /// </returns>    
        T GetValue<T>(TKey key);

        /// <summary>
        /// Returns true if a value with the specified key exists.
        /// </summary>
        /// <param name="name">The key of the value to check for.</param>
        /// <returns>Will return True if a value with the specified key exists.</returns>
        bool HasValue(TKey name);

        /// <summary>
        /// Removes a value from the data store
        /// </summary>
        /// <param name="key">The key of the value to remove.</param>
        void RemoveValue(TKey key);

        /// <summary>
        /// Sets the value in the data store.
        /// </summary>
        /// <param name="key">
        /// The key of the value to set.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>    
        void SetValue(TKey key, object value);

        /// <summary>
        /// Gets an array of value keys.
        /// </summary>
        /// <returns>Returns an array of value keys.</returns>
        TKey[] GetValueKeys();
    }
}