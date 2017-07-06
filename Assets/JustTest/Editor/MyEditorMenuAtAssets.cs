using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using System;

public class MyEditorMenuAtAssets : ScriptableObject
{

    [MenuItem("Assets/MyEditor/AddChild")]
    static void AddChild()
    {
        Debug.Log("craet empty game object");
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);

        foreach (Transform transform in transforms)
        {
            GameObject newChild = new GameObject("_Child");
            newChild.transform.parent = transform;
        }
    }

    [MenuItem("Assets/MyEditor/AddForlder")]
    static void AddFolder()
    {
        Debug.Log("hello");

        if (!AssetDatabase.IsValidFolder("Assets/_Scenes"))
            AssetDatabase.CreateFolder("Assets", "_Scenes");

        if (!AssetDatabase.IsValidFolder("Assets/_Scripts"))
            AssetDatabase.CreateFolder("Assets", "_Scripts");

        if (!AssetDatabase.IsValidFolder("Assets/_Images"))
            AssetDatabase.CreateFolder("Assets", "_Images");

        if (!AssetDatabase.IsValidFolder("Assets/_Materials"))
            AssetDatabase.CreateFolder("Assets", "_Materials");

        if (!AssetDatabase.IsValidFolder("Assets/_Prefabs"))
            AssetDatabase.CreateFolder("Assets", "_Prefabs");

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        return;

        //菜单方法必须声明为static
        string name = "newScene";
        string path = "";
        if (Selection.activeObject)
        {
            path = AssetDatabase.GetAssetPath(Selection.activeObject);
            //如何path所指向的对象不是文件夹，则获取其上一级的路径
            if (Path.GetExtension(path) != "")
            {
                path = Path.GetDirectoryName(path);
            }
            path = Path.Combine(path + "/", name);
        }

        if (!Directory.Exists(path) && !string.IsNullOrEmpty(name))
        {
            //AssetDatabase.CreateAsset(sinfo,path);
            // Selection.activeObject=sinfo;
        }

    }


   



    [MenuItem("Assets/MyEditor/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        //打包资源到指定目录,注意目录必须先存在，如果不存在，会报错
        //Debug.Log(Directory.GetCurrentDirectory());
        
        //string path = string.Format("{0}/{1}", Directory.GetCurrentDirectory(), "BundleFiles");
        string path = string.Format("{0}/../{1}", Application.dataPath, "BundleFiles");
        
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        
        // 打开保存面板，获得用户选择的路径  "窗口名称",文件名，后缀名
        //string savePath = EditorUtility.SaveFilePanel("Save Resource", "", "", "");

        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        /*
        BuildPipeline.BuildAssetBundle有5个参数，
         * 第一个是主资源，
         * 第二个是资源数组，这两个参数必须有一个不为null，如果主资源存在于资源数组中，是没有任何关系的，如果设置了主资源，可以通过Bundle.mainAsset来直接使用它
           第三个参数是路径，一般我们设置为  Application.streamingAssetsPath + Bundle的目标路径和Bundle名称
           第四个参数有四个选项，BuildAssetBundleOptions.CollectDependencies会去查找依赖，BuildAssetBundleOptions.CompleteAssets会强制包含整个资源，BuildAssetBundleOptions.DeterministicAssetBundle会确保生成唯一ID，在打包依赖时会有用到，其他选项没什么意义
         
            PS.想打包进AssetBundle中的二进制文件，文件名的后缀必须为“.bytes”
         * string.Format("file://{0}/{1}", Application.streamingAssetsPath, bundlePath); 
            在安卓下路径不一样，如果是安卓平台的本地Bundle，需要用jar:file://作为前缀，并且需要设置特殊的路径才能加载 
            string.Format("jar:file://{0}!/assets/{1}", Application.dataPath, bundlePath);
         */
    }

    [MenuItem("Assets/MyEditor/Get AssetBundle names")]
    static void GetNames()
    {//遍历读取assetbundle对象名称

        /*
        var paths = AssetDatabase.GetAllAssetPaths();
        foreach (var path in paths)
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(path);
            var labels = AssetDatabase.GetLabels(asset);
            if (labels.Length > 0)
            {
                Debug.Log(asset.name + ": " + string.Join(",", labels));
            }
        }
         */

        

        AssetBundle aa = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/BundleFiles/BundleFiles");
        if (aa == null)
            Debug.Log("aa is null");
        else
        {
            AssetBundleManifest mf = aa.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            if (mf != null)
            {
                string[] strs = mf.GetAllAssetBundles();
                foreach (var str in strs)
                {
                    Debug.Log("FindAssets Name: " + Application.streamingAssetsPath + "/BundleFiles/" + str);
                }
            }
            else
                Debug.Log("mf Empty");
        }

        aa.Unload(true);
        

       
        
        var names = AssetDatabase.GetAllAssetBundleNames();
        
        foreach (var name in names)
        {
            Debug.Log("AssetBundle Name: " + name );                        
            var paths1 = AssetDatabase.GetAssetPathsFromAssetBundle(name);
            foreach (var name1 in paths1)
                Debug.Log("---------------Asset Path: " + name1);
        }

       
    }

    [MenuItem("Assets/MyEditor/CopyFloder")]
    static void CopyFloder()
    {
        //FileUtil.CopyFileOrDirectory(Application.dataPath + "/Example Assets", Application.dataPath + "/effect/Example Assets");
                

        try
        {
         //   FileUtil.MoveFileOrDirectory(Application.dataPath + "/Example_Assets", Application.dataPath + "/effect"); //报错
            //AssetDatabase.CopyAsset(Application.dataPath + "/Example_Assets", Application.dataPath + "/effect");
          //  AssetDatabase.CopyAsset("Assets/Example_Assets", "Assets/effect/Example_Assets");
            AssetDatabase.MoveAsset("Assets/Example_Assets", "Assets/effect/Example_Assets");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
       // AssetDatabase.MoveAsset(Application.dataPath + "/Example Assets", Application.dataPath + "/effect");
    }


    [MenuItem("Assets/MyEditor/AtlasMaker")]
    //读取路径的png图片，并生成prefab
    static private void MakeAtlas()
    {
        Debug.Log("MakeAtlas Call...");
        string spriteDir = Application.dataPath + "/Resources/Sprite";

        if (!Directory.Exists(spriteDir))
        {
            Directory.CreateDirectory(spriteDir);
        }

        DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/_Images/icons");        
        
        foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
        {
            Debug.Log("dirInfo:" + dirInfo.Name + "  count:" + dirInfo.GetFiles("*.png", SearchOption.AllDirectories).Length);
            foreach (FileInfo pngFile in dirInfo.GetFiles("*.png", SearchOption.AllDirectories))
            {
                string allPath = pngFile.FullName;
                string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
                Debug.Log("assetPath:" + assetPath);

                TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
             //   Debug.Log("textureImporter:" + textureImporter.textureType + "    " + textureImporter.textureFormat);

                Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                if (tex)
                    Debug.Log("tex:" + tex.name);

                /*
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                GameObject go = new GameObject(sprite.name);
                go.AddComponent<SpriteRenderer>().sprite = sprite;
                allPath = spriteDir + "/" + sprite.name + ".prefab";
                string prefabPath = allPath.Substring(allPath.IndexOf("Assets"));
                PrefabUtility.CreatePrefab(prefabPath, go);
                GameObject.DestroyImmediate(go);
                 */
            }
        }


        /*对应读取代码
          void Start () 
	        {
		        CreatImage(loadSprite("image0"));
		        CreatImage(loadSprite("image1"));
	        }
 
	        private void CreatImage(Sprite sprite ){
		        GameObject go = new GameObject(sprite.name);
		        go.layer = LayerMask.NameToLayer("UI");
		        go.transform.parent = transform;
		        go.transform.localScale= Vector3.one;
		        Image image = go.AddComponent<Image>();
		        image.sprite = sprite;
		        image.SetNativeSize();
	        }
 
	        private Sprite loadSprite(string spriteName){
		        return Resources.Load<GameObject>("Sprite/" + spriteName).GetComponent<SpriteRenderer>().sprite;
	        }
          */

        //设置纹理格式
        /*
        string AtlasName = new DirectoryInfo(Path.GetDirectoryName(assetPath)).Name;
        TextureImporter textureImporter = assetImporter as TextureImporter;
        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.spritePackingTag = AtlasName;
        textureImporter.mipmapEnabled = false;
         */
    }

}
