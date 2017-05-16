using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureTestMain : MonoBehaviour {

    public Text OutPut;

	// Use this for initialization
	void Start () {

     


	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ShowDefault()
    {
        //这些属性值设置与Importer，绝对与当前项目设定的平台类型（非实际运行平台)
        OutPut.text = getInfo("bgdefault");            
    }

    public void ShowCompare()
    {
           OutPut.text = getInfo("bgetc4");            
           OutPut.text += getInfo("bgrgb16");
           OutPut.text += getInfo("bgrgba32");            
    }
    
    

    private string getInfo(string path)
    {
        float time = Time.realtimeSinceStartup;
        Texture2D tex = Resources.Load<Texture2D>(path);

        string str = string.Empty;
        str = tex.format.ToString() + "\n";
        str += tex.width.ToString() + " _ " + tex.height.ToString() + "\n";
        str += "加载时间 " + (Time.realtimeSinceStartup - time).ToString() + "\n";

        return str;
    }




    public void LoadCityAb()
    {
        if (listTexs == null)
            listTexs = new List<Texture2D>();

        StartCoroutine(startLoad());
    
    }

    public void UnloadCityAb()
    {
        StopAllCoroutines();
        if (www != null)
        {
            www.Dispose();
            www = null;

            if (ab != null)
            {
                ab.Unload(true);
                ab = null;
            }

        }

        while (listTexs.Count > 0)
            listTexs.RemoveAt(0);

        System.GC.Collect();  //对于被多次Unload(flase)的无法释放

        Resources.UnloadUnusedAssets(); //Unload(flase) 有效释放 未测试在移动设备平台
    }

    WWW www = null;
    AssetBundle ab = null;

    public RawImage loadImg;
    List<Texture2D> listTexs;
    IEnumerator startLoad()
    {
        if (www != null)
        {
            www.Dispose();
            www = null;

            if (ab != null)
            {
                ab.Unload(false);
                ab = null;
            }            
          
        }

       

        www = new WWW("http://10.0.16.49:92/city.unity3d");
        while (!www.isDone)
        {
            Debug.Log(www.progress);
          yield return null;
        }

        if (www.error != null)
            Debug.Log("WWW download had an error:" + www.error);


        ab = www.assetBundle;


        Texture2D tex = ab.LoadAsset<Texture2D>("city");
        Texture2D tex1 = ab.LoadAsset<Texture2D>("Assets/_Images/city.jpg");
        Texture tex2 = ab.LoadAsset<Texture>("city");
        Texture tex3 = ab.LoadAsset<Texture>("city.jpg");
        
        listTexs.Add(tex);

        loadImg.texture = tex;


        /*
        www.Dispose();
        www = new WWW("http://10.0.16.49:92/cube.unity3d");
        while (!www.isDone)
        {
            yield return null;        
        }

        if (string.IsNullOrEmpty(www.error))
        {
            GameObject.Instantiate<GameObject>(www.assetBundle.LoadAsset<GameObject>("Cube.prefab"));   
        }
        */

        www.Dispose();
        www = new WWW("http://10.0.16.49:92/special.unity3d");
        while (!www.isDone)
        {
            yield return null;
        }

        if (string.IsNullOrEmpty(www.error))
        {
             loadImg.texture = www.assetBundle.LoadAsset<Texture2D>("mana1");    //同名的取得的资源不确定
         //   Texture2D t = www.assetBundle.LoadAsset<Texture2D>("Assets/_Images/icons/mana1.png");   //指定路径的ok
          //    loadImg.texture = t;
              www.assetBundle.Unload(false);
        }
        else
            Debug.Log("WWW download had an error:" + www.error);
               
        yield return null;
    }




}
