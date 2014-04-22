namespace Codefarts.GridMapping
{
    using System;

    using UnityEditor;

    using UnityEngine;

    using Object = UnityEngine.Object;

    /// <summary>
    /// Contains helper methods.
    /// </summary>
    public class Helpers
    {
        /// <summary>
        /// Performs an Instantiate depending on the <see cref="gameObject"/> parameters <see cref="PrefabUtility.GetPrefabType"/>.
        /// </summary>
        /// <param name="gameObject">The came object to clone.</param>
        /// <returns>Returns a reference to a new game object.</returns>
        /// <remarks>This method will use <see cref="PrefabUtility.GetPrefabType"/> in order to determine whether to use <see cref="PrefabUtility"/> to instantiate the game object.</remarks>
        public static GameObject PerformInstantiation(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return null;
            }

#if UNITY_EDITOR
            // TODO: This method need unit testing for the various prefab types
            var prefabType = PrefabUtility.GetPrefabType(gameObject);
            switch (prefabType)
            {
                case PrefabType.Prefab:
                case PrefabType.ModelPrefab:
                    return (GameObject)PrefabUtility.InstantiatePrefab(gameObject);

                case PrefabType.ModelPrefabInstance:
                case PrefabType.MissingPrefabInstance:
                case PrefabType.DisconnectedPrefabInstance:
                case PrefabType.DisconnectedModelPrefabInstance:
                case PrefabType.PrefabInstance:
                    // get path the asset source
                    var path = GetSourcePrefab(gameObject);
                    if (path == null)
                    {
                        // could not find (it may have been procedurally generated) so just return null
                        return null;
                    }

                    // get original instance
                    var reference = (GameObject)AssetDatabase.LoadMainAssetAtPath(path);

                    // instantiate from original and return the result
                    return (GameObject)PrefabUtility.InstantiatePrefab(reference);
            }

#endif
            // attempt normal instantiate
            var instantiate = (GameObject)Object.Instantiate(gameObject);
            instantiate.name = gameObject.name;
            return instantiate;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Gets the source path to a prefab if any.
        /// </summary>
        /// <param name="prefab">The prefab reference to get the asset path for.</param>
        /// <returns>Returns a asset path for a prefab.</returns>
        /// <remarks>This method will attempt to find the source asset of the given <see cref="UnityEngine.Object"/> by 
        /// walking up the parent prefab hierarchy.</remarks>
        public static string GetSourcePrefab(Object prefab)
        {
            // if no prefab specified then return null
            if (prefab == null)
            {
                return null;
            }

            // attempt to get the path
            var path = AssetDatabase.GetAssetPath(prefab);

            //  Debug.Log(string.Format("path: \"{0}\"", path));
            // if no path returned it may be an instantiated prefab so try to get the parent prefab
            while (String.IsNullOrEmpty(path))
            {
                // try parent prefab
                var parent = PrefabUtility.GetPrefabParent(prefab);

                // no parent so must be generated through code so just exit loop
                if (parent == null)
                {
                    break;
                }

                // attempt to get path for 
                path = AssetDatabase.GetAssetPath(parent);
                //                Debug.Log(string.Format("path2: \"{0}\"", path));

                // set prefab reference to parent for next loop
                prefab = parent;
            }

            // return the path if any
            return path;
        }
#endif
    }
}
