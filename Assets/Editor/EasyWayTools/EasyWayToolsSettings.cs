using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class EasyWayToolsSettings : EditorWindow
{
	static EWScriptableObject eWSettings;

	private void Awake()
	{
		GetEWScriptableObject();
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
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Extract Materials from Models", EditorStyles.boldLabel);

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Select Material Folder:");

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
