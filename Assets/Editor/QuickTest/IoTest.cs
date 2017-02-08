using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class IoTest  {

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
    


}

