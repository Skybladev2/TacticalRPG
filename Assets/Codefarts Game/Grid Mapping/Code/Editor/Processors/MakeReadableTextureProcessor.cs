/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.GridMapping.Editor.Processors
{
    using Codefarts.GridMapping.Editor;

    using UnityEditor;

    /// <summary>
    /// Provides a processor for making a texture readable when it is imported into unity.
    /// </summary>
    public class MakeReadableTextureProcessor : AssetPostprocessor
    {
        /// <summary>
        /// Called by Unity when preprocessing occurs.
        /// </summary>
        public void OnPreprocessTexture()
        {
            var assetPathFilename = System.IO.Path.GetFileNameWithoutExtension(this.assetPath);
            if (assetPathFilename != null && !assetPathFilename.EndsWith(GlobalConstants.ReadableTextureString))
            {
                return;
            }

            var textureImporter = this.assetImporter as TextureImporter;
            if (textureImporter == null)
            {
                return;
            }

            // make readable
            textureImporter.isReadable = true;

            // textureImporter.mipmapEnabled = false;
            // textureImporter.textureType = TextureImporterType.Image;
            // textureImporter.npotScale = TextureImporterNPOTScale.None;
        }
    }
}