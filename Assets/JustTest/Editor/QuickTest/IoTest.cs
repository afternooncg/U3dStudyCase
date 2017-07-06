using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class IoTest  {


    [MenuItem("Assets/Io/新项目增加")]
    static void AddFolder()
    {
        Debug.Log("hello");

        if (!AssetDatabase.IsValidFolder("Assets/_Scenes"))
            AssetDatabase.CreateFolder("Assets", "_Scenes");

        if (!AssetDatabase.IsValidFolder("Assets/_Scripts"))
            AssetDatabase.CreateFolder("Assets", "_Scripts");

        if (!AssetDatabase.IsValidFolder("Assets/_Images"))
            AssetDatabase.CreateFolder("Assets", "_Images");

        if (!AssetDatabase.IsValidFolder("Assets/_Materials"))
            AssetDatabase.CreateFolder("Assets", "_Materials");

        if (!AssetDatabase.IsValidFolder("Assets/_Prefabs"))
            AssetDatabase.CreateFolder("Assets", "_Prefabs");

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        return;

        //菜单方法必须声明为static
        string name = "newScene";
        string path = "";
        if (Selection.activeObject)
        {
            path = AssetDatabase.GetAssetPath(Selection.activeObject);
            //如何path所指向的对象不是文件夹，则获取其上一级的路径
            if (Path.GetExtension(path) != "")
            {
                path = Path.GetDirectoryName(path);
            }
            path = Path.Combine(path + "/", name);
        }

        if (!Directory.Exists(path) && !string.IsNullOrEmpty(name))
        {
            //AssetDatabase.CreateAsset(sinfo,path);
            // Selection.activeObject=sinfo;
        }

    }

    [MenuItem("QuickTest/Io/CheckPathExists")]
    public static void CheckExists()
    {
        string dir = "e:/somefolder";
        Debug.Log("System.IO.Directory.Exists(dir): " + System.IO.Directory.Exists(dir));

        DirectoryInfo dirInfo = new DirectoryInfo(dir);
        Debug.Log("DirectoryInfo.Exists: " + dirInfo.Exists);
                
        Debug.Log("File.Exists: " + File.Exists(dir));


        FileInfo fileInfo = new FileInfo(dir);
        Debug.Log("FileInfo.Exists: " + fileInfo.Exists);

        

        string filePath = "i:/testunity.txt";
        if (File.Exists(filePath))
            File.Delete(filePath);


        StreamWriter fw = File.CreateText(filePath);
        fw.WriteLine("hello world");
        fw.Close();


        fw = File.AppendText(filePath);
        fw.WriteLine("yes i do");
        fw.Close();


        AssetDatabase.CreateFolder("Assets", "hello");
        
    }

    [MenuItem("QuickTest/Io/ShowUnitySystemPath")]
    public static void ShowUnitySystemPath()
    {
        Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);
        Debug.Log("Application.streamingAssetsPath: " + Application.streamingAssetsPath);
        Debug.Log("Application.dataPath: " + Application.dataPath); 
    }

   
    [MenuItem("QuickTest/Io/复制指定后缀文件")]
    static void CopySpacelFiles()
    {
        string path = PubConfig.PersiterPath + "/streamingAssets";
        string path1 = PubConfig.PersiterPath + "/streamingAssets/new";

        if (!Directory.Exists(path1))
            Directory.CreateDirectory(path1);

        string withoutExtensions = "*.meta";
        string withoutExtensions1 = "*.cs";
        foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
        {
            //UnityEditor.FileUtil.DeleteFileOrDirectory(file);
            string tmp = Path.GetExtension(file).ToLower();
            if (!withoutExtensions.Contains(tmp) && !withoutExtensions1.Contains(tmp))
            {
                Debug.Log(file);
                File.Copy(file, path1 + file.Substring(file.LastIndexOf('\\')), true);
            }
                
        }



    }

    [MenuItem("QuickTest/Io/复制GameData目录到StreameAssets")]
    static void CopyGameDataToStreameAssets()
    {
        float time = Time.realtimeSinceStartup;

        //FileUtil.CopyFileOrDirectory(Application.dataPath + "/GameData", Application.streamingAssetsPath);
        FileUtil.CopyFileOrDirectory("Assets/GameData/UI", "Assets/StreamingAssets/Assets/GameData/UI");
        Debug.Log("复制耗时:" + (Time.realtimeSinceStartup - time).ToString());

        AssetDatabase.Refresh();
    }

    [MenuItem("QuickTest/Io/移动GameData目录到StreameAssets")]
    static void MoveGameDataToStreameAssets()
    {

        if (!System.IO.Directory.Exists(Application.streamingAssetsPath + "/Assets"))
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Assets/");

        return;

        float time = Time.realtimeSinceStartup;

        //FileUtil.CopyFileOrDirectory(Application.dataPath + "/GameData", Application.streamingAssetsPath);
        FileUtil.MoveFileOrDirectory("Assets/GameData/UI", "Assets/StreamingAssets/Assets/GameData/UI");

        FileUtil.MoveFileOrDirectory("Assets/StreamingAssets/Assets/GameData/UI", "Assets/GameData/UI");
        Debug.Log("移动耗时:" + (Time.realtimeSinceStartup - time).ToString());

        AssetDatabase.Refresh();
    }
       
}

