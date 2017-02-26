using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ForMobileCopyTest
{

    [MenuItem("JustTest/ForMobileCopyTest/create FileListData")]
    static void CreateScripteObject()
    {
        ScriptableObjectUtility.CreateAsset<FileListData>("Assets/Resources/ScriptObjs/FileListData.asset");
    }


    [MenuItem("JustTest/ForMobileCopyTest/批量设置名称")]
    static void BatSetAbNameOk()
    {
        string path = Application.dataPath + "/GameData/Lua";
        string extensions = "*.txt";

        Debug.Log(path);

        FileListData data = AssetDatabase.LoadAssetAtPath<FileListData>("Assets/Resources/ScriptObjs/FileListData.asset");
        for (int i = 0; i < data.Files.Count; i++)
        {
            AssetBundleHandle.SetAssetBundleName(data.Files[i], data.Files[i]+".unity3d");
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        /*

            foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
            {
                //UnityEditor.FileUtil.DeleteFileOrDirectory(file);
                string tmp = Path.GetExtension(file).ToLower();
                //Debug.Log(file + " " + tmp);
                if (extensions.Contains(tmp))
                {
                    Debug.Log("Assets/StreamingAssets/" + file.Replace(Application.streamingAssetsPath + "/", ""));
                    AssetBundleHandle.SetAssetBundleName("Assets/StreamingAssets/" + file.Replace(Application.streamingAssetsPath + "/", ""));
                }

            }
         */
    }

    [MenuItem("JustTest/ForMobileCopyTest/遍历StreamAssets_lua目录并生成文件列表写到FileListData.asset")]
    static void FillToFileListDataAsset()
    {
       // string path = Application.streamingAssetsPath + "/Lua";

        string path = Application.dataPath + "/GameData/Lua";

        //string extensions = "*.bytes";
        string extensions = "*.bytes";

        FileListData data = AssetDatabase.LoadAssetAtPath<FileListData>("Assets/Resources/ScriptObjs/FileListData.asset");
        data.Files.Clear();

        //Debug.Log(path);

        string replaceStr = Application.dataPath.Replace("Assets", "");

        foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
        {
            //UnityEditor.FileUtil.DeleteFileOrDirectory(file);
            string tmp = Path.GetExtension(file).ToLower();
            //Debug.Log(file + " " + tmp);
            if (extensions.Contains(tmp))
            {
                Debug.Log(file);
                data.Files.Add(file.Replace(replaceStr, ""));
            }

        }

        EditorUtility.SetDirty(data);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        
    }


   
}
