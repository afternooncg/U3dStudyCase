using UnityEngine;
using System.Collections;

using System.IO;
using System.Collections.Generic;


public class AbVo
{
    public string hash;
    public bool isPersit;       //true 走 persit路径  flase 走包路径
}


public class AbFilesInfoHandle : MonoBehaviour
{ 

    Dictionary<string, AbVo> m_Dict = new Dictionary<string, AbVo>();                   //存放加载信息
    
    public delegate  void LoadAbFilesInfoCallBack(string str);

    private LoadAbFilesInfoCallBack onGetAbFilesInfo;

    static public string AbResVersionFileName = "AssetsFilesVersion.txt";                     //记录ab文件版本用

    string AbFilesInfoPath = "";                                                        //或许可放到共用配置文件

    static string m_LocalSaveUrl = "";                                                         //本地保存地址

    static string m_RemoteUrl = "http://127.0.0.1:92/" + AbResVersionFileName;                 //远程加载地址

    

    private List<string> m_NeedDownFiles = new List<string>();                          //需要加载的列表

    WWW m_www;



    static public string PersitentDataPath
    {
        get 
        {
            if (string.IsNullOrEmpty(m_LocalSaveUrl))
                m_LocalSaveUrl = Application.dataPath.Replace("Assets", "PersiterData/AssetBundles/");

            return m_LocalSaveUrl;
        }
    }

    void Awake()
    {       

        AbFilesInfoPath = Path.Combine(PersitentDataPath, AbResVersionFileName);


        string  str = "";
        //一运行就检测是否存在本地version
        if (!File.Exists(AbFilesInfoPath))
        {
           
#if UNITY_EDITOR
           str =  UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/JustTest/AssetBundle/Data/" + AbResVersionFileName).ToString();
           //Resources.Load<TextAsset>(AbResVersionFileName.Replace(".txt","")).ToString();
#endif

            SaveToPersistentPath(str);
        }
        else
        { 
            str = readFileContent(AbFilesInfoPath);            
        }

        CreateDictFromStr(str, ref m_Dict);
        
    }

    void Start()
    {
        //Debug.Log(Application.persistentDataPath);

        //加载远程对比
        LoadAbFilesInfo(testOnGetAbFilesInfo);
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
        if (File.Exists(AbFilesInfoPath))
            File.Delete(AbFilesInfoPath);

        StreamWriter sw = File.CreateText(AbFilesInfoPath);

        //Debug.Log("SaveCall:" + str);
        
        sw.Write(str);
        sw.Close();      

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

            if (File.Exists(AbFilesInfoPath))
            {
                StreamReader s = File.OpenText(AbFilesInfoPath);
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


    IEnumerator startLoadFromRemote()
    { 
        if(m_www == null)
            m_www = new WWW(m_RemoteUrl);

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
        AbFilesInfoHandle.CreateDictFromStr(remoteStr, ref newDict);

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
        if (File.Exists(path))
        {
            StreamReader s = File.OpenText(AbFilesInfoPath);
            string str = s.ReadToEnd();
            s.Close();

            return str;
        }
        else
            return string.Empty;
        
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
