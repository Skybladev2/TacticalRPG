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
    /// Provides a menu for general grid map settings.
    /// </summary>
    public class GridMappingDrawingRulesSettingsMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridMappingDrawingRulesSettingsMenuItem"/> class.
        /// </summary>
        public static void Draw()
        {
            var local = LocalizationManager.Instance;

            SettingHelpers.DrawSettingsIntField(GlobalConstants.ShowDrawingRulesButtonSizeKey, local.Get("SETT_ShowDrawRulesSize"), 22, 1, 100, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.DrawingRulesEntrySizeKey, local.Get("SETT_DrawingRuleEntrySize"), 22, 1, 100, Helpers.RedrawInspector);
            SettingHelpers.DrawSettingsBoolField(GlobalConstants.DrawingRulesOutputToConsoleKey, local.Get("SETT_DrawingRuleConsoleOutput"), false);
            SettingHelpers.DrawSettingsBoolField(GlobalConstants.DrawingRulesConfirmationKey, local.Get("SETT_DrawingRulesConfirmation"), true);   
        }
    }
}