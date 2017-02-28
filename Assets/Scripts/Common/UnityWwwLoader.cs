using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnityWwwLoader : MonoBehaviour
{

    /// <summary>
    /// 辅助类 用检查是否存在相同的请求
    /// </summary>
    public class WWWRequestRef
    {
        // Fields
        public int refCount;
        public WWW req;

        // Methods
        public WWWRequestRef(WWW req, int refCount)
        {
            this.req = req;
            this.refCount = refCount;
        }
    }


    private LoadInfo cachedLoadInfo;

    public bool IsLoading
    {
        get;
        private set;
    }
    public bool isStartForLoad             // 用于指示Start未执行前，先被设置了LoaderInfo等Loading的状态 = true
    {
        get;
        private set;
    }
    private bool isStart = false;                   //执行Start后设置为ture;
   



    //记录是否存在相同的url
    public static Dictionary<string, WWWRequestRef> wwwMap = new Dictionary<string, WWWRequestRef>();

    private static int loadingCount = 0;


    void Awake()
    {
        isStartForLoad  =  false;
    }
    public static bool IsObjLoading()
    {
        return loadingCount > 0;
    }



    private IEnumerator LoadAssetFromWWW(LoadInfo li)
    {
        if (li.loadProgress != null)
        {
            li.loadProgress(0, 0f);
        }
        yield return null;
        while (!Caching.ready)
        {
            yield return null;
        }

        string localUrl = li.remoteUrl + li.assetName;

       // Debug.Log("loadformurl: " + localUrl);

        WWW req = null;
        WWWRequestRef iteratorVariable3 = null;


        if (!wwwMap.TryGetValue(localUrl, out iteratorVariable3))
        {
            req = new WWW(localUrl);

            iteratorVariable3 = new WWWRequestRef(req, 1);
            wwwMap[localUrl] = iteratorVariable3;
            
        }
        else
        {
            req = iteratorVariable3.req;
            iteratorVariable3.refCount++;
        }

                
        while (!req.isDone)
        {
            /*
            if (req.error != null)
            {
                Debug.LogWarning( localUrl + " " +  req.error);
                break;
            }
             */
            if (li.loadProgress != null)
            {
                li.loadProgress(0, req.progress);
            }
            yield return null;
        }
        

        if (li.loadProgress != null)
        {
            li.loadProgress(0, 1f);
        }

        yield return null;

        
        if (li.loadFinished != null)
            li.loadFinished(req, li);
        

        if (iteratorVariable3 != null)
        {
            iteratorVariable3.refCount--;
            if (iteratorVariable3.refCount <= 0)
            {
                wwwMap.Remove(localUrl);
               
            }
        }
        yield return null;


        if (req != null)
            req.Dispose();


        if (li.autoDestroy)
        {
            Object.Destroy(this.gameObject);
        }



    }

     IEnumerator Loading(LoadInfo li)
    {        

        this.IsLoading = true;

        ++loadingCount;

        yield return this.StartCoroutine(this.LoadAssetFromWWW(li));

        --loadingCount;
        this.IsLoading = false;
    }





    private void Start()
    {
        Object.DontDestroyOnLoad(this);
       
        if (this.isStartForLoad)
        {
            if (this.cachedLoadInfo == null)
                return;
            base.StartCoroutine(this.Loading(this.cachedLoadInfo));
            this.isStartForLoad = false;
        }
        this.isStart = true;
    }

    public void BeginLoad(LoadInfo loadInfo)
    {
        if (loadInfo == null || this.isStartForLoad)
            return;

        if (this.isStart)
        {
            if (!this.IsLoading)
            {
                base.StartCoroutine(this.Loading(loadInfo));
                isStartForLoad = false;
            }
            else
            {
                Debug.Log("Start load " + loadInfo.assetName + " failed : loader is busy.");
            }
        }
        else
        {
            this.cachedLoadInfo = loadInfo;
            this.isStartForLoad = true;
        }
    }




    #region 加载辅助对象


    /// <summary>
    /// 加载器辅助类
    /// </summary>



    public class LoadInfo
    {
        // Fields
        public string assetName;

        public string remoteUrl;                                        //远程路径

        public string localSavePath;                                    //本地保存路径

        public UnityWwwLoader.OnLoadFinished loadFinished;
        public UnityWwwLoader.OnLoadProgress loadProgress;

        public bool autoDestroy = false;

        

    }

    public delegate void OnLoadFinished(WWW www, LoadInfo li = null);

    public delegate void OnLoadProgress(int step, float progress);
    #endregion
}
