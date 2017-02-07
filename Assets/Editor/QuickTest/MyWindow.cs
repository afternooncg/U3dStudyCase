using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.AnimatedValues;

public class MyWindow : EditorWindow {


    public enum MyEnum
	{
		Test_1,
		
		Test_2,
		
		Test_3,
		
		Test_4,
		
		Test_5,
	}


    AnimBool m_ShowExtraFields;

    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    GUIContent[] options = new GUIContent[] { new GUIContent("Rigidbody"), new GUIContent("Box Collider"), new GUIContent("Sphere Collider") };
	int index = 0;

    [MenuItem("QuickTest/EditorWindow/OpenMyWindow")]
    public static void ShowWin()
    {
        MyWindow win = EditorWindow.GetWindow<MyWindow>();
       // win.position = new Rect(100, 100, 300, 300);  // 窗口的坐标
    }


    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);  //单行标题
        GUILayout.Label(myString, EditorStyles.boldLabel);  //单行标题

        myString = EditorGUILayout.TextField("Text Field", myString); //文本框

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled); //可以启用或禁用ToggleGroup包围版块

        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("MyFloat", myFloat,0,10f);
        EditorGUILayout.EndToggleGroup();

        //简单水平容器
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("显示通知"))
            ShowNotification(new GUIContent("显示文本消息通知！"));
        if (GUILayout.Button("清除通知"))
            RemoveNotification();

        EditorGUILayout.ObjectField("ss",Selection.activeObject,typeof(GameObject),true);

        GUILayout.EndHorizontal();        

        if (GUILayout.Button("定制长宽按钮", new GUILayoutOption[] { GUILayout.Width(200), GUILayout.Height(50) }))
        {
            myString = "更新mystring";

            this.Repaint();
        }

        m_ShowExtraFields.target = EditorGUILayout.ToggleLeft("Show extra fields", m_ShowExtraFields.target);

        if (EditorGUILayout.BeginFadeGroup(m_ShowExtraFields.faded))
        {                               
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("显示通知1"))
                ShowNotification(new GUIContent("显示文本消息通知！"));
            if (GUILayout.Button("清除通知1"))
                RemoveNotification();
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFadeGroup();
        index = EditorGUILayout.Popup(new GUIContent("测试列表"),index, options);

        EditorGUILayout.HelpBox("什么东西啊", MessageType.Info);

        //EditorGUILayout.Popup(new GUIContent("测试列表1"), index);
        
    }

    public void Awake()
    {
        //在资源中读取一张贴图
        Debug.Log("Awake is Call");
    }
 
    void OnFocus()
    {
        Debug.Log("OnFocus");
    }

    void OnLostFocus()
    {
        Debug.Log("OnLostFocus");
    }

    void OnEnable()
    {
        m_ShowExtraFields = new AnimBool(true);
        m_ShowExtraFields.valueChanged.AddListener(Repaint);
    }

    void OnDisable()
    {
        Debug.Log("script was disable");
    }

    void OnDestroy()
    {
        Debug.Log("OnDestory");
    }

    void OnHierarchyChange()
    {
        Debug.Log("当Hierarchy视图中的任何对象发生改变时调用一次");
    }

    void OnProjectChange()
    {
        Debug.Log("当Project视图中的资源发生改变时调用一次");
    }

    void OnInspectorUpdate()
    {
        //Debug.Log("窗口面板的更新");
        //这里开启窗口的重绘，不然窗口信息不会刷新
        this.Repaint();
    }


    void Update()
    {
        if (index == 1)
        {
            myString = options[index].text;
        }
    }
}
