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
    using System;

    using UnityEngine;

    /// <summary>
    /// Provides a model for a <see cref="GridMap"/> layer.
    /// </summary>
    [Serializable]
    public class GridMapLayerModel : UnityEngine.Object
    {
        /// <summary>
        /// Gets or sets the name of the layer.
        /// </summary>
        [SerializeField]
        public string Name;

        /// <summary>
        /// Gets or sets whether the layer is visible.
        /// </summary>
        [SerializeField]
        public bool Visible;

        /// <summary>
        /// Gets or sets whether the layer is locked.
        /// </summary>
        [SerializeField]
        public bool Locked;
    }
}