using UnityEngine;
using System.Collections;

using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking;


public class AbVo
{
    public string hash;
    public bool isPersit;       //true 走 persit路径  flase 走包路径
}


public class AssetFilesVersionHandle : MonoBehaviour
{
    
    public const string ConfigFileName = "AssetsFilesVersion.txt";                        //配置文件名
    
    string m_PersiterConfigFileSavePath = "";                                                //设备配置文件保存位置
    string m_LocalConfigFileSavePath = "";                                                  //游戏内文件保存位置

    static string m_PersiterAssetSavePathRoot = "";                                         //设备AssetBunlde保存根目录

    public static string PersiterAssetSavePathRoot
    {
        get { return AssetFilesVersionHandle.m_PersiterAssetSavePathRoot; }
       
    }
    static string m_LocalAssetSavePathRoot = "";                                           //游戏内AssetBunlde根目录

    static string m_RemoteConfigUrl = "http://127.0.0.1:92/" + ConfigFileName;           //远程加载地址

    List<KeyValuePair<string, string>> m_data1;
    List<KeyValuePair<string, string>> m_data2;


    Dictionary<string, AbVo> m_Dict = new Dictionary<string, AbVo>();                   //存放加载信息
    
    public delegate  void LoadAbFilesInfoCallBack(string str);

    private LoadAbFilesInfoCallBack onGetAbFilesInfo;
    

    private List<string> m_NeedDownFiles = new List<string>();                          //需要加载的列表

    WWW m_www;

    int m_remoteVersion;
    int m_total;                                                                        //需要更新的资源数量
    UnityWebRequest m_req;

    public delegate void OnGetFilterLists(ref List<string> list);                       //回调
    public OnGetFilterLists OnGetFilterListsCallBack;

 
    void Awake()
    {
        
#if UNITY_EDITOR || UNITY_STANEALONE_WIN
        m_PersiterAssetSavePathRoot = Application.dataPath.Replace("Assets", "PersiterData/AssetBundles/");
#else
        m_LocalAssetSavePathRoot = Path.Combine(Application.persistentDataPath, AssetBundles); 
#endif

        m_PersiterConfigFileSavePath = Path.Combine(m_PersiterAssetSavePathRoot, ConfigFileName);

        m_LocalAssetSavePathRoot = Application.streamingAssetsPath;

        m_LocalConfigFileSavePath = Path.Combine(m_LocalAssetSavePathRoot, ConfigFileName);
        
       
    }

    public void Init(OnGetFilterLists callback)
    {
        OnGetFilterListsCallBack -= callback;
        OnGetFilterListsCallBack += callback;
    }


    IEnumerator StartCheck()
    {
        string str = "";

        if (GetLocalVersion() > GetPersiterVersion())
        {//游戏内写设备保存位置

            Directory.Delete(m_PersiterConfigFileSavePath, true);

            Directory.CreateDirectory(m_PersiterAssetSavePathRoot);

            str = readFileContent(m_LocalConfigFileSavePath);

            SaveToPersistentPath(str);

            FileHelper.CopyDirectory(m_LocalAssetSavePathRoot, m_PersiterAssetSavePathRoot);
            

        }


        yield return GetRemoteVersion();

        if (GetPersiterVersion() < m_remoteVersion)
        {//远程写保存位置

            yield return loadRemoteConfigFile();        
        }
               

/*
     if(true)
     { 
#if UNITY_EDITOR
          //  str = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/JustTest/AssetBundle/Data/" + ConfigFileName).ToString();
            //Resources.Load<TextAsset>(AbResVersionFileName.Replace(".txt","")).ToString();
#endif

          
        }
        else
        {
            str = readFileContent(m_PersiterConfigFileSavePath);
        }

        CreateDictFromStr(str, ref m_Dict);
        



        //Debug.Log(Application.persistentDataPath);

        //加载远程对比
        LoadAbFilesInfo(testOnGetAbFilesInfo);
        */
        yield return null;
    }


    //游戏内置版本
    int GetLocalVersion()
    {
        if (!File.Exists(m_LocalConfigFileSavePath))
            return 0;
        else
            return 1;
    }

    //本地保存位置版本
    int GetPersiterVersion()
    {
        if (!File.Exists(m_PersiterConfigFileSavePath))
            return 0;
        else
            return 1;
    }

    //远程版本
    IEnumerator GetRemoteVersion()
    {
        yield return null;

        m_remoteVersion = 3;

        
    }


    #region 版本对比
 public bool CompareVersion(string v1, string v2)
    {
        // true: v1>v2 false v1 <= v2
        int v1Int = VersionStrToInt(v1);
        int v2Int = VersionStrToInt(v2);
        return v1Int > v2Int ? true : false;
    }
 #endregion
    
    
    #region 版本转数值 如1.0.1 
public static int VersionStrToInt(string version)
    {
        int finalValue = 0, sectionValue = 0, sectionCount = 0, NUM_SECTIONS = 3;

        for (int i = 0; i < version.Length || sectionCount < NUM_SECTIONS; i++)
        {
            if (i >= version.Length || version[i] == '.')
            {
                Debug.Assert(sectionValue < 1000, "Version string '" + version + "' section " + (sectionCount + 1) + " is too long.");
                sectionCount++;
                finalValue *= 1000;
                finalValue += sectionValue;
                sectionValue = 0;
                continue;
            }
            Debug.Assert(version[i] - '0' >= 0 && version[i] - '0' <= 9, "Version string '" + version + "' has invalid character '" + version[i] + "'.");
            sectionValue *= 10;
            sectionValue += version[i] - '0';
        }
        return finalValue;
    } 
#endregion


void filterNeedUpdateFiles(string localStr, string remoteStr)
{

    Dictionary<string, AbVo> localDict = new Dictionary<string, AbVo>();
    AssetFilesVersionHandle.CreateDictFromStr(localStr, ref localDict);

    //SaveToPersistentPath(str);
    string[] lines = remoteStr.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
      

    for (int i = 0; i < lines.Length; i++)
    {
        if (string.IsNullOrEmpty(lines[i]))
            continue;
        
        string[] voline = lines[i].Split(',');
        string key = voline[0];

        if (!localDict.ContainsKey(key))
        {//新增            
            m_NeedDownFiles.Add(key);
        }
        else if (localDict[voline[0]].hash.CompareTo(voline[1]) != 0)
        {
            m_NeedDownFiles.Add(key);
        }
        else
            localDict.Remove(key);

        
    }



}


    //测试用
    void testOnGetAbFilesInfo(string str)
    {

        //SaveToPersistentPath(str);
        string[] lines = str.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);

        List<AbVo> lists = new List<AbVo>();

        for(int i=0;i<lines.Length;i++)
        {
            if (string.IsNullOrEmpty(lines[i]))
                continue;

            AbVo vo = new AbVo();

            string[] voline = lines[i].Split(',');

            Debug.Log(lines[i]);
           // vo.name = voline[0];
            vo.hash = voline[1];

          //  Debug.Log(vo.name + "_" + vo.hash);

            lists.Add(vo);
        }


    }


    //保存本地文件
    private void SaveToPersistentPath(string str)
    {
        FileHelper.CreateTxtFile(m_PersiterConfigFileSavePath, str);
    }
    

    //检测是否更新
    bool checkIsUpdated()
    {
        return true;
    }

    public void LoadAbFilesInfo(LoadAbFilesInfoCallBack callBack)
    {

        if (callBack != null)
        {
            onGetAbFilesInfo -= callBack;
            onGetAbFilesInfo += callBack;
        }


        //从web服务器或服务端包获取版本信息对比
        if (checkIsUpdated())
        {//需要更新
            
            StartCoroutine(startLoadFromRemote());
        }
        else
        {//需要考虑和本地的比 以后看具体实现 

            string str = "";

            if (File.Exists(m_PersiterConfigFileSavePath))
            {
                StreamReader s = File.OpenText(m_PersiterConfigFileSavePath);
                str = s.ReadToEnd();
                s.Close();
                
            }
            else
            {
                str = Resources.Load<TextAsset>("AbFilesInfo").ToString();

                SaveToPersistentPath(str);                
            
            }

            if (callBack != null)
                callBack(str); 

        }

    }


    IEnumerator loadRemoteConfigFile()
    {
        if (m_req == null)
            m_req = UnityWebRequest.Get(m_RemoteConfigUrl);


        yield return m_req.Send();


        if (string.IsNullOrEmpty(m_req.error) && m_req.responseCode == 200)
        {

            filterNeedUpdateFiles(readFileContent(m_PersiterConfigFileSavePath), m_req.downloadHandler.text);

            if (m_NeedDownFiles.Count > 0)            
            {//保存新的
                Debug.Log("Save new from remote");
                
                SaveToPersistentPath(m_www.text);

                if (OnGetFilterListsCallBack != null)
                    OnGetFilterListsCallBack(ref m_NeedDownFiles);
            }            
        }
        else
        {
            Debug.Log("远程配置读取异常");
        }
    
    }


    IEnumerator startLoadFromRemote()
    { 
        if(m_www == null)
            m_www = new WWW(m_RemoteConfigUrl);

        while(!m_www.isDone)
        {
            yield return null;
        }

        if(string.IsNullOrEmpty(m_www.error))
        {

            bool flag = checkIsChange(m_www.text);

            if (flag)
            {//保存新的
                Debug.Log("Save new from remote");

                SaveToPersistentPath(m_www.text);
            }

            if (onGetAbFilesInfo != null)
                onGetAbFilesInfo(m_www.text);
        }
        else
        {
            Debug.Log("远程配置读取异常");
        }
        
        
    }

    //对比
    bool checkIsChange(string remoteStr)
    {        
        
        Dictionary<string, AbVo> newDict = new Dictionary<string, AbVo>();
        AssetFilesVersionHandle.CreateDictFromStr(remoteStr, ref newDict);

        bool flag = false;
        foreach (string key in newDict.Keys)
        {
            if (!m_Dict.ContainsKey(key))
            {//新增
                m_Dict.Add(key, newDict[key]);
                m_NeedDownFiles.Add(key);
                flag = true;
            }
            else if (m_Dict[key].hash.CompareTo(newDict[key].hash) != 0)
            {
                m_Dict[key] = newDict[key];
                m_NeedDownFiles.Add(key);
                flag = true;
            }
        
        }

        return flag;
    }


    //读取指定路径文件
    private string readFileContent(string path)
    {
        return FileHelper.ReadTxtFile(path);        
    }


    //解析verstion文件，并返回dict对象
    public static void CreateDictFromStr(string str, ref Dictionary<string, AbVo> dict)
    {
        if (dict == null)
            return;

        string[] lines = str.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);

        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i]))
                continue;

            AbVo vo = new AbVo();

            string[] voline = lines[i].Split(',');

            //Debug.Log(lines[i]);
            // vo.name = voline[0];
            vo.hash = voline[1];
            vo.isPersit = false;

            dict.Add(voline[0], vo);
        }

    }

}
