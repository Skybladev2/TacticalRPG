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
    using Codefarts.Localization;

    /// <summary>
    /// Provides a menu for general grid mapping settings.
    /// </summary>
    public class GridMappingSceneViewSettingsMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridMappingSceneViewSettingsMenuItem"/> class.
        /// </summary>
        public static void Draw()
        {
            var local = LocalizationManager.Instance;

            var items = new[] 
            { 
                local.Get("SETT_DontShowMouseCoordinates"), 
                local.Get("SETT_ShowMouseCoordinatesUnderPointer"), 
                local.Get("SETT_ShowMouseCoordinatesAbovePointer"),
                local.Get("SETT_ShowMouseCoordinatesTopLeft"),
                local.Get("SETT_ShowMouseCoordinatesTopRight"),
                local.Get("SETT_ShowMouseCoordinatesBottomLeft"),
                local.Get("SETT_ShowMouseCoordinatesBottomRight")
            };

            SettingHelpers.DrawSettingsPopup(GlobalConstants.MouseCoordinatesKey, local.Get("SETT_MouseCoordinatesStyle"), items, 0, Helpers.RedrawInspector);

            // SettingHelpers.DrawSettingsBoolField(GlobalConstants.MouseCoordinatesActiveSceneViewOnlyKey, local.Get("SETT_MouseCoordinatesStyle"), items, 0, Helpers.RedrawInspector);
        }
    }
}