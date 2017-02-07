using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility  
{

     public static T CreateAsset<T>() where T : ScriptableObject
    {

        string path = "Assets/GameData/System/";

        /* 可选定Asset对象生成
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!string.IsNullOrEmpty(path) &&  !string.IsNullOrEmpty(Path.GetExtension(path)))
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }        
         */

        return CreateAsset<T>(path + typeof(T).ToString() + ".asset");
        
    }

    public static T CreateAsset<T>(string pPath) where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        CreateAsset(pPath, asset);

        return asset;
    }



    /// <summary>
    ///  生成 Asset 文件
    /// </summary>
    /// <param name="pPath"></param>
    /// <param name="pAsset"></param>
    /// <returns></returns>
    public static void CreateAsset(string pPath, ScriptableObject pAsset, bool isRenameOnExist = true)
    {

        //可以避免覆盖，如果已存在同名资源,会自动重命名
        string assetPathAndName = pPath;
        if (isRenameOnExist)
            AssetDatabase.GenerateUniqueAssetPath(pPath);

        AssetDatabase.CreateAsset(pAsset, assetPathAndName);

        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = pAsset;
    }
}
