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
                        if (Path.GetFileName(AssetDatabase.GUIDToAssetPath(projectTexture)).StartsWith(materialName))
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

                            string SlotMatchedTexture = "";

                            foreach (var matMatchedTexture in matMatchedTextures)
                            {
                                string textureName = Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(matMatchedTexture));

                                foreach (var textureSuf in searchingTextureSuf)
                                {
                                    if (textureName.EndsWith(textureSuf.Trim(' ')))
                                        SlotMatchedTexture = matMatchedTexture;
                                }
                            }

                            if (SlotMatchedTexture.Length > 0)
                            {
                                Debug.Log("Texture for " + profileItem.materialSlot + " material slot is " + Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(SlotMatchedTexture)));
                            }
                        }

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
}
