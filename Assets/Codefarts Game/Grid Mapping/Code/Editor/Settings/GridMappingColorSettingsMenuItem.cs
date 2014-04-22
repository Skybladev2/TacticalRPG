/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Editor.Settings
{
    using System;

    using Codefarts.CoreProjectCode.Settings;
    using Codefarts.GridMapping.Editor;
    using Codefarts.Localization;

    using UnityEditor;

    using Color = Codefarts.GridMapping.Common.Color;

    /// <summary>
    /// Provides a menu for grid mapping color settings.
    /// </summary>
    public class GridMappingColorSettingsMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridMappingColorSettingsMenuItem"/> class.
        /// </summary>
        public static void Draw()
        {
            var local = LocalizationManager.Instance;

            var drawControl = new Action<string, string, Color>((text, key, defaultColor) =>
                {
                    var settings = SettingsManager.Instance;
                    var colorValue = defaultColor;

                    var value = (Color)EditorGUILayout.ColorField(text, settings.GetColorSetting(key, defaultColor));
                    if (value != colorValue)
                    {
                        settings.SetColorSetting(key, value);
                        Helpers.RedrawInspector();
                    }
                });

            drawControl(local.Get("SETT_BorderColor"), GlobalConstants.BorderColorKey, Color.White);
            drawControl(local.Get("SETT_GridColor"), GlobalConstants.GridColorKey, Color.Gray);
            drawControl(local.Get("SETT_GuidelineColor"), GlobalConstants.GuidelineColorKey, Color.Yellow);
            drawControl(local.Get("SETT_MarkerColor"), GlobalConstants.MarkerColorKey, Color.Red);
        }    
    }
}