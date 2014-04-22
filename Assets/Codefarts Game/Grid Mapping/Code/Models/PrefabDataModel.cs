/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Models
{

    using UnityEngine;

    /// <summary>
    /// Provides a model for prefab drawing brushes
    /// </summary>
    public class PrefabDataModel
    {
        /// <summary>
        /// Gets or sets a reference to a prefab so it can be instantiated. 
        /// </summary>
        public GameObject Prefab { get; set; }

        /// <summary>
        /// Gets or sets the name of the prefab.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category that the prefab belongs to.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the pre-rendered texture for the prefab.
        /// </summary>
        public Texture2D Texture { get; set; }
    }
}