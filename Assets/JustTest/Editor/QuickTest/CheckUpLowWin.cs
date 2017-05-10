using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CheckUpLowWin : EditorWindow
{


    [MenuItem("QuickTest/IO/检查文件大小写")]
    public static void ShowWin()
    {
        Rect wr = new Rect(0, 0, 800, 500);
        CheckUpLowWin win = EditorWindow.GetWindowWithRect<CheckUpLowWin>(wr, true, "检查资源文件大小写");
        //win.position = new Rect(100, 100, 300, 300);  // 窗口的坐标
        win.Show();
    }



    private Vector2 scrollPositionInfo = Vector2.zero;
    private static List<string> m_Info = new List<string>();

    bool m_selectAll = false;

    //需要自行加入检查的目录
    private static string[] m_paths = new string[]{

        //"Assets/Effect",
        "Assets/uLua/Lua/data",
        "Assets/uLua/Lua/logic",
        "Assets/uLua/Lua/ui"
    
    };

    void OnGUI()
    {

        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 13;
        myStyle.normal.textColor = Color.white;

        GUILayout.Label("待检测目录", myStyle);
        GUILayout.TextArea(string.Join("\n", m_paths));




        if (GUILayout.Button("开始检查", new GUILayoutOption[] { GUILayout.Height(25) }))
        {
            BeginCheck();

        
        }


        if (m_Info.Count > 0)
        {
            scrollPositionInfo = GUILayout.BeginScrollView(scrollPositionInfo, GUILayout.Width(800), GUILayout.Height(500));
            foreach (string info in m_Info)
            {
                GUILayout.Label(info);
            }

            GUILayout.EndScrollView();
        }

        EditorGUI.BeginChangeCheck();
        m_selectAll = GUILayout.Toggle(m_selectAll, "全选");
        if (EditorGUI.EndChangeCheck())
            Debug.Log("ok");


    }

    private void BeginCheck()
    {
        RemoveNotification();
        m_Info.Clear();

        for (int i = 0; i < m_paths.Length; i++)
        {
            checkFolder(m_paths[i]);
        }


        if (m_Info.Count == 0)
        {
            // EditorGUILayout.HelpBox("资源名称正常", MessageType.Info);
            ShowNotification(new GUIContent("资源名称正常！"));

        }

    }



    private void checkFolder(string path)
    {
        if (!Directory.Exists(path))
            return;
        string withoutExtensions = "*.meta";

        foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
        {
            //UnityEditor.FileUtil.DeleteFileOrDirectory(file);
            string tmp = Path.GetExtension(file).ToLower();
            if (withoutExtensions.Contains(tmp))
                continue;

            tmp = Path.GetFileName(file);
            if (!tmp.Equals(tmp.ToLower()))
            {
                m_Info.Add(file);
            }
        }
    }
}
