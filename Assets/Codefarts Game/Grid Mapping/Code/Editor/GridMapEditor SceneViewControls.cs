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
    using Codefarts.CoreProjectCode.Settings;

    using UnityEngine;
    using UnityEditor;

     /// <summary>
    /// Provides a editor for the <see cref="GridMap"/> component.
    /// </summary>
    public partial class GridMapEditor
    {
        /// <summary>
        /// Draws in scene controls.
        /// </summary>
        private void DrawInSceneControls()
        {
            var settings = SettingsManager.Instance;

            var sceneViewRect = SceneView.currentDrawingSceneView.position;
            var current = Event.current;

            var mousePositionStyle = settings.GetSetting(GlobalConstants.MouseCoordinatesKey, 0);
            var mousePositionContent = new GUIContent();
            if (mousePositionStyle != 0 && current.type == EventType.MouseMove)
            {
                var gridPosition = this.GetGridPosition(this.mouseHitPosition);
                mousePositionContent.text = string.Format("{0}x{1}", gridPosition.X, gridPosition.Y);
                var mouseContentSize = GUI.skin.label.CalcSize(mousePositionContent);

                var rect = new Rect();
                var mousePosition = current.mousePosition;
                switch (mousePositionStyle)
                {
                    case 1: // under mouse
                        rect = new Rect(mousePosition.x - (mouseContentSize.x / 2), mousePosition.y + 15, mouseContentSize.x, mouseContentSize.y);
                        break;

                    case 2:    // above mouse
                        rect = new Rect(mousePosition.x + (mouseContentSize.x / 2), mousePosition.y - 15, mouseContentSize.x, mouseContentSize.y);
                        break;

                    case 3:    // top left of scene view
                        rect = new Rect(0, 0, mouseContentSize.x, mouseContentSize.y);
                        break;

                    case 4:   // top right of scene view
                        rect = new Rect(sceneViewRect.width - mouseContentSize.x, 0, mouseContentSize.x, mouseContentSize.y);
                        break;

                    case 5:    // bottom left of scene view
                        rect = new Rect(0, sceneViewRect.height - mouseContentSize.y, mouseContentSize.x, mouseContentSize.y);
                        break;

                    case 6:    // bottom right of scene view
                        rect = new Rect(sceneViewRect.width - mouseContentSize.x, sceneViewRect.height - mouseContentSize.y, mouseContentSize.x, mouseContentSize.y);
                        break;
                }

                Handles.BeginGUI();
                GUI.Label(rect, mousePositionContent);
                Handles.EndGUI();
            }    
        }
    }
}