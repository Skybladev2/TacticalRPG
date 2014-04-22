/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Scripts.Character
{
    using System;

    using Codefarts.GridMapping.Common;

    using UnityEngine;

    using Vector3 = UnityEngine.Vector3;

    /// <summary>
    /// Provides a simple 2D axis movement controller.
    /// </summary>
    public class CharacterMovement2D : MonoBehaviour
    {
        /// <summary>
        /// Defines the movement speed.
        /// </summary>
        public float MovementSpeed;

        /// <summary>
        /// Refers to the target transform that will be moved.
        /// </summary>
        public Transform MoveTarget;

        /// <summary>
        /// Holds a reference to a CharacterController reference.
        /// </summary>
        private CharacterController controller;

        /// <summary>
        /// Defines the axis that movement will occur along.
        /// </summary>
        public CommonAxis Axis;


        /// <summary>
        /// Start method called by unity.
        /// </summary>
        void Start()
        {
            // attempt to get the CharacterController reference
            this.controller = this.MoveTarget.gameObject.GetComponent<CharacterController>();
        }

        /// <summary>
        /// Update method called by unity.
        /// </summary>
       private void Update()
        {
            // if no character controller just exit
            if (this.controller == null)
            {
                return;
            }

            // calculate the move vector
            Vector3 moveDirection;
            switch (this.Axis)
            {
                case CommonAxis.XY:
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
                    break;
                case CommonAxis.XZ:
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    break;
                case CommonAxis.ZY:
                    moveDirection = new Vector3(0, Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            moveDirection = this.MoveTarget.TransformDirection(moveDirection) * this.MovementSpeed;
           
            // move the controller
            this.controller.Move(moveDirection * Time.deltaTime);
        }
    }
}