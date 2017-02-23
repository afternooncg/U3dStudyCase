using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class IoTestMain : MonoBehaviour {

    UIInput m_input;

    string m_savePath = "";

	// Use this for initialization
	void Start () {

        m_savePath = Path.Combine(PubConfig.PersiterPath, "IoTest");

        if (!Directory.Exists(m_savePath))
            Directory.CreateDirectory(m_savePath);

        UIEventListener.Get(GameObject.Find("BtnCreateTxt")).onClick = onCreateTxtFile;
        UIEventListener.Get(GameObject.Find("BtnAppendTxt")).onClick = onAppendTxtFile;
        UIEventListener.Get(GameObject.Find("BtnCreateBin")).onClick = onCreateBinFile;
        UIEventListener.Get(GameObject.Find("BtnDelFile")).onClick = onDeleteFile;
        UIEventListener.Get(GameObject.Find("BtnShowAppPath")).onClick = onShowAppPath;
        UIEventListener.Get(GameObject.Find("BtnTestCopyFolder")).onClick = OnTestCopyPerfore;
        
        m_input = GameObject.Find("Text").GetComponent<UIInput>();

        int[,] a = new int[2, 0];

        Debug.Log(a.GetLength(0) + " " + a.GetLength(1));
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    public void onCreateTxtFile(GameObject go)
    {
        string path = Path.Combine(m_savePath, "Text.txt");

        FileInfo fi = new FileInfo(path);

        if (fi.Exists)
        {            
            m_input.value = FileHelper.ReadTxtFile(path) + "\n";

            FileHelper.CreateTxtFile(path, "第" + (count++).ToString() + "次写入文件hello");

            m_input.value += "覆盖写入新文件";
        }
        else
        {
            m_input.value = path + "不存在";

            FileHelper.CreateTxtFile(path, "第1次写入文件hello");
        }

    }

    int count = 0;
    public void onAppendTxtFile(GameObject go)
    {
        string path = Path.Combine(m_savePath, "Text.txt");

        FileInfo fi = new FileInfo(path);
        
        if (fi.Exists)
        {
            m_input.value = FileHelper.ReadTxtFile(path) + "\n";

            FileHelper.CreateTxtFile(path, "第" + (count++).ToString() + "次写入文件",true);

            m_input.value += "追加写入新文件\n";


            m_input.value += FileHelper.ReadTxtFile(path) + "\n";
        }
        else
        {
            m_input.value = path + "不存在";

            FileHelper.CreateTxtFile(path, "第1次写入文件hello");
        }
    }

    public void onCreateBinFile(GameObject go)
    {

        StartCoroutine(loadAssetAndSaveLocal());
    }

    public void onDeleteFile(GameObject go)
    {
        string path = Path.Combine(m_savePath, "Text.txt");
        FileHelper.DeleteFile(path);

    }
    
    IEnumerator loadAssetAndSaveLocal()
    {

        //string url = PubConfig.RemoteWWWRoot + "/Assetbundle/assets/game.unity3d";
        string url = PubConfig.RemoteWWWRoot + "/Assetbundle/assets/justtest/assetbundle/resource/materials/mat1.mat.unity3d";
        string savepath = PubConfig.PersiterPath + "/IoTest/game.unity3d";

        UnityWebRequest uwreq = UnityWebRequest.Get(url);

        yield return uwreq.Send();
        

        if(!uwreq.isError)
        {
            
            byte[] data =  uwreq.downloadHandler.data;
            FileHelper.CreateBinFile(savepath, data, data.Length);

            m_input.value = savepath + "保存成功";
        }
        yield return null;

    
    }


    public void onShowAppPath(GameObject go)
    {
        m_input.value = "Application.dataPath: " + Application.dataPath + "\n";
        m_input.value += "Application.persistentDataPath: " + Application.persistentDataPath + "\n";
        m_input.value += "Application.streamingAssetsPath: " + Application.streamingAssetsPath + "\n";

        string newPath = Path.Combine(PubConfig.PersiterPath, "streamingAssets");
        if(!Directory.Exists(newPath))
            Directory.CreateDirectory(newPath);

        FileHelper.CopyDirectory(Application.streamingAssetsPath, newPath);


        

        RemoteFileLoader.LoadInfo li = new RemoteFileLoader.LoadInfo();
        li.assetName = "testcount.txt";
        li.remoteUrl = PubConfig.RemoteWWWRoot;
        li.autoDestroy = true;
        li.loadFinished = OnLoadTestCountTxtFinished;

        new GameObject("loadtxt").AddComponent<RemoteFileLoader>().BeginLoad(li);
        
    }


    void OnLoadTestCountTxtFinished(DownloadHandler handle, string path)
    {

        string file = Path.Combine(Path.Combine(PubConfig.PersiterPath, "streamingAssets"), AssetFilesVersionHandle.ConfigFileName);
        FileHelper.CreateTxtFile(file, handle.text);

        m_input.value += FileHelper.ReadTxtFile(file);


        
    }



    #region 测试大量文件复制效率
  void OnTestCopyPerfore(GameObject go)
    {
        float begin = Time.realtimeSinceStartup;

        m_input.value = begin.ToString() +　"\n";

        string newPath = Path.Combine(PubConfig.PersiterPath, "Android/Lua");
        if (!Directory.Exists(newPath))
            Directory.CreateDirectory(newPath);

#if UNITY_EDITOR
        FileHelper.CopyDirectory(Application.streamingAssetsPath , newPath);
#elif UNITY_ANDROID
        FileHelper.CopyDirectory(Application.dataPath + “!/assets/Lua" , newPath);        
#endif
        m_input.value += ((Time.realtimeSinceStartup - begin)).ToString() ;

    }
  #endregion
}
