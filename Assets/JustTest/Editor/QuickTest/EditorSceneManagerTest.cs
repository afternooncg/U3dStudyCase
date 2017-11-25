using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EditorSceneManagerTest  {

    [MenuItem("QuickTest/EditSceneManger/Open",false, 100)]
    public static void Open()
    {
        Debug.Log(EditorApplication.isPlaying + "  " + Application.isPlaying);
        Debug.Log("EditorApplication.applicationContentsPath:" + EditorApplication.applicationContentsPath);
        Debug.Log("EditorApplication.applicationPath:" + EditorApplication.applicationPath);

        EditorApplication.update += Update;
        EditorApplication.playmodeStateChanged += delegate()
        {
            Debug.Log("EditorApplication.isPlaying " + EditorApplication.isPlaying);
            if (!Application.isPlaying)
                EditorSceneManager.OpenScene("Assets/JustTest/PrefabEdit/PrefabEdit.unity");
        };        
       
        if (!Application.isPlaying)
        {
            EditorSceneManager.OpenScene("Assets/JustTest/PrefabEdit/PrefabEdit.unity");
        }
        else
        {
            UnityEditor.EditorApplication.isPlaying = false;   

        }


        
                                      
    }
    private static void Update()
    {
        Debug.Log(EditorApplication.isPlaying + "  " + Application.isPlaying);
        
    }

}
