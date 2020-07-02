using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EasyWayScriptableObject")]
public class EWScriptableObject : ScriptableObject
{
    //Remap Materials Parameters
    public int materialSearch = 2;
    public int materialName = 1;
    public string materialFolderPath = "";
}
