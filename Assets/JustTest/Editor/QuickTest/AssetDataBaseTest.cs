using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class AssetDataBaseTest {

    [MenuItem("QuickTest/AssetDataBase/取的依赖GetDependencies")]
	static public void Call0 () {

        //string[] str = AssetDatabase.GetDependencies("Assets/Resources/fish/cruscarp.fbx");
        string[] str = AssetDatabase.GetDependencies("Assets/Resources/fish/cruscarp.prefab");
        //string[] str = AssetDatabase.GetDependencies("Assets/Materials/No Name.mat");
        Debug.Log("str:" + str.Length);

        //有个比较特别的是会包含自己
        for (int i = 0; i < str.Length; i++)
            Debug.Log("str:" + str[i]);

	}

    [MenuItem("QuickTest/AssetDataBase/AutoSetAbName")]
    static public void AutoSetAbName()
    {

        Debug.Log(AssetDatabase.GenerateUniqueAssetPath("Assets/_Images/load_bk.png"));
       // Debug.Log(AssetDatabase.GenerateUniqueAssetPath("efg/efg"));

        return;
        string[] folders = AssetDatabase.GetSubFolders("Assets/GameRes");
        for (int i = 0; i < folders.Length; i++)
        {
            
             string[] guids = AssetDatabase.FindAssets("",new string[]{folders[i]});
             for (int j = 0; j < guids.Length; j++)
             {
                 string assetPath = AssetDatabase.GUIDToAssetPath(guids[j]);
                 if (!AssetDatabase.IsValidFolder(assetPath))
                 {
                     AssetImporter import = AssetImporter.GetAtPath(assetPath);

                     Debug.Log(Path.GetDirectoryName(import.assetPath) + "   " + Path.GetFileNameWithoutExtension(import.assetPath));
                     
                     if (import != null)
                     {
                         import.assetBundleName = Path.GetDirectoryName(import.assetPath);
                         import.SaveAndReimport();
                     }
                     else
                     {
                         Debug.Log("Null Import:" + assetPath);
                     }
                     
                 }
                  
             }            
        }
        
    }


    //遍历子目录,子文件设定ab名称 path: 绝对路径
    
    static void SetAbNameWithFolder(string path)
    {       
        string appdataPath = Application.dataPath.Replace('/', '\\');
        Debug.Log(appdataPath);

        
        
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] allFileInfos = dirInfo.GetFiles();

        string abName = "Assets/GameRes/" + path;
        
        for (int j = 0; j < allFileInfos.Length; j++)
        {
            if (Path.GetExtension(allFileInfos[j].Name).ToLower().CompareTo(".meta") != 0)
            {

                string assetpath = "Assets" + (allFileInfos[j].FullName).Replace(appdataPath, "").Replace('\\', '/');
                Debug.Log("fileName:" + assetpath);
                
                AssetImporter import = AssetImporter.GetAtPath(assetpath);
                if (import == null)
                {
                    Debug.Log("Empty");
                    continue;
                }


                import.assetBundleName = abName;
                import.SaveAndReimport();
            }
        }

        foreach(DirectoryInfo info in dirInfo.GetDirectories())
        {
            SetAbNameWithFolder(info.FullName);
        }
    }


    //FindAsset By label
    [MenuItem("QuickTest/AssetDataBase/FindAssetByLabel")]
    public static void FindAssetByLabel()
    {
        string[] guids = AssetDatabase.FindAssets("l:MyAddLabel");
        for (int i = 0; i < guids.Length; i++)
            Debug.Log("FindAssetByLabel " + AssetDatabase.GUIDToAssetPath(guids[i]));
    }

	

      //FindAsset By label
    [MenuItem("QuickTest/AssetDataBase/列出所有AbName及包含的资源")]
    public static void ListAllAbName()
    {
        var names = AssetDatabase.GetAllAssetBundleNames();

        foreach (var name in names)
        {
            Debug.Log("AssetBundle Name: " + name);
            var paths1 = AssetDatabase.GetAssetPathsFromAssetBundle(name);
            foreach (var name1 in paths1)
                Debug.Log("---------------Asset Path: " + name1);
        }
    }



    [MenuItem("Assets/AssetDataBase/批量图片生成sprite预设")]
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



                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                if (sprite != null)
                {
                    GameObject go = new GameObject(sprite.name);
                    go.AddComponent<SpriteRenderer>().sprite = sprite;
                    allPath = spriteDir + "/" + sprite.name + ".prefab";
                    string prefabPath = allPath.Substring(allPath.IndexOf("Assets"));
                    PrefabUtility.CreatePrefab(prefabPath, go);
                    GameObject.DestroyImmediate(go);
                }



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
