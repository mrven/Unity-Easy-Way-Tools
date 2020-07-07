﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class EasyWayToolsSettings : EditorWindow
{
	Vector2 scrollPos;

	static EWScriptableObject eWSettings;

	EWScriptableObject.AssignmentProfile newProfile = new EWScriptableObject.AssignmentProfile 
	{ 
		profileName = "", 
		shaderName = "", 
		assignmentProfileItems = new List<EWScriptableObject.AssignmentProfile.AssignmentProfileItem>().ToArray()
	};
	List<EWScriptableObject.AssignmentProfile.AssignmentProfileItem> newProfileItems = new List<EWScriptableObject.AssignmentProfile.AssignmentProfileItem>();

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

	int assignmentProfileIndex = 0;
	static string[] assignmentProfiles;

	private void Awake()
	{
		GetEWScriptableObject();
		matSearch = (materialSearch)eWSettings.materialSearch;
		matName = (materialName)eWSettings.materialName;
		moveMaterials = eWSettings.moveMaterials;
	}

	static private void GetExistingAssignmentProfiles()
	{
		List<string> profilesList = new List<string>();

		foreach (var assignmentProfile in eWSettings.assignmentProfilesList)
		{
			profilesList.Add(assignmentProfile.profileName);
		}

		profilesList.Add("Add New Profile...");
		assignmentProfiles = profilesList.ToArray();

	}

	[MenuItem("Tools/Easy Way Tools/Settings")]
	static void ShowWindow()
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

		GetExistingAssignmentProfiles();
	}

	void OnGUI()
	{
		float windowWidth = position.width - 25;
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		
		if (eWSettings == null)
		{
			GetEWScriptableObject();
		}


		//------------------------------------ Extract Materials -----------------------------------------
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Extract Materials from Models", EditorStyles.boldLabel, GUILayout.Width(windowWidth));

		EditorGUILayout.Space();
		matSearch = (materialSearch)EditorGUILayout.EnumPopup("Material Search:", (materialSearch)eWSettings.materialSearch, GUILayout.Width(windowWidth));
		matName = (materialName)EditorGUILayout.EnumPopup("Material Name:", (materialName)eWSettings.materialName, GUILayout.Width(windowWidth));
		

		if (eWSettings.materialSearch != (int)matSearch || eWSettings.materialName != (int)matName)
		{
			eWSettings.materialSearch = (int)matSearch;
			eWSettings.materialName = (int)matName;
			SaveSettings();
		}


		EditorGUILayout.Space();
		moveMaterials = GUILayout.Toggle(eWSettings.moveMaterials, "Move Extracted Materials to Folder", GUILayout.Width(windowWidth));
		if (eWSettings.moveMaterials != moveMaterials)
		{
			eWSettings.moveMaterials = moveMaterials;
			SaveSettings();
		}

		if (moveMaterials)
		{
			EditorGUILayout.BeginHorizontal(GUILayout.Width(windowWidth));
			EditorGUILayout.TextField(eWSettings.materialFolderPath, GUILayout.ExpandWidth(true));
			if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
			{
				eWSettings.materialFolderPath = EditorUtility.SaveFolderPanel("Material Folder", eWSettings.materialFolderPath, Application.dataPath);
				SaveSettings();
			}
			EditorGUILayout.EndHorizontal();

			if (!eWSettings.materialFolderPath.Contains(Application.dataPath))
			{
				EditorGUILayout.BeginHorizontal(GUILayout.Width(windowWidth));
				EditorGUILayout.HelpBox("Choose the folder in Project", MessageType.Error);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.Space();
		}

		//------------------------------------ Texture Assignment Tool -----------------------------------------
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Texture Assignment Tool", EditorStyles.boldLabel, GUILayout.Width(windowWidth));

		EditorGUILayout.Space();

		assignmentProfileIndex = EditorGUILayout.Popup("Assignment Profiles:", assignmentProfileIndex, assignmentProfiles, GUILayout.Width(windowWidth));

		if (assignmentProfileIndex < assignmentProfiles.Length - 1)
		{
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal(GUILayout.Width(windowWidth));
			EditorGUILayout.LabelField("Shader Full Name:", GUILayout.Width(windowWidth / 2));
			EditorGUILayout.LabelField(eWSettings.assignmentProfilesList[assignmentProfileIndex].shaderName, GUILayout.Width(windowWidth / 2));
			EditorGUILayout.EndHorizontal();
		}
		else
		{
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal(GUILayout.Width(windowWidth));
			EditorGUILayout.LabelField("Profile Name:", GUILayout.Width(windowWidth / 2));
			newProfile.profileName = EditorGUILayout.TextField(newProfile.profileName, GUILayout.Width(windowWidth / 2));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal(GUILayout.Width(windowWidth));
			EditorGUILayout.LabelField("Shader Full Name:", GUILayout.Width(windowWidth / 2));
			newProfile.shaderName = EditorGUILayout.TextField(newProfile.shaderName, GUILayout.Width(windowWidth / 2));
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal(GUILayout.Width(windowWidth));
		EditorGUILayout.LabelField("Material Slot:", EditorStyles.boldLabel, GUILayout.Width(windowWidth / 2));
		EditorGUILayout.LabelField("Texture Name:", EditorStyles.boldLabel, GUILayout.Width(windowWidth / 2));
		EditorGUILayout.EndHorizontal();

		if (assignmentProfileIndex < assignmentProfiles.Length - 1)
		{
			foreach (var assignmentProfileItem in eWSettings.assignmentProfilesList[assignmentProfileIndex].assignmentProfileItems)
			{
				string[] textureNames = assignmentProfileItem.textureName.Split(',');
				EditorGUILayout.Space();

				EditorGUILayout.BeginHorizontal(GUILayout.Width(windowWidth));
				EditorGUILayout.LabelField(assignmentProfileItem.materialSlot);
				EditorGUILayout.EndHorizontal();
				foreach (string textureName in textureNames)
				{
					EditorGUILayout.BeginHorizontal(GUILayout.Width(windowWidth));
					EditorGUILayout.LabelField(" ", GUILayout.Width(windowWidth / 2));
					EditorGUILayout.LabelField(textureName.Trim(' '), GUILayout.Width(windowWidth / 2));
					EditorGUILayout.EndHorizontal();
				}
			}

			EditorGUILayout.Space();

			if (GUILayout.Button("Delete This Profile", GUILayout.Width(windowWidth)))
			{
				eWSettings.assignmentProfilesList.Remove(eWSettings.assignmentProfilesList[assignmentProfileIndex]);
				SaveSettings();
				GetExistingAssignmentProfiles();
				assignmentProfileIndex = 0;
			}

			EditorGUILayout.Space();
		}
		else
		{
			EditorGUILayout.Space();

			

			for (int index = 0; index < newProfileItems.Count; index++)
			{
				var tempProfileItem = newProfileItems[index];
				EditorGUILayout.BeginHorizontal(GUILayout.Width(windowWidth));
				tempProfileItem.materialSlot = EditorGUILayout.TextField(newProfileItems[index].materialSlot, GUILayout.Width(windowWidth / 2));
				tempProfileItem.textureName = EditorGUILayout.TextField(newProfileItems[index].textureName, GUILayout.Width(windowWidth / 2));
				EditorGUILayout.EndHorizontal();

				newProfileItems[index] = tempProfileItem;
			}

			EditorGUILayout.Space();

			if (GUILayout.Button("Add New Material Slot", GUILayout.Width(windowWidth)))
			{
				newProfileItems.Add(new EWScriptableObject.AssignmentProfile.AssignmentProfileItem { });
			}

			if (GUILayout.Button("Delete Last Material Slot", GUILayout.Width(windowWidth)))
			{
				if (newProfileItems.Count > 0)
					newProfileItems.Remove(newProfileItems[newProfileItems.Count - 1]);
			}

			if (GUILayout.Button("Clear All Material Slots", GUILayout.Width(windowWidth)))
			{
				newProfileItems.Clear();
			}

			if (GUILayout.Button("Save New Profile", GUILayout.Width(windowWidth)))
			{
				newProfile.assignmentProfileItems = newProfileItems.ToArray();

				if (newProfile.profileName.Length > 0 && newProfile.shaderName.Length > 0)
				{
					eWSettings.assignmentProfilesList.Add(newProfile);
					SaveSettings();
					GetExistingAssignmentProfiles();
					assignmentProfileIndex = eWSettings.assignmentProfilesList.Count - 1;
					newProfile.profileName = "";
					newProfile.shaderName = "";
					newProfileItems.Clear();
				}
				else
				{
					Debug.Log("Profile not Saved. Profile Name and/or Shader Name are Empty");
					EditorGUILayout.EndHorizontal();
				}
			}

			EditorGUILayout.Space();
		}

		EditorGUILayout.EndScrollView();

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

}
