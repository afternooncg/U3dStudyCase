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

  

   [MenuItem("Assets/ContextMenuItemDemo右键菜单,菜单路径只能放在Assets下")]
   public static void ContextMenuItemDemo()
   {
       Debug.Log("Project 面板 Assets 目录右键点击触发！");
   }


   
}
