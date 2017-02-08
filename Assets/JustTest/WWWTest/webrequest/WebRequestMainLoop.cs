using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.IO;

public class WebRequestMainLoop : MonoBehaviour
{



    // Use this for initialization
    void Start()
    {


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
        GameObject.Find("Text").GetComponent<Text>().text = "Wait for begin load txt...";

        yield return new WaitForSeconds(1);

        UnityWebRequest ureq = UnityWebRequest.Get("http://codespace365.com/Test/text1.txt");
        //UnityWebRequest ureq = UnityWebRequest.Get("http://adfsa.com/a.asp");  //无效地址并不会使isError=true可能需要配合 responseCode


        //yield return ureq.Send();  //隐藏了loading过程
        ureq.Send();



        while (!ureq.isDone)
        {
            if (ureq.isError)
            {
                GameObject.Find("Text").GetComponent<Text>().text = "Error";
                break;
            }
            GameObject.Find("Text").GetComponent<Text>().text = ureq.downloadProgress.ToString();

            yield return 0;
        }


        if (ureq.isError || !string.IsNullOrEmpty(ureq.error))
            GameObject.Find("Text").GetComponent<Text>().text = "Error";
        else if (ureq.responseCode != 200)
            GameObject.Find("Text").GetComponent<Text>().text = "Error url";
        else
        {
            GameObject.Find("Text").GetComponent<Text>().text = ureq.downloadHandler.text;

            

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
        GameObject.Find("Text").GetComponent<Text>().text = "Wait for begin load texture...";

        yield return new WaitForSeconds(1);

        UnityWebRequest ureq = UnityWebRequest.GetTexture("http://127.0.0.1:92/test.jpg");
        //UnityWebRequest ureq = UnityWebRequest.GetTexture("http://127.0.0.1:92/test1.jpg");  //错误的url也无法触发isError属性

        ureq.Send();
        
        while (!ureq.isDone)
        {
            GameObject.Find("Text").GetComponent<Text>().text = ureq.downloadProgress.ToString();            
            yield return 0;
        }

          if (ureq.isError || ureq.responseCode != 200)
          {
                
              GameObject.Find("Text").GetComponent<Text>().text = "Error img url";            
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
        UnityWebRequest ureq = UnityWebRequest.GetAssetBundle("http://127.0.0.1:92/Assetbundle/assets/1390616300363.jpg");
        ureq.Send();

        while (!ureq.isDone)
        {
            
            GameObject.Find("Text").GetComponent<Text>().text = ureq.downloadProgress.ToString();

            yield return 0;
        }

        if (ureq.isError || !string.IsNullOrEmpty(ureq.error))
            GameObject.Find("Text").GetComponent<Text>().text = "Error ab url";
        else if (ureq.responseCode != 200)
            GameObject.Find("Text").GetComponent<Text>().text = "Error ab url";
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



        UnityWebRequest ureq = UnityWebRequest.Post("http://127.0.0.1:92/testpost.asp", formData);
        ureq.uploadHandler.contentType = "multipart/form-data";
        yield return ureq.Send();

        if(ureq.isError || ureq.responseCode != 200)
            GameObject.Find("Text").GetComponent<Text>().text = "Error post url";
        else

            GameObject.Find("Text").GetComponent<Text>().text = ureq.downloadHandler.text;
        yield return 0;
    }

}
