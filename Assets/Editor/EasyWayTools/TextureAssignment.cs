using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Rendering;

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
                                    if (textureSuf.Trim(' ').Length > 0)
                                    {
                                        if (textureName.EndsWith(textureSuf.Trim(' ')))
                                            slotMatchedTexture = matMatchedTexture;
                                    }
                                }
                            }

                            if (slotMatchedTexture.Length > 0)
                            {
                                Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(slotMatchedTexture), typeof(Texture2D));
                                material.SetTexture(profileItem.materialSlot, texture);

                                string renderPipeline = GetRenderPipeline();

                                if (renderPipeline == "Legacy")
                                {
                                    if (profileItem.materialSlot == "_BumpMap")
                                        material.EnableKeyword("_NORMALMAP");
                                    
                                    if (profileItem.materialSlot == "_MetallicGlossMap")
                                        material.EnableKeyword("_METALLICGLOSSMAP");
                                    
                                    if (profileItem.materialSlot == "_SpecGlossMap")
                                        material.EnableKeyword("_SPECGLOSSMAP");

                                    if (profileItem.materialSlot == "_EmissionMap")
                                    {
                                        MaterialEditor.FixupEmissiveFlag(material);
                                        material.SetColor("_EmissionColor", Color.white);
                                        material.EnableKeyword("_EMISSION");
                                        material.globalIlluminationFlags = 0;
                                    }
                                }

                                if (renderPipeline == "URP")
                                {
                                    if (profileItem.materialSlot == "_BumpMap")
                                        material.EnableKeyword("_NORMALMAP");

                                    if (profileItem.materialSlot == "_MetallicGlossMap")
                                    {
                                        material.DisableKeyword("_SPECULAR_SETUP");
                                        material.EnableKeyword("_METALLICSPECGLOSSMAP");
                                        material.SetFloat("_WorkflowMode", 1);
                                        material.SetFloat("_Smoothness", 1);
                                    }

                                    if (profileItem.materialSlot == "_SpecGlossMap")
                                    {
                                        material.EnableKeyword("_SPECULAR_SETUP");
                                        material.SetFloat("_WorkflowMode", 0);
                                        material.SetFloat("_Smoothness", 1);
                                        material.EnableKeyword("_METALLICSPECGLOSSMAP");
                                    }

                                    if (profileItem.materialSlot == "_EmissionMap")
                                    {
                                        MaterialEditor.FixupEmissiveFlag(material);
                                        material.SetColor("_EmissionColor", Color.white);
                                        material.EnableKeyword("_EMISSION");
                                        material.globalIlluminationFlags = 0;
                                    }
                                }

                                if (renderPipeline == "HDRP")
                                {
                                    if (profileItem.materialSlot == "_NormalMap")
                                        material.EnableKeyword("_NORMALMAP");
                                    if (profileItem.materialSlot == "_MaskMap")
                                        material.EnableKeyword("_MASKMAP");
                                    if (profileItem.materialSlot == "_SpecularColorMap")
                                        material.EnableKeyword("_MATERIAL_FEATURE_SPECULAR_COLOR");

                                }

                            }
                        }

                        string materialPath = AssetDatabase.GetAssetPath(material);
                        AssetDatabase.Refresh();
                        AssetDatabase.ImportAsset(materialPath, ImportAssetOptions.ForceUpdate);
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

        if (eWSettings.assignmentProfilesList.Count < 1)
        {
            eWSettings.InitDefaultAssignmentProfiles();
            SaveSettings();
        }
    }

    static void SaveSettings()
    {
        EditorUtility.SetDirty(eWSettings);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    static string GetFileName(string fileName)
    {
        return Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(fileName));
    }

    static string GetRenderPipeline()
    {
        string renderPipeline = "";

        if (GraphicsSettings.renderPipelineAsset == null)
        {
            renderPipeline = "Legacy";
        }
        else if (GraphicsSettings.renderPipelineAsset.GetType().Name.Contains("HDRender"))
        {
            renderPipeline = "HDRP";
        }
        else if (GraphicsSettings.renderPipelineAsset.GetType().Name.Contains("UniversalRender"))
        {
            renderPipeline = "URP";
        }

        return renderPipeline;
    }
}
