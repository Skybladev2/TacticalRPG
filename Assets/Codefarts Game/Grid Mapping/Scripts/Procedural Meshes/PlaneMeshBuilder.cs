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
    public class PlaneMeshBuilder : MonoBehaviour
    {
        private static Mesh sharedInstance;

        public static Mesh SharedInstance
        {
            get
            {
                if (sharedInstance == null)
                {
                    sharedInstance = MeshGenerationHelpers.CreatePlane(1, 1, 1, 1);
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

            var collider = this.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.size = new Vector3(1, 0, 1);
            }
        }
    }
}