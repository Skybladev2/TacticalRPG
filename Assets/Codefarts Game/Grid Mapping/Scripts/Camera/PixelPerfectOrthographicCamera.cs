// <copyright>
//   Copyright (c) 2012 Codefarts
//   All rights reserved.
//   contact@codefarts.com
//   http://www.codefarts.com
// </copyright>

namespace Codefarts.GridMapping.Scripts.Camera
{
    using UnityEngine;

    /// <summary>
    ///     Provides a MonoBehavior for changing the orthographic size of a camera.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]   
    public class PixelPerfectOrthographicCamera : MonoBehaviour
    {
        /// <summary>
        ///  Stores the pixels per unit value. 
        /// </summary>
        [SerializeField]
        private float pixelsPerUnit = 100;

        /// <summary>
        /// Stores the zoom value.
        /// </summary>
        [SerializeField]
        private float zoom = 1;

        #region Public Properties

        /// <summary>
        ///     Gets or sets the pixels per unit for the camera.
        /// </summary>
        public float PixelsPerUnit
        {
            get
            {
                return this.pixelsPerUnit;
            }

            set
            {
                this.pixelsPerUnit = value;
            }
        }

        /// <summary>
        ///     Gets or sets the camera zoom level.
        /// </summary>
        public float Zoom
        {
            get
            {
                return this.zoom;
            }

            set
            {
                this.zoom = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        public void Update()
        {
            this.camera.orthographicSize = Screen.height / 2f / this.PixelsPerUnit / this.Zoom;
        }

        #endregion
    }
}