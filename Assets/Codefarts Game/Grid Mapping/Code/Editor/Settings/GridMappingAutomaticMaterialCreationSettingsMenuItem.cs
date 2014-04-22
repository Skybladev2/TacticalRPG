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
    using Codefarts.GridMapping.Editor.Controls;
    using Codefarts.Localization;

    using UnityEngine;

    /// <summary>
    /// Provides a menu for general grid map settings.
    /// </summary>
    public class GridMappingAutomaticMaterialCreationSettingsMenuItem
    {
        /// <summary>
        /// Holds a reference to a <see cref="SelectOutputFolderControl"/> type used by the <see cref="Draw"/> method.
        /// </summary>
        private static SelectOutputFolderControl folderControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridMappingAutomaticMaterialCreationSettingsMenuItem"/> class.
        /// </summary>
        public static void Draw()                                 
        {
            if (folderControl == null)
            {
                folderControl = new SelectOutputFolderControl();
                folderControl.SetOutputPath(PlayerPrefs.GetString(GlobalConstants.AutoMaterialCreationFolderKey, string.Empty));
                folderControl.OutputPathChanged += (sender, e) => PlayerPrefs.SetString(GlobalConstants.AutoMaterialCreationFolderKey, folderControl.OutputPath);
            }

            folderControl.Draw();

            var local = LocalizationManager.Instance;

            SettingHelpers.DrawSettingsColorPicker(GlobalConstants.DefaultAutoMaterialCreationColorKey, local.Get("SETT_DefaultTileMaterialCreationColor"), Color.white);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.AutoMaterialCreationDefaultWidthKey, local.Get("SETT_TileMaterialCreationDefaultTileWidth"), 32);
            SettingHelpers.DrawSettingsIntField(GlobalConstants.AutoMaterialCreationDefaultHeightKey, local.Get("SETT_TileMaterialCreationDefaultTileHeight"), 32);
            var items = new[] { local.Get("Hierarchy"), local.Get("Flat") };
            SettingHelpers.DrawSettingsPopup(GlobalConstants.AutoMaterialCreationShaderListStyleKey, local.Get("SETT_HierarchicalMaterialCreationShaderList"), items, 0);
            GUILayout.Label(local.Get("Shaders"));
            SettingHelpers.DrawSettingsTextBox(GlobalConstants.AutoMaterialCreationShadersKey, "Diffuse\r\nTransparent/Diffuse\r\nTransparent/Cutout/Diffuse\r\nTransparent/Cutout/Soft Edge Unlit");
        }
    }
}