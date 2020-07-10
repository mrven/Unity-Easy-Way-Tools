using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class EasyWayToolsSettings : EditorWindow
{
	string version = "1.0 (10 Jul, 2020)";

	Vector2 scrollPos;
	bool showProfile = true;

	static EWScriptableObject eWSettings;

	EWScriptableObject.AssignmentProfile newProfile = new EWScriptableObject.AssignmentProfile 
	{ 
		profileName = "", 
		shaderName = "", 
		assignmentProfileItems = new List<EWScriptableObject.AssignmentProfile.AssignmentProfileItem>().ToArray()
	};
	List<EWScriptableObject.AssignmentProfile.AssignmentProfileItem> newProfileItems = new List<EWScriptableObject.AssignmentProfile.AssignmentProfileItem>();

	enum MaterialName
	{
		FromModelMaterial = 1,
		ModelNameAndModelMaterial = 2,
		ByBaseTextureName = 3
	}

	enum MaterialSearch
	{
		LocalFolder = 0,
		RecursiveUp = 1,
		ProjectWide = 2
	}

	enum AssignmentMethod
	{
		StartsWithMaterialName = 0,
		ContainsMaterialName = 1
	}

	MaterialSearch matSearch = MaterialSearch.ProjectWide;
	MaterialName matName = MaterialName.FromModelMaterial;
	AssignmentMethod assignmentMethod = AssignmentMethod.StartsWithMaterialName;

	bool moveMaterials = true;

	int assignmentProfileIndex = 0;
	static string[] assignmentProfiles;

	private void Awake()
	{
		GetEWScriptableObject();
		matSearch = (MaterialSearch)eWSettings.materialSearch;
		matName = (MaterialName)eWSettings.materialName;
		moveMaterials = eWSettings.moveMaterials;
		assignmentMethod = (AssignmentMethod)eWSettings.assignmentMethod;
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
		matSearch = (MaterialSearch)EditorGUILayout.EnumPopup("Material Search:", (MaterialSearch)eWSettings.materialSearch, GUILayout.Width(windowWidth));
		matName = (MaterialName)EditorGUILayout.EnumPopup("Material Name:", (MaterialName)eWSettings.materialName, GUILayout.Width(windowWidth));
		

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

		assignmentMethod = (AssignmentMethod)EditorGUILayout.EnumPopup("Texture Search Method:", (AssignmentMethod)eWSettings.assignmentMethod, GUILayout.Width(windowWidth));


		if (eWSettings.assignmentMethod != (int)assignmentMethod)
		{
			eWSettings.assignmentMethod = (int)assignmentMethod;
			SaveSettings();
		}

		assignmentProfileIndex = EditorGUILayout.Popup("Assignment Profiles:", assignmentProfileIndex, assignmentProfiles, GUILayout.Width(windowWidth));

		if (assignmentProfileIndex < assignmentProfiles.Length - 1)
		{
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal(GUILayout.Width(windowWidth));
			EditorGUILayout.LabelField("Shader Full Name:", EditorStyles.boldLabel, GUILayout.Width(windowWidth / 2));
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
			showProfile = EditorGUILayout.Foldout(showProfile, "Profile");
			if (showProfile)
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
						if (textureName.Trim(' ').Length > 0)
						{
							EditorGUILayout.BeginHorizontal(GUILayout.Width(windowWidth));
							EditorGUILayout.LabelField(" ", GUILayout.Width(windowWidth / 2));
							EditorGUILayout.LabelField(textureName.Trim(' '), GUILayout.Width(windowWidth / 2));
							EditorGUILayout.EndHorizontal();
						}
					}
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
				newProfileItems.Add(new EWScriptableObject.AssignmentProfile.AssignmentProfileItem {textureName = "", materialSlot = "" });
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
				List<EWScriptableObject.AssignmentProfile.AssignmentProfileItem> filteredNewProfileItems = new List<EWScriptableObject.AssignmentProfile.AssignmentProfileItem>();

				foreach (var newProfileItem in newProfileItems)
				{
					if (newProfileItem.materialSlot.Trim(' ').Length > 0 && newProfileItem.textureName.Trim(' ').Length > 0)
					{
						filteredNewProfileItems.Add(newProfileItem);
					}
				}

				newProfile.assignmentProfileItems = filteredNewProfileItems.ToArray();

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

		//------------------------------------------------------Version----------------------------------

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Version", EditorStyles.boldLabel, GUILayout.Width(windowWidth));
		EditorGUILayout.LabelField(version, GUILayout.Width(windowWidth));
		EditorGUILayout.Space();

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
