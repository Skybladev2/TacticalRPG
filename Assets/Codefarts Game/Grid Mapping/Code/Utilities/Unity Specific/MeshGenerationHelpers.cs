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
    using System;

    using UnityEngine;

    /// <summary>
    /// Provides methods that are used to generate various basic mesh shapes.
    /// </summary>
    public class MeshGenerationHelpers
    {
        public static Mesh CreateInsideCornerCube2D()
        {
            var mesh = new Mesh();

            var vertices = new[]
            {
                new Vector3(0.5f, 0f, -0.5f),
                new Vector3(0.206f, 0f, -0.207f),
                new Vector3(0.423f, 0f, 0.117f),
                new Vector3(-0.118f, 0f, -0.424f),
                new Vector3(-0.5f, 0f, -0.5f),
                new Vector3(-0.5f, 0f, -0.5f),
                new Vector3(0.499f, 0f, 0.5f),
                new Vector3(0.5f, 0f, 0.5f)
            };

            var normals = new[]
            {
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f)
            };

            var uv = new[]
            {
                new Vector2(0.9995f, 0.0005f),
                new Vector2(0.7056765f, 0.2904059f),
                new Vector2(0.921147f, 0.6155706f),
                new Vector2(0.3805118f, 0.07493529f),
                new Vector2(0.0005f, 0.0005f),
                new Vector2(0.0005f, 0.0005f),
                new Vector2(0.9955823f, 0.9995f),
                new Vector2(0.9995f, 0.9995f)
            };

            var triangles = new[]
            {
                0, 1, 2,
                0, 3, 1,
                0, 4, 3,
                0, 5, 4,
                2, 6, 0,
                6, 7, 0
            };

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.name = "InsideCorner2DFromCode";
            return mesh;
        }

        public static Mesh CreateInsideCornerCube()
        {
            var mesh = new Mesh();

            var vertices = new[]
                {
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(0.087f, -0.309f, -0.5f),
                    new Vector3(0.308f, -0.088f, -0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(0.45f, 0.19f, -0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f),
                    new Vector3(-0.191f, -0.451f, -0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(0.308f, -0.088f, 0.5f),
                    new Vector3(0.087f, -0.309f, 0.5f),
                    new Vector3(0.45f, 0.19f, 0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f),
                    new Vector3(-0.191f, -0.451f, 0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f),
                    new Vector3(0.45f, 0.19f, -0.5f),
                    new Vector3(0.45f, 0.19f, 0.5f),
                    new Vector3(0.308f, -0.088f, -0.5f),
                    new Vector3(0.308f, -0.088f, 0.5f),
                    new Vector3(-0.191f, -0.451f, -0.5f),
                    new Vector3(-0.191f, -0.451f, 0.5f),
                    new Vector3(0.087f, -0.309f, 0.5f),
                    new Vector3(0.087f, -0.309f, -0.5f),
                    new Vector3(0.308f, -0.088f, 0.5f),
                    new Vector3(0.308f, -0.088f, -0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f)
                };

            var normals = new[]
                {
                    new Vector3(-0.016f, -1f, 0f),
                    new Vector3(-0.016f, -1f, 0f),
                    new Vector3(-0.016f, -1f, 0f),
                    new Vector3(-0.016f, -1f, 0f),
                    new Vector3(1f, -0.016f, 0f),
                    new Vector3(1f, -0.016f, 0f),
                    new Vector3(1f, -0.016f, 0f),
                    new Vector3(1f, -0.016f, 0f),
                    new Vector3(-0.016f, -0.016f, -1f),
                    new Vector3(-0.016f, -0.016f, -1f),
                    new Vector3(-0.016f, -0.016f, -1f),
                    new Vector3(-0.016f, -0.016f, -1f),
                    new Vector3(-0.016f, -0.016f, -1f),
                    new Vector3(-0.016f, -0.016f, -1f),
                    new Vector3(-0.016f, -0.016f, -1f),
                    new Vector3(-0.016f, -0.016f, 1f),
                    new Vector3(-0.016f, -0.016f, 1f),
                    new Vector3(-0.016f, -0.016f, 1f),
                    new Vector3(-0.016f, -0.016f, 1f),
                    new Vector3(-0.016f, -0.016f, 1f),
                    new Vector3(-0.016f, -0.016f, 1f),
                    new Vector3(-0.016f, -0.016f, 1f),
                    new Vector3(-0.99f, 0.141f, 0f),
                    new Vector3(-0.99f, 0.141f, 0f),
                    new Vector3(-0.955f, 0.297f, 0f),
                    new Vector3(-0.955f, 0.297f, 0f),
                    new Vector3(-0.809f, 0.587f, 0f),
                    new Vector3(-0.809f, 0.587f, 0f),
                    new Vector3(-0.333f, 0.937f, -0.109f),
                    new Vector3(-0.333f, 0.937f, -0.109f),
                    new Vector3(-0.619f, 0.778f, -0.109f),
                    new Vector3(-0.619f, 0.778f, -0.109f),
                    new Vector3(-0.809f, 0.587f, 0f),
                    new Vector3(-0.809f, 0.587f, 0f),
                    new Vector3(-0.175f, 0.968f, -0.179f),
                    new Vector3(-0.175f, 0.968f, -0.179f)
                };

            var uv = new[]
                {
                    new Vector2(0.9955828f, 0.9955828f),
                    new Vector2(0.9955828f, 0.0004995465f),
                    new Vector2(0.0004995465f, 0.0004995465f),
                    new Vector2(0.0004995465f, 0.9955828f),
                    new Vector2(0.0004995465f, 0.0004995465f),
                    new Vector2(0.0004995465f, 0.9955828f),
                    new Vector2(0.9955828f, 0.9955828f),
                    new Vector2(0.9955828f, 0.0004995465f),
                    new Vector2(0.5842295f, 0.1885468f),
                    new Vector2(0.8075356f, 0.4118529f),
                    new Vector2(0.9955828f, 0.0004995465f),
                    new Vector2(0.948571f, 0.6900061f),
                    new Vector2(0.9955828f, 0.9955828f),
                    new Vector2(0.3060763f, 0.04751136f),
                    new Vector2(0.0004995465f, 0.0004995465f),
                    new Vector2(0.0004995465f, 0.0004995465f),
                    new Vector2(0.1885468f, 0.4118529f),
                    new Vector2(0.4118529f, 0.1885468f),
                    new Vector2(0.04751136f, 0.6900061f),
                    new Vector2(0.0004995465f, 0.9955828f),
                    new Vector2(0.6900061f, 0.04751136f),
                    new Vector2(0.9955828f, 0.0004995465f),
                    new Vector2(0.9955828f, 0.9955828f),
                    new Vector2(0.9955828f, 0.0004995465f),
                    new Vector2(0.6900061f, 0.0004995465f),
                    new Vector2(0.6900061f, 0.9955828f),
                    new Vector2(0.4118529f, 0.0004995465f),
                    new Vector2(0.4118529f, 0.9955828f),
                    new Vector2(0.3060763f, 0.0004995465f),
                    new Vector2(0.3060763f, 0.9955828f),
                    new Vector2(0.5842295f, 0.9955828f),
                    new Vector2(0.5842295f, 0.0004995465f),
                    new Vector2(0.8075356f, 0.9955828f),
                    new Vector2(0.8075356f, 0.0004995465f),
                    new Vector2(0.0004995465f, 0.0004995465f),
                    new Vector2(0.0004995465f, 0.9955828f)
                };

            var triangles = new[]
                {
                    0, 1, 2,
                    2, 3, 0,
                    4, 5, 6,
                    6, 7, 4,
                    8, 9, 10,
                    9, 11, 10,
                    11, 12, 10,
                    10, 13, 8,
                    10, 14, 13,
                    15, 16, 17,
                    15, 18, 16,
                    15, 19, 18,
                    17, 20, 15,
                    20, 21, 15,
                    22, 23, 24,
                    24, 25, 22,
                    25, 24, 26,
                    26, 27, 25,
                    28, 29, 30,
                    30, 31, 28,
                    31, 30, 32,
                    32, 33, 31,
                    29, 28, 34,
                    34, 35, 29
                };

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.name = "InsideCornerFromCode";
            return mesh;
        }

        public static Mesh CreateRamp(Vector3 size)
        {

            var mesh = new Mesh();
            var halfX = Math.Abs(size.x - 0) < Mathf.Epsilon ? 0 : size.x / 2f;
            var halfY = Math.Abs(size.y - 0) < Mathf.Epsilon ? 0 : size.y / 2f;
            var halfZ = Math.Abs(size.z - 0) < Mathf.Epsilon ? 0 : size.z / 2f; 

            var vertices = new[]
                {
                    new Vector3(-halfX, halfY, -halfZ),
                    new Vector3(halfX, -halfY, -halfZ),
                    new Vector3(-halfX, -halfY, -halfZ),
                    new Vector3(halfX, -halfY, halfZ),
                    new Vector3(-halfX, halfY, halfZ),
                    new Vector3(-halfX, -halfY, halfZ),
                    new Vector3(halfX, -halfY, -halfZ),
                    new Vector3(-halfX, -halfY, halfZ),
                    new Vector3(-halfX, -halfY, -halfZ),
                    new Vector3(halfX, -halfY, halfZ),
                    new Vector3(-halfX, -halfY, -halfZ),
                    new Vector3(-halfX, halfY, halfZ),
                    new Vector3(-halfX, halfY, -halfZ),
                    new Vector3(-halfX, -halfY, halfZ),
                    new Vector3(-halfX, halfY, -halfZ),
                    new Vector3(halfX, -halfY, halfZ),
                    new Vector3(halfX, -halfY, -halfZ),
                    new Vector3(-halfX, halfY, halfZ)
                };

            var normals = new[]
                {
                    new Vector3(0f, 0f, -1f),
                    new Vector3(0f, 0f, -1f),
                    new Vector3(0f, 0f, -1f),
                    new Vector3(0f, 0f, 1f),
                    new Vector3(0f, 0f, 1f),
                    new Vector3(0f, 0f, 1f),
                    new Vector3(0f, -1f, 0f),
                    new Vector3(0f, -1f, 0f),
                    new Vector3(0f, -1f, 0f),
                    new Vector3(0f, -1f, 0f),
                    new Vector3(-1f, 0f, 0f),
                    new Vector3(-1f, 0f, 0f),
                    new Vector3(-1f, 0f, 0f),
                    new Vector3(-1f, 0f, 0f),
                    new Vector3(1f, 1f, 0f),
                    new Vector3(1f, 1f, 0f),
                    new Vector3(1f, 1f, 0f),
                    new Vector3(1f, 1f, 0f)
                };

            var uv = new[]
                {
                    new Vector2(0.0004994869f, 0.9916806f),
                    new Vector2(0.9916806f, 0.0004994869f),
                    new Vector2(0.0004994869f, 0.0004994869f),
                    new Vector2(0.0004994869f, 0.0004994869f),
                    new Vector2(0.9916806f, 0.9916806f),
                    new Vector2(0.9916806f, 0.0004994869f),
                    new Vector2(0.0004994869f, 0.0004994869f),
                    new Vector2(0.9916806f, 0.9916806f),
                    new Vector2(0.9916806f, 0.0004994869f),
                    new Vector2(0.0004994869f, 0.9916806f),
                    new Vector2(0.0004994869f, 0.0004994869f),
                    new Vector2(0.9916806f, 0.9916806f),
                    new Vector2(0.9916806f, 0.0004994869f),
                    new Vector2(0.0004994869f, 0.9916806f),
                    new Vector2(0.0004994869f, 0.0004994869f),
                    new Vector2(0.9916806f, 0.9916806f),
                    new Vector2(0.9916806f, 0.0004994869f),
                    new Vector2(0.0004994869f, 0.9916806f)
                };

            var triangles = new[]
                {
                    0, 1, 2,
                    3, 4, 5,
                    6, 7, 8,
                    7, 6, 9,
                    10, 11, 12,
                    11, 10, 13,
                    14, 15, 16,
                    17, 15, 14
                };

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.name = "RampFromCode";
            return mesh;
        }

        public static Mesh CreateRamp2DHighPoly()
        {
            var mesh = new Mesh();

            var vertices = new[]
            {
                new Vector3(0.25f, 0f, 0.25f),
                new Vector3(0.5f, 0f, 0.5f),
                new Vector3(0.5f, 0f, 0.25f),
                new Vector3(0.25f, 0f, 0f),
                new Vector3(0.5f, 0f, 0f),
                new Vector3(0f, 0f, 0f),
                new Vector3(0.25f, 0f, -0.251f),
                new Vector3(0.5f, 0f, -0.251f),
                new Vector3(0f, 0f, -0.251f),
                new Vector3(-0.251f, 0f, -0.251f),
                new Vector3(0.25f, 0f, -0.5f),
                new Vector3(0.5f, 0f, -0.5f),
                new Vector3(0f, 0f, -0.5f),
                new Vector3(-0.251f, 0f, -0.5f),
                new Vector3(-0.5f, 0f, -0.5f)
            };

            var normals = new[]
            {
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f)
            };

            var uv = new[]
            {
                new Vector2(0.2473115f, 0.2473115f),
                new Vector2(0.0004995167f, 0.0004995167f),
                new Vector2(0.0004995167f, 0.2473115f),
                new Vector2(0.2473115f, 0.4980412f),
                new Vector2(0.0004995167f, 0.4980412f),
                new Vector2(0.4980412f, 0.4980412f),
                new Vector2(0.2473115f, 0.7487709f),
                new Vector2(0.0004995167f, 0.7487709f),
                new Vector2(0.4980412f, 0.7487709f),
                new Vector2(0.7487709f, 0.7487709f),
                new Vector2(0.2473115f, 0.9955829f),
                new Vector2(0.0004995167f, 0.9955829f),
                new Vector2(0.4980412f, 0.9955829f),
                new Vector2(0.7487709f, 0.9955829f),
                new Vector2(0.9955829f, 0.9955829f)
            };

            var triangles = new[]
            {
                0, 1, 2,
                2, 3, 0,
                3, 2, 4,
                5, 0, 3,
                4, 6, 3,
                6, 4, 7,
                8, 3, 6,
                3, 8, 5,
                9, 5, 8,
                7, 10, 6,
                10, 7, 11,
                12, 6, 10,
                6, 12, 8,
                13, 8, 12,
                8, 13, 9,
                14, 9, 13
            };

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.name = "Ramp2DHighPolyFromCode";
            return mesh;
        }

        public static Mesh CreateRamp2D(Vector3 size)
        {
            var mesh = new Mesh();
            var halfX = Math.Abs(size.x - 0) < Mathf.Epsilon ? 0 : size.x / 2f;
            var halfZ = Math.Abs(size.z - 0) < Mathf.Epsilon ? 0 : size.z / 2f;

            var vertices = new[]
            {
                new Vector3(-halfX, 0, -halfZ),
                new Vector3(-halfX, 0,  halfZ),
                new Vector3(halfX , 0,  halfZ)       
            };

            var normals = new[]
            {
                new Vector3(-halfX, 1f, -halfZ),
                new Vector3(-halfX, 1f,  halfZ),
                new Vector3(halfX , 1f,  halfZ)
            };

            var uv = new[]
            {
                new Vector2(0.0004994869f, 0.0004994869f),
                new Vector2(0.0004994869f, 0.9916806f),
                new Vector2(0.9916806f, 0.9916806f)
            };

            var triangles = new[]
            {
                0, 1, 2
            };

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.name = "Ramp2DFromCode";
            return mesh;
        }

        public static Mesh CreateCornerRamp(Vector3 size)
        {
            var mesh = new Mesh();
            var halfX = Math.Abs(size.x - 0) < Mathf.Epsilon ? 0 : size.x / 2f;
            var halfY = Math.Abs(size.y - 0) < Mathf.Epsilon ? 0 : size.y / 2f;
            var halfZ = Math.Abs(size.z - 0) < Mathf.Epsilon ? 0 : size.z / 2f;

            var vertices = new[]
                {
                    new Vector3(-halfX, halfY, -halfZ),
                    new Vector3(halfX, -halfY, -halfZ),
                    new Vector3(-halfX, -halfY, -halfZ),
                    new Vector3(halfX, -halfY, -halfZ),
                    new Vector3(-halfX, -halfY, halfZ),
                    new Vector3(-halfX, -halfY, -halfZ),
                    new Vector3(halfX, -halfY, halfZ),
                    new Vector3(-halfX, -halfY, -halfZ),
                    new Vector3(-halfX, -halfY, halfZ),
                    new Vector3(-halfX, halfY, -halfZ),
                    new Vector3(-halfX, halfY, -halfZ),
                    new Vector3(halfX, -halfY, halfZ),
                    new Vector3(halfX, -halfY, -halfZ),
                    new Vector3(-halfX, halfY, -halfZ),
                    new Vector3(-halfX, -halfY, halfZ),
                    new Vector3(halfX, -halfY, halfZ)
                };

            var normals = new[]
                {
                    new Vector3(0f, 0f, -1f),
                    new Vector3(0f, 0f, -1f),
                    new Vector3(0f, 0f, -1f),
                    new Vector3(0f, -1f, 0f),
                    new Vector3(0f, -1f, 0f),
                    new Vector3(0f, -1f, 0f),
                    new Vector3(0f, -1f, 0f),
                    new Vector3(-1f, 0f, 0f),
                    new Vector3(-1f, 0f, 0f),
                    new Vector3(-1f, 0f, 0f),
                    new Vector3(1f, 1f, 0f),
                    new Vector3(1f, 1f, 0f),
                    new Vector3(1f, 1f, 0f),
                    new Vector3(0f, 1f, 1f),
                    new Vector3(0f, 1f, 1f),
                    new Vector3(0f, 1f, 1f)
                };

            var uv = new[]
                {
                    new Vector2(0.0004994869f, 0.9955829f),
                    new Vector2(0.9955829f, 0.0004994869f),
                    new Vector2(0.0004994869f, 0.0004994869f),
                    new Vector2(0.0004994869f, 0.0004994869f),
                    new Vector2(0.9955829f, 0.9955829f),
                    new Vector2(0.9955829f, 0.0004994869f),
                    new Vector2(0.0004994869f, 0.9955829f),
                    new Vector2(0.0004994869f, 0.0004994869f),
                    new Vector2(0.0004994869f, 0.9955829f),
                    new Vector2(0.9955829f, 0.0004994869f),
                    new Vector2(0.9955829f, 0.0004994869f),
                    new Vector2(0.0004994869f, 0.9955829f),
                    new Vector2(0.0004994869f, 0.0004994869f),
                    new Vector2(0.9955829f, 0.0004994869f),
                    new Vector2(0.9955829f, 0.9955829f),
                    new Vector2(0.0004994869f, 0.9955829f)
                };

            var triangles = new[]
                {
                    0, 1, 2,
                    3, 4, 5,
                    4, 3, 6,
                    7, 8, 9,
                    10, 11, 12,
                    13, 14, 15
                };

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.name = "CornerRampFromCode";
            return mesh;
        }

        public static Mesh CreatePlane(float width, float height, int columns, int rows)
        {
            var mesh = new Mesh();

            var halfX = width / 2;
            var halfY = height / 2;

            var vertices = new[]
                {   
                    new Vector3(-halfX, 0, -halfY),
                    new Vector3(-halfX, 0, halfY),
                    new Vector3(halfX, 0, halfY),
                    new Vector3(halfX, 0, -halfY)
                };

            var normals = new[]
                {
                    new Vector3(-halfX, 1, -halfY),
                    new Vector3(-halfX, 1, halfY),
                    new Vector3(halfX, 1, halfY),
                    new Vector3(halfX, 1, -halfY)
                };

            var uv = new[]
                {
                    new Vector2(0,0),
                    new Vector2(0,1),
                    new Vector2(1,1),
                    new Vector2(1,0)
                };

            var triangles = new[]
                {
                    0, 1, 2,
                    0, 2, 3
                };

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.name = "2PolyPlaneFromCode";
            return mesh;
        }

        public static Mesh CreateOutsideCornerCube2D()
        {
            return CreateOutsideCornerCube2D(5);
        }

        public static Mesh CreateOutsideCornerCube2D(int count)
        {
            var mesh = new Mesh();

            //var verts = new List<Vector3>();
            //var norms = new List<Vector3>();
            //var uvs = new List<Vector2>();

            //var step = 90 / count;
            //var angle = 0;
            //verts.Add(new Vector3(-0.5f, 0, -0.5f));

            //for (int i = 0; i < count+1; i++)
            //{
            //    verts.Add(new Vector3(Mathf.Sin(angle) + 0.5f, 0, Mathf.Cos(angle) + 0.5f));
            //    norms.Add(Vector3.up);
            //    uvs.Add(new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)));
            //    angle += step;
            //}

            //var tris = new List<int>();
            //for (int i = 0; i < count ; i++)
            //{
            //    tris.AddRange(new[] { 0, i, i + 1 });
            //}
            //   mesh.vertices = verts.ToArray();
            //mesh.normals = norms.ToArray();
            //mesh.uv = uvs.ToArray();
            //mesh.triangles = tris.ToArray();

            var vertices = new[]
            {
                new Vector3(-0.5f,   0,  0.5f),
                new Vector3(0.5f,    0,  0.5f),
                new Vector3(0.451f,  0,  0.191f),
                new Vector3(0.309f,  0,  -0.088f),
                new Vector3(0.088f,  0,  -0.309f),
                new Vector3(-0.191f, 0,   -0.451f),
                new Vector3(-0.5f,   0,  -0.5f)
            };

            var normals = new[]
            {
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f),
                new Vector3(0f, 1f, 0f)
            };

            var uv = new[]
            {
                new Vector2(0.0004995465f, 0.9995002f),
                new Vector2(0.9995005f, 0.9995005f),
                new Vector2(0.9506058f, 0.6907918f),
                new Vector2(0.8087081f, 0.4123021f),
                new Vector2(0.5876972f, 0.1912914f),
                new Vector2(0.3092072f, 0.04939392f),
                new Vector2(0.0004997849f, 0.0004995465f)
            };

            var triangles = new[]
            { 
                0, 5, 6, 
                0, 4, 5,
                0, 3, 4,
                0, 2, 3,
                0, 1, 2
            };

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.name = "OutsideCorner2DFromCode";

            return mesh;
        }

        public static Mesh CreateOutsideCornerCube()
        {
            var mesh = new Mesh();


            var vertices = new[]
            {
                new Vector3(0.308f, -0.088f, -0.5f),
                new Vector3(0.087f, -0.309f, -0.5f),
                new Vector3(-0.191f, -0.451f, -0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.45f, 0.19f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(0.308f, -0.088f, 0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(-0.191f, -0.451f, 0.5f),
                new Vector3(0.45f, 0.19f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.087f, -0.309f, 0.5f),
                new Vector3(0.45f, 0.19f, 0.5f),
                new Vector3(0.45f, 0.19f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.308f, -0.088f, 0.5f),
                new Vector3(0.308f, -0.088f, -0.5f),
                new Vector3(0.087f, -0.309f, 0.5f),
                new Vector3(0.087f, -0.309f, -0.5f),
                new Vector3(-0.191f, -0.451f, 0.5f),
                new Vector3(-0.191f, -0.451f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f)
            };

            var normals = new[]
            {
                new Vector3(-0.016f, -0.016f, -1f),
                new Vector3(-0.016f, -0.016f, -1f),
                new Vector3(-0.016f, -0.016f, -1f),
                new Vector3(-0.016f, -0.016f, -1f),
                new Vector3(-0.016f, -0.016f, -1f),
                new Vector3(-0.016f, -0.016f, -1f),
                new Vector3(-0.016f, -0.016f, -1f),
                new Vector3(-1f, -0.016f, 0f),
                new Vector3(-1f, -0.016f, 0f),
                new Vector3(-1f, -0.016f, 0f),
                new Vector3(-1f, -0.016f, 0f),
                new Vector3(-0.016f, 1f, 0f),
                new Vector3(-0.016f, 1f, 0f),
                new Vector3(-0.016f, 1f, 0f),
                new Vector3(-0.016f, 1f, 0f),
                new Vector3(-0.016f, -0.016f, 1f),
                new Vector3(-0.016f, -0.016f, 1f),
                new Vector3(-0.016f, -0.016f, 1f),
                new Vector3(-0.016f, -0.016f, 1f),
                new Vector3(-0.016f, -0.016f, 1f),
                new Vector3(-0.016f, -0.016f, 1f),
                new Vector3(-0.016f, -0.016f, 1f),
                new Vector3(0.937f, -0.333f, -0.109f),
                new Vector3(0.937f, -0.333f, -0.109f),
                new Vector3(0.968f, -0.175f, -0.179f),
                new Vector3(0.968f, -0.175f, -0.179f),
                new Vector3(0.778f, -0.619f, -0.109f),
                new Vector3(0.778f, -0.619f, -0.109f),
                new Vector3(0.587f, -0.809f, 0f),
                new Vector3(0.587f, -0.809f, 0f),
                new Vector3(0.297f, -0.955f, 0f),
                new Vector3(0.297f, -0.955f, 0f),
                new Vector3(0.141f, -0.99f, 0f),
                new Vector3(0.141f, -0.99f, 0f)
            };

            var uv = new[]
            {
                new Vector2(0.8078431f, 0.4117647f),
                new Vector2(0.5843137f, 0.1882352f),
                new Vector2(0.3058823f, 0.04705876f),
                new Vector2(-5.960464E-08f, -5.960464E-08f),
                new Vector2(0.9490196f, 0.690196f),
                new Vector2(-5.960464E-08f, 0.9960784f),
                new Vector2(0.9960784f, 0.9960784f),
                new Vector2(-5.960464E-08f, -5.960464E-08f),
                new Vector2(-5.960464E-08f, 0.9960784f),
                new Vector2(0.9960784f, 0.9960784f),
                new Vector2(0.9960784f, -5.960464E-08f),
                new Vector2(0.9960784f, 0.9960784f),
                new Vector2(0.9960784f, -5.960464E-08f),
                new Vector2(-5.960464E-08f, -5.960464E-08f),
                new Vector2(-5.960464E-08f, 0.9960784f),
                new Vector2(0.1882352f, 0.4117647f),
                new Vector2(0.9960784f, -5.960464E-08f),
                new Vector2(0.690196f, 0.04705876f),
                new Vector2(0.04705876f, 0.690196f),
                new Vector2(0.9960784f, 0.9960784f),
                new Vector2(-5.960464E-08f, 0.9960784f),
                new Vector2(0.4117647f, 0.1882352f),
                new Vector2(0.1764705f, 0.9960784f),
                new Vector2(0.1764705f, -5.960464E-08f),
                new Vector2(-5.960464E-08f, -5.960464E-08f),
                new Vector2(-5.960464E-08f, 0.9960784f),
                new Vector2(0.3882352f, 0.9960784f),
                new Vector2(0.3882352f, -5.960464E-08f),
                new Vector2(0.6078431f, 0.9960784f),
                new Vector2(0.6078431f, -5.960464E-08f),
                new Vector2(0.8196078f, 0.9960784f),
                new Vector2(0.8196078f, -5.960464E-08f),
                new Vector2(0.9960784f, 0.9999999f),
                new Vector2(0.9999999f, -5.960464E-08f)
            };

            var triangles = new[]
            {
                0, 1, 2,
                2, 3, 0,
                3, 4, 0,
                3, 5, 4,
                5, 6, 4,
                7, 8, 9,
                9, 10, 7,
                11, 12, 13,
                13, 14, 11,
                15, 16, 17,
                15, 18, 16,
                18, 19, 16,
                18, 20, 19,
                17, 21, 15,
                22, 23, 24,
                24, 25, 22,
                23, 22, 26,
                26, 27, 23,
                27, 26, 28,
                28, 29, 27,
                29, 28, 30,
                30, 31, 29,
                31, 30, 32,
                32, 33, 31
            };

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.name = "OutsideCornerFromCode";

            return mesh;
        }

        public static Mesh CreatePyramid()
        {

            var mesh = new Mesh();

            var vertices = new[]
            {
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0f, 0.5f, 0f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(0f, 0.5f, 0f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0f, 0.5f, 0f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0f, 0.5f, 0f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(0f, -0.5f, 0f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f)
            };

            var normals = new[]
            {
                new Vector3(-0.008f, 0.443f, 0.896f),
                new Vector3(-0.008f, 0.443f, 0.896f),
                new Vector3(-0.008f, 0.443f, 0.896f),
                new Vector3(-0.899f, 0.438f, 0f),
                new Vector3(-0.899f, 0.438f, 0f),
                new Vector3(-0.899f, 0.438f, 0f),
                new Vector3(-0.008f, 0.443f, -0.896f),
                new Vector3(-0.008f, 0.443f, -0.896f),
                new Vector3(-0.008f, 0.443f, -0.896f),
                new Vector3(0.894f, 0.443f, -0.058f),
                new Vector3(0.894f, 0.443f, -0.058f),
                new Vector3(0.894f, 0.443f, -0.058f),
                new Vector3(-0.008f, -1f, 0f),
                new Vector3(-0.008f, -1f, 0f),
                new Vector3(-0.008f, -1f, 0f),
                new Vector3(-0.008f, -1f, 0f),
                new Vector3(-0.008f, -1f, 0f)
            };

            var uv = new[]
            {
                new Vector2(0.9955829f, 0.0004994869f),
                new Vector2(0.0004994869f, 0.0004994869f),
                new Vector2(0.4980412f, 0.9955829f),
                new Vector2(0.9955829f, 0.0004994869f),
                new Vector2(0.0004994869f, 0.0004994869f),
                new Vector2(0.4980412f, 0.9955829f),
                new Vector2(0.9955829f, 0.0004994869f),
                new Vector2(0.0004994869f, 0.0004994869f),
                new Vector2(0.4980412f, 0.9955829f),
                new Vector2(0.9955829f, 0.0004994869f),
                new Vector2(0.0004994869f, 0.0004994869f),
                new Vector2(0.4980412f, 0.9955829f),
                new Vector2(0.0004994869f, 0.0004994869f),
                new Vector2(0.4980412f, 0.4980412f),
                new Vector2(0.9955829f, 0.0004994869f),
                new Vector2(0.0004994869f, 0.9955829f),
                new Vector2(0.9955829f, 0.9955829f)
            };

            var triangles = new[]
            {
                0, 1, 2,
                3, 4, 5,
                6, 7, 8,
                9, 10, 11,
                12, 13, 14,
                15, 13, 12,
                16, 13, 15,
                14, 13, 16
            };

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.name = "PyramidFromCode";
            return mesh;
        }
    }
}