using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TestAssetbundleMain : MonoBehaviour
{

    List<AssetBundle> m_listMatAbs;

    // Use this for initialization
    void Start()    {
        
        m_listMatAbs = new List<AssetBundle>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region 加载3个mat ab，包含共同的shader(单独打包)

    public void handleBtnLoad3Mat()
    {
#if UNITY_EDITOR

        string abPathRoot = Application.dataPath.Replace("Assets", "PersiterData/AssetBundles/");

        AssetBundleManifest main = AssetBundle.LoadFromFile(abPathRoot + "AssetBundles").LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        string path = abPathRoot + "/assets/justtest/assetbundle/resource/materials/";

        AssetBundle ab = AssetBundle.LoadFromFile(path + "mat1.mat.unity3d");
        if (main != null)
        {
            string[] dps = main.GetDirectDependencies(ab.name);
            for (int i = 0; i < dps.Length; i++)
            {
                Debug.Log(dps[i]);
                //m_listMatAbs.Add(AssetBundle.LoadFromFile(dps[i].Substring(0,dps[i].LastIndexOf("."))));     
                m_listMatAbs.Add(AssetBundle.LoadFromFile(abPathRoot + dps[i]));

            }
        }


        m_listMatAbs.Add(ab);
        m_listMatAbs.Add(AssetBundle.LoadFromFile(path + "mat2.mat.unity3d"));
        m_listMatAbs.Add(AssetBundle.LoadFromFile(path + "mat3.mat.unity3d"));

        Debug.Log("load end!");

#endif
    }
    #endregion


    #region 加载Asset 观察结果: shader如果不单独打包，那么会被重复打入mat1-3,加载时内存中也存在3份copy

    public void handleBtnLoad3MatAsset()
    {
        Debug.Log("begin load asset");
        for (int i = 1; i < m_listMatAbs.Count; i++)
        {
            if (i == 0)
                continue;
            Material m = m_listMatAbs[i].LoadAsset<Material>("mat" + i.ToString() + ".mat");
            if (m != null)
                Debug.Log("load m " + i);
        }
    }
    #endregion



    #region 测试单独Loader
 public void handleTestLoader()
    {

        RemoteFileLoader.LoadInfo li = new RemoteFileLoader.LoadInfo();
        li.assetName = "Assetbundle/assets/justtest/assetbundle/resource/materials/mat1.mat.unity3d";
        li.remoteUrl = PubConfig.RemoteWWWRoot ;
        li.loadProgress = loadindMat;
        li.loadFinished = loadAssetAndSaveLocal;
        li.autoDestroy = true;

        GameObject.Find("Text").GetComponent<Text>().text = "loading:  " + li.assetName + "\n";
        RemoteFileLoader loader = new GameObject("testLoader").AddComponent<RemoteFileLoader>();
        loader.BeginLoad(li);


        RemoteFileLoader.LoadInfo li1 = new RemoteFileLoader.LoadInfo();
        li1.assetName = "Assetbundle/assets/justtest/assetbundle/resource/materials/mat1.mat.unity3d";
        li1.remoteUrl = PubConfig.RemoteWWWRoot ;
        li1.loadProgress = loadindMat;
        li1.loadFinished = loadAssetAndSaveLocal1;
        li1.autoDestroy = true;

        GameObject.Find("Text").GetComponent<Text>().text += "loading:  " + li.assetName + "\n";
        RemoteFileLoader loader1 = new GameObject("testLoader1").AddComponent<RemoteFileLoader>();
        loader1.BeginLoad(li1);
    }


    void loadindMat(int step,float progress)
    {
        Debug.Log("loading... " + progress.ToString());

    }

    void loadAssetAndSaveLocal(DownloadHandler handle, RemoteFileLoader.LoadInfo li)
    {

        string path = li.assetName;

        string savepath = PubConfig.PersiterPath + "/IoTest/game_load.unity3d";

        byte[] data = handle.data;
        FileHelper.CreateBinFile(savepath, data, data.Length);

        GameObject.Find("Text").GetComponent<Text>().text += "loading... complete" ;
    }

    void loadAssetAndSaveLocal1(DownloadHandler handle, RemoteFileLoader.LoadInfo li)
    {    
        GameObject.Find("Text").GetComponent<Text>().text += "loading...the same complete";
    }
 #endregion


    public void handleTestRemoteManger()
    {

        RemoteAssetBundleManager.Instance.Init();
    
    }


    void OnDestroy()
    {
        for (int i = 0; i < m_listMatAbs.Count; i++)
            m_listMatAbs[i].Unload(true);

        m_listMatAbs.Clear();
    }

}
