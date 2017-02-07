using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorTest  {


    static int onChangeCount = 0;

   [MenuItem("QuickTest/Select/OnSelectionChange")] 
   public static void LogSelector()
   {
       Selection.selectionChanged += OnSelectionChange;     
           
   }


   [MenuItem("QuickTest/Ping Selected")]
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

   private static void OnSelectionChange()
   {
       onChangeCount++;
       Debug.Log("Selection Changed...");

       if(onChangeCount>=3)       
       {
           Debug.Log("Selection Changed Remove...");
           Selection.selectionChanged -= OnSelectionChange;     
       }
       
   }

   [MenuItem("QuickTest/EditorWin显示Lable")]//在这里方法中就可以绘制面板。   
   [ContextMenu("QuickTest/EditorWin显示Lable")]
   public static void ShowMyWin()
   {
       Debug.Log("hello");
   }


   [MenuItem("Assets/ContextMenuItemDemo")]
   public static void ContextMenuItemDemo()
   {
       Debug.Log("Project 面板 Assets 目录右键点击触发！");
   }


     [MenuItem("QuickTest/AssetImporter")]
   static public void AssetImporterTest()
   {
       string path = AssetDatabase.GetAssetPath(Selection.activeObject);
       AssetImporter import = AssetImporter.GetAtPath(path);
       import.userData = "MyDataSaveInMetaFile";         
       import.SaveAndReimport();
   }



     [MenuItem("QuickTest/BatSetTextureTypeAndCreateAssetBunldes")]
     public static void CreateAssetBunldes()
     {
         Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

         foreach (Object obj in SelectedAsset)
         {
             Debug.Log(AssetDatabase.GetAssetPath(obj));
             TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(obj));
             ti.textureType = TextureImporterType.GUI;
             ti.filterMode = FilterMode.Point;
             //ti.textureFormat = TextureImporterFormat.RGBA32;

             string targetPath = Application.dataPath + "/StreamAssets/" + obj.name + ".assetbundle";
             /*
             if (BuildPipeline.BuildAssetBundle(obj, null, targetPath))
             {
                 Debug.Log(obj.name + "资源打包成功");
             }
             else
             {
                 Debug.Log(obj.name + "资源打包失败");
             }*/
         }

         AssetDatabase.Refresh();

     }  
}
