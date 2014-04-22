/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping
{
    using System;
    using System.Collections.Generic;

    using Codefarts.GridMapping.Common;
    using Codefarts.GridMapping.Models;

    using UnityEditor;

    using UnityEngine;

    using Object = UnityEngine.Object;
    using Vector3 = Codefarts.GridMapping.Common.Vector3;

    /// <summary>
    /// Provides extension methods for the <see cref="GridMap"/> type.
    /// </summary>
    public static class GridMapExtensionMethods
    {
        /// <summary>
        /// Adds a layer to the map.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <exception cref="ArgumentOutOfRangeException">Count can not be less than 1.</exception>
        public static void AddLayer(this GridMap map)
        {
            Array.Resize(ref map.Layers, map.Layers.Length + 1);
            map.Layers[map.Layers.Length - 1] = new GridMapLayerModel { Visible = true, Locked = false };
        }

#if UNITY_EDITOR
        /// <summary>
        /// Adds a layer to the map.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="allowUndo">If true will allow the user to undo the action.</param>       
        /// <exception cref="ArgumentOutOfRangeException">Count can not be less than 1.</exception>
        public static void AddLayer(this GridMap map, bool allowUndo)
        {
            var local = Localization.LocalizationManager.Instance;
            if (allowUndo)
            {
                Undo.RecordObject(map, local.Get("AddLayer"));
            }

            AddLayer(map);
        }
#endif

        /// <summary>
        /// Sets the number of map layers.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="count">The number of layers that the <see cref="GridMap"/> will have.</param>
        /// <exception cref="ArgumentOutOfRangeException">Count can not be less than 1.</exception>
        public static void SetLayerCount(this GridMap map, int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            while (map.Layers.Length != count)
            {
                if (count < map.Layers.Length)
                {
                    Array.Resize(ref map.Layers, map.Layers.Length - 1);
                    // map.Layers.RemoveAt(map.Layers.Length - 1);
                }
                else
                {
                    Array.Resize(ref map.Layers, map.Layers.Length + 1);
                    map.Layers[map.Layers.Length - 1] = new GridMapLayerModel { Visible = true, Locked = false };
                    //map.Layers.Add(new GridMapLayerModel { Visible = true, Locked = false });
                }
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Sets the number of map layers.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="count">The number of layers that the <see cref="GridMap"/> will have.</param>
        /// <param name="allowUndo">If true will allow the user to undo the action.</param>       
        /// <exception cref="ArgumentOutOfRangeException">Count can not be less than 1.</exception>
        public static void SetLayerCount(this GridMap map, int count, bool allowUndo)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            if (allowUndo)
            {
                var local = Localization.LocalizationManager.Instance;
                Undo.RecordObject(map, local.Get("SetLayerCount"));
            }

            SetLayerCount(map, count);
        }
#endif

        /// <summary>
        /// Gets the name of the specified layer.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="index">The index of the layer.</param>
        /// <returns>Returns the name of the layer.</returns>
        public static string GetLayerName(this GridMap map, int index)
        {
            return map.Layers[index].Name;
        }

        /// <summary>
        /// Gets all the layer names.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <returns>Returns the name of all the layers.</returns>
        public static string[] GetLayerNames(this GridMap map)
        {
            var names = new string[map.Layers.Length];
            for (var i = 0; i < map.Layers.Length; i++)
            {
                names[i] = map.Layers[i].Name;
            }

            return names;
        }

        /// <summary>
        /// Gets the name of the specified layer.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="index">
        /// The index of the layer.
        /// </param>
        /// <param name="value">
        /// The new name for the layer.
        /// </param>    
        public static void SetLayerName(this GridMap map, int index, string value)
        {
            map.Layers[index].Name = value;
        }

        /// <summary>
        /// Checks for an existing prefab at a specified location
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="layer">The map layer index.</param>
        /// <param name="column">
        /// The column of the prefab.
        /// </param>
        /// <param name="row">
        /// The row of the prefab.
        /// </param>
        /// <returns>
        /// Will return a reference to a existing prefab if one is found.
        /// </returns>
        /// <remarks>This method can be slow and is not recommended for use within your game logic.</remarks>
        public static GameObject SearchForExistingPrefab(this GridMap map, int layer, int column, int row)
        {
            var transform = map.transform;
            GameObject prefab = null;
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.name.EndsWith(string.Format("_l{0}_c{1}_r{2}", layer, column, row)))
                {
                    prefab = child.gameObject;
                    break;
                }
            }

            return prefab;
        }

        /// <summary>
        /// Moves the the specified layer up the layer hierarchy.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="index">
        /// The index of the layer that will be deleted.
        /// </param>       
        /// <param name="count">The number of positions to move the layer.</param>       
        public static void MoveLayerUp(this GridMap map, int index, int count)
        {
            MoveLayerUp(map, index, count, null, null);
        }

        /// <summary>
        /// Moves the the specified layer up the layer hierarchy.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="index">
        /// The index of the layer that will be deleted.
        /// </param>       
        /// <param name="count">The number of positions to move the layer.</param>       
        /// <param name="callback">A callback that will be called with the map, and game objects that will be affected by the move.</param>
        /// <param name="completedCallback">A callback that will be called with the map, and game objects that were affected by the move.</param>
        public static void MoveLayerUp(this GridMap map, int index, int count, Action<SwapModel> callback, Action<SwapModel> completedCallback)
        {
            var newIndex = index - count;
            newIndex = newIndex < 0 ? 0 : newIndex;
            if (newIndex == index)
            {
                return;
            }

            SwapLayers(map, index, newIndex, callback, completedCallback);
        }

        /// <summary>
        /// Moves the the specified layer down the layer hierarchy.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="index">
        /// The index of the layer that will be deleted.
        /// </param>
        /// <param name="count">The number of positions to move the layer.</param>       
        public static void MoveLayerDown(this GridMap map, int index, int count)
        {
            MoveLayerDown(map, index, count, null, null);
        }

        /// <summary>
        /// Moves the the specified layer down the layer hierarchy.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="index">
        /// The index of the layer that will be deleted.
        /// </param>
        /// <param name="count">The number of positions to move the layer.</param>       
        /// <param name="callback">A callback that will be called with the map, and game objects that will be affected by the move.</param>
        /// <param name="completedCallback">A callback that will be called with the map, and game objects that were affected by the move.</param>
        public static void MoveLayerDown(this GridMap map, int index, int count, Action<SwapModel> callback, Action<SwapModel> completedCallback)
        {
            var newIndex = index + count;
            newIndex = newIndex > map.Layers.Length - 1 ? map.Layers.Length - 1 : newIndex;
            if (newIndex == index)
            {
                return;
            }

            SwapLayers(map, index, newIndex, callback, completedCallback);
        }

        /// <summary>
        /// Moves the the specified layer down the layer hierarchy.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="index">
        /// The index of the layer that will be deleted.
        /// </param>
        /// <param name="visible">The prefabs for the layer will have there active states set.</param>       
        public static void SetLayerVisibility(this GridMap map, int index, bool visible)
        {
            SetLayerVisibility(map, index, visible, null);
        }

        /// <summary>
        /// Moves the the specified layer down the layer hierarchy.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="index">
        /// The index of the layer that will be deleted.
        /// </param>
        /// <param name="visible">The prefabs for the layer will have there active states set.</param>
        /// <param name="callback">A callback with one parameter containing the items whose visibility will be set.</param>
        public static void SetLayerVisibility(this GridMap map, int index, bool visible, Action<GameObject[]> callback)
        {
            var items = map.GetLayerPrefabs(index);

            if (callback != null)
            {
                callback(items.ToArray());
            }

            foreach (var item in items)
            {
                item.SetActive(visible);
            }

            map.Layers[index].Visible = visible;
        }

        /// <summary>
        /// Deletes the the specified layer from a <see cref="GridMap"/> type.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="index">
        /// The index of the layer that will be deleted.
        /// </param>
        /// <remarks>This method does not record an undo operation.</remarks>
        public static void DeleteLayer(this GridMap map, int index)
        {
            // find list of objects to delete
            var items = map.GetLayerPrefabs(index);
            while (items.Count > 0)
            {
#if UNITY_EDITOR
                Object.DestroyImmediate(items[0]);
#else
                Object.Destroy(items[0]);
#endif
                items.RemoveAt(0);
            }

            var list = new List<GridMapLayerModel>(map.Layers);
            list.RemoveAt(index);
            map.Layers = list.ToArray();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Deletes the the specified layer from a <see cref="GridMap"/> type.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="index">
        /// The index of the layer that will be deleted.
        /// </param>
        /// <param name="allowUndo">If true will allow the user to undo the action.</param>       
        public static void DeleteLayer(this GridMap map, int index, bool allowUndo)
        {
            if (allowUndo)
            {
                Undo.RecordObject(map, Localization.LocalizationManager.Instance.Get("DeleteLayer"));
            }

            DeleteLayer(map, index);
        }
#endif

        /// <summary>
        /// Retrieves a list of prefabs that belong to a layer.
        /// </summary>
        /// <param name="map">
        /// The reference to a <see cref="GridMap"/> type.
        /// </param>
        /// <param name="layerIndex">
        /// The index of the layer that will have it's prefabs returned.
        /// </param>
        /// <returns>
        /// Returns a <see cref="List{T}"/> containing the game objects that exist on the specified layer.
        /// </returns>
        public static List<GameObject> GetLayerPrefabs(this GridMap map, int layerIndex)
        {
            // find list of objects to delete
            var items = new List<GameObject>();
            for (var col = 0; col < map.Columns; col++)
            {
                for (var row = 0; row < map.Rows; row++)
                {
                    var obj = map.SearchForExistingPrefab(layerIndex, col, row);
                    if (obj != null)
                    {
                        items.Add(obj);
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Finds and renames prefabs so that they are on different layers
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="index">The index of the layer.</param>
        /// <param name="newIndex">The index of the other layer.</param>
        public static void SwapLayers(this GridMap map, int index, int newIndex)
        {
            SwapLayers(map, index, newIndex, null, null);
        }

        /// <summary>
        /// Finds and renames prefabs so that they are on different layers
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="index">The index of the layer.</param>
        /// <param name="newIndex">The index of the other layer.</param>
        /// <param name="callback">A callback that will be called with the map, and game objects that will be affected by the swap.</param>
        /// <param name="completedCallback">A callback that will be called with the map, and game objects that were affected by the swap.</param>
        /// <remarks>The <see cref="callback"/> and <see cref="completedCallback"/> parameters will be called with the fallowing arguments (Map, LayerIndex, NewLayerIndex, LayerA, LayerB).</remarks>
        public static void SwapLayers(this GridMap map, int index, int newIndex, Action<SwapModel> callback, Action<SwapModel> completedCallback)
        {
            // find list of objects to delete
            var layerA = new List<GameObject>();
            var layerB = new List<GameObject>();
            for (var col = 0; col < map.Columns; col++)
            {
                for (var row = 0; row < map.Rows; row++)
                {
                    var obj = map.SearchForExistingPrefab(index, col, row);
                    if (obj != null)
                    {
                        layerA.Add(obj);
                    }

                    obj = map.SearchForExistingPrefab(newIndex, col, row);
                    if (obj != null)
                    {
                        layerB.Add(obj);
                    }
                }
            }

            var layerAObjects = layerA.ToArray();
            var layerBObjects = layerB.ToArray();
            var swapModel = new SwapModel() { Map = map, LayerIndex = index, NewLayerIndex = newIndex, LayerAObjects = layerAObjects, LayerBObjects = layerBObjects };
            if (callback != null)
            {
                callback(swapModel);
            }

            // setup variables                                      
            const string NameFormat = "{0}_l{1}_c{2}_r{3}";

            // setup processor callback
            var changeNames = new Action<List<GameObject>, int>(
                (stack, layerIndex) =>
                {
                    foreach (var item in stack)
                    {
                        string name;
                        int layer;
                        int column;
                        int row;
                        if (!item.name.TryParsePrefabName(out name, out layer, out column, out row))
                        {
                            continue;
                        }

                        // give the prefab a name that represents it's location within the grid map
                        item.name = string.Format(NameFormat, name, layerIndex, column, row);

                        // move prefab into proper layer position
                        var transform = item.transform;
                        var position = transform.localPosition;
                        position.y = layerIndex * map.Depth;
                        transform.position = position;
                    }
                });

            // change names
            changeNames(layerA, newIndex);
            changeNames(layerB, index);

            // change model indexes
            var model = map.Layers[index];
            map.Layers[index] = map.Layers[newIndex];
            map.Layers[newIndex] = model;

            if (completedCallback != null)
            {
                completedCallback(swapModel);
            }
        }

        /// <summary>
        /// positions a prefab on the map.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="row">The row where the prefab will be placed.</param>
        /// <param name="prefab">The prefab to be placed. </param>
        /// <param name="layer">The layer where placement will occur.</param>
        /// <param name="column">The column where the prefab will be placed.</param>
        /// <param name="autoCenterPrefab">Determines whether or not to automatically center the prefab within the grid cell.</param>
        /// <param name="autoScalePrefab">If true the prefab will be automatically scaled to fit within the confines of the maps cell size.</param>
        public static void PositionPrefabOnMap(this GridMap map, int layer, int column, int row, GameObject prefab, bool autoCenterPrefab, bool autoScalePrefab)
        {
            if (prefab == null)
            {
                throw new ArgumentNullException("prefab");
            }

#if PERFORMANCE
            var perf = PerformanceTesting<PerformanceID>.Instance;
            perf.Start(PerformanceID.PositionPrefabOnMapExtensionMethod);
#endif

            var transform = prefab.transform;
            var bounds = new Bounds();
            if (autoScalePrefab)
            {
                // map.ScalePrefab(prefab);

#if PERFORMANCE
                perf.Start(PerformanceID.PositionPrefabOnMap_ScalePrefab);
#endif
                // get bounds of the prefab
                var encapsulate = false;
                if (Utilities.Helpers.GetBoundWithChildren(transform, ref bounds, ref encapsulate))
                {
                    // apply scaling to the prefab
                    var scale = transform.localScale;

                    // set x/z scale
                    scale.x *= map.CellWidth / bounds.size.x;
                    scale.z *= map.CellHeight / bounds.size.z;
                    //  scale.y *= (map.Depth / bounds.size.y) * (scale.x / scale.z);

                    // check for infinity values and if found set to 0
                    scale.x = float.IsInfinity(scale.x) ? 0 : scale.x;
                    scale.y = float.IsInfinity(scale.y) ? 0 : scale.y;
                    scale.z = float.IsInfinity(scale.z) ? 0 : scale.z;

                    scale.x = float.IsNaN(scale.x) ? 1 : scale.x;
                    scale.y = float.IsNaN(scale.y) ? 1 : scale.y;
                    scale.z = float.IsNaN(scale.z) ? 1 : scale.z;

                    // set scale
                    transform.localScale = scale;

#if PERFORMANCE
                    perf.Stop(PerformanceID.PositionPrefabOnMap_ScalePrefab);
#endif
                }
            }

#if PERFORMANCE
            perf.Start(PerformanceID.PositionPrefabOnMap_SetParent);
#endif
            transform.parent = map.transform;
#if PERFORMANCE
            perf.Stop(PerformanceID.PositionPrefabOnMap_SetParent);
#endif

#if PERFORMANCE
            perf.Start(PerformanceID.PositionPrefabOnMap_Rotate);
#endif
            // get maps rotation
            UnityEngine.Vector3 axis;
            float angle;
            map.transform.rotation.ToAngleAxis(out angle, out axis);
            // apply maps rotation to the prefab so the prefab is rotated along with the map
            transform.Rotate(axis, angle, Space.World);
#if PERFORMANCE
            perf.Stop(PerformanceID.PositionPrefabOnMap_Rotate);
#endif

            var gridPositionInLocalSpace = Vector3.Zero;
            if (autoCenterPrefab)
            {
#if PERFORMANCE
                perf.Start(PerformanceID.PositionPrefabOnMap_AutoCenter);
#endif
                // ensure prefab is perfectly centered in the middle of the grid cell based on it's render bounds
                var encapsulate = false;

                // zero out the prefab's position
                transform.position = Vector3.Zero;

                // get rendering bounds for the prefab if not already done so
                Utilities.Helpers.GetBoundWithChildren(transform, ref bounds, ref encapsulate);

                gridPositionInLocalSpace = new Vector3(
                     (column * map.CellWidth) + (map.CellWidth / 2) - bounds.center.x,
                     (layer * map.Depth) - bounds.center.y,
                     (row * map.CellHeight) + (map.CellHeight / 2) - bounds.center.z);

#if PERFORMANCE
                perf.Stop(PerformanceID.PositionPrefabOnMap_AutoCenter);
#endif
            }
            else
            {
                // set the prefabs position in the center of the grid cell not taking into account it's render bounds
                gridPositionInLocalSpace = new Vector3(
                    (column * map.CellWidth) + (map.CellWidth / 2),
                    layer * map.Depth,
                    (row * map.CellHeight) + (map.CellHeight / 2));
            }

            // set the prefabs transform position
#if PERFORMANCE
            perf.Start(PerformanceID.PositionPrefabOnMap_SetPosition);
#endif
            transform.localPosition = gridPositionInLocalSpace;
#if PERFORMANCE
            perf.Stop(PerformanceID.PositionPrefabOnMap_SetPosition);
#endif

#if PERFORMANCE
            perf.Stop(PerformanceID.PositionPrefabOnMapExtensionMethod);
#endif
        }

        /// <summary>
        /// Draws a rectangular area of the map with a specified prefab.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="prefab">The prefab to be drawn. Specify null to erase.</param>
        /// <param name="layer">The layer where drawing will occur.</param>
        /// <param name="left">The left position of the rectangle in grid co-ordinates.</param>
        /// <param name="top">The top position of the rectangle in grid co-ordinates.</param>
        /// <param name="autoCenterPrefab">Determines whether or not to automatically center the prefab within the grid cell.</param>
        /// <param name="autoScalePrefab">If true the prefab will be automatically scaled to fit within the confines of the maps cell size.</param>
        public static void DrawRectangle(
            this GridMap map,
            int layer,
            int left,
            int top,
            int width,
            int height,
            GameObject prefab,
            bool autoCenterPrefab,
            bool autoScalePrefab)
        {
            DrawRectangle(map, layer, left, top, width, height, prefab, autoCenterPrefab, autoScalePrefab, null);
        }

        /// <summary>
        /// Draws a rectangular area of the map with a specified prefab.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="prefab">The prefab to be drawn. Specify null to erase.</param>
        /// <param name="layer">The layer where drawing will occur.</param>
        /// <param name="left">The left position of the rectangle in grid co-ordinates.</param>
        /// <param name="top">The top position of the rectangle in grid co-ordinates.</param>
        /// <param name="autoCenterPrefab">Determines whether or not to automatically center the prefab within the grid cell.</param>
        /// <param name="autoScalePrefab">If true the prefab will be automatically scaled to fit within the confines of the maps cell size.</param>
        /// <param name="callback">Provides a draw callback function that if returns true will allow the prefab to be drawn. </param>
        /// <remarks>If <see cref="callback"/> is null the check is ignored and drawing is allowed.</remarks>
        public static void DrawRectangle(this GridMap map, int layer, int left, int top, int width, int height, GameObject prefab,
            bool autoCenterPrefab, bool autoScalePrefab, Func<int, int, int, GameObject, bool> callback)
        {
            if (prefab == null)
            {
                throw new ArgumentNullException("prefab");
            }

            var dimensions = new Rect(0, 0, map.Columns, map.Rows);
            var firstRun = true;

            if (!dimensions.Intersects(new Rect(left, top, width, height)))
            {
                return;
            }

            var material = prefab.renderer.sharedMaterial;
            var getClone = new Func<GameObject>(() =>
                {
                    var value = firstRun ? prefab : Helpers.PerformInstantiation(prefab);

                    // clear material references before assigning new material
                    value.renderer.material = null;
                    value.renderer.sharedMaterial = material;
                    return value;
                });

            for (var rowIndex = 0; rowIndex < height; rowIndex++)
            {
                if (rowIndex + top < 0 || rowIndex + top > map.Rows - 1)
                {
                    continue;
                }

                if (left < 0 || left > map.Columns - 1 && left + width - 1 < 0 || left + width - 1 > map.Columns - 1)
                {
                    continue;
                }

                // left side
                var clonedObject = getClone();
                firstRun = false;
                if (callback != null && !callback(left, rowIndex + top, layer, clonedObject))
                {
                    continue;
                }

                map.PositionPrefabOnMap(layer, left, rowIndex + top, clonedObject, autoCenterPrefab, autoScalePrefab);

                if (left != left + width - 1)
                {
                    // right side
                    clonedObject = getClone();
                    //                    clonedObject = firstRun ? prefab : Helpers.PerformInstantiation(prefab);
                    if (callback != null && !callback(left + width - 1, rowIndex + top, layer, clonedObject))
                    {
                        continue;
                    }

                    map.PositionPrefabOnMap(layer, left + width - 1, rowIndex + top, clonedObject, autoCenterPrefab, autoScalePrefab);
                }
            }

            if (left == left + width - 1)
            {
                return;
            }

            for (var columnIndex = 1; columnIndex < width - 1; columnIndex++)
            {
                if (columnIndex + left < 0 || columnIndex + left > map.Columns - 1)
                {
                    continue;
                }

                if (top < 0 || top > map.Rows - 1 && top + height - 1 < 0 || top + height - 1 > map.Rows - 1)
                {
                    continue;
                }

                // top side
                var clonedObject = getClone();
                //  var clonedObject = firstRun ? prefab : Helpers.PerformInstantiation(prefab);
                if (callback != null && !callback(columnIndex + left, top, layer, clonedObject))
                {
                    continue;
                }

                map.PositionPrefabOnMap(layer, columnIndex + left, top, clonedObject, autoCenterPrefab, autoScalePrefab);

                if (top != top + height - 1)
                {
                    // bottom side
                    clonedObject = getClone();
                    //   clonedObject = firstRun ? prefab : Helpers.PerformInstantiation(prefab);
                    if (callback != null && !callback(columnIndex + left, top + height - 1, layer, clonedObject))
                    {
                        continue;
                    }

                    map.PositionPrefabOnMap(layer, columnIndex + left, top + height - 1, clonedObject, autoCenterPrefab, autoScalePrefab);
                }
            }
        }

        /// <summary>
        /// Erases a rectangular area of the map with a specified prefab.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="layer">The layer where drawing will occur.</param>
        /// <param name="left">The left position of the rectangle in grid co-ordinates.</param>
        /// <param name="top">The top position of the rectangle in grid co-ordinates.</param>
        public static void EraseRectangle(
            this GridMap map,
            int layer,
            int left,
            int top,
            int width,
            int height)
        {
            EraseRectangle(map, layer, left, top, width, height, null);
        }

        /// <summary>
        /// Erases a rectangular area of the map with a specified prefab.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="layer">The layer where drawing will occur.</param>
        /// <param name="left">The left position of the rectangle in grid co-ordinates.</param>
        /// <param name="top">The top position of the rectangle in grid co-ordinates.</param>
        /// <param name="callback">Provides a draw callback function that if returns true will allow the prefab to be drawn. </param>
        /// <remarks>If <see cref="callback"/> is null the check is ignored and drawing is allowed.</remarks>
        public static void EraseRectangle(this GridMap map, int layer, int left, int top, int width, int height, Func<int, int, int, GameObject, bool> callback)
        {
            var dimensions = new Rect(0, 0, map.Columns, map.Rows);

            var rect = new Rect(left, top, width, height);
            if (!dimensions.Intersects(rect))
            {
                return;
            }

            var transform = map.transform;
            var i = 0;
            while (i < transform.childCount)
            {
                var child = transform.GetChild(i).gameObject;

                string name;
                int column;
                int row;
                int layerIndex;
                if (child.name.TryParsePrefabName(out name, out layerIndex, out column, out row))
                {
                    // check if name is within expected bounds
                    if (layer != layerIndex || column < 0 || column < left || column > map.Columns - 1 || column > rect.xMax - 1 ||
                        row < 0 || row < top || row > map.Rows - 1 || row > rect.yMax - 1)
                    {
                        i++;
                        continue;
                    }

                    // check if column & row are at edges
                    if (column != (int)rect.xMin & column != (int)rect.xMax - 1 & row != (int)rect.yMin & row != (int)rect.yMax - 1)
                    {
                        i++;
                        continue;
                    }

                    // check if there is a callback and if so call it for validation whether we can destroy the game object
                    if (callback != null && !callback(column, row, layer, child))
                    {
                        continue;
                    }

#if UNITY_EDITOR
                    Object.DestroyImmediate(child);
#else
                    Object.Destroy(child);           
#endif

                    continue;
                }

                i++;
            }
        }

        /// <summary>
        /// Fills a rectangular area of the map with a specified prefab.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="prefab">The prefab to be drawn. Specify null to erase.</param>
        /// <param name="layer">The layer where drawing will occur.</param>
        /// <param name="left">The left position of the rectangle in grid co-ordinates.</param>
        /// <param name="top">The top position of the rectangle in grid co-ordinates.</param>
        /// <param name="autoCenterPrefab">Determines whether or not to automatically center the prefab within the grid cell.</param>
        /// <param name="autoScalePrefab">If true the prefab will be automatically scaled to fit within the confines of the maps cell size.</param>
        public static void FillRectangle(
            this GridMap map,
            int layer,
            int left,
            int top,
            int width,
            int height,
            GameObject prefab,
            bool autoCenterPrefab,
            bool autoScalePrefab)
        {
            FillRectangle(map, layer, left, top, width, height, prefab, autoCenterPrefab, autoScalePrefab, null);
        }

        /// <summary>
        /// Fills a rectangular area of the map with a specified prefab.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="prefab">The prefab to be drawn. Specify null to erase.</param>
        /// <param name="layer">The layer where drawing will occur.</param>
        /// <param name="left">The left position of the rectangle in grid co-ordinates.</param>
        /// <param name="top">The top position of the rectangle in grid co-ordinates.</param>
        /// <param name="autoCenterPrefab">Determines whether or not to automatically center the prefab within the grid cell.</param>
        /// <param name="autoScalePrefab">If true the prefab will be automatically scaled to fit within the confines of the maps cell size.</param>
        /// <param name="callback">Provides a draw callback function that if returns true will allow the prefab to be drawn. </param>
        /// <remarks>If <see cref="callback"/> is null the check is ignored and drawing is allowed.</remarks>
        public static void FillRectangle(this GridMap map, int layer, int left, int top, int width, int height, GameObject prefab,
            bool autoCenterPrefab, bool autoScalePrefab, Func<int, int, int, GameObject, bool> callback)
        {
            if (prefab == null)
            {
                throw new ArgumentNullException("prefab");
            }

            var dimensions = new Rect(0, 0, map.Columns, map.Rows);
            var firstRun = true;

            if (!dimensions.Intersects(new Rect(left, top, width, height)))
            {
                return;
            }

            var material = prefab.renderer.sharedMaterial;
            var getClone = new Func<GameObject>(() =>
            {
                var value = firstRun ? prefab : Helpers.PerformInstantiation(prefab);

                // clear material references before assigning new material
                value.renderer.material = null;
                value.renderer.sharedMaterial = material;
                return value;
            });

#if PERFORMANCE
            var perf = PerformanceTesting<PerformanceID>.Instance;
            perf.Start(PerformanceID.FillRectangleExtensionMethod);
#endif

            for (var rowIndex = 0; rowIndex < height; rowIndex++)
            {
                if (rowIndex + top < 0 || rowIndex + top > map.Rows - 1)
                {
                    continue;
                }

                for (var columnIndex = 0; columnIndex < width; columnIndex++)
                {
                    if (columnIndex + left < 0 || columnIndex + left > map.Columns - 1)
                    {
                        continue;
                    }
#if PERFORMANCE
                    perf.Start(PerformanceID.FillRectangleExtensionMethod_Instantiation);
#endif
                    var clonedObject = getClone();
#if PERFORMANCE
                    perf.Stop(PerformanceID.FillRectangleExtensionMethod_Instantiation);
#endif
                    firstRun = false;

#if PERFORMANCE
                    perf.Start(PerformanceID.FillRectangleExtensionMethod_Callback);
#endif
                    if (callback != null && !callback(columnIndex + left, rowIndex + top, layer, clonedObject))
                    {
#if PERFORMANCE
                        perf.Stop(PerformanceID.FillRectangleExtensionMethod_Callback);
#endif
                        continue;
                    }
#if PERFORMANCE
                    perf.Stop(PerformanceID.FillRectangleExtensionMethod_Callback);
#endif
                    map.PositionPrefabOnMap(layer, columnIndex + left, rowIndex + top, clonedObject, autoCenterPrefab, autoScalePrefab);
                }
            }

#if PERFORMANCE
            perf.Stop(PerformanceID.FillRectangleExtensionMethod);
#endif
        }

        /// <summary>
        /// Erases a rectangular area of the map.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="layer">The layer where drawing will occur.</param>
        /// <param name="left">The left position of the rectangle in grid co-ordinates.</param>
        /// <param name="top">The top position of the rectangle in grid co-ordinates.</param>
        public static void EraseFilledRectangle(
            this GridMap map,
            int layer,
            int left,
            int top,
            int width,
            int height)
        {
            EraseFilledRectangle(map, layer, left, top, width, height, null);
        }

        /// <summary>
        /// Erases a rectangular area of the map.
        /// </summary>
        /// <param name="map">The reference to a <see cref="GridMap"/> type.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="layer">The layer where drawing will occur.</param>
        /// <param name="left">The left position of the rectangle in grid co-ordinates.</param>
        /// <param name="top">The top position of the rectangle in grid co-ordinates.</param>
        /// <param name="callback">Provides a erase callback function that if returns true will allow the prefab to be erased. </param>
        /// <remarks>If <see cref="callback"/> is null the check is ignored and erasing is allowed.</remarks>
        public static void EraseFilledRectangle(this GridMap map, int layer, int left, int top, int width, int height,
              Func<int, int, int, GameObject, bool> callback)
        {
            var dimensions = new Rect(0, 0, map.Columns, map.Rows);

            var rect = new Rect(left, top, width, height);
            if (!dimensions.Intersects(rect))
            {
                return;
            }

            var transform = map.transform;
            var i = 0;
            while (i < transform.childCount)
            {
                var child = transform.GetChild(i).gameObject;

                string name;
                int column;
                int row;
                int layerIndex;
                if (child.name.TryParsePrefabName(out name, out layerIndex, out column, out row))
                {
                    // check if name is within expected bounds
                    if (layer != layerIndex || column < 0 || column > map.Columns - 1 || row < 0 || row > map.Rows - 1 || !rect.Contains(new UnityEngine.Vector2(column, row)))
                    {
                        i++;
                        continue;
                    }

                    // check if there is a callback and if so call it for validation whether we can destroy the game object
                    if (callback != null && !callback(column, row, layer, child.gameObject))
                    {
                        continue;
                    }

#if UNITY_EDITOR
                    Object.DestroyImmediate(child);
#else
                    Object.Destroy(child);           
#endif

                    continue;
                }

                i++;
            }
        }

        /// <summary>
        /// Scales a prefab to the dimensions of the maps row/column cell size.
        /// </summary>
        /// <param name="prefab">
        /// Reference to the prefab to scale.
        /// </param>
        /// <param name="map">
        /// Reference to the map that will be used to determine how much to scale the prefab.
        /// </param>
        /// <remarks>This method can be slow if used too often and is not recommended for use in your game logic.</remarks>
        public static void ScalePrefab(this GridMap map, GameObject prefab)
        {
            if (prefab == null)
            {
                throw new ArgumentNullException("prefab");
            }

#if PERFORMANCE
            var perf = PerformanceTesting<PerformanceID>.Instance;
            perf.Start(PerformanceID.ScalePrefab);
#endif
            // get bounds of the prefab
            var bounds = new Bounds();
            var encapsulate = false;
            var transform = prefab.transform;
            if (!Utilities.Helpers.GetBoundWithChildren(transform, ref bounds, ref encapsulate))
            {
                return;
            }

            // apply scaling to the prefab
            var scale = transform.localScale;

            // set x/z scale
            scale.x *= map.CellWidth / bounds.size.x;
            scale.z *= map.CellHeight / bounds.size.z;

            // check for infinity values and if found set to 0
            scale.x = float.IsInfinity(scale.x) ? 0 : scale.x;
            scale.y = float.IsInfinity(scale.y) ? 0 : scale.y;
            scale.z = float.IsInfinity(scale.z) ? 0 : scale.z;

            // set scale
            transform.localScale = scale;

#if PERFORMANCE
            perf.Stop(PerformanceID.ScalePrefab);
#endif
        }
    }
}