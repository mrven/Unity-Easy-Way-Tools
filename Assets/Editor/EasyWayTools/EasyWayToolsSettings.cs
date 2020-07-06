using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class EasyWayToolsSettings : EditorWindow
{
	static EWScriptableObject eWSettings;

	enum materialName
	{
		FromModelMaterial = 1,
		ModelNameAndModelMaterial = 2,
		ByBaseTextureName = 3
	}

	enum materialSearch
	{
		LocalFolder = 0,
		RecursiveUp = 1,
		ProjectWide = 2
	}

	materialSearch matSearch = materialSearch.ProjectWide;
	materialName matName = materialName.FromModelMaterial;

	bool moveMaterials = true;

	private void Awake()
	{
		GetEWScriptableObject();
		matSearch = (materialSearch)eWSettings.materialSearch;
		matName = (materialName)eWSettings.materialName;
		moveMaterials = eWSettings.moveMaterials;
	}

	[MenuItem("Tools/Easy Way Tools/Settings")]
	static void Init()
	{
		if (eWSettings == null)
		{
			GetEWScriptableObject();
		}
		EditorWindow editorWindow = EditorWindow.GetWindow(typeof(EasyWayToolsSettings));
		editorWindow.autoRepaintOnSceneChange = true;
		editorWindow.Show();
		GUIContent titleContent = new GUIContent("Easy Way Tools Settings");
		editorWindow.titleContent = titleContent;
	}

	void OnGUI()
	{
		//------------------------------------ Extract Materials -----------------------------------------
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Extract Materials from Models", EditorStyles.boldLabel);

		EditorGUILayout.Space();
		matSearch = (materialSearch)EditorGUILayout.EnumPopup("Material Search:", (materialSearch)eWSettings.materialSearch);
		matName = (materialName)EditorGUILayout.EnumPopup("Material Name:", (materialName)eWSettings.materialName);
		

		if (eWSettings.materialSearch != (int)matSearch || eWSettings.materialName != (int)matName)
		{
			eWSettings.materialSearch = (int)matSearch;
			eWSettings.materialName = (int)matName;
			SaveSettings();
		}


		EditorGUILayout.Space();
		moveMaterials = GUILayout.Toggle(eWSettings.moveMaterials, "Move Extracted Materials to Folder");
		if (eWSettings.moveMaterials != moveMaterials)
		{
			eWSettings.moveMaterials = moveMaterials;
			SaveSettings();
		}

		if (moveMaterials)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.TextField(eWSettings.materialFolderPath, GUILayout.ExpandWidth(true));
			if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
			{
				eWSettings.materialFolderPath = EditorUtility.SaveFolderPanel("Material Folder", eWSettings.materialFolderPath, Application.dataPath);
				SaveSettings();
			}
			EditorGUILayout.EndHorizontal();

			if (!eWSettings.materialFolderPath.Contains(Application.dataPath))
				EditorGUILayout.HelpBox("Choose the folder in Project", MessageType.Error);
			EditorGUILayout.Space();
		}

		//------------------------------------ Remap Textures -----------------------------------------

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

	static void SaveSettings()
	{
		EditorUtility.SetDirty(eWSettings);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}
		
}
