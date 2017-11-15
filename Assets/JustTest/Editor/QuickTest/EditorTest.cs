using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorTest  {


    static int onChangeCount = 0;

   [MenuItem("QuickTest/EditorTest面板相关/OnSelectionChange")] 
   public static void LogSelector()
   {
       Selection.selectionChanged += OnSelectionChange;     
           
   }

   private static void OnSelectionChange()
   {
       onChangeCount++;
       Debug.Log("Selection Changed...");

       if (onChangeCount >= 3)
       {
           Debug.Log("Selection Changed Remove...");
           Selection.selectionChanged -= OnSelectionChange;
       }

   }


   [MenuItem("QuickTest/EditorTest面板相关/Ping Selected实现gameobject高亮跳动提示效果")]
	private static void Ping() {
		if(!Selection.activeObject) {
			Debug.LogError("Select an object to ping");
			return;
		}
		EditorGUIUtility.PingObject(Selection.activeObject);

        string abc = "";
        Debug.Log(abc + "  " + abc.LastIndexOf(";") + "  " + abc.Length);
        if (abc.LastIndexOf(";") == (abc.Length - 1))
        {
            abc = abc.Substring(0, 0);
            Debug.Log("do");
        }
        Debug.Log(abc);
	}


   [MenuItem("QuickTest/EditorTest面板相关/CopySerialized")]
   private static void TestCopySerialized()
   {
       string path1 = "Assets/Resources/CloseFrame.prefab";
       string path2 = "Assets/Resources/CloseFrame1.prefab";

       GameObject go1 = AssetDatabase.LoadAssetAtPath<GameObject>(path1);
       PubCloseFrame pb1 = go1.GetComponent<PubCloseFrame>();

       GameObject go2 = new GameObject();
       PubCloseFrame pb2 = go2.AddComponent<PubCloseFrame>();
      // EditorUtility.CopySerialized(go1, go2);        直接这样是空的
        EditorUtility.CopySerialized(pb1, pb2);        
      // AssetDatabase.CreateAsset(go2, path2);
       PrefabUtility.CreatePrefab(path2, go2);

       //如果是系统的asset controll matailer,应该可以直接copy

       path1 = "Assets/Resources/MenuConfig.asset";
       path2 = "Assets/Resources/MenuConfig1.asset";

       MenuConfig m1 = AssetDatabase.LoadAssetAtPath<MenuConfig>(path1);
       MenuConfig m2 = ScriptableObjectUtility.CreateAsset<MenuConfig>(path2);
       EditorUtility.CopySerialized(m1, m2);      

   }
  
    


   [MenuItem("Assets/ContextMenuItemDemo右键菜单,菜单路径只能放在Assets下")]
   public static void ContextMenuItemDemo()
   {
       Debug.Log("Project 面板 Assets 目录右键点击触发！");
   }



   [MenuItem("QuickTest/EditorTest面板相关/SerializedObject 读取测试")]
   private static void TestSerializedObject()
   {    
       //SerializedObject  so =   new SerializedObject(AssetDatabase.LoadAssetAtPath("ProjectSettings/GraphicsSettings.asset",typeof(UnityEngine.Object)));
       SerializedObject so = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset")[0]);
       SerializedProperty sps = so.GetIterator();
       while (sps.NextVisible(true))
       {        
           if (sps.name.Equals("m_AlwaysIncludedShaders"))
           {
               Debug.Log(sps.name);

              

               for (int i = 0; i < sps.arraySize; i++)
               {
                   SerializedProperty item = sps.GetArrayElementAtIndex(i);
                   Debug.Log(item.objectReferenceValue);


                  //  item.objectReferenceValue = Shader.Find("shadername"); //设置值
               }
               /*
               while (its.NextVisible(true))
               {
                   Debug.Log(its.name);
               }*/

           }


       }

       SerializedProperty m_Shader = so.FindProperty("m_Deferred" + ".m_Shader");
       Debug.Log(m_Shader.GetArrayElementAtIndex(0));
       Debug.Log(m_Shader.objectReferenceValue);
       // so.ApplyModifiedProperties (); //save


       
   }
   
}
