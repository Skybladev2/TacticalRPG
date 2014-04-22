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

    using UnityEngine;

    /// <summary>
    /// Provides a self terminating <see cref="MonoBehaviour"/> that can terminate after a delay and invoke a callback.
    /// </summary>
    [ExecuteInEditMode]
    public class SelfTerminatingObject : MonoBehaviour
    {
        /// <summary>
        /// Stores the last time used when determining when the countdown delay had started.
        /// </summary>
        private DateTime lastTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfTerminatingObject"/> class.
        /// </summary>
        public SelfTerminatingObject()
        {
            this.lastTime = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the callback to be called upon termination.
        /// </summary>
        public Action Callback { get; set; }

        /// <summary>
        /// Gets or sets the delay time before terminating.
        /// </summary>
        public float Delay { get; set; }

        /// <summary>
        /// Creates a update callback that self terminates.
        /// </summary>
        /// <param name="callback">A reference to a callback to be called when the object terminates.</param>
        /// <returns>Returns a reference to a <see cref="GameObject"/> that will self terminate.</returns>
        public static GameObject CreateUpdateCallback(Action callback)
        {
            return CreateUpdateCallback(callback, 0);
        }

        /// <summary>
        /// Creates a update callback that self terminates.
        /// </summary>
        /// <param name="callback">A reference to a callback to be called when the object terminates.</param>
        /// <param name="delay">The time to wait before self terminating.</param>
        /// <returns>Returns a reference to a <see cref="GameObject"/> that will self terminate.</returns>
        public static GameObject CreateUpdateCallback(Action callback, float delay)
        {
            var obj = new GameObject();
            var com = obj.AddComponent<SelfTerminatingObject>();
            com.lastTime = DateTime.Now;
            com.Delay = delay;
            com.Callback = callback;

            return obj;
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        public void OnGui()
        {
            this.PerformCheck();
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        public void Update()
        {
            this.PerformCheck();
        }

        /// <summary>
        /// Performs a check t determine weather or not it is time to terminate the object yet.
        /// </summary>
        private void PerformCheck()
        {
            // check of the delay time has passed
            if (DateTime.Now > this.lastTime + TimeSpan.FromSeconds(this.Delay))
            {
                // check if there is a callback specified and if so call it
                if (this.Callback != null)
                {
                    this.Callback();
                }

#if UNITY_EDITOR
                DestroyImmediate(this.gameObject);
#else
                this.Destroy(this.gameObject);
#endif
            }
        }
    }
}
