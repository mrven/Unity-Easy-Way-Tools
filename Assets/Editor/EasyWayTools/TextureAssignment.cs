using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureAssignment: Editor
{
    static EWScriptableObject eWSettings;

    [MenuItem("Assets/Easy Way Tools/Texture Assignment Tool")]
    private static void TextureAssignmentTool()
    {
        string[] projectTextures = AssetDatabase.FindAssets("t:Texture2D", new [] { "Assets" });
        var selected = Selection.objects;

        GetEWScriptableObject();

        foreach (var o in selected)
        {
            if (o.GetType() == typeof(Material))
            {
                Material material = (Material)o;
                string shaderName = material.shader.name;
                EWScriptableObject.AssignmentProfile matchedProfile = new EWScriptableObject.AssignmentProfile();

                foreach (EWScriptableObject.AssignmentProfile assignmentProfile in eWSettings.assignmentProfilesList)
                {
                    if (assignmentProfile.shaderName == shaderName)
                        matchedProfile = assignmentProfile;
                }

                if (matchedProfile.profileName != null)
                {
                    //Filter Textures contain/start with Material Name

                    string materialName = material.name;

                    List<string> matMatchedTextures = new List<string>();

                    foreach (string projectTexture in projectTextures)
                    {
                        if ((GetFileName(projectTexture).StartsWith(materialName) && eWSettings.assignmentMethod == 0) || (GetFileName(projectTexture).Contains(materialName) && eWSettings.assignmentMethod == 1))
                        {
                            matMatchedTextures.Add(projectTexture);
                        }
                    }

                    //If was found textures with material name, Try assign it to Material Slots
                    if (matMatchedTextures.Count > 0)
                    {
                        foreach (EWScriptableObject.AssignmentProfile.AssignmentProfileItem profileItem in matchedProfile.assignmentProfileItems)
                        {
                            string[] searchingTextureSuf = profileItem.textureName.Split(',');

                            string slotMatchedTexture = "";

                            foreach (var matMatchedTexture in matMatchedTextures)
                            {
                                string textureName = GetFileName(matMatchedTexture);

                                foreach (var textureSuf in searchingTextureSuf)
                                {
                                    if (textureName.EndsWith(textureSuf.Trim(' ')))
                                        slotMatchedTexture = matMatchedTexture;
                                }
                            }

                            if (slotMatchedTexture.Length > 0)
                            {
                                Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(slotMatchedTexture), typeof(Texture2D));
                                material.SetTexture(profileItem.materialSlot, texture);
                            }
                        }

                        string fullMaterialPath = Application.dataPath + "/" + AssetDatabase.GetAssetPath(material).Remove(0, 7);
                        AssetDatabase.Refresh();
                        AssetDatabase.ImportAsset(fullMaterialPath, ImportAssetOptions.ForceUpdate);
                        AssetDatabase.Refresh();
                    }
                }
                else 
                {
                    Debug.Log("Profile for Material " + material.name + " not found");
                }

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

    static string GetFileName(string fileName)
    {
        return Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(fileName));
    }
}
