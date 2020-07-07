using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.IO;


public class ExtractMaterials : Editor
{
    static EWScriptableObject eWSettings;

    
    [MenuItem("Assets/Easy Way Tools/Extract and Remap Materials")]
    private static void ExtractModelsMaterials()
    {
        var selected = Selection.objects;
        var materialsFolder = "Assets";
        List<Object> modelsList = new List<Object>();

        GetEWScriptableObject();

        materialsFolder += eWSettings.materialFolderPath.Replace(Application.dataPath, "") + "/";

            //Filter Models from Selected Assets
        foreach (Object model in selected)
        {
            string assetPath = AssetDatabase.GetAssetPath(model);

            if (assetPath.ToLower().EndsWith(".fbx") || assetPath.ToLower().EndsWith(".obj"))
            {
                modelsList.Add(model);
            }
        }

        foreach (Object model in modelsList)
        {
            var assetPath = AssetDatabase.GetAssetPath(model);

            var assetFolder = assetPath.Remove(assetPath.Length - model.name.Length - 4, model.name.Length + 4);
            string[] preExistingFiles = new string[0];
            string[] postExistingFiles = new string[0];

            if (AssetDatabase.IsValidFolder(assetFolder + "Materials"))
            {
                preExistingFiles = Directory.GetFiles(Application.dataPath + "/" + assetFolder.Remove(0, 7) + "Materials/");
            }

            var assetImporter = AssetImporter.GetAtPath(assetPath);
            ModelImporter modelImporter = assetImporter as ModelImporter;
            modelImporter.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
            modelImporter.SearchAndRemapMaterials((ModelImporterMaterialName)eWSettings.materialName, (ModelImporterMaterialSearch)eWSettings.materialSearch);
            modelImporter.materialLocation = ModelImporterMaterialLocation.External;
            modelImporter.SaveAndReimport();
            modelImporter.materialLocation = ModelImporterMaterialLocation.InPrefab;
            modelImporter.SaveAndReimport();


            if (AssetDatabase.IsValidFolder(assetFolder + "Materials") 
                    && eWSettings.materialFolderPath.Contains(Application.dataPath) && eWSettings.moveMaterials)
            {
                //Filter Extracted Model's Materials
                postExistingFiles = Directory.GetFiles(Application.dataPath + "/" + assetFolder.Remove(0, 7) + "Materials/");

                foreach (string preFile in preExistingFiles)
                {
                    postExistingFiles = postExistingFiles.Where(val => val != preFile).ToArray();
                }

                //Move materials to Destination Folder
                foreach (string file in postExistingFiles)
                {
                    string oldMaterialPath = "";
                    string newMaterialPath = "";
                    if (file.ToUpper().EndsWith(".MAT"))
                    {
                        oldMaterialPath = assetFolder + "Materials/" + Path.GetFileName(file);
                        newMaterialPath = materialsFolder + Path.GetFileName(file);

                        AssetDatabase.MoveAsset(oldMaterialPath, newMaterialPath);
                    }
                }

                //Delete Old Materials Folder if Empty
                int materialFolderFilesCount = Directory.GetFiles(Application.dataPath + "/" + assetFolder.Remove(0, 7) + "Materials/").Length;
                if (materialFolderFilesCount == 0)
                    AssetDatabase.DeleteAsset(assetFolder + "Materials");
            }

        }
    }

    static void GetEWScriptableObject()
    {
        string eWScriptObjPath = "Assets/Editor/EasyWayTools/EWSettings.asset";
        eWSettings = (EWScriptableObject)AssetDatabase.LoadAssetAtPath(eWScriptObjPath, typeof(EWScriptableObject));
        if (eWSettings == null)
        {
            eWSettings = ScriptableObject.CreateInstance<EWScriptableObject>();
            AssetDatabase.CreateAsset(eWSettings, eWScriptObjPath);
            AssetDatabase.Refresh();
        }
    }
}
