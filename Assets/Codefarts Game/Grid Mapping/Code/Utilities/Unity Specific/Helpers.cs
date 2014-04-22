/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Utilities
{
    using UnityEngine;

    /// <summary>
    /// Provides general helper methods for unity.
    /// </summary>
    public partial class Helpers
    {
        /// <summary>
        /// Gets the rendering bounds of the transform.
        /// </summary>
        /// <param name="transform">The game object to get the bounding box for.</param>
        /// <param name="pBound">The bounding box reference that will </param>
        /// <param name="encapsulate">Used to determine if the first bounding box to be 
        /// calculated should be encapsulated into the <see cref="pBound"/> argument.</param>
        /// <returns>Returns true if at least one bounding box was calculated.</returns>
        public static bool GetBoundWithChildren(Transform transform, ref Bounds pBound, ref bool encapsulate)
        {
            var didOne = false;

            // get 'this' bound
            if (transform.gameObject.renderer != null)
            {
                var bound = transform.gameObject.renderer.bounds;
                if (encapsulate)
                {
                    pBound.Encapsulate(bound.min);
                    pBound.Encapsulate(bound.max);
                }
                else
                {
                    pBound.min = bound.min; 
                    pBound.max = bound.max; 
                    encapsulate = true;
                }

                didOne = true;
            }

            // union with bound(s) of any/all children
            foreach (Transform child in transform)
            {
                if (GetBoundWithChildren(child, ref pBound, ref encapsulate))
                {
                    didOne = true;
                }
            }

            return didOne;
        }

        public static Bounds GetBounds(GameObject gameObject)
        {
            //get gameObject object bounds
            var filter = gameObject.GetComponent<SkinnedMeshRenderer>() as Renderer;
            if (filter == null) filter = gameObject.GetComponent<MeshRenderer>();
            var bounds = new Bounds(gameObject.transform.position, new Vector3());
            if (filter == null)
            {
                var mf = gameObject.GetComponent<MeshFilter>();
                if (mf != null)
                {
                    bounds.size = mf.sharedMesh.bounds.size;
                }
                else
                {
                    if (gameObject.collider != null)
                    {
                        bounds.size = gameObject.collider.bounds.size;
                    }
                }
            }
            else
            {
                bounds.size = filter.bounds.size;
            }

            return bounds;
        }

        public static void CalculateMeshTangents(Mesh mesh)
        {
            //speed up math by copying the mesh arrays
            int[] triangles = mesh.triangles;
            Vector3[] vertices = mesh.vertices;
            Vector2[] uv = mesh.uv;
            Vector3[] normals = mesh.normals;

            //variable definitions
            int triangleCount = triangles.Length;
            int vertexCount = vertices.Length;

            var tan1 = new Vector3[vertexCount];
            var tan2 = new Vector3[vertexCount];

            var tangents = new Vector4[vertexCount];

            for (long a = 0; a < triangleCount; a += 3)
            {
                long i1 = triangles[a + 0];
                long i2 = triangles[a + 1];
                long i3 = triangles[a + 2];

                Vector3 v1 = vertices[i1];
                Vector3 v2 = vertices[i2];
                Vector3 v3 = vertices[i3];

                Vector2 w1 = uv[i1];
                Vector2 w2 = uv[i2];
                Vector2 w3 = uv[i3];

                float x1 = v2.x - v1.x;
                float x2 = v3.x - v1.x;
                float y1 = v2.y - v1.y;
                float y2 = v3.y - v1.y;
                float z1 = v2.z - v1.z;
                float z2 = v3.z - v1.z;

                float s1 = w2.x - w1.x;
                float s2 = w3.x - w1.x;
                float t1 = w2.y - w1.y;
                float t2 = w3.y - w1.y;

                float r = 1.0f / (s1 * t2 - s2 * t1);

                var sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                var tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

                tan1[i1] += sdir;
                tan1[i2] += sdir;
                tan1[i3] += sdir;

                tan2[i1] += tdir;
                tan2[i2] += tdir;
                tan2[i3] += tdir;
            }


            for (long a = 0; a < vertexCount; ++a)
            {
                var n = normals[a];
                var t = tan1[a];

                //Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
                //tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
                Vector3.OrthoNormalize(ref n, ref t);
                tangents[a].x = t.x;
                tangents[a].y = t.y;
                tangents[a].z = t.z;

                tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
            }

            mesh.tangents = tangents;
        }
    }
}
