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

    public int assignmentMethod = 0;

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
        public AssignmentProfileItem[] assignmentProfileItems;
    }

    public List<AssignmentProfile> assignmentProfilesList = new List<AssignmentProfile>();

    public void InitDefaultAssignmentProfiles()
    {
        //Standard
        List<AssignmentProfile.AssignmentProfileItem> apItems = new List<AssignmentProfile.AssignmentProfileItem>
        {
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MainTex", textureName = "Albedo, AlbedoTransparency, Diffuse, DiffuseTransparency, BaseColor, Color, BaseColorMap, ColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MetallicGlossMap", textureName = "Metallic, MetallicSmoothness, MetallicMap, MetallicSmooth" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_BumpMap", textureName = "Normal, NormalMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_ParallaxMap", textureName = "Height, HeightMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_OcclusionMap", textureName = "AO, Occlusion, AmbientOcclusion, OcclusionMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_EmissionMap", textureName = "Emission, EmissionMap, Emissive, EmissiveColor, EmissiveMap, EmissiveColorMap" }
        };

        AssignmentProfile item = new AssignmentProfile
        {
            profileName = "Standard",
            shaderName = "Standard",
            assignmentProfileItems = apItems.ToArray()
        };

        assignmentProfilesList.Add(item);

        //Standard Specular
        apItems = new List<AssignmentProfile.AssignmentProfileItem>
        {
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MainTex", textureName = "Albedo, AlbedoTransparency, Diffuse, DiffuseTransparency, BaseColor, Color, BaseColorMap, ColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_SpecGlossMap", textureName = "Specular, SpecularSmoothness, SpecularMap, SpecularGlossiness, SpecGloss, SpecSmooth" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_BumpMap", textureName = "Normal, NormalMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_ParallaxMap", textureName = "Height, HeightMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_OcclusionMap", textureName = "AO, Occlusion, AmbientOcclusion, OcclusionMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_EmissionMap", textureName = "Emission, EmissionMap, Emissive, EmissiveColor, EmissiveMap, EmissiveColorMap" }
        };

        item = new AssignmentProfile
        {
            profileName = "Standard Specular",
            shaderName = "Standard (Specular setup)",
            assignmentProfileItems = apItems.ToArray()
        };

        assignmentProfilesList.Add(item);

        //HDRP Lit
        apItems = new List<AssignmentProfile.AssignmentProfileItem>
        {
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_BaseColorMap", textureName = "Albedo, AlbedoTransparency, Diffuse, DiffuseTransparency, BaseColor, Color, BaseColorMap, ColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MaskMap", textureName = "Mask, MaskMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_NormalMap", textureName = "Normal, NormalMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_BentNormalMap", textureName = "BentNormal, Bent, BentNormalMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_HeightMap", textureName = "Height, HeightMap, Tesselation, TesselationMap, Displacement, DisplacementMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_TangentMap", textureName = "Tangent, TangentMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_AnisotropyMap", textureName = "Anisotropy, AnisotropyMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_SubsurfaceMaskMap", textureName = "Subsurface, SubsurfaceMap, SubsurfaceMask, SubsurfaceMaskMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_ThicknessMap", textureName = "Thickness, ThicknessMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_IridescenceThicknessMap", textureName = "IridescenceThickness, IridescenceThicknessMap, Iridescence, IridescenceMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_IridescenceMaskMap", textureName = "IridescenceMaskMap, IridescenceMask" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_CoatMaskMap", textureName = "Coat, CoatMap, CoatMask, CoatMaskMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_SpecularColorMap", textureName = "Specular, SpecularMap, SpecularColor, SpecularColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_EmissiveColorMap", textureName = "Emission, EmissionMap, Emissive, EmissiveColor, EmissiveMap, EmissiveColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MainTex", textureName = "Albedo, AlbedoTransparency, Diffuse, DiffuseTransparency, BaseColor, Color, BaseColorMap, ColorMap" }
        };

        item = new AssignmentProfile
        {
            profileName = "HDRP Lit",
            shaderName = "HDRP/Lit",
            assignmentProfileItems = apItems.ToArray()
        };

        assignmentProfilesList.Add(item);

        //URP Lit
        apItems = new List<AssignmentProfile.AssignmentProfileItem>
        {
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_BaseMap", textureName = "Albedo, AlbedoTransparency, Diffuse, DiffuseTransparency, BaseColor, Color, BaseColorMap, ColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MetallicGlossMap", textureName = "Metallic, MetallicSmoothness, MetallicMap, MetallicSmooth" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_SpecGlossMap", textureName = "Specular, SpecularSmoothness, SpecularMap, SpecularGlossiness, SpecGloss, SpecSmooth" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_BumpMap", textureName = "Normal, NormalMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_OcclusionMap", textureName = "AO, Occlusion, AmbientOcclusion, OcclusionMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_EmissionMap", textureName = "Emission, EmissionMap, Emissive, EmissiveColor, EmissiveMap, EmissiveColorMap" },
            new AssignmentProfile.AssignmentProfileItem { materialSlot = "_MainTex", textureName = "Albedo, AlbedoTransparency, Diffuse, DiffuseTransparency, BaseColor, Color, BaseColorMap, ColorMap" }
        };

        item = new AssignmentProfile
        {
            profileName = "URP Lit",
            shaderName = "Universal Render Pipeline/Lit",
            assignmentProfileItems = apItems.ToArray()
        };

        assignmentProfilesList.Add(item);

    }

}
