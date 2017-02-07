using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public static class AbilityInfoUtility
{
    public static AbilityInfo Create(string _path, string _name)
    {
        //
        if (new DirectoryInfo(_path).Exists == false)
        {
            Debug.LogError("can't create asset, path not found");
            return null;
        }
        if (string.IsNullOrEmpty(_name))
        {
            Debug.LogError("can't create asset, the name is empty");
            return null;
        }
        string assetPath = Path.Combine(_path, _name + ".asset");

        //
        AbilityInfo newAbilityInfo = ScriptableObject.CreateInstance<AbilityInfo>();
        AssetDatabase.CreateAsset(newAbilityInfo, assetPath);
        Selection.activeObject = newAbilityInfo;
        return newAbilityInfo;
    }


    [MenuItem ("Assets/MyEditor/Ability Info")]
    public static void CreateTest () {
        // get current selected directory
        
        string assetName = "New AbilityInfo";
        string assetPath = "Assets";
        if ( Selection.activeObject ) {
            
            assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            Debug.Log("mmmm:" + assetPath);
            if ( Path.GetExtension(assetPath) != "" ) {
                assetPath = Path.GetDirectoryName(assetPath);
            }
        }
 
        //
        bool doCreate = true;
        string path = Path.Combine( assetPath, assetName + ".asset" );
        Debug.Log("path:" + path);
        FileInfo fileInfo = new FileInfo(path);
        if ( fileInfo.Exists ) {
            doCreate = EditorUtility.DisplayDialog( assetName + " already exists.",
                                                    "Do you want to overwrite the old one?",
                                                    "Yes", "No" );
        }
        if ( doCreate ) {
            AbilityInfo abilityInfo = AbilityInfoUtility.Create ( assetPath, assetName );
            Selection.activeObject = abilityInfo;
            // EditorGUIUtility.PingObject(border);
        }
    }
}

