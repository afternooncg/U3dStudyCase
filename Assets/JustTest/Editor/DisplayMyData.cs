using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Threading;

[System.Serializable]

public class MyData
{
    public int a;
    public int b;
}


//这个属性会被加到Component菜单下 %q ctrl+q  快捷方式
[AddComponentMenu("项目常用代码/DisplayMyData_显示数据  %q")]
[CreateAssetMenu]
public class DisplayMyData : ScriptableObject {

    public MyData myData;

    public int[] ary;

    public int num1;

    //隐藏不显示
    [HideInInspector]
    public int num2;

    [Range(0, 100)]
    public int num3;

    [MyRangeAttribute(0,100,"魔法值")]
    public int num4;
    public static DisplayMyData m_instance;
	// Use this for initialization
	void Start () {

        Debug.Log("开始加载Bundle");
    //    this.StartCoroutine(LoadDataHolder());
        Debug.Log("加载Bundle完毕");
	}
	
	// Update is called once per frame
	void Update () {

        
	}

    [ContextMenu("测试编辑时执行UpdateData")]
    void UpdateData ()
    {
        this.num1 += 1;
        //this.transform.Translate(new Vector3(-1, 0, 0));
    }


    [MenuItem("QuickTest/ScriptableObject/Create Data Asset")]

    static void CreateDataAsset()
    {

        //资料 Asset 路径

        string holderAssetPath = "Assets/Resources/";

        if (!Directory.Exists(holderAssetPath)) Directory.CreateDirectory(holderAssetPath);

        //建立实体

        DataHolder holder = ScriptableObject.CreateInstance<DataHolder>();
        holder.Init();
        //使用 holder 建立名为 dataHolder.asset 的资源

        AssetDatabase.CreateAsset(holder, holderAssetPath + "dataHolder.asset");

    }

    [MenuItem("QuickTest/ScriptableObject/Resoure load Data Asset")]
    static void ResoureLoadDataAsset()
    {

        //资料 Asset 路径

        DataHolder holder = (DataHolder)Resources.Load("dataHolder", typeof(DataHolder));
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("i:=" + holder.integers[i].ToString());
        }

    }


    [MenuItem("QuickTest/ScriptableObject/Create Data AssetBundle")]

    static void CreateDataAssetBundle()
    {

        // AssetBundle 的资料夹路径及副档名

        string targetDir = "_DataAssetBundles" + Path.DirectorySeparatorChar;

        string extensionName = ".assetData";

        //取得在 Project 视窗中选择的资源(包含资料夹的子目录中的资源)

        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        //建立存放 AssetBundle 的资料夹

        if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

        foreach (Object obj in SelectedAsset)
        {

            //只处理型别为 DataHolder 的资源

            if (!(obj is DataHolder)) continue;

            string targetFile = targetDir + obj.name + extensionName;

            Debug.Log("targetFile  " + targetFile);
            //建立 AssetBundle 默认根目录是项目目录
            
            //if (BuildPipeline.BuildAssetBundle(obj, null, targetFile, BuildAssetBundleOptions.CollectDependencies)) 
             //   Debug.Log(obj.name + " 建立完成");

           // else Debug.Log(obj.name + " 建立失败");

        }

    }



   

    private IEnumerator LoadDataHolder()
    {

        // AssetBundle 档案路径  Application.dataPath Assets目录

        string path = string.Format("file://{0}/../_DataAssetBundles/{1}.assetData", Application.dataPath, "dataHolder");
        Debug.Log("path :" + path);
        // 载入 AssetBundle

        WWW bundle = new WWW(path);

        //等待载入完成

        yield return bundle;

        //取出 dataHolder 资源的内容

        DataHolder holder = (DataHolder)bundle.assetBundle.mainAsset;

        //卸载 AssetBundle
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("i:=" + holder.strings[i].ToString());
        }

        bundle.assetBundle.Unload(false);

    }

    [MenuItem("QuickTest/ScriptableObject/SaveMyObj")]
    public static void SaveMyObj()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null)
        {
            Debug.Log("请先选择");

            return;
        }

        
               
        
        SerializeTest st = go.GetComponent<SerializeTest>();
        SerializeTest.MyObj my = new SerializeTest.MyObj();
        my.name = "aa";
        //my.newOjbect = PrefabUtility.CreateEmptyPrefab("Resour")
        st.ListObj.Add(my);

        EditorApplication.SaveScene();
        /*
        EditorUtility.SetDirty(go);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
         */


        EditorUtility.DisplayProgressBar("Searching", string.Format("Finding ({0}/{1}), please wait...", 0, 10), 1);

        EditorUtility.ClearProgressBar();
    }


    [MenuItem("QuickTest/ScriptableObject/CreateDataAssetFromScriptObject")]
    public static void CreateDataAssetFromScriptObject()
    {

        TestDataAsset pAsset = ScriptableObject.CreateInstance<TestDataAsset>();
        pAsset.CreateData();

        //自动改名
        //string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/JustTest/EditorTest/ScriptObj/DataAsset.asset");
        string assetPathAndName = ("Assets/JustTest/EditorTest/ScriptObj/DataAsset.asset"); //也会自动

        Object obj = AssetDatabase.LoadAssetAtPath("Assets/JustTest/EditorTest/ScriptObj/DataAsset.asset", typeof(TestDataAsset));

        if (obj!=null)
        {
            Debug.Log("yes .......");
            Selection.activeObject = null;
            AssetDatabase.DeleteAsset(assetPathAndName);
            AssetDatabase.SaveAssets();
            
        }


        UnityEngine.Debug.Log("Creating scriptable object at " + assetPathAndName);

        AssetDatabase.CreateAsset(pAsset, assetPathAndName);

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = pAsset;
    }

    [MenuItem("QuickTest/AssetDataBase/TestDependencies")]
    public static void TestDependencies()
    {
        string curPathName = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        string[] names = AssetDatabase.GetDependencies(new string[] { curPathName });  //该资源依赖的东东
        Debug.Log("curPathName " + curPathName);
        foreach (string name in names)
        {
            Debug.Log("dep:" + name);
        }

        Thread th = new Thread(new ParameterizedThreadStart(TestDependenciesInSubThread));
        th.Start(curPathName);
        
    }


    static public void TestDependenciesInSubThread(object strname)
    {
        string curPathName = (string)strname;
        string[] allGuids = AssetDatabase.FindAssets("t:Prefab t:Scene", new string[] { "Assets" });
        foreach (string guid in allGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string[] names1 = AssetDatabase.GetDependencies(new string[] { assetPath });  //依赖的东东
            foreach (string name in names1)
            {
                if (name == curPathName)
                {
                    Debug.Log("Find:" + assetPath);
                    break;
                }
            }

        }
    
    }

     [MenuItem("QuickTest/AssetDataBase/ListAllAlats")]
    static public void ListAllAlats()
    {


        string[] allGuids = AssetDatabase.FindAssets("t:Prefab t:Material", new string[] { "Assets/_Images/atlases" });
        Debug.Log(allGuids);
        foreach (string guid in allGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

           

            string grayassetPath = string.Empty;

            if (assetPath.EndsWith(".prefab"))
                grayassetPath = assetPath.Split('.')[0] + "_gray.prefab";
            else  if(assetPath.EndsWith(".mat"))
                grayassetPath = assetPath.Split('.')[0] + "_gray.mat";
            
            Debug.Log(assetPath);

            if (assetPath.IndexOf("_gray") >= 0)
            {
                continue;   
            }
            else
            {                
                GameObject graygo = AssetDatabase.LoadAssetAtPath<GameObject>(grayassetPath);
                if (assetPath.EndsWith(".prefab"))
                {
                    GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                    if (graygo == null)
                    {
                        Object target = PrefabUtility.CreateEmptyPrefab(grayassetPath) as Object;

                        //GameObject twoCube = PrefabUtility.InstantiatePrefab(go) as GameObject;
                        PrefabUtility.ReplacePrefab(go, target);
                    
                    }
                    else
                    {
                        PrefabUtility.ReplacePrefab(go, graygo);
                    }                
                
                }
                else if (assetPath.EndsWith(".mat"))
                {
                    Material mat = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                    Debug.Log(".MAT DOING" + " " + mat);
                    if (graygo == null)
                    {
                        Debug.Log(assetPath);
                        Debug.Log(mat.name + " " + assetPath);

                        Material mat1 = new Material(mat.shader);
                        mat1.mainTexture = mat.mainTexture;
                        //AssetDatabase.CreateAsset(mat1, grayassetPath);

                        
                        bool flag = AssetDatabase.CopyAsset(assetPath, grayassetPath );
                        if (flag)
                        {
                            Debug.Log("Copy Succ");
                        }
                        else
                            Debug.Log("Copy Fail");
                        AssetDatabase.Refresh();
                        //GameObject twoCube = PrefabUtility.InstantiatePrefab(go) as GameObject;
                        //PrefabUtility.ReplacePrefab(go, target);

                    }
                    else
                    {
                        //PrefabUtility.ReplacePrefab(go, graygo);
                    }            
                
                }
                    

                
                
            }
            

         //   AssetDatabase.CreateAsset(target, assetPath);

           // PrefabUtility.CreateEmptyPrefab()
            

        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}



//自定义脚本类面板

[CustomEditor(typeof(DisplayMyData))]
public class DisplayMyaDataInspactor : Editor
{

    //在OnInspectorGUI()中实现对Position、Rotation和Scale的显示。以及添加自定义的Button。
    //扩展Editor与扩展EditorWindow唯一的不同在于你需要重写的是OnInspectorGUI而不是OnGUI。另外，如果你想绘制默认的可编辑项，只需调用DrawDefaultInspector即可。
    public override void OnInspectorGUI()
    {
        
        GUILayout.Label("hello");
        serializedObject.Update();
        GUILayout.Button(new GUIContent("测试"), EditorStyles.miniButton, GUILayout.MaxWidth(100));
        GUILayout.Button(new GUIContent("测试"), EditorStyles.miniButtonLeft, GUILayout.MaxWidth(100));
        GUILayout.Button(new GUIContent("测试"), EditorStyles.miniButtonMid, GUILayout.MaxWidth(100));
        GUILayout.Button(new GUIContent("测试"), EditorStyles.miniButtonRight, GUILayout.MaxWidth(100));
                
        EditorGUILayout.PropertyField(serializedObject.FindProperty("num1"),true);
        Show(serializedObject.FindProperty("ary"));
        
        //EditorGUI.PropertyField()  手动控制位置
        DrawDefaultInspector();

        serializedObject.Update();
    }


    public static void Show(SerializedProperty list)
    {
        EditorGUILayout.PropertyField(list);
        EditorGUI.indentLevel += 1;
        for(int i = 0; i < list.arraySize; i++)
        {
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
        }
        EditorGUI.indentLevel -= 1;
    }
}



//自定义类型面板 EditorGUILayout和EditorGUI的区别在于，EditorGUI是手动排版而EditorGUILayout是自动排版
[CustomPropertyDrawer(typeof(MyData))]
public class MyDataDrawer : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Debug.Log("label.text:" + label.text + "---" + EditorGUI.indentLevel + "--" );
       
       
        int oldIndentLevel = EditorGUI.indentLevel;
        
      //  EditorGUI.PrefixLabel(position, label);
     //   EditorGUI.PrefixLabel(position, new GUIContent("MyData Str"));
        //EditorGUILayout.BeginVertical();
        /* 
        //EditorGUI.PropertyField(position, property.FindPropertyRelative("position"));
        EditorGUI.PropertyField(position, property.FindPropertyRelative("position"), GUIContent.none); //隐藏lable
         */
        
         Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent("MyData Str"));  //似乎只有最后1个有用
      //  EditorGUI.PrefixLabel(position, label);
        contentPosition.x += 50;
        contentPosition.y += 50;
        EditorGUI.indentLevel += 1;

        label = EditorGUI.BeginProperty(contentPosition, label, property);  //BeginProperty EndProperty 标签允许对当前块做删除和复制 右键菜单
        contentPosition.y += 18;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("a"), new GUIContent(property.FindPropertyRelative("a").name));
        contentPosition.y += 18;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("b"), new GUIContent(property.FindPropertyRelative("b").name));
                 EditorGUI.EndProperty();
                 EditorGUI.indentLevel -= 1;

        
        /*
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);  //contentPosition，不包含filed左边标签占有区域的剩余Rect

        if (position.height > 16f)
        {
            position.height = 16f;
            EditorGUI.indentLevel += 1;
            contentPosition = EditorGUI.IndentedRect(position);
            contentPosition.y += 18f;
        }
        
        
        contentPosition.width *= 0.75f; //整个标签的宽度
        EditorGUI.indentLevel = 0; //控制层级表现
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("a"), new GUIContent(property.FindPropertyRelative("a").name));
        contentPosition.x += contentPosition.width;
        contentPosition.width /= 3f;
        EditorGUIUtility.labelWidth = 14f;//指定左侧标签长度
       // EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("color"), new GUIContent("C"));
        
        EditorGUI.EndProperty();
        EditorGUILayout.EndVertical();
        EditorGUI.indentLevel = oldIndentLevel; 
         */

        EditorGUI.indentLevel += 1;
        EditorGUI.HelpBox(position, "hello", MessageType.Info);
        EditorGUI.indentLevel += 1;
        EditorGUI.TextField(position, "hello world");

    }
}







public class MyRangeAttribute : PropertyAttribute
{ // 这3个变量是在Inspector面板中显示的 
    public float min;	// 定义一个float类型的最大 
    public float max; // 定义一个float类型的最大 
    public string label; // 显示标签  
    // 在脚本（1）ValueRangeExample定义[MyRangeAttribute(0,100,"玩家魔法值")] // 就可以使用这个功能了，但是为什么这里的string label = ""呢 // 因为给了一个初值的话，在上面定义[MyRangeAttribute(0,100)]就不会报错了，不会提示没有2个参数的方法 
    public MyRangeAttribute(float min, float max, string label = "")
    {
        this.min = min; this.max = max; this.label = label;
    }
}



// 使用绘制器，如果使用了[MyRangeAttribute（0,100,&quot;lable&quot;）]这种自定义的属性<a title="抽屉" href="index.php?c=search&key=%E6%8A%BD%E5%B1%89" target="_blank">抽屉</a>
// 就执行下面代码对MyRangeAttribute进行补充
[CustomPropertyDrawer(typeof(MyRangeAttribute))]

/// &lt;summary&gt;
/// 脚本位置：要求放在Editor文件夹下，其实不放也可以运行
/// 脚本功能：对MyRangeAttribute脚本的功能实现
/// 创建事件：2015.07.26
/// &lt;/summary&gt;

// 一定要继承绘制器类 PropertyDrawer
public class MyRangeAttributeDrawer : PropertyDrawer
{
    // 重写OnGUI的方法（坐标，SerializedProperty 序列化属性，显示的文字）
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent lable)
    {
        // attribute 是PropertyAttribute类中的一个属性
        // 调用MyRangeAttribute中的最大和最小值还有文字信息，用于绘制时候的显示
        MyRangeAttribute range = attribute as MyRangeAttribute;
        // 判断传进来的值类型
        if (property.propertyType == SerializedPropertyType.Float)
        {
            EditorGUI.Slider(position, property, range.min, range.max, range.label);
        }
        else if (property.propertyType == SerializedPropertyType.Integer)
        {
            EditorGUI.IntSlider(position, property, (int)range.min, (int)range.max, range.label);
        }
    }


   
    
}