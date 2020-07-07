using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EasyWayScriptableObject")]
public class EWScriptableObject : ScriptableObject
{
    //Remap Materials Parameters
    public int materialSearch = 2;
    public int materialName = 1;
    public bool moveMaterials = true;
    public string materialFolderPath = "";

    [System.Serializable]
    public struct AssignmentProfile
    {
        [System.Serializable]
        public struct AssignmentProfileItem
        {
            public string materialSlot;
            public string textureName;
        }
        public string name;
        public AssignmentProfileItem[] textureMappingItems;
    }

    public List<AssignmentProfile> assignmentProfilesList = new List<AssignmentProfile>();

    public void InitTextureMapping()
    {

        List<AssignmentProfile.AssignmentProfileItem> apItems = new List<AssignmentProfile.AssignmentProfileItem>
        {
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MainTex", textureName = "_Albedo, _AlbedoTransparency, _Diffuse, _DiffuseTransparency, _BaseColor, _Color, _BaseColorMap, _ColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MetallicGlossMap", textureName = "_Metallic, _MetallicSmoothness, _MetallicMap, _MetallicSmooth" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_BumpMap", textureName = "_Normal, _NormalMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_ParallaxMap", textureName = "_Height, _HeightMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_OcclusionMap", textureName = "_AO, _Occlusion, _AmbientOcclusion, _OcclusionMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_EmissionMap", textureName = "_Emission, _EmissionMap, _Emissive, EmissiveColor, _EmissiveMap, _EmissiveColorMap" }
        };

        AssignmentProfile item = new AssignmentProfile
        {
            name = "Standard",
            textureMappingItems = apItems.ToArray()
        };

        assignmentProfilesList.Add(item);


    }

}
