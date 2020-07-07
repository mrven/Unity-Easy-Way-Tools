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
        public string profileName;
        public string shaderName;
        public AssignmentProfileItem[] textureMappingItems;
    }

    public List<AssignmentProfile> assignmentProfilesList = new List<AssignmentProfile>();

    public void InitDefaultAssignmentProfiles()
    {
        //Standard
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
            profileName = "Standard",
            shaderName = "Standard",
            textureMappingItems = apItems.ToArray()
        };

        assignmentProfilesList.Add(item);

        //Standard Specular
        apItems = new List<AssignmentProfile.AssignmentProfileItem>
        {
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MainTex", textureName = "_Albedo, _AlbedoTransparency, _Diffuse, _DiffuseTransparency, _BaseColor, _Color, _BaseColorMap, _ColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_SpecGlossMap", textureName = "_Specular, _SpecularSmoothness, _SpecularMap, _SpecularGlossiness, _SpecGloss, _SpecSmooth" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_BumpMap", textureName = "_Normal, _NormalMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_ParallaxMap", textureName = "_Height, _HeightMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_OcclusionMap", textureName = "_AO, _Occlusion, _AmbientOcclusion, _OcclusionMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_EmissionMap", textureName = "_Emission, _EmissionMap, _Emissive, EmissiveColor, _EmissiveMap, _EmissiveColorMap" }
        };

        item = new AssignmentProfile
        {
            profileName = "Standard Specular",
            shaderName = "Standard (Specular setup)",
            textureMappingItems = apItems.ToArray()
        };

        assignmentProfilesList.Add(item);

        //HDRP Lit
        apItems = new List<AssignmentProfile.AssignmentProfileItem>
        {
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_BaseColorMap", textureName = "_Albedo, _AlbedoTransparency, _Diffuse, _DiffuseTransparency, _BaseColor, _Color, _BaseColorMap, _ColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MaskMap", textureName = "_Mask, _MaskMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_NormalMap", textureName = "_Normal, _NormalMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_BentNormalMap", textureName = "_BentNormal, _Bent, _BentNormalMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_HeightMap", textureName = "_Height, _HeightMap, _Tesselation, _TesselationMap, _Displacement, DisplacementMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_TangentMap", textureName = "_Tangent, _TangentMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_AnisotropyMap", textureName = "_Anisotropy, _AnisotropyMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_SubsurfaceMaskMap", textureName = "_Subsurface, _SubsurfaceMap, _SubsurfaceMask, _SubsurfaceMaskMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_ThicknessMap", textureName = "_Thickness, _ThicknessMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_IridescenceThicknessMap", textureName = "_IridescenceThickness, _IridescenceThicknessMap, _Iridescence, _IridescenceMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_IridescenceMaskMap", textureName = "_IridescenceMaskMap, _IridescenceMask" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_CoatMaskMap", textureName = "_Coat, _CoatMap, _CoatMask, _CoatMaskMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_SpecularColorMap", textureName = "_Specular, _SpecularMap, _SpecularColor, _SpecularColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_EmissiveColorMap", textureName = "_Emission, _EmissionMap, _Emissive, EmissiveColor, _EmissiveMap, _EmissiveColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MainTex", textureName = "_Albedo, _AlbedoTransparency, _Diffuse, _DiffuseTransparency, _BaseColor, _Color, _BaseColorMap, _ColorMap" }
        };

        item = new AssignmentProfile
        {
            profileName = "HDRP Lit",
            shaderName = "HDRP/Lit",
            textureMappingItems = apItems.ToArray()
        };

        assignmentProfilesList.Add(item);

        //URP Lit
        apItems = new List<AssignmentProfile.AssignmentProfileItem>
        {
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_BaseMap", textureName = "_Albedo, _AlbedoTransparency, _Diffuse, _DiffuseTransparency, _BaseColor, _Color, _BaseColorMap, _ColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MetallicGlossMap", textureName = "_Metallic, _MetallicSmoothness, _MetallicMap, _MetallicSmooth" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_SpecGlossMap", textureName = "_Specular, _SpecularSmoothness, _SpecularMap, _SpecularGlossiness, _SpecGloss, _SpecSmooth" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_BumpMap", textureName = "_Normal, _NormalMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_EmissionMap", textureName = "_Emission, _EmissionMap, _Emissive, EmissiveColor, _EmissiveMap, _EmissiveColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MainTex", textureName = "_Albedo, _AlbedoTransparency, _Diffuse, _DiffuseTransparency, _BaseColor, _Color, _BaseColorMap, _ColorMap" }
        };

        item = new AssignmentProfile
        {
            profileName = "URP Lit",
            shaderName = "Universal Render Pipeline/Lit",
            textureMappingItems = apItems.ToArray()
        };

        assignmentProfilesList.Add(item);

    }

}
