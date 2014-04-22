/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.GridMapping.Editor
{
    using System;

    using Codefarts.GridMapping.Common;
    using Codefarts.GridMapping.Models;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Provides editor helpers methods.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Holds the time when the scene view was last repainted on purpose.
        /// </summary>
        private static DateTime lastSceneViewRepaint;

        /// <summary>
        /// Used to restrict scene view repaints to 10ms intervals.
        /// </summary>
        public static void RepaintSceneView()
        {
            if (DateTime.Now > lastSceneViewRepaint + TimeSpan.FromMilliseconds(10))
            {
                lastSceneViewRepaint = DateTime.Now;
                SceneView.RepaintAll();
            }
        }

        /// <summary>
        /// If the PERFORMANCE symbol is available will report the performance metric information out to the console.
        /// </summary>
#if PERFORMANCE
        public static void ReportPerformanceTimes()
        {
            var values = (PerformanceID[])Enum.GetValues(typeof(PerformanceID));
            var perf = PerformanceTesting<PerformanceID>.Instance;
            var result = String.Empty;
            foreach (var value in values)
            {
                result += String.Format("{0} - Total: {1}ms Average: {2} Count: {3}\r\n", value, perf.TotalMilliseconds(value), perf.AverageMilliseconds(value), perf.GetStartCount(value));
            }

            result += String.Format("Total Performance Times - Total: {0}ms", perf.TotalMilliseconds(values));
            Debug.Log(result);
        }
#endif       

        // TODO: This feels like a hack there has to be a better solution then calling setdirty on selected objects
        // this also does not work if the inspector is locked to a gridmap and the selection is selecting another object
        // a better solution must be worked out
        public static void RedrawInspector()
        {
            // grab any GridMap types if they are selected and set them as dirty causing inspector to redraw
            // this allows for live updating so user can see changes in inspector immediately
            var objects = Selection.GetFiltered(typeof(GridMap), SelectionMode.TopLevel);
            foreach (var o in objects)
            {
                EditorUtility.SetDirty(o);
            }
        }

        /// <summary>
        /// Checks to see if neighbors around a grid position match the rules neighbor conditional check.
        /// </summary>
        /// <param name="gridPosition">The column and row to check.</param>
        /// <param name="layer">The layer to check.</param>
        /// <param name="ignoreCenter">If true the center will be ignored.</param>
        /// <param name="rule">The rule that will be used to compare neighbors.</param>
        /// <param name="getPrefabCallback">A callback that returns a prefab at a given grid location.</param>
        /// <returns>True if the rule neighbors match the prefabs on the grid.</returns>
        public static bool CheckNeighborsPassed(Point gridPosition, int layer, bool ignoreCenter, DrawRuleModel rule, Func<int, int, int, GameObject> getPrefabCallback)
        {
            if (getPrefabCallback == null)
            {
                throw new ArgumentNullException("getPrefabCallback");
            }

            // check if neighbors match the rule
            var indexes = new[]
                {
                    new Point(-1, 1), new Point(0, 1), new Point(1, 1), 
                    new Point(-1, 0),  new Point(0, 0),  new Point(1, 0), 
                    new Point(-1, -1),  new Point(0, -1),  new Point(1, -1)
                };

            var passed = true;

            // check for each layer
            for (var i = 0; i < indexes.Length; i++)
            {
                // ignore index 4
                if (i == 4 && ignoreCenter)
                {
                    continue;
                }

                // get position
                var position = gridPosition + indexes[i];

                // check if neighbor specified and if there is a prefab at the neighbor index
                passed = rule.Neighbors[i] == (getPrefabCallback(position.X, position.Y, layer) != null);

                // if did not pass the test exit the loop
                if (!passed)
                {
                    break;
                }
            }

            return passed;
        }

        /// <summary>
        /// Creates a new prefab based on the currently selected primitive type.
        /// </summary>
        /// <param name="gridMapEditor">A reference to a grid map editor.</param>
        /// <returns>Returns a reference to the prefab that was created.</returns>
        public static GameObject CreatePrimitivePrefab(GridMapEditor gridMapEditor)
        {
            GameObject prefab = null;

            var gridMappingService = GridMappingService.Instance;

            // if current prefab is not null
            if (gridMappingService.CurrentPrefab != null)
            {
                prefab = GridMapping.Helpers.PerformInstantiation(gridMappingService.CurrentPrefab);
            }

            // set material if one specified & there is a mesh renderer & apply material is checked
            if (!gridMapEditor.SelectingCustomPrefab && gridMappingService.CurrentMaterial != null && prefab != null && prefab.renderer != null && gridMapEditor.ApplyMaterial)
            {
                // clear material references before assigning new material
                prefab.renderer.material = null;
                prefab.renderer.sharedMaterial = gridMappingService.CurrentMaterial;
            }

            return prefab;
        }
    }
}