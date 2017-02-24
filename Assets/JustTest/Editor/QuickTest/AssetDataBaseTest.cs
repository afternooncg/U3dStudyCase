using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class AssetDataBaseTest {

    [MenuItem("QuickTest/AssetDataBase/取的依赖GetDependencies")]
	static public void Call0 () {

        //string[] str = AssetDatabase.GetDependencies("Assets/Resources/fish/cruscarp.fbx");
        string[] str = AssetDatabase.GetDependencies("Assets/Resources/fish/cruscarp.prefab");
        //string[] str = AssetDatabase.GetDependencies("Assets/Materials/No Name.mat");
        Debug.Log("str:" + str.Length);

        //有个比较特别的是会包含自己
        for (int i = 0; i < str.Length; i++)
            Debug.Log("str:" + str[i]);

	}

    [MenuItem("QuickTest/AssetDataBase/AutoSetAbName")]
    static public void AutoSetAbName()
    {

        Debug.Log(AssetDatabase.GenerateUniqueAssetPath("Assets/_Images/load_bk.png"));
       // Debug.Log(AssetDatabase.GenerateUniqueAssetPath("efg/efg"));

        return;
        string[] folders = AssetDatabase.GetSubFolders("Assets/GameRes");
        for (int i = 0; i < folders.Length; i++)
        {
            
             string[] guids = AssetDatabase.FindAssets("",new string[]{folders[i]});
             for (int j = 0; j < guids.Length; j++)
             {
                 string assetPath = AssetDatabase.GUIDToAssetPath(guids[j]);
                 if (!AssetDatabase.IsValidFolder(assetPath))
                 {
                     AssetImporter import = AssetImporter.GetAtPath(assetPath);

                     Debug.Log(Path.GetDirectoryName(import.assetPath) + "   " + Path.GetFileNameWithoutExtension(import.assetPath));
                     
                     if (import != null)
                     {
                         import.assetBundleName = Path.GetDirectoryName(import.assetPath);
                         import.SaveAndReimport();
                     }
                     else
                     {
                         Debug.Log("Null Import:" + assetPath);
                     }
                     
                 }
                  
             }            
        }
        
    }


    //遍历子目录,子文件设定ab名称 path: 绝对路径
    
    static void SetAbNameWithFolder(string path)
    {       
        string appdataPath = Application.dataPath.Replace('/', '\\');
        Debug.Log(appdataPath);

        
        
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] allFileInfos = dirInfo.GetFiles();

        string abName = "Assets/GameRes/" + path;
        
        for (int j = 0; j < allFileInfos.Length; j++)
        {
            if (Path.GetExtension(allFileInfos[j].Name).ToLower().CompareTo(".meta") != 0)
            {

                string assetpath = "Assets" + (allFileInfos[j].FullName).Replace(appdataPath, "").Replace('\\', '/');
                Debug.Log("fileName:" + assetpath);
                
                AssetImporter import = AssetImporter.GetAtPath(assetpath);
                if (import == null)
                {
                    Debug.Log("Empty");
                    continue;
                }


                import.assetBundleName = abName;
                import.SaveAndReimport();
            }
        }

        foreach(DirectoryInfo info in dirInfo.GetDirectories())
        {
            SetAbNameWithFolder(info.FullName);
        }
    }
	
}
