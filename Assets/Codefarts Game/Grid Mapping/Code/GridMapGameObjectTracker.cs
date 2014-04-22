// <copyright>
//   Copyright (c) 2012 Codefarts
//   All rights reserved.
//   contact@codefarts.com
//   http://www.codefarts.com
// </copyright>

namespace Codefarts.GridMapping
{
    using System;

    using UnityEngine;

    public class GridMapGameObjectTracker
    {
        /// <summary>
        /// Holds a reference to a <see cref="GridMap"/> object.
        /// </summary>
        private readonly GridMap map;

        /// <summary>
        /// Holds an array of <see cref="GameObject"/>
        /// </summary>
        private Array3D<GameObject> objects;

        private bool processing;

        private bool cancel;

        private float progress;


        public GridMapGameObjectTracker(GridMap map, bool scan, bool background)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }

            this.map = map;
            if (scan)
            {
                this.Scan(background);
            }
        }

        public void Set(int column, int row, int layer, GameObject value)
        {
            if (column < 0 || column > this.objects.Width - 1 || row < 0 || row > this.objects.Height - 1 || layer < 0 || layer > this.objects.Depth - 1)
            {
                return;
            }

            this.objects.Set(column, row, layer, value);
        }

        public GameObject Get(int column, int row, int layer)
        {
            if (column < 0 || column > this.objects.Width - 1 || row < 0 || row > this.objects.Height - 1 || layer < 0 || layer > this.objects.Depth - 1)
            {
                return null;
            }

            return this.objects.Get(column, row, layer);
        }

        public void Scan(bool background)
        {
            if (this.processing)
            {
                return;
            }

            if (background)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(cb => this.DoScan(0));
            }
            else
            {
                this.DoScan(0);
            }
        }

        public void Cancel()
        {
            this.cancel = true;
        }

        public float GetProgress()
        {
            return this.progress;
        }

        private void DoScan(int sleep)
        {
            this.cancel = false;
            this.processing = true;
            this.objects = new Array3D<GameObject>(this.map.Columns, this.map.Rows, Math.Max(this.map.Layers.Length, 1));
            var transform = this.map.transform;
            for (var i = 0; i < transform.childCount; i++)
            {
                // process child
                var child = transform.GetChild(i);
                string prefabName;
                int layer;
                int column;
                int row;
                if (child.name.TryParsePrefabName(out prefabName, out layer, out column, out row))
                {
                    this.Set(column, row, layer, child.gameObject);
                }

                // check if canceled
                if (this.cancel)
                {
                    break;
                }

                // update progress
                this.progress = (float)i / transform.childCount;

                // check if threat should sleep between each entry
                if (sleep > 0)
                {
                    System.Threading.Thread.Sleep(sleep);
                }
            }

            this.cancel = false;
            this.processing = false;
        }
    }
}
