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

        m_input = GameObject.Find("Text").GetComponent<UIInput>();

        
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
}
