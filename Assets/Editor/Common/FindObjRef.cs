using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


//用于查找指定资源的引用并列表查看

public class FindObjRef : EditorWindow {

    
    [MenuItem("Assets/检索资源引用")]
    public static void ShowWin()
    {
        Rect wr = new Rect(0, 0, 800, 500);
        FindObjRef win = EditorWindow.GetWindowWithRect<FindObjRef>(wr, true, "检查资源引用");
        //win.position = new Rect(100, 100, 300, 300);  // 窗口的坐标
        win.Show();        
    }


    
    private Vector2 scrollPositionInfo = Vector2.zero;
    private static List<string> m_Info = new List<string>();


    private string m_rootPath = string.Empty;                                                                                   //根路径    
    List<string> m_findfiles = new List<string>();                                                                              //待查文件,只在打开窗口初始化一次    
    List<string> m_withExtensions = new List<string>() { ".prefab", ".unity" };                                                 //待查类型

    //需要自行加入检查的目录
    private static string[] m_paths = new string[]{
        
        "Assets/GameData"   
    };
       

    public FindObjRef()
    { //初始化

        m_Info.Clear();
       
    }

    void OnEnable()
    {
        Debug.Log(Application.dataPath);
        m_findfiles.Clear();
        m_rootPath = Application.dataPath.Replace("Assets", string.Empty);
        initAllSearchFiles();
        Find();
    }


    //获取全部文件
    void initAllSearchFiles()
    {
        for (int i = 0; i < m_paths.Length; i++)
        {
            getFoldersFiles(m_paths[i]);
        }

        Debug.Log("共:" + m_findfiles.Count);
    }

    void getFoldersFiles(string path)
    {
        if (!Directory.Exists(path))
            return;

        string[] files = Directory.GetFiles(m_rootPath + path, "*.*", SearchOption.AllDirectories);        
        for (int i = 0; i < files.Length; i++)
        {
            if (m_withExtensions.Contains(Path.GetExtension(files[i]).ToLower()))
            {
                m_findfiles.Add(files[i]);
            }
        }

    }


    void OnGUI()
    {

        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 13;
        myStyle.normal.textColor = Color.white;

        GUILayout.Label("待检测目录", myStyle);
        GUILayout.TextArea(string.Join(";", m_paths));
        string str = Selection.activeObject != null ? "当前选中对象:" + AssetDatabase.GetAssetPath(Selection.activeObject) : "未选中对象";
        EditorGUILayout.HelpBox(str,MessageType.Info);

        if (GUILayout.Button("开始检查", new GUILayoutOption[] { GUILayout.Height(25) }))
        {
            Find();
        }


        if (m_Info.Count > 0)
        {
            scrollPositionInfo = GUILayout.BeginScrollView(scrollPositionInfo, GUILayout.Width(800), GUILayout.Height(500));
            foreach (string info in m_Info)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(info), typeof(UnityEngine.Object), GUILayout.Width(200));
                GUILayout.Label(info);
                if (GUILayout.Button("查看"))
                {
                    EditorApplication.ExecuteMenuItem("Assets/Export Package...");                    
                
                }
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }



    }






    private void Find()
    {
        RemoveNotification();

        if (EditorSettings.serializationMode != SerializationMode.ForceText)
        {
            ShowNotification(new GUIContent("项目未设置为SerializationMode.ForceText模式")); 
            return;
        }

        if (Selection.activeObject == null)
        {
            ShowNotification(new GUIContent("请在Project视图先选择一个对象"));
            return;
        }
        else if (AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(Selection.activeObject)))
        {
            ShowNotification(new GUIContent("文件夹对象无效"));
            return;
        }

        m_Info.Clear();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!string.IsNullOrEmpty(path))
        {
            string guid = AssetDatabase.AssetPathToGUID(path);
            int total = m_findfiles.Count;

            int startIndex = 0;

            if (startIndex >= m_findfiles.Count)
            {
                Debug.Log("匹配结束");
                if(m_Info.Count == 0)
                    ShowNotification(new GUIContent("未查到引用"));
                EditorApplication.update = null;
                return;
            }


            EditorApplication.update = delegate()
            {                

                for (int i = 0; i < 5; i++)
                {
                    string file = m_findfiles[startIndex];
                    bool isCancel = EditorUtility.DisplayCancelableProgressBar("匹配资源中", file, (float)startIndex / (float)total);
                                     
                    string str = File.ReadAllText(file);
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (Regex.IsMatch(str, guid))
                        {
                            m_Info.Add(file.Replace(m_rootPath,string.Empty));
                            Debug.Log(file, AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(file)));
                        }
                    }


                    startIndex++;
                    if (isCancel || startIndex >= m_findfiles.Count)
                    {
                        EditorUtility.ClearProgressBar();
                        EditorApplication.update = null;
                        startIndex = 0;
                        if (m_Info.Count == 0 && !isCancel)
                            ShowNotification(new GUIContent("未查到引用"));
                        Debug.Log("匹配结束");
                        return;
                    }
                }


            };
        }
    }

    [MenuItem("Assets/Find References", true)]
    static private bool VFind()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        return (!string.IsNullOrEmpty(path));
    }

    static private string GetRelativeAssetsPath(string path)
    {
        return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
    }

}
