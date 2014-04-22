/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.Map2D.Editor
{
    using System;
    using System.Collections.Generic;

    using Codefarts.GridMapping.Common;
    using Codefarts.GridMapping.Utilities;

    /// <summary>
    /// Provides extension methods for the <see cref="Map2DModel"/> type.
    /// </summary>
    public static class Map2DModelExtensionMethods
    {
        /// <summary>
        /// Adds a new layer to the map.
        /// </summary>
        /// <param name="map">
        /// The map model.
        /// </param>
        /// <param name="width">The layer width.</param>
        /// <param name="height">The layer height.</param>
        public static Map2DLayerModel AddNewLayer(this Map2DModel map, int width, int height)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }

            if (map.Layers == null)
            {
                map.Layers = new List<Map2DLayerModel>();
            }

            var model = new Map2DLayerModel();
            model.Image = new GenericImage<Color>(width, height);
            model.Image.Clear(Color.Green);
            model.Texture = model.Image.ToTexture2D();
            map.Layers.Add(model);
            if (map.ActiveLayer < 0)
            {
                map.ActiveLayer = map.Layers.Count - 1;
            }

            Map2DService.Instance.OnNewMapLayer(Map2DService.Instance, new MapEventArgs(map));
            return model;
        }
    }
}