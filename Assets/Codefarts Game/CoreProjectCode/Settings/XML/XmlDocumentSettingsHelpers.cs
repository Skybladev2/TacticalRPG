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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Contains helper methods for the xml settings implementation.
    /// </summary>
    public class XmlDocumentSettingsHelpers
    {
        /// <summary>
        /// Reads a xml settings file and returns the results.
        /// </summary>
        /// <param name="file">The settings file to be read.</param>
        /// <param name="filterDuplicates">True to filter out any duplicate settings that may be read.</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> containing key value pairs of the settings.</returns>
        public static IEnumerable<KeyValuePair<string, object>> ReadSettings(string file, bool filterDuplicates)
        {
            // load the settings file using a XmlDocument object
            var xml = new XmlDocument();
            xml.Load(file);

            // ensure that the documentation element root not is correct
            if (xml.DocumentElement == null || xml.DocumentElement.Name != "settings")
            {
                throw new FileLoadException("Settings file root node is not \"settings\"!");
            }

            // read the settings into a key value pair of results
            var results = from x in xml.DocumentElement.ChildNodes.OfType<XmlNode>()
                          let attributes = x.Attributes
                          where attributes != null
                          where x.Name == "entry" && attributes != null && attributes.Count > 0 
                          let key = attributes["key"]
                          where key != null && !string.IsNullOrEmpty(key.Value)
                          select new KeyValuePair<string, object>(key.InnerText, x.InnerText);

            // if filtering duplicates remove any settigns with matching keys
            if (filterDuplicates)
            {
                var list = new List<KeyValuePair<string, object>>();
                foreach (var pair in results)
                {
                    if (list.All(x => x.Key != pair.Key))
                    {
                        list.Add(pair);
                    }
                }

                return list;
            }

            // return the results
            return results;
        }
    }
}
