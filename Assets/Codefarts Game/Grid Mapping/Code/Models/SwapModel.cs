// <copyright>
//   Copyright (c) 2012 Codefarts
//   All rights reserved.
//   contact@codefarts.com
//   http://www.codefarts.com
// </copyright>

namespace Codefarts.GridMapping.Models
{
    using UnityEngine;

    public class SwapModel
    {
        public GridMap Map { get; set; }
        public int LayerIndex { get; set; }
        public int NewLayerIndex { get; set; }
        public GameObject[] LayerAObjects { get; set; }
        public GameObject[] LayerBObjects { get; set; }
    }
}