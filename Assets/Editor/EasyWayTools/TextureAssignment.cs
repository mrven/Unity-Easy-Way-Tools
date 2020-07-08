using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
                    Debug.Log("Profile " + matchedProfile.profileName + " founded!");
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
