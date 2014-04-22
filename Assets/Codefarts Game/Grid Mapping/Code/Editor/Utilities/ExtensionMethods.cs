/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Editor.Utilities
{
    using System;
    using UnityEditor;
    using System.Globalization;
    using System.Linq;

    using Codefarts.GridMapping.Models;

    using Helpers = Codefarts.GridMapping.Helpers;

    /// <summary>
    /// Provides extension methods for the <see cref="DrawRuleModel"/> type.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Gets a string containing the Xml markup for the values in the <see cref="model"/> parameter.
        /// </summary>
        /// <param name="model">The model to be converted to a Xml string.</param>
        /// <returns>Returns a string containing the Xml markup for the values in the <see cref="model"/> parameter.</returns>
        /// <exception cref="ArgumentNullException">If <see cref="model"/> is null.</exception>
        public static string ToXml(this DrawRuleModel model)
        {
            // ensure model has a reference
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            // get the source path for the model
            var sourcePrefab = Helpers.GetSourcePrefab(model.Prefab);

            // build a list of alternate prefab
            var alternates = from item in model.Alternates
                             let prefabPath = Helpers.GetSourcePrefab(item.Prefab)
                             where item != null && item.Prefab != null
                             select string.Format(new string(' ', 14) + "<alternate id=\"{0}\">\r\n" +
                                                                        "<prefabsource><![CDATA[{1}]]></prefabsource>\r\n" +
                                                                        "</alternate>", AssetDatabase.AssetPathToGUID(prefabPath), prefabPath);

            // return a Xml string containing the data from the model
            return string.Format(
                "    <rule>\r\n" +
                "        <name><![CDATA[{0}]]></name>\r\n" +
                "        <enabled>{1}</enabled>\r\n" +
                "        <prefab>{2}</prefab>\r\n" +
                "        <prefabsource><![CDATA[{11}]]></prefabsource>\r\n" +
                "        <upperneighbors enabled=\"{3}\" states=\"{4}\"></upperneighbors>\r\n" +
                "        <neighbors states=\"{5}\"></neighbors>\r\n" +
                "        <lowerneighbors enabled=\"{6}\" states=\"{7}\"></lowerneighbors>\r\n" +
                "        <alternates>\r\n{8}\r\n" +
                "        </alternates>\r\n" +
                "        <alloworiginal>{9}</alloworiginal>\r\n" +
                "        <description><![CDATA[{10}]]></description>\r\n" +
                "        <category><![CDATA[{12}]]></category>\r\n" +
                "    </rule>",
                string.IsNullOrEmpty(model.Name) ? string.Empty : model.Name.Trim(),
                model.Enabled,
                model.Prefab == null ? string.Empty : AssetDatabase.AssetPathToGUID(Helpers.GetSourcePrefab(model.Prefab)).ToString(CultureInfo.InvariantCulture),
                model.NeighborsUpperEnabled,
                string.Join(string.Empty, model.NeighborsUpper.Select(x => x ? "1" : "0").ToArray()),
                string.Join(string.Empty, model.Neighbors.Select(x => x ? "1" : "0").ToArray()),
                model.NeighborsLowerEnabled,
                string.Join(string.Empty, model.NeighborsLower.Select(x => x ? "1" : "0").ToArray()),
                model.Alternates == null ? string.Empty : string.Join("\r\n", alternates.ToArray()),
                model.AllowOriginal,
                string.IsNullOrEmpty(model.Description) ? string.Empty : model.Description.Trim(),
                sourcePrefab ?? string.Empty,
                string.IsNullOrEmpty(model.Category) ? string.Empty : model.Category.Trim());
        }
    }
}
