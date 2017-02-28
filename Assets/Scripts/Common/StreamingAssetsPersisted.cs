using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class StreamingAssetsPersisted : MonoBehaviour
{

    private int m_loaderCount = 5;                         //加载队列长度

    private int m_total = 0;                        //总共加载文件
    private int m_count = 0;                        //当前已加载

    private List<UnityWwwLoader> m_loaders = new List<UnityWwwLoader>();
    private Queue<UnityWwwLoader.LoadInfo> loadQueue = new Queue<UnityWwwLoader.LoadInfo>();

    public Action OnCopyFilesComplete;
    public Action<string> OnCopyOneFileComplete;



    void Awake()
    {
        CreateLoader();
    }


    void Start()
    {
        FileListData data = Resources.Load<FileListData>("ScriptObjs/FileListData");
        m_total = data.Files.Count;


        for (int i = 0; i < m_total; i++)
        {
            string assetName = data.Files[i].ToLower() + ".unity3d";
            LoadAsset(assetName, loadAssetAndSaveLocal, null);
        }

    }


    void loadAssetAndSaveLocal(WWW www, UnityWwwLoader.LoadInfo li)
    {
        string path = li.assetName;
        if(www.bytes.Length > 0)
            FileHelper.CreateBundleFile(Path.Combine(PubConfig.PersiterPath + "/streamingAssets", path), www.bytes);
        else
            FileHelper.CreateTxtFile(Path.Combine(PubConfig.PersiterPath + "/streamingAssets", path + "1"), "ss");
             
        m_count++;


        if (OnCopyOneFileComplete != null)
            OnCopyOneFileComplete(www.url + " " + www.bytes.Length + " " + www.error);

        if (m_count >= m_total)
        {
            
            Debug.Log("销毁StreamingAssetPersited");
            
            if (OnCopyFilesComplete != null) 
                OnCopyFilesComplete();

            OnCopyFilesComplete = null;
             GameObject.Destroy(gameObject);

        }
    }

    #region 加载资源文件
    private void LoadAsset(string assetName1, UnityWwwLoader.OnLoadFinished loadFinished = null, UnityWwwLoader.OnLoadProgress loadProgress = null)
    {
        UnityWwwLoader.LoadInfo li = new UnityWwwLoader.LoadInfo()
        {
            assetName = assetName1,
            loadFinished = loadFinished,
            loadProgress = loadProgress,

#if UNITY_EDITOR
            remoteUrl = "file://" + Application.streamingAssetsPath + "/",
#else
          remoteUrl = Application.streamingAssetsPath + "/",
#endif

        };

        loadQueue.Enqueue(li);
        CheckLoadQueue();
    }
    #endregion

    #region 创建加载器队列
    private void CreateLoader()
    {
        DestroyLoader();
        for (int i = 0; i < m_loaderCount; i++)
        {
            UnityWwwLoader item = new GameObject("_WwwFileLoader").AddComponent<UnityWwwLoader>();  //{ hideFlags = HideFlags.HideAndDontSave }
            m_loaders.Add(item);
        }
    }
    #endregion

    #region 销毁加载器队列
    private void DestroyLoader()
    {
        foreach (UnityWwwLoader loader in m_loaders)
        {
            UnityEngine.Object.Destroy(loader.gameObject);
        }
        m_loaders.Clear();
    }
    #endregion

    #region 从队列获取可用请求
    private UnityWwwLoader.LoadInfo GetLoadInfo()
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
            foreach (UnityWwwLoader loader in m_loaders)
            {
                if (!loader.IsLoading  && !loader.isStartForLoad)
                {
                    UnityWwwLoader.LoadInfo loadInfo = GetLoadInfo();
                    if (loadInfo == null)
                    {//空或已在队列的丢弃
                        break;
                    }

                    loader.BeginLoad(loadInfo);
                }
            }
        }
    }
    #endregion


    // Update is called once per frame
    void Update()
    {

        if (loadQueue.Count > 0)
            CheckLoadQueue();
    }


    void OnDestroy()
    {
        this.DestroyLoader();
    }
}
