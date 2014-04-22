/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.Map2D.Editor.Settings
{
    using Codefarts.GridMapping.Editor;
    using Codefarts.Localization;

    /// <summary>
    /// Provides a menu for general grid mapping settings.
    /// </summary>
    public class Map2DGeneralSettingsMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Map2DGeneralSettingsMenuItem"/> class.
        /// </summary>
        public static void Draw()
        {
            var local = LocalizationManager.Instance;

            SettingHelpers.DrawSettingsCheckBox(GlobalConstants.Map2DShowCheckerboardBackground, local.Get("SETT_Map2DShowCheckerboardBackground"), false);
        }
    }
}