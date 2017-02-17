using UnityEngine;
using System.Collections;

public class TestWwwDownCache : MonoBehaviour {


    public GameObject CubeGo;
    private WWW www;

	// Use this for initialization
	void Start () {


        //StartCoroutine("LoadFromWww");
        //StartCoroutine("LoadFromWwwCache");

       // StartCoroutine("LoadFromWww1");
        //StartCoroutine("LoadFromWww3");

        LoadFromFile();
	}



    void LoadFromFile()
    {
        float b = Time.realtimeSinceStartup;
       //AssetBundle ab =  AssetBundle.LoadFromFile("file://" + Application.dataPath + "/JustTest/BundleTest/Tex/ab2.assetbundle");  //不需要flile
      AssetBundle ab = AssetBundle.LoadFromFile(Application.dataPath + "/JustTest/BundleTest/Tex/ab2.assetbundle");      
       Debug.Log("ab " + ab);
        Texture2D tex = ab.LoadAsset<Texture2D>("bg2");
        if (tex != null)
            Debug.Log(tex.width + " " + tex.height);


        CubeGo.GetComponent<Renderer>().material.mainTexture = tex;

        Debug.Log("LoadTime:" + (Time.realtimeSinceStartup - b));
    }

    IEnumerator LoadFromWww()
    {
        float b = Time.realtimeSinceStartup;

        www = new WWW("file://" + Application.dataPath + "/JustTest/BundleTest/Tex/ab1.assetbundle");

        if (www.isDone)
            yield return new WaitForEndOfFrame();


        Texture2D tex = www.assetBundle.LoadAsset<Texture2D>("bg1");
        if (tex != null)
            Debug.Log(tex.width + " " + tex.height);


        CubeGo.GetComponent<Renderer>().material.mainTexture = tex;

        Debug.Log("LoadTime:" + (Time.realtimeSinceStartup - b));
    }


    IEnumerator LoadFromWwwCache()
    {
        float b = Time.realtimeSinceStartup;
         www = WWW.LoadFromCacheOrDownload("file://" + Application.dataPath + "/JustTest/BundleTest/Tex/ab2.assetbundle", 2);

        if (www.isDone)
            yield return new WaitForEndOfFrame();


        Texture2D tex = www.assetBundle.LoadAsset<Texture2D>("bg2");
        if (tex != null)
            Debug.Log(tex.width + " " + tex.height);


        CubeGo.GetComponent<Renderer>().material.mainTexture = tex;

        Debug.Log("LoadTime:" + (Time.realtimeSinceStartup - b));
    }

    IEnumerator LoadFromWww1()
    {
        float b = Time.realtimeSinceStartup;

        www = new WWW("file://" + Application.dataPath + "/JustTest/BundleTest/Tex/mat.assetbundle");

        if (www.isDone)
            yield return new WaitForEndOfFrame();



     //   CubeGo.GetComponent<Renderer>().material = www.assetBundle.LoadAsset<Material>("mymat");
        CubeGo.GetComponent<Renderer>().material = www.assetBundle.mainAsset as Material;

        Debug.Log("LoadTime:" + (Time.realtimeSinceStartup - b));
    }

    IEnumerator LoadFromWww3()
    {
        float b = Time.realtimeSinceStartup;

        www = new WWW("file://" + Application.dataPath + "/JustTest/BundleTest/Tex/cube.assetbundle");

        if (www.isDone)
            yield return new WaitForEndOfFrame();



        //   CubeGo.GetComponent<Renderer>().material = www.assetBundle.LoadAsset<Material>("mymat");
       GameObject.Instantiate( www.assetBundle.mainAsset as GameObject);
       Debug.Log("cube ok");
       

       www = new WWW("file://" + Application.dataPath + "/JustTest/BundleTest/Tex/Sphere.assetbundle");

       if (www.isDone)
           yield return new WaitForEndOfFrame();
       Debug.Log("sphere ok");
       GameObject.Instantiate(www.assetBundle.mainAsset as GameObject);
        Debug.Log("LoadTime:" + (Time.realtimeSinceStartup - b));
    }

	// Update is called once per frame
	void Update () {
	
	}
}
