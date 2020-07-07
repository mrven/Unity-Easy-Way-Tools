using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ConvertNormal : Editor
{
    [MenuItem("Assets/Easy Way Tools/Convert Normal Map")]
    private static void ConvertNormalMap()
    {
        var selected = Selection.objects;

        foreach (Object texture in selected)
        {
            if (texture.GetType() == typeof(Texture2D))
            {


                string sourcePath = AssetDatabase.GetAssetPath(texture);

                TextureImporter sourceImporter = (TextureImporter)AssetImporter.GetAtPath(sourcePath);

                //Check Texture is Normal Map
                if (sourceImporter.textureType == TextureImporterType.NormalMap)
                {
                    //Get Raw Copy Of Texture
                    string rawPath = "Assets/raw_" + Path.GetFileName(sourcePath);

                    AssetDatabase.CopyAsset(sourcePath, rawPath);
                    AssetDatabase.ImportAsset(rawPath);

                    TextureImporter rawImporter = (TextureImporter)AssetImporter.GetAtPath(rawPath);
                    rawImporter.textureType = TextureImporterType.Default;
                    rawImporter.sRGBTexture = false;
                    rawImporter.mipmapEnabled = false;
                    rawImporter.isReadable = true;
                    rawImporter.npotScale = TextureImporterNPOTScale.None;
                    rawImporter.wrapMode = TextureWrapMode.Clamp;
                    rawImporter.maxTextureSize = 8192;
                    rawImporter.textureCompression = TextureImporterCompression.Uncompressed;
                    rawImporter.SaveAndReimport();

                    //Convert DirectX Normal to OpenGL Normal
                    Texture2D source = (Texture2D)AssetDatabase.LoadAssetAtPath(rawPath, typeof(Texture2D));
                    Texture2D converted = new Texture2D(source.width, source.height, TextureFormat.RGBAFloat, true, true);
                    Material convertMaterial = new Material(Shader.Find("Hidden/ConvertNormalShader"));

                    convertMaterial.SetTexture("_Source", source);

                    RenderTexture convertedRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

                    Graphics.Blit(source, convertedRT, convertMaterial);

                    converted.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0, false);
                    converted.Apply();
                    
                    //Save Converted Normal
                    byte[] textureBytes = converted.EncodeToPNG();

                    string convertedLocalPath = sourcePath.Remove(sourcePath.Length - texture.name.Length - 4, texture.name.Length + 4) + "/" + texture.name + "_OGL.png";
                    string convertedFullPath = Application.dataPath + "/" + convertedLocalPath.Remove(0, 7);
                    System.IO.File.WriteAllBytes(convertedFullPath, textureBytes);
                    AssetDatabase.ImportAsset(convertedLocalPath);

                    TextureImporter convertedTextureImporter = (TextureImporter)AssetImporter.GetAtPath(convertedLocalPath);
                    convertedTextureImporter.textureType = TextureImporterType.NormalMap;
                    convertedTextureImporter.mipmapEnabled = sourceImporter.mipmapEnabled;
                    convertedTextureImporter.isReadable = sourceImporter.isReadable;
                    convertedTextureImporter.npotScale = sourceImporter.npotScale;
                    convertedTextureImporter.wrapMode = sourceImporter.wrapMode;
                    convertedTextureImporter.maxTextureSize = sourceImporter.maxTextureSize;
                    convertedTextureImporter.textureCompression = sourceImporter.textureCompression;
                    convertedTextureImporter.anisoLevel = sourceImporter.anisoLevel;
                    convertedTextureImporter.normalmapFilter = sourceImporter.normalmapFilter;
                    convertedTextureImporter.SaveAndReimport();

                    //Delete Raw Texture, RT and Material
                    AssetDatabase.DeleteAsset(rawPath);
                    Object.DestroyImmediate(converted);
                    Object.DestroyImmediate(convertMaterial);
                }

            }
        }
    }
}
