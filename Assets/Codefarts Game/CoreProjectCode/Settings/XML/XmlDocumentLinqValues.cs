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
    using System.IO;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Provides a XML file based <see cref="IValues{TKey}"/> repository.
    /// </summary>
    public class XmlDocumentLinqValues : IValues<string>
    {
        #region Fields

        /// <summary>
        /// The event args.
        /// </summary>
        private readonly ValueChangedEventArgs<string> eventArgs = new ValueChangedEventArgs<string>();

        /// <summary>
        /// The data store.
        /// </summary>
        private Dictionary<string, object> dataStore = new Dictionary<string, object>();

#if !UNITY_WEBPLAYER

        /// <summary>
        /// The last read time.
        /// </summary>
        private DateTime lastReadTime = DateTime.MinValue;

        /// <summary>
        /// The last write time.
        /// </summary>
        private DateTime lastWriteTime = DateTime.MinValue;

#endif
        /// <summary>
        /// The read delay in seconds.
        /// </summary>
        private int readDelayInSeconds;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDocumentLinqValues"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name where the settings will be stored.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <see cref="fileName"/> parameter is empty of null.
        /// </exception>
        /// <exception cref="Exception">If invalid path or filename characters are detected.</exception>
        /// <remarks>Sets a default <see cref="ReadDelayInSeconds"/> of 5 seconds.</remarks>
        public XmlDocumentLinqValues(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            var directoryName = Path.GetDirectoryName(fileName);
            if (directoryName != null && directoryName.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                throw new Exception("Invalid path characters detected!");
            }

            var name = Path.GetFileName(fileName);
            if (name != null && name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                throw new Exception("Invalid filename characters detected!");
            }

            this.readDelayInSeconds = 5;
            this.FileName = fileName;

            this.Read();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="XmlDocumentLinqValues"/> class. 
        /// </summary>
        ~XmlDocumentLinqValues()
        {
            // remember to flush out any changes.
            this.Write();
        }

        #endregion

        /// <summary>
        /// The value changed.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<string>> ValueChanged;

        #region Public Properties
       
        /// <summary>
        /// Gets or sets the read delay in seconds.
        /// </summary>
        public int ReadDelayInSeconds
        {
            get
            {
                return this.readDelayInSeconds;
            }

            set
            {
                this.readDelayInSeconds = Math.Max(0, value);
            }
        }

        /// <summary>
        /// Gets the file name where the settings will be stored at.
        /// </summary>
        public string FileName { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets an array of setting names.
        /// </summary>
        /// <returns>Returns an array of setting names.</returns>
        public string[] GetValueKeys()
        {
            this.Read();
            return this.dataStore.Keys.ToArray();
        }

        /// <summary>
        /// Gets a setting value.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the return type to be returned.
        /// </typeparam>
        /// <param name="key">
        /// The access key for the setting.
        /// </param>
        /// <returns>
        /// Returns the settings value.
        /// </returns>
        /// <exception cref="InvalidCastException">
        /// May occur if the settings value cannot be converted to a <see cref="T"/> type.
        /// </exception>
        public T GetValue<T>(string key)
        {
            var type = typeof(T);
            this.Read();
            return (T)Convert.ChangeType(this.dataStore[key], type);
        }

        /// <summary>
        /// Will return true if a setting with the specified name exists.
        /// </summary>
        /// <param name="name">
        /// The name of the setting to check for.
        /// </param>
        /// <returns>
        /// Returns true if a setting with the specified name exists.
        /// </returns>
        /// <remarks>
        /// <see cref="name"/> is case sensitive.
        /// </remarks>
        public bool HasValue(string name)
        {
            // ensure data store has been read before checking if the key exists
            this.Read();
            return this.dataStore.ContainsKey(name);
        }

        /// <summary>
        /// Removes a value from the <see cref="IValues{TKey}"/>.
        /// </summary>
        /// <param name="key">
        /// The settings key to be removed.
        /// </param>
        public void RemoveValue(string key)
        {
            this.dataStore.Remove(key);
            this.Write();
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
            if (!this.dataStore.ContainsKey(key))
            {
                this.dataStore.Add(key, null);
            }

            this.dataStore[key] = value.ToString();

            this.Write();
            this.DoSettingChanged(key, value);
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event.
        /// </summary>
        /// <param name="name">
        /// The name of the settings key.
        /// </param>
        /// <param name="value">
        /// The value that was set.
        /// </param>
        protected void DoSettingChanged(string name, object value)
        {
            // if there is no event handler assigned just return
            if (this.ValueChanged == null)
            {
                return;
            }

            this.eventArgs.Name = name;
            this.eventArgs.Value = value;
            this.ValueChanged(this, this.eventArgs);
            this.eventArgs.Value = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads values into the <see cref="dataStore"/> field using Linq.
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// If the xml file where settings are saved no longer exists.
        /// </exception>
        /// <exception cref="FileLoadException">
        /// If the xml settings file could not be read properly.
        /// </exception>
        private void Read()
        {
#if !UNITY_WEBPLAYER
            // only update reading settings file every 5 seconds
            if (DateTime.Now < this.lastReadTime + TimeSpan.FromSeconds(this.readDelayInSeconds))
            {
                return;
            }
#endif

            // check if the file exists
            if (!File.Exists(this.FileName))
            {
                throw new FileNotFoundException("Could not find settings file.", this.FileName);
            }

            // if we are not compiling against the unity web player then check if last write time is greater then the files write time
#if !UNITY_WEBPLAYER
            var info = new FileInfo(this.FileName);
            var writeTime = info.LastWriteTime;

            // check if the file has been written to since last read attempt
            if (writeTime <= this.lastWriteTime)
            {
                return;
            }

#endif

            // try to read settings
            var results = XmlDocumentSettingsHelpers.ReadSettings(this.FileName, true);

            // store the settings
            this.dataStore = results.ToDictionary(k => k.Key, v => v.Value);

            // if we are not compiling against the unity web player then record the write time
#if !UNITY_WEBPLAYER
            this.lastWriteTime = writeTime;
            this.lastReadTime = DateTime.Now;
#endif
        }

        /// <summary>
        /// Saves current values in the <see cref="dataStore"/> to a xml file using Linq.
        /// </summary>
        private void Write()
        {
            // get path to settings file
            var directoryName = Path.GetDirectoryName(this.FileName);

            // check to ensure that the directory exists
            if (directoryName != null && !Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            // create a XmlDocument to store the settings in
            var doc = new XmlDocument();
            var declaration = doc.CreateXmlDeclaration("1.0", null, null);

            // create root settings element
            var settings = doc.CreateElement("settings");
            doc.AppendChild(settings);
            doc.InsertBefore(declaration, doc.DocumentElement);

            // read existing settings file values
            var existingValues = XmlDocumentSettingsHelpers.ReadSettings(this.FileName, true);

            // setup a comparer and merge the existing values from the xml file to the settings currently being managed 
            var comparer =
                EqualityComparerCallback<KeyValuePair<string, object>>.Compare(
                    (x, y) => string.CompareOrdinal(x.Key, y.Key) == 0);
            var entries = this.dataStore.Union(existingValues, comparer);

            // create series of entries for each setting in the XmlDocument object
            var nodesToWrite = entries.OrderBy(x => x.Key).Select(
                x =>
                {
                    var entry = doc.CreateElement("entry");
                    entry.InnerText = x.Value.ToString();
                    var key = doc.CreateAttribute("key");
                    key.InnerText = x.Key;
                    entry.Attributes.Append(key);
                    return entry;
                });

            // add nodes to settings root node
            foreach (var node in nodesToWrite)
            {
                settings.AppendChild(node);
            }

            // save the XmlDocument to an xml file
            doc.Save(this.FileName);

            // if we are not compiling against the unity web player then record the last write time
#if !UNITY_WEBPLAYER
            this.lastWriteTime = File.GetLastWriteTime(this.FileName);
#endif
        }

        #endregion
    }
}