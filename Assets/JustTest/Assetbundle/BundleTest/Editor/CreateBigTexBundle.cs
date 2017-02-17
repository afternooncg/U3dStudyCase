using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class CreateBigTexBundle  {


    private static string savePath = Application.dataPath + Path.AltDirectorySeparatorChar + "JustTest/BundleTest/Tex/";
	
    [MenuItem("QuickTest/TestAssetBundle/BuildOneGo")]
    public static void Execute1()
    {
        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>( "Assets/JustTest/BundleTest/Tex/bg1.png");

        //BuildPipeline.BuildAssetBundle(tex, null, savePath + "ab1.assetbundle");
    }

    [MenuItem("QuickTest/TestAssetBundle/BuildTwoGo")]
    public static void Execute2()
    {
        List<Object> objs = new List<Object>();

        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>( "Assets/JustTest/BundleTest/Tex/bg1.png");
        objs.Add(tex);

        tex = AssetDatabase.LoadAssetAtPath<Texture2D>( "Assets/JustTest/BundleTest/Tex/bg2.png");
        objs.Add(tex);

       // BuildPipeline.BuildAssetBundle(null,objs.ToArray(), savePath + "ab2.assetbundle");

    }


    //打包mat,但是不打包mat关联的图
    [MenuItem("QuickTest/TestAssetBundle/BuildMatToAb")]
    public static void Execute3()
    {

       AssetBundle ab = AssetBundle.LoadFromFile("H:/StudyCode/u3d5_3/U3dStudy/BundleFiles/mymat");
       Material mts = ab.LoadAsset<Material>("mymat.mat");
       Debug.Log("mts:" + mts);

       if (Selection.activeGameObject != null)
       {
           Selection.activeGameObject.GetComponent<MeshRenderer>().sharedMaterial = mts;
       }

        //BuildPipeline.BuildAssetBundles()

    }
}
