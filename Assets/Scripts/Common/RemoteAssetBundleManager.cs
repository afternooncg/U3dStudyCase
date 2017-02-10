using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class RemoteAssetBundleManager : MonoBehaviour
{

    private  int loaderCount = 5;                 //最大同时加载队列数

    private  List<RemoteFileLoader> loaders = new List<RemoteFileLoader>();
    private  Queue<RemoteFileLoader.LoadInfo> loadQueue = new Queue<RemoteFileLoader.LoadInfo>();
    private AssetFilesVersionHandle m_versionHandle;

    static RemoteAssetBundleManager m_instance;

    public static RemoteAssetBundleManager Instance
    {
        get
        {


            //if ( instance == null )
            if (object.ReferenceEquals(m_instance, null))
            {
                if (!Application.isPlaying)
                {
                    Debug.Log("Tried accessing RemoteAssetBundleManager while not playing the game (e.g. Editor code).  This is bad.");
                    return null;
                }

                GameObject gameData = new GameObject("_CC2RemoteAssetBundleManager");

                m_instance = gameData.AddComponent<RemoteAssetBundleManager>();
                

                DontDestroyOnLoad(m_instance.gameObject);

            }


            return m_instance;
        }
    }

    void Awake()
    {
        CreateLoader();
    }


    public void Init()
    {
        if (m_versionHandle == null)
        {
            m_versionHandle = gameObject.AddComponent<AssetFilesVersionHandle>();
            m_versionHandle.Init(UpdateRemoteFiles);
        }
        else
            StartCoroutine(m_versionHandle.StartCheckeRemoteAssets());
    }

    void UpdateRemoteFiles(ref List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            LoadAsset(list[i], loadAssetAndSaveLocal, null);        
        }
        
    }

    void loadAssetAndSaveLocal(DownloadHandler handler, string path)
    {
        Debug.Log("save path " + path);
        FileHelper.CreateBinFile(Path.Combine(AssetFilesVersionHandle.PersiterAssetSavePathRoot, path), handler.data, handler.data.Length);
    }

    #region 加载资源文件
  public void LoadAsset(string assetName, RemoteFileLoader.OnLoadFinished loadFinished = null, RemoteFileLoader.OnLoadProgress loadProgress = null)
    {
        RemoteFileLoader.LoadInfo li = new RemoteFileLoader.LoadInfo()
        {
            assetName = assetName,                                    
            loadFinished = loadFinished,
            loadProgress = loadProgress,
            remoteUrl = PubConfig.RemoteWWWRoot
        };        

        loadQueue.Enqueue(li);                
        CheckLoadQueue();
    }
  #endregion
        
    #region 创建加载器队列
    private void CreateLoader()
    {
        DestroyLoader();
        for (int i = 0; i < loaderCount; i++)
        {
            RemoteFileLoader item = new GameObject("_RemoteFileLoader").AddComponent<RemoteFileLoader>();  //{ hideFlags = HideFlags.HideAndDontSave }
            loaders.Add(item);
        }
    }
    #endregion

    #region 销毁加载器队列
    private void DestroyLoader()
    {
        foreach (RemoteFileLoader loader in loaders)
        {
            UnityEngine.Object.Destroy(loader.gameObject);
        }
        loaders.Clear();
    }
    #endregion

    #region 从队列获取可用请求
    private  RemoteFileLoader.LoadInfo GetLoadInfo()
    {
        if (loadQueue.Count > 0)
        {
            return loadQueue.Dequeue();
        }
        return null;
    }
    #endregion

    #region 检测是否有加载请求可用
    private void CheckLoadQueue()
    {
        if (loadQueue.Count > 0)
        {
            foreach (RemoteFileLoader loader in loaders)
            {
                if (!loader.IsLoading)
                {
                    RemoteFileLoader.LoadInfo loadInfo = GetLoadInfo();
                    if (loadInfo == null)
                    {//空或已在队列的丢弃
                        break;
                    }


                    LoadAsset(loader, loadInfo);
                }
            }
        }
    }
    #endregion

    private void LoadAsset(RemoteFileLoader loader, RemoteFileLoader.LoadInfo li)
    {
        loader.BeginLoad(li);
    }

    // Update is called once per frame
    void Update()
    {

        if (loadQueue.Count > 0)
            CheckLoadQueue();
    }


}
