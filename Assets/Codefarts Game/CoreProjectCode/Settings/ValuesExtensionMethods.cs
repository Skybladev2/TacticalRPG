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
    using System;

    /// <summary>
    /// Provides extension methods for the <see cref="IValues{TKey}" /> interface.
    /// </summary>
    public static class ValuesExtensionMethods
    {
        /// <summary>
        /// Tries to get a setting and if it fails will return the default value.
        /// </summary>
        /// <param name="values">
        /// Reference to a <see cref="IValues{TKey}"/> type.
        /// </param>
        /// <param name="key">The key name for the setting.</param>
        /// <param name="defaultValue">
        /// The default value to return if there was a problem getting the setting.
        /// </param>
        /// <typeparam name="TKey">
        /// Defines the index key type that will be use to identify the value.
        /// </typeparam>
        /// <typeparam name="T">
        /// Defines the type that will be returned
        /// </typeparam>
        /// <returns>
        /// Returns the value of the setting or the default value if there was a problem retrieving the setting.
        /// </returns>
        public static T GetSetting<TKey, T>(this IValues<TKey> values, TKey key, T defaultValue)
        {
            T value;
            return TryGetSetting(values, key, out value) ? value : defaultValue;
        }

        /// <summary>
        /// Tries to get the setting value.
        /// </summary>
        /// <param name="values">
        /// Reference to the <see cref="IValues{TKey}"/> implementation.
        /// </param>
        /// <param name="key">
        /// The key of the setting.
        /// </param>
        /// <param name="value">
        /// The value of the setting if successful.
        /// </param>
        /// <typeparam name="TKey">
        /// Defines the index key type that will be use to identify the value.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of setting to retrieve.
        /// </typeparam>
        /// <returns>
        /// Returns true if the setting was retrieved successfully. Will return false if the setting was missing or threw an exception.
        /// </returns>
        public static bool TryGetSetting<TKey, T>(this IValues<TKey> values, TKey key, out T value)
        {
            if (!values.HasValue(key))
            {
                value = default(T);
                return false;
            }

            T retrievedValue;
            try
            {
                retrievedValue = values.GetValue<T>(key);
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }

            value = retrievedValue;
            return true;
        }
    }
}
