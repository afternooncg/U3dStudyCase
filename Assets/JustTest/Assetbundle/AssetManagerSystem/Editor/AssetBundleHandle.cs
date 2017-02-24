using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// 本项目的扩展插件菜单定义推荐放本文件,第3方随意
/// </summary>
public class AbToolMenu
{

    #region AssetBundle 相关
    [MenuItem("JustTest/AssetBundle/设置AbName")]
    static public void SetAllAssetBundle()
    {
        AssetBundleHandle.BatSetAssetBundle();
    }


    [MenuItem("JustTest/AssetBundle/生成Ab资源")]
    public static void BulidingAssetBundle()
    {
       if (!Directory.Exists(AssetBundleHandle.PersistentDataPath))
         Directory.CreateDirectory(AssetBundleHandle.PersistentDataPath);

        BuildPipeline.BuildAssetBundles(AssetBundleHandle.PersistentDataPath, BuildAssetBundleOptions.None, BuildTarget.Android);
        Debug.Log("test");
    }

    [MenuItem("JustTest/AssetBundle/生成Ab资源清单")]
    public static void BulidingAssetBundleListInfo()
    {

        AssetBundleHandle.BulidingAssetBundleListInfo();
    }


    #endregion



}




/// <summary>
/// 本类处理项目AssetBandle 分包相关处理
/// </summary>
public class AssetBundleHandle
{

    const string GameDataSystemPath = "Assets/JustTest/Assetbundle/Data/";
    const string ShareData_Path = GameDataSystemPath + "SharedAsset.asset";
    const string AssetBuild_Path = GameDataSystemPath + "AssetBuildPaths.asset";

    static string AbFileInfoPath = GameDataSystemPath + "AssetsFilesVersion.txt";//+ AbFilesInfoHandle.AbResVersionFileName;


    static public string PersistentDataPath
    {
        get { return Application.dataPath.Replace("Assets", "PersiterData/AssetBundles"); }
    }


    #region 从配置表批量设置资源的ab名
    static public void BatSetAssetBundle()
    {
        
        if (AssetDatabase.LoadAssetAtPath<AssetBuildPaths>(AssetBuild_Path) == null)
            ScriptableObjectUtility.CreateAsset<AssetBuildPaths>(AssetBuild_Path);

        if (AssetDatabase.LoadAssetAtPath<SharedAsset>(ShareData_Path) == null)
            ScriptableObjectUtility.CreateAsset<AssetBuildPaths>(ShareData_Path);

        AssetBuildPaths data = AssetDatabase.LoadAssetAtPath<AssetBuildPaths>(AssetBuild_Path);
        AssetBuildPathObj[] paths = data.AssetPath;

        for (int i = 0; i < paths.Length; i++)
        {

            if (paths[i].prefab == null)
            {
                Debug.LogWarning("GameObject 为空 id = " + i);
                continue;
            }

            string assetPath = AssetDatabase.GetAssetPath(paths[i].prefab);
            string abName = Path.GetDirectoryName(assetPath) + "/" + Path.GetFileName(assetPath) + ".unity3d";//Path.Combine(Path.GetDirectoryName(assetPath),Path.GetFileNameWithoutExtension(assetPath));
            Debug.Log("abName:" + abName);
            SetAssetBundleName(assetPath, abName);
            
        }


        SharedAsset sharedata = AssetDatabase.LoadAssetAtPath<SharedAsset>(ShareData_Path);

        //字体
        List<GameObject> gos = sharedata.Fonts;
        SetShareAssetAbName<GameObject>(gos);


        //合图
        gos = sharedata.Altases;
        Debug.Log(gos.Count);
        SetShareAssetAbName<GameObject>(gos);


        //纹理
        List<Texture> textures = sharedata.Textures;
        SetShareAssetAbName<Texture>(textures);

        
        //着色器
        List<Shader> shaders = sharedata.Shaders;
        SetShareAssetAbName<Shader>(shaders);

        //纹理
        List<Material> materials = sharedata.Materials;
        SetShareAssetAbName<Material>(materials);


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    #endregion

    #region 批量设置资源的ab名
    static public void SetAllAssetBundle()
    {
        if (AssetDatabase.LoadAssetAtPath<SharedAsset>(ShareData_Path) == null)
            ScriptableObjectUtility.CreateAsset<SharedAsset>(ShareData_Path);

        if (AssetDatabase.LoadAssetAtPath<AssetBuildPaths>(AssetBuild_Path) == null)
            ScriptableObjectUtility.CreateAsset<AssetBuildPaths>(AssetBuild_Path);


        //注意顺序，现处理单独打的包，再处理共享包,可以保证共享包不会被单独导出
        SetSingeleAbname();

        SetShareAbname();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }
 #endregion



    #region 设置ShareAsset资源的AbName
static public void SetShareAssetAbName<T>(List<T> coll)
    {

        foreach (T item in coll)
        {
            string path = AssetDatabase.GetAssetPath(item as Object);

            if (!string.IsNullOrEmpty(path))
            {
                string abName = Path.GetDirectoryName(path) + "/" + Path.GetFileName(path) + ".unity3d";//Path.Combine(Path.GetDirectoryName(assetPath),Path.GetFileNameWithoutExtension(assetPath));
                //Debug.Log("abName:" + abName);
                SetAssetBundleName(path, abName);
            }
            else
                Debug.LogWarning("异常ShareAssete资源:" + typeof(T));
        }

    }
#endregion


    //设置共享资源ab
    static public void SetShareAbname()
    {

        SharedAsset data = AssetDatabase.LoadAssetAtPath<SharedAsset>(ShareData_Path);

        //字体
        List<GameObject> gos = data.Fonts;
        SetAbNameWithName<GameObject>(gos, data.FontsShareName);


        //合图
        gos = data.Altases;
        Debug.Log(gos.Count);
        SetAbNameWithName<GameObject>(gos, data.AltasesShareName);


        //纹理
        List<Texture> textures = data.Textures;
        SetAbNameWithName<Texture>(textures, data.TexturesShareName);



        //着色器
        List<Shader> shaders = data.Shaders;
        SetAbNameWithName<Shader>(shaders, data.ShadersShareName);

    }


    //遍历处理
    static public void SetAbNameWithName<T>(List<T> coll, string name = "")
    {

        foreach (T item in coll)
        {
            string path = AssetDatabase.GetAssetPath(item as Object);

            if (!string.IsNullOrEmpty(path))
                SetAssetBundleName(path, name);
            else
                Debug.Log(string.Format("异常{0}资源 ", name));
        }

    }

    #region 设置指定位置资源的AssetBundleName
   static public void SetAssetBundleName(string assetPath, string abname = "")
    {
        AssetImporter import = AssetImporter.GetAtPath(assetPath);

        //Debug.Log(Path.GetDirectoryName(import.assetPath) + "   " + Path.GetFileNameWithoutExtension(import.assetPath));

        if (import != null)
        {
            //Debug.Log(import.assetBundleName + "_" + abname);

            //已设置过的不处理
            if (!string.IsNullOrEmpty(abname) && string.Compare(import.assetBundleName, abname.ToLower()) == 0)
                return;

            if (string.IsNullOrEmpty(abname))
                abname = Path.GetDirectoryName(import.assetPath);

            import.assetBundleName = abname;
            Debug.Log("指定命名:" + abname);
            import.SaveAndReimport();
        }
        else
            Debug.Log("Null Import:" + assetPath);

    }
   #endregion

    //遍历指定的文件夹,查找所有资源(包括子文件夹内)  单个资源指定abname
    public static void SetSingeleAbname()
    {

        /*
        AssetBuildPaths data = AssetDatabase.LoadAssetAtPath<AssetBuildPaths>(AssetBuild_Path);
        AssetBuildPathObj[] paths = data.AssetPath;

        for (int i = 0; i < paths.Length; i++)
        {

            string[] guids = AssetDatabase.FindAssets("", new string[] { paths[i].path });

            for (int j = 0; j < guids.Length; j++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[j]);

                if (AssetDatabase.IsValidFolder(assetPath))
                    continue;

                string abName = Path.GetDirectoryName(assetPath) + "/" + Path.GetFileNameWithoutExtension(assetPath) + ".unity3d";//Path.Combine(Path.GetDirectoryName(assetPath),Path.GetFileNameWithoutExtension(assetPath));
                Debug.Log("abName:" + abName);
                SetAssetBundleName(assetPath, abName);

            }
        }
        */
    }



    //生成清单文件
    public static void BulidingAssetBundleListInfo()
    {
        //先清理无用abname
        AssetDatabase.RemoveUnusedAssetBundleNames();


        var names = AssetDatabase.GetAllAssetBundleNames();

        AssetBundle ab = AssetBundle.LoadFromFile(PersistentDataPath + "/AssetBundles");

        AssetBundleManifest mf = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");


        StringBuilder sb = new StringBuilder();
        foreach (var name in names)
        {
            // Debug.Log("AssetBundle Name: " + name);

            sb.AppendLine(name + "," + mf.GetAssetBundleHash(name));
            /*
            var paths1 = AssetDatabase.GetAssetPathsFromAssetBundle(name);            
            foreach (var name1 in paths1)
                Debug.Log("---------------Asset Path: " + name1);
             */
        }

        ab.Unload(true);


        //写文件
        string path = Application.dataPath.Replace("Assets", "") + AbFileInfoPath;

        StreamWriter sw;
        if (!File.Exists(path))
        {
            sw = File.CreateText(path);
        }
        else
        {
            sw = new StreamWriter(File.OpenWrite(path));
        }

        sw.Write(sb.ToString());
        sw.Close();

        AssetDatabase.Refresh();

    }

}
