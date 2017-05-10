using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AutoBuild  {

    
    [MenuItem("QuickTest/AutoBuild/设置发布设定")]
    static void SetProjectBuildSetting()
    {
        AutoSetDefine.SetDefines();
    }

    [MenuItem("QuickTest/AutoBuild/生成项目")]
    static void AutoBuildProject()
    {

        string[] scenes = FindEnabledEditorScenes();
        string buildingPath = "";
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:

                buildingPath = Application.dataPath.Replace("Assets", "") + "/test.apk";
                break;

            case BuildTarget.iOS:
                
                break;

            default:
                
                break;
        }

        if (!Directory.Exists(Application.streamingAssetsPath + "/Assets"))
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Assets/");

        Debug.Log("复制GameData到StreamingAssets");
        FileUtil.CopyFileOrDirectory("Assets/GameData", "Assets/StreamingAssets/Assets/GameData");


        

        BuildProject(scenes, buildingPath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);

        
        FileUtil.DeleteFileOrDirectory("Assets/StreamingAssets/Assets/GameData");
        Debug.Log("删除GameData到StreamingAssets");

        AssetDatabase.Refresh();
    }


    [MenuItem("QuickTest/AutoBuild/查看Scenes")]

    static void ListScenes()
    { 
        string[] scenes = FindEnabledEditorScenes();
        for(int i=0;i<scenes.Length;i++)
            Debug.Log(scenes[i].ToString());
    }



    
    #region 获取可用场景
 private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }
 #endregion


 static void BuildProject(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
 {

     EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
     string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);

     if (res.Length > 0)
     {
         throw new Exception("BuildPlayer failure: " + res);
     }
 }

}
