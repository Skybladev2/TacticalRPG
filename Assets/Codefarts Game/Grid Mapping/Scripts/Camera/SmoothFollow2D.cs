/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Scripts.Camera
{
    using System;

    using Codefarts.GridMapping.Common;

    using UnityEngine;

    using Vector2 = UnityEngine.Vector2;
    using Vector3 = UnityEngine.Vector3;

    /// <summary>
    /// Provides a MonoBehavior for one object to follow another object along a specified 2D axis.
    /// </summary>
    public class SmoothFollow2D : MonoBehaviour
    {
        /// <summary>
        /// The target to be followed.
        /// </summary>
        public Transform Target;

        /// <summary>
        /// The lag time it takes for the follower to come to move.
        /// </summary>
        public float smoothTime = 0.3f;

        /// <summary>
        /// The distance from the target.
        /// </summary>
        public float distance = 10;

        /// <summary>
        /// Holds a reference to the transform.
        /// </summary>
        private Transform transformReference;

        /// <summary>
        /// Stores the movement velocity.
        /// </summary>
        private Vector2 velocity;  
        
        /// <summary>
        /// Defines the axis that following will occur on.
        /// </summary>
        public CommonAxis axis;

        /// <summary>
        /// Called on game start.
        /// </summary>
        private void Start()
        {
            this.transformReference = this.transform;
        }

        /// <summary>
        /// Updates the following targets position.
        /// </summary>
        private void Update()
        {
            // determine what axis to follow on
            switch (this.axis)
            {
                case CommonAxis.XY:
                    this.transformReference.position = new Vector3(
                               Mathf.SmoothDamp(this.transformReference.position.x, this.Target.position.x, ref this.velocity.x, this.smoothTime),
                               Mathf.SmoothDamp(this.transformReference.position.y, this.Target.position.y, ref this.velocity.y, this.smoothTime),
                               this.Target.position.z - this.distance);
                    break;
             
                case CommonAxis.XZ:
                    this.transformReference.position = new Vector3(
                            Mathf.SmoothDamp(this.transformReference.position.x, this.Target.position.x, ref this.velocity.x, this.smoothTime),
                            this.Target.position.y - this.distance,
                            Mathf.SmoothDamp(this.transformReference.position.z, this.Target.position.z, ref this.velocity.y, this.smoothTime));
                    break;
             
                case CommonAxis.ZY:
                    this.transformReference.position = new Vector3(
                              Mathf.SmoothDamp(this.transformReference.position.z, this.Target.position.z, ref this.velocity.x, this.smoothTime),
                              Mathf.SmoothDamp(this.transformReference.position.y, this.Target.position.y, ref this.velocity.y, this.smoothTime),
                              this.Target.position.x - this.distance);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}