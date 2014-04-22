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

    using Codefarts.Localization;

    /// <summary>
    /// Used as the central hub to manage settings for various Codefarts tools.
    /// </summary>
    public sealed class SettingsManager : IValues<string>
    {
        /// <summary>
        /// Holds a singleton reference to the settings manager.
        /// </summary>
        private static SettingsManager settingsManager;

        /// <summary>
        /// Holds on to a <see cref="ValueChangedEventArgs{T}"/> used for the <see cref="ValueChanged"/> event.
        /// </summary>
        private readonly ValueChangedEventArgs<string> eventArgs = new ValueChangedEventArgs<string>();

        /// <summary>
        /// Holds a reference to a <see cref="IValues{TKey}"/> provider used for storing and retrieving setting values.
        /// </summary>
        private IValues<string> values;

        /// <summary>
        /// Raised when a setting value is changed.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<string>> ValueChanged;

        /// <summary>
        /// Gets a singleton reference to a <see cref="SettingsManager"/>.
        /// </summary>
        public static SettingsManager Instance
        {
            get
            {
                return settingsManager ?? (settingsManager = new SettingsManager());
            }
        }

        /// <summary>
        /// Gets or sets a reference to a <see cref="IValues{TKey}"/> provider used for storing and retrieving setting values.
        /// </summary>
        public IValues<string> Values
        {
            get
            {
                return this.values;
            }

            set
            {
                if (this.values != null)
                {
                    this.values.ValueChanged -= this.ValuesValueChanged;
                }

                this.values = value;

                if (this.values != null)
                {
                    this.values.ValueChanged += this.ValuesValueChanged;
                }
            }
        }

        /// <summary>
        /// Gets a setting value.
        /// </summary>
        /// <typeparam name="T">Specifies the return type to be returned.</typeparam>
        /// <param name="key">The access key for the setting.</param>
        /// <returns>Returns the settings value.</returns>
        /// <exception cref="NullReferenceException">Thrown when the <see cref="Values"/> property has no <see cref="IValues{TKey}"/> implementation assigned to it.</exception>
        public T GetValue<T>(string key)
        {
            // if values provider is not set throw an exception.
            if (this.values == null)
            {
                var local = LocalizationManager.Instance;
                throw new NullReferenceException(local.Get("ERR_SettingsPropNotSet"));
            }

            return this.values.GetValue<T>(key);
        }

        /// <summary>
        /// Gets all the setting keys.
        /// </summary>
        /// <returns>
        /// Returns an array of all the setting keys.
        /// </returns>
        public string[] GetValueKeys()
        {
            return this.values.GetValueKeys();
        }

        /// <summary>
        /// Returns true if a setting is present.
        /// </summary>
        /// <param name="key">The name of the key to check for.</param>
        /// <returns>Returns true if a key exists.</returns>
        public bool HasValue(string key)
        {
            if (this.values == null)
            {
                return false;
            }

            return this.values.HasValue(key);
        }

        /// <summary>
        /// Removes a value from the <see cref="SettingsManager"/>.
        /// </summary>
        /// <param name="key">The settings key to be removed.</param>
        public void RemoveValue(string key)
        {
            this.values.RemoveValue(key);
        }

        /// <summary>
        /// Set a setting value.
        /// </summary>
        /// <param name="key">
        /// The setting key to be set.
        /// </param>
        /// <param name="value">
        /// The value to be assigned.
        /// </param>                  
        public void SetValue(string key, object value)
        {
            if (this.values == null)
            {
                var local = LocalizationManager.Instance;
                throw new NullReferenceException(local.Get("ERR_SettingsPropNotSet"));
            }

            this.values.SetValue(key, value);
            this.DoSettingChanged(key, value);
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event handler.
        /// </summary>
        /// <param name="name">
        /// The name of the setting.
        /// </param>
        /// <param name="value">
        /// The value that the setting was set to.
        /// </param>
        private void DoSettingChanged(string name, object value)
        {
            // if no event handler assigned just exit
            if (this.ValueChanged == null)
            {
                return;
            }
            
            this.eventArgs.Name = name;
            this.eventArgs.Value = value;
            this.ValueChanged(this, this.eventArgs);
            this.eventArgs.Value = null;
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event handler.
        /// </summary>
        /// <param name="sender">
        /// Typically a reference to the caller object.
        /// </param>
        /// <param name="e">
        /// The <see cref="ValueChangedEventArgs{T}"/> containing information about what setting changed.
        /// </param>
        private void ValuesValueChanged(object sender, ValueChangedEventArgs<string> e)
        {
            this.DoSettingChanged(e.Name, e.Value);
        }
    }
}