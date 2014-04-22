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

    using UnityEngine;                          

    /// <summary>
    /// provides extension methods for the <see cref="IValues{TKey}"/> type.
    /// </summary>
    public static class SettingsExtensionMethods
    {
        /// <summary>
        /// Gets a color setting from the settings instance.
        /// </summary>
        /// <param name="values">
        /// The settings reference.
        /// </param>
        /// <param name="name">
        /// The name of the setting to retrieve.
        /// </param>
        /// <param name="defaultValue">
        /// The default color value to use if a error occurs or no setting with the specified <see cref="name"/> was found.
        /// </param>
        /// <returns>
        /// Returns the color for the specified setting.
        /// </returns>
        public static Color32 GetColorSetting(this IValues<string> values, string name, Color32 defaultValue)
        {
            // set default value
            var color = defaultValue;

            // check if setting exists
            if (values.HasValue(name))
            {
                // attempt to get setting string
                var value = values.GetSetting(name, string.Format("{0} {1} {2} {3}", color.a, color.r, color.g, color.b));

                // parse the color string
                ParseColorString(name, value, ref color);
            }

            // return the color
            return color;
        }

        /// <summary>
        /// Parses a string containing color component values separated by a space character.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The string to be parsed.</param>
        /// <param name="color">Will contain the color if parsing was successful.</param>
        /// <returns>Returns true if successful.</returns>
        public static bool ParseColorString(string name, string value, ref Color32 color)
        {
            // split value
            var parts = value.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            // check if there were 4 parts
            if (parts.Length != 4)
            {
                // error and return default value
#if UNITY_EDITOR
                Debug.LogError(string.Format("Bad setting value retrieved for setting '{0}'.", name));
#endif
                return true;
            }

            // allocate byte array for color components
            var cols = new byte[parts.Length];
            for (int i = 0; i < cols.Length; i++)
            {
                if (!byte.TryParse(parts[i], out cols[i]))
                {
                    // error and return default value
#if UNITY_EDITOR
                    Debug.LogError(string.Format("Bad setting value retrieved for setting '{0}'.", name));
#endif
                    return true;
                }
            }

            // set color components
            color.a = cols[0];
            color.r = cols[1];
            color.g = cols[2];
            color.b = cols[3];
            return false;
        }

        /// <summary>
        /// Stores a color setting.
        /// </summary>
        /// <param name="values">
        /// The settings.
        /// </param>
        /// <param name="name">
        /// The name of the setting.
        /// </param>
        /// <param name="color">
        /// The color to be stored.
        /// </param>
        public static void SetColorSetting(this IValues<string> values, string name, Color32 color)
        {
            // convert to string
            var value = string.Format("{0} {1} {2} {3}", color.a, color.r, color.g, color.b);

            // store setting
            values.SetValue(name, value);
        }
    }
}
