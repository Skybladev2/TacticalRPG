/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Scripts.ProceduralMeshes
{
    using Codefarts.GridMapping.Utilities;

    using UnityEngine;

    [ExecuteInEditMode]
    public class OutsideCornerMeshBuilder : MonoBehaviour
    {
        private static Mesh sharedInstance;

        public static Mesh SharedInstance
        {
            get
            {
                if (sharedInstance == null)
                {
                    sharedInstance = MeshGenerationHelpers.CreateOutsideCornerCube();
                    sharedInstance.RecalculateBounds();
                    sharedInstance.Optimize();
                }

                return sharedInstance;
            }
        }

        private void Awake()
        {
            var filter = this.GetComponent<MeshFilter>();

            filter.sharedMesh = null;
            filter.sharedMesh = SharedInstance;

            var collider = this.GetComponent<MeshCollider>();
            if (collider != null)
            {
                collider.sharedMesh = null;
                collider.sharedMesh = filter.sharedMesh;
                collider.convex = true;
            }
        }
    }
}