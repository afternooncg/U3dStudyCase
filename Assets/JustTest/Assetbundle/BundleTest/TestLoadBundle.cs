using UnityEngine;
using System.Collections;
using System.IO;

//测试加载
//AssetBundle.LoadFromFile(path);  直接本地加载,方便测试
//WWW远程加载模式


public class TestLoadBundle : MonoBehaviour {

    public GameObject EmptySprite;

    public GameObject EmptyCube;


    public GameObject EmptySprite1;

    public GameObject EmptyCube1;

    public UISprite SpNgui;
    Texture m_txt = null;

	// Use this for initialization
	void Start () {

        StartCoroutine("LoadFromCache");
        return;
        Debug.Log("Application.platform " + Application.platform);



        TestPathApi();
        


    #if UNITY_ANDROID
        Debug.Log("IN ANDROID");
    #elif UNITY_IPHONE
		Debug.Log("IN IPHONE");
    #elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        Debug.Log("IN WIN");
    #else
        Debug.Log("IN ?");      
    #endif
            //比较重要的几个路径
        Debug.Log("Window下的Application.streamingAssetsPath ：" + Application.streamingAssetsPath);
        Debug.Log("Window下的Application.dataPath ：" + Application.dataPath);

       // string path = string.Format("file:///{0}/{1}/{2}", Directory.GetCurrentDirectory(), "BundleFiles", "BundleFiles");

        StartCoroutine("LoadBundleFile");

        /*
        string path = string.Format("{0}/../{1}/{2}", Application.dataPath, "Assetbundle", "Assetbundle");

        AssetBundle manifestBundle = AssetBundle.LoadFromFile(path);
        Debug.Log(manifestBundle);
        

        if (manifestBundle != null)
        {
            AssetBundleManifest manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");

            //获取依赖文件列表;
            string[] cubedepends = manifest.GetAllDependencies("assets/myresources/cube0.prefab");
            AssetBundle[] dependsAssetbundle = new AssetBundle[cubedepends.Length];

            for (int index = 0; index < cubedepends.Length; index++)
            {
                //加载所有的依赖文件;
                dependsAssetbundle[index] = AssetBundle.LoadFromFile(Application.dataPath
                                                                     + "/../Assetbundle/"
                                                                     + cubedepends[index]);


            }

            //加载我们需要的文件;"
            AssetBundle cubeBundle = AssetBundle.LoadFromFile(Application.dataPath
                                                              + "/../Assetbundle/assets/myresources/cube0.prefab");
            GameObject cube = cubeBundle.LoadAsset("Cube0") as GameObject;
            if (cube != null)
            {
                Instantiate(cube);
            }
        }
         */
	}
	
    

    IEnumerator LoadFromCache ()
	{

        float begin = Time.realtimeSinceStartup;
        /*
        Debug.Log("begin:" + begin);
        for (int i = 0; i < 100; i++ )
        {
            Debug.Log("hello" + i);
            yield return null;
        }
         */
        

        while (!Caching.ready)
			yield return null;
        const string AssetBundleRoot = "BundleFiles";
        string path = string.Format("file:///{0}/{1}/{2}", Directory.GetCurrentDirectory(), AssetBundleRoot, "altas1");
		var www = WWW.LoadFromCacheOrDownload(path, 1);
		yield return www;
		if(!string.IsNullOrEmpty(www.error))
		{
			Debug.Log(www.error);
            yield return null;
		}

        UIAtlas atlas = (www.assetBundle.LoadAsset("common") as GameObject ).GetComponent<UIAtlas>();
        Debug.Log("xxx:" + atlas.name + " ");
        SpNgui.atlas = atlas;
        SpNgui.spriteName = "bg_frame";
        /*
        AssetBundle sprites = www.assetBundle;
        Debug.Log("K:" + sprites.LoadAsset("1122"));
        Sprite sp = sprites.LoadAsset("1122") as Sprite;
         * 
       // Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        EmptySprite.GetComponent<SpriteRenderer>().sprite = sp;
		//var asset = myLoadedAssetBundle.mainAsset;
        */
        Debug.Log("end " + (Time.realtimeSinceStartup - begin));
	}


    IEnumerator  LoadBundleFile()
    {
        const string AssetBundleRoot = "BundleFiles";
        string path = string.Format("file:///{0}/{1}/{2}", Directory.GetCurrentDirectory(), AssetBundleRoot, "BundleFiles");
        WWW www = new WWW(path);
        yield return www;

        if(!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(string.Format("资源加载异常:{0},  path:{1}" , www.error, path));
            yield return null; 
        }

       AssetBundle asb =   www.assetBundle;  //主bundle比较特殊，只有主清单资源
        
        
       AssetBundleManifest asbManifest = asb.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
     


     //  string path1 = string.Format("file:///{0}/{1}/{2}", Directory.GetCurrentDirectory(), AssetBundleRoot, "bagsprite");
       string path1 = string.Format("{0}/../{1}/{2}", Application.dataPath, AssetBundleRoot, "bagsprite");
       AssetBundle sprites = AssetBundle.LoadFromFile(path1) as AssetBundle;


       //sprite换纹理
        Texture2D tex = sprites.LoadAsset("1122") as Texture2D;
        Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        EmptySprite.GetComponent<SpriteRenderer>().sprite = sp;

        //mat换纹理
       EmptyCube.GetComponent<Renderer>().material.mainTexture = sprites.LoadAsset("1122") as Texture2D;


       string path2 = string.Format("{0}/../{1}/{2}", Application.dataPath, AssetBundleRoot, "bagicon");
       AssetBundle icons = AssetBundle.LoadFromFile(path2) as AssetBundle;

       m_txt = icons.LoadAsset("1082") as Texture;
       Texture2D tex1 = icons.LoadAsset("1082") as Texture2D;

       
       Sprite sp1 = Sprite.Create(tex1, new Rect(0, 0, tex1.width, tex1.height), new Vector2(0.5f, 0.5f));
       EmptySprite1.GetComponent<SpriteRenderer>().sprite = sp1;

       //mat换纹理
       EmptyCube1.GetComponent<Renderer>().material.mainTexture = icons.LoadAsset("1083") as Texture2D;

        //由于后边代码有重复加载行为。必须先卸载
       sprites.Unload(true);
       icons.Unload(true);
     
             

        
        //读取一级子bundle
         string[] subAsbs = asbManifest.GetAllAssetBundles();
         AssetBundle[] arySubAsb = new AssetBundle[subAsbs.Length];
         for (int i = 0; i < subAsbs.Length;i++ )
         {
             Debug.Log(subAsbs[i]);
             string subpath = string.Format("{0}/{1}/{2}", Directory.GetCurrentDirectory(), AssetBundleRoot, subAsbs[i]);
             //AssetBundleManifest asbManifest = asb.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
             arySubAsb[i] = AssetBundle.LoadFromFile(subpath);
             if (arySubAsb[i] != null)
             {                                   

                 //读取依赖
                 string[] dps = asbManifest.GetAllDependencies(subAsbs[i]);

                 Debug.Log(subAsbs[i] + "的依赖:");
                 if (dps != null)
                 {
                    
                     for (int k = 0; k < dps.Length; k++)
                         Debug.Log("-----------------"+ dps[k]);
                 }
                 
                 
                 //加载全部指定类型
                 Object[] manis = arySubAsb[i].LoadAllAssets(typeof(Texture2D));
                 for (int j = 0; j < manis.Length; j++)
                     Debug.Log("主资源加载各类型资源" + manis[j]);
                 
                 Debug.Log("arySubsAsb[" + i + "] " + arySubAsb[i].name);  //显示bundle名字
             }
             
            
         }       
        




        // AssetBundle cubeBundle = AssetBundle.LoadFromFile(Application.dataPath + "/../BundleFiles/assets/myresources/cube0.prefab");
    }


	void OnGUI () {

        if (m_txt != null)
            GUI.DrawTexture(new Rect(0, 0, 100, 100), m_txt);
	
	}


    void TestPathApi()
    {

        //几个取当前路径的方法
        Debug.Log("System.Environment.CurrentDirectory: " + System.Environment.CurrentDirectory);
        Debug.Log("Directory.GetCurrentDirectory(): " + Directory.GetCurrentDirectory());

        if (Application.isWebPlayer)
        {//web模式读取才有值
            Debug.Log("Application.absoluteURL: " + Application.absoluteURL);
            Debug.Log("System.IO.Path.GetDirectoryName(Application.absoluteURL) " + System.IO.Path.GetDirectoryName(Application.absoluteURL));
        }
        else
        {
            Debug.Log("System.IO.Path.GetDirectoryName( Directory.GetCurrentDirectory()) " + System.IO.Path.GetDirectoryName(Directory.GetCurrentDirectory()));

        }        

    }
}
