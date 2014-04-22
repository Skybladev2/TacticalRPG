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

    using Codefarts.CoreProjectCode.Settings;

    using UnityEditor;

    /// <summary>
    ///     Provides a editor window for quickly selecting rotation values.
    /// </summary>
    public class RotationsWindow : EditorWindow
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public RotationsWindow()
        {
            // hook into settings and repaint the window if the desired setting changes
            SettingsManager settings = SettingsManager.Instance;
            settings.ValueChanged += (s, e) =>
                {
                    if (e.Name == GlobalConstants.RotationWindowOrientationKey)
                    {
                        this.Repaint();
                    }
                };
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     Provides a closed event when the window is closed.
        /// </summary>
        public event EventHandler Closed;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a reference to the associated <see cref="GridMapEditor" />.
        /// </summary>
        public GridMapEditor Editor { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Called by unity when the window is closed.
        /// </summary>
        public void OnDestroy()
        {
            if (this.Closed != null)
            {
                this.Closed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Called by unity to draw the user interface.
        /// </summary>
        public void OnGUI()
        {
            // if nothing selected just exit
            if (Selection.activeGameObject == null || this.Editor == null)
            {
                return;
            }

            // get orientation setting
            SettingsManager settings = SettingsManager.Instance;
            bool displayVertically = settings.GetSetting(GlobalConstants.RotationWindowOrientationKey, true);

            // if not oriented vertically use horizontal
            if (!displayVertically)
            {
                EditorGUILayout.BeginHorizontal();
            }

            // draw grids
            this.Editor.DrawRotationButtonGrid(0);
            this.Editor.DrawRotationButtonGrid(1);
            this.Editor.DrawRotationButtonGrid(2);

            if (!displayVertically)
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        ///     Invoked by unity.
        /// </summary>
        public void OnSelectionChange()
        {
            this.Repaint();
        }

        #endregion
    }
}