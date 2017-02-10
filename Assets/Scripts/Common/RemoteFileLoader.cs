using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 单个文件资源加载器
/// </summary>
public class RemoteFileLoader : MonoBehaviour {

    /// <summary>
    /// 辅助类 用检查是否存在相同的请求
    /// </summary>
    public class WebRequestRef
    {
        // Fields
        public int refCount;
        public UnityWebRequest req;

        // Methods
        public WebRequestRef(UnityWebRequest req, int refCount)
        {
            this.req = req;
            this.refCount = refCount;
        }
    }
       

        private LoadInfo cachedLoadInfo;

        public bool IsLoading;
        private bool isStart;
        private bool isStartForLoad;

                

        //记录是否存在相同的url
        public static Dictionary<string, WebRequestRef> wwwMap = new Dictionary<string, WebRequestRef>();

        private static int loadingCount = 0;


        
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
                        

            string localUrl = li.remoteUrl + "/" + li.assetName;
            Debug.Log("loadformurl: " + localUrl);

            UnityWebRequest req = null;
            WebRequestRef iteratorVariable3 = null;


            if (!wwwMap.TryGetValue(localUrl, out iteratorVariable3))
            {
                req = UnityWebRequest.Get(localUrl);
                iteratorVariable3 = new WebRequestRef(req, 1);
                wwwMap[localUrl] = iteratorVariable3;
                req.Send();
            }
            else
            {
                req = iteratorVariable3.req;
                iteratorVariable3.refCount++;
            }

            

            while (!req.isDone)
            {
                if (req.error != null || req.responseCode!=200)
                {
                    Debug.LogWarning(string.IsNullOrEmpty(req.error) ?  localUrl + "  ResponseCode:" + req.responseCode : req.error);
                    break;
                }
                if (li.loadProgress != null)
                {
                    li.loadProgress(0, req.downloadProgress);
                }
                yield return null;
            }


            if (li.loadProgress != null)
            {
                li.loadProgress(0, 1f);
            }
          
            yield return null;

            if (li.loadFinished != null)
                li.loadFinished(req.downloadHandler, li.assetName);

            if (iteratorVariable3 != null)
            {
                iteratorVariable3.refCount--;
                if (iteratorVariable3.refCount <= 0)
                {                     
                    wwwMap.Remove(localUrl);
                    if (req != null)
                        req.Dispose();
                }
            }
            yield return null;

            if (li.autoDestroy)
            {
                Object.Destroy(this.gameObject);
            }
            
            
            
        }

        public IEnumerator Loading(LoadInfo li)
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
                base.StartCoroutine(this.Loading(this.cachedLoadInfo));
                this.isStartForLoad = false;
            }
            this.isStart = true;
        }

        public void BeginLoad(LoadInfo loadInfo)
        {
            if (this.isStart)
            {
                if (!this.IsLoading)
                {
                    base.StartCoroutine(this.Loading(loadInfo));
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
            
            public RemoteFileLoader.OnLoadFinished loadFinished;            
            public RemoteFileLoader.OnLoadProgress loadProgress;

            public bool autoDestroy = false;
        }

        public delegate void OnLoadFinished(DownloadHandler  handObj, string path);

        public delegate void OnLoadProgress(int step, float progress);
 #endregion
    
}
