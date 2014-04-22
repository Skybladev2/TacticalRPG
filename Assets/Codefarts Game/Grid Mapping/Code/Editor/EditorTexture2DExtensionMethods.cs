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
    using System.IO;

    using Codefarts.GridMapping.Utilities;

    using UnityEditor;

    using UnityEngine;

    using Object = UnityEngine.Object;

    /// <summary>
    /// Provides extension methods for the <see cref="Texture2D"/> type.
    /// </summary>
    public static class EditorTexture2DExtensionMethods
    {
        /// <summary>
        /// Clones a texture into a readable texture asset and converts it into a <see cref="GenericImage{T}"/> type. Then remove the cloned texture.
        /// </summary>
        /// <param name="sourceTexture">The non readable texture typically selected from the asset browser window that will have a
        /// temporary readable copy of it self made then promptly destroyed.</param>
        /// <returns>Returns a <see cref="GenericImage{T}"/> containing the same pixel data as the <see cref="sourceTexture"/> parameter.</returns>
        public static GenericImage<Color> CreateGenericImage(this Texture2D sourceTexture)
        {
            // if no texture provided just exit
            if (sourceTexture == null)
            {
                throw new ArgumentNullException("sourceTexture");
            }

            // get the file path to the source texture file
            var file = AssetDatabase.GetAssetPath(sourceTexture);

            // if no file returned just exit
            if (string.IsNullOrEmpty(file) || !File.Exists(file))
            {
                throw new Exception("Texture asset might have been generated dynamically.");
            }

            // get the directory that the file is in
            var filePath = Path.GetDirectoryName(file);

            // construct a temp filename for the new readable texture file
            GenericImage<Color> readableTexture = null;
            if (filePath == null)
            {
                return null;
            }

            var tempFile = Path.Combine(filePath, Path.GetFileNameWithoutExtension(file) + GlobalConstants.ReadableTextureString);
            tempFile = AssetDatabase.GenerateUniqueAssetPath(Path.ChangeExtension(tempFile, Path.GetExtension(file)));

            try
            {
                // make a copy of the file source texture file
                File.Copy(file, tempFile);

                // import the new temp asset texture
                AssetDatabase.ImportAsset(tempFile, ImportAssetOptions.Default);

                // attempt to load the new temporary texture asset
                var tempTexture = AssetDatabase.LoadAssetAtPath(tempFile, typeof(Texture2D)) as Texture2D;

                // attempt to convert the readable texture into a generic image type
                var colors = tempTexture.GetPixels32(0);
                readableTexture = new GenericImage<Color>(tempTexture.width, tempTexture.height);
                var index = 0;
                for (var y = readableTexture.Height - 1; y >= 0; y--)
                {
                    for (var x = 0; x < readableTexture.Width; x++)
                    {
                        readableTexture[x, y] = colors[index++];
                    }
                }

                // destroy the temp texture reference
                Object.DestroyImmediate(tempTexture, true);
            }
            catch
            {
            }

            try
            {   // attempt to delete temp texture asset
                if (File.Exists(tempFile))
                {
                    // try to ensure that the temp texture asset is removed
                    AssetDatabase.DeleteAsset(tempFile);
                }
            }
            catch
            {
            }

            return readableTexture;
        }
    }
}
