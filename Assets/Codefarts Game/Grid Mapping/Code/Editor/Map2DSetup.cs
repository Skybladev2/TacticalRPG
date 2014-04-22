/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.Map2D.Editor
{
    using Codefarts.CoreProjectCode;
    using Codefarts.CoreProjectCode.Services;
    using Codefarts.GridMapping.Editor.Settings;
    using Codefarts.Localization;
    using Codefarts.Map2D.Editor.Settings;

    using UnityEditor;

    /// <summary>
    /// Provides settings that integrate it the unity preferences window.
    /// </summary>
    [InitializeOnLoad]
    public class Map2DSetup
    {
        /// <summary>
        /// Initializes static members of the <see cref="Map2DSetup"/> class.
        /// </summary>
        static Map2DSetup()
        {
            EditorCallbackService.Instance.Register(() =>
                {
                    var local = LocalizationManager.Instance;
                    var config = UnityPreferencesManager.Instance;

                    config.Register(local.Get("SETT_Map2DGeneral"), Map2DGeneralSettingsMenuItem.Draw);
                    config.Register(local.Get("SETT_Map2DDrawingTools"), Map2DDrawingToolsSettingsMenuItem.Draw);
                });
        }     
    }
}