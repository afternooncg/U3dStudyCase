using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.IO;

public class WebRequestMainLoop : MonoBehaviour
{


    Text m_Output;
    // Use this for initialization
    void Start()
    {
        m_Output = gameObject.transform.Find("Text").GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {

    }



    #region Get测试
    public void handleBtnGetClick()
    {

        StartCoroutine("TestGet");

    }

    private IEnumerator TestGet()
    {
       m_Output.text = "Wait for begin load txt...";

        yield return new WaitForSeconds(1);

        UnityWebRequest ureq = UnityWebRequest.Get("http://codespace365.com/Test/text1.txt");
        //UnityWebRequest ureq = UnityWebRequest.Get("http://adfsa.com/a.asp");  //无效地址并不会使isError=true可能需要配合 responseCode


        //yield return ureq.Send();  //隐藏了loading过程
        ureq.Send();



        while (!ureq.isDone)
        {
            if (ureq.isError)
            {
               m_Output.text = "Error";
                break;
            }
           m_Output.text = ureq.downloadProgress.ToString();

            yield return 0;
        }


        if (ureq.isError || !string.IsNullOrEmpty(ureq.error))
           m_Output.text = "Error";
        else if (ureq.responseCode != 200)
           m_Output.text = "Error url";
        else
        {
           m_Output.text = ureq.downloadHandler.text;

            

        }


        ureq.Dispose();
       
    }
    #endregion

    #region GetTexture测试
 public void handleBtnGetTextureClick()
    {
        StartCoroutine(TestGetTexture());
    }
 

    private IEnumerator TestGetTexture()
    {
       m_Output.text = "Wait for begin load texture...";

        yield return new WaitForSeconds(1);

        UnityWebRequest ureq = UnityWebRequest.GetTexture(PubConfig.RemoteWWWRoot  + "/test.jpg");
        //UnityWebRequest ureq = UnityWebRequest.GetTexture(PubConfig.RemoteWWWRoot  +"/test1.jpg");  //错误的url也无法触发isError属性

        ureq.Send();
        
        while (!ureq.isDone)
        {
           m_Output.text = ureq.downloadProgress.ToString();            
            yield return 0;
        }

          if (ureq.isError || ureq.responseCode != 200)
          {
                
             m_Output.text = "Error img url";            
          }
        else
          {
          Texture2D tex2d = ((DownloadHandlerTexture)ureq.downloadHandler).texture;               
                 
                GameObject.Find("RawImage").GetComponent<RawImage>().texture =  DownloadHandlerTexture.GetContent(ureq);
          }

          
        
        yield return 0;
    }
 #endregion
        
    #region GetAb
  public void handleBtnGetAbClick()
    {
        StartCoroutine(GetAb());
    }

    IEnumerator GetAb()
    {
        UnityWebRequest ureq = UnityWebRequest.GetAssetBundle(PubConfig.RemoteWWWRoot  +"/Assetbundle/assets/1390616300363.jpg");
        ureq.Send();

        while (!ureq.isDone)
        {
            
           m_Output.text = ureq.downloadProgress.ToString();

            yield return 0;
        }

        if (ureq.isError || !string.IsNullOrEmpty(ureq.error))
           m_Output.text = "Error ab url";
        else if (ureq.responseCode != 200)
           m_Output.text = "Error ab url";
        else
        {
            AssetBundle ab = ((DownloadHandlerAssetBundle)ureq.downloadHandler).assetBundle;

            GameObject.Find("RawImage").GetComponent<RawImage>().texture = ab.LoadAsset<Texture>("1390616300363.jpg");
        }

        yield return 0;
    }
  #endregion

    public void handleBtnPostClick()
    {
        StartCoroutine("PostTest");
    }

    IEnumerator PostTest()
    {
        

        //ureq.SetRequestHeader("postvar", "okok");  //错误添加模式


        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("postvar=v1"));

        //以下模式异常
        formData.Add(new MultipartFormDataSection("postvar", "v1"));
        //formData.Add(new MultipartFormDataSection("postvar2", "v2"));
        //formData.Add(new MultipartFormDataSection("postvar3", "v3"));

        //formData.Add(new MultipartFormDataSection("postvar1=v1&postvar2=v2"));

        WWWForm form = new WWWForm();
        form.AddField("postvar", "v1");



        UnityWebRequest ureq = UnityWebRequest.Post(PubConfig.RemoteWWWRoot + "/testpost.asp", formData);
        ureq.uploadHandler.contentType = "multipart/form-data";
        yield return ureq.Send();

        if(ureq.isError || ureq.responseCode != 200)
           m_Output.text = "Error post url";
        else

           m_Output.text = ureq.downloadHandler.text;
        yield return 0;
    }



    public void handleBtnGetRarClick()
    {
        StartCoroutine(TestGetWarAndSave());
    }
 
    private IEnumerator TestGetWarAndSave()
    {
        m_Output.text = "Wait for begin load rar...";

        yield return new WaitForSeconds(1);

        UnityWebRequest ureq = new UnityWebRequest(PubConfig.RemoteWWWRoot + "/TestPost.unity3d");
        ureq.downloadHandler = new DownloadHandlerBuffer();
        //UnityWebRequest ureq = UnityWebRequest.GetTexture(PubConfig.RemoteWWWRoot  +"/test1.jpg");  //错误的url也无法触发isError属性

        ureq.Send();

        while (!ureq.isDone)
        {
            m_Output.text = ureq.downloadProgress.ToString();
            yield return 0;
        }

        if (ureq.isError || ureq.responseCode != 200)
        {

            m_Output.text = "Error img url";
        }
        else
        {
            FileHelper.CreateBinFile(PubConfig.PersiterPath + "/streamingAssets/TestPost.unity3d", ureq.downloadHandler.data, ureq.downloadHandler.data.Length);
            FileHelper.CreateBundleFile(PubConfig.PersiterPath + "/streamingAssets/TestPost1.unity3d", ureq.downloadHandler.data);
        }



        yield return 0;
    }

}
