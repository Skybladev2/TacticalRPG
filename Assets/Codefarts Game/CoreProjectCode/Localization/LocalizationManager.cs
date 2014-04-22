/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

#if UNITY_EDITOR
    using UnityEngine;
#endif

    /// <summary>
    /// Provides a class for retrieving localized strings.
    /// </summary>
    public class LocalizationManager
    {
        /// <summary>
        /// Holds a reference to a <see cref="LocalizationManager"/> singleton.
        /// </summary>
        private static LocalizationManager singleton;

        /// <summary>
        /// The dictionary where of localized strings are stored.
        /// </summary>
        private readonly IDictionary<CultureInfo, IDictionary<string, string>> data;

        /// <summary>
        /// Prevents a default instance of the <see cref="LocalizationManager"/> class from being created.
        /// </summary>
        private LocalizationManager()
        {
            this.data = new Dictionary<CultureInfo, IDictionary<string, string>>();
            this.CurrentCulture = CultureInfo.CurrentCulture;
            this.DefaultCulture = new CultureInfo("en-us");
        }

        /// <summary>
        /// Gets a singleton instance of <see cref="LocalizationManager"/>.
        /// </summary>
        public static LocalizationManager Instance
        {
            get
            {
                return singleton ?? (singleton = new LocalizationManager());
            }
        }

        /// <summary>
        /// Gets or sets the current <see cref="CultureInfo"/> that will be used when retrieving a localized string.
        /// </summary>
        public CultureInfo CurrentCulture { get; set; }

        /// <summary>
        /// Gets or sets the default <see cref="CultureInfo"/> that will be used as a fall back culture if <see cref="CurrentCulture"/> is unavailable. <seealso cref="Get"/>
        /// </summary>
        public CultureInfo DefaultCulture { get; set; }

        /// <summary>
        /// Retrieves a localized string.
        /// </summary>
        /// <param name="key">The key name associated with the localized string.</param>
        /// <returns>Retrieves the localized string.</returns>
        /// <remarks>If the key is not available for the <see cref="CurrentCulture"/> this method will try to retrieve the string using the <see cref="DefaultCulture"/>.</remarks>
        public string Get(string key)
        {
            try
            {
                // check if key is present in the current culture
                if (this.CurrentCulture != null && this.data.ContainsKey(this.CurrentCulture) && this.data[this.CurrentCulture].ContainsKey(key))
                {
                    return this.data[this.CurrentCulture][key];
                }

                // try the default fall back culture
                if (this.DefaultCulture != null && this.data.ContainsKey(this.DefaultCulture) && this.data[this.DefaultCulture].ContainsKey(key))
                {
                    return this.data[this.DefaultCulture][key];
                }
            }
            catch (Exception)
            {
#if UNITY_EDITOR
                // something went wrong report the error
                Debug.LogWarning("Could not retrieve localization key: " + key);
#endif
            }

            // if unavailable just return null.
            return null;
        }

        /// <summary>
        /// Gets all entries for the current culture.
        /// </summary>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> containing the registered entries.</returns>
        public IEnumerable<KeyValuePair<string, string>> GetEntries()
        {
            return this.data[this.CurrentCulture];
        }

        /// <summary>
        /// Registers a dataset of localized strings from a <see cref="IDictionary{TKey,TValue}"/> reference.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> that the strings will be registered into.</param>
        /// <param name="entries">A <see cref="IDictionary{TKey,TValue}"/> containing the localized strings to add.</param>
        /// <remarks>This method will override any existing key values.</remarks>
        public void Register(CultureInfo culture, IDictionary<string, string> entries)
        {
            this.Register(culture, entries, true);
        }

        /// <summary>
        /// Registers a dataset of localized strings from a <see cref="IDictionary{TKey,TValue}"/> reference.
        /// </summary>
        /// <param name="culture">
        /// The <see cref="CultureInfo"/> that the strings will be registered into.
        /// </param>
        /// <param name="entries">
        /// A <see cref="IDictionary{TKey,TValue}"/> containing the localized strings to add.
        /// </param>
        /// <param name="replace">
        /// If true existing key values will be replaced. Otherwise the existing values will be left as they are.
        /// </param>
        public void Register(CultureInfo culture, IDictionary<string, string> entries, bool replace)
        {
            if (!this.data.ContainsKey(culture))
            {
                this.data.Add(culture, new Dictionary<string, string>());
            }

            var dictionary = this.data[culture];
            foreach (var entry in entries)
            {
                if (replace)
                {
                    if (!dictionary.ContainsKey(entry.Key))
                    {
                        dictionary.Add(entry.Key, entry.Value);
                    }
                    else
                    {
                        dictionary[entry.Key] = entry.Value;
                    }
                }
                else
                {
                    if (!dictionary.ContainsKey(entry.Key))
                    {
                        dictionary.Add(entry.Key, entry.Value);
                    }
                }
            }
        }
    }
}