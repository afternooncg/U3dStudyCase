using UnityEngine;
using System.Collections;

public class TestDownAbs : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
	
	}

    public string BundleURL;// = Application.dataPath + "/Resources/BundleFiles;";
    public string AssetName;

    void Start()
    {
	/*
        //首先加载Manifest文件;

        Debug.Log(Application.dataPath + "/../BundleFiles");
        AssetBundle manifestBundle = AssetBundle.CreateFromFile(Application.dataPath + "/../BundleFiles/BundleFiles");
        if (manifestBundle != null)
        {
            AssetBundleManifest manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");

            //获取依赖文件列表;
            string[] cubedepends = manifest.GetAllDependencies("bagicon");
            AssetBundle[] dependsAssetbundle = new AssetBundle[cubedepends.Length];

            for (int index = 0; index < cubedepends.Length; index++)
            {
                //加载所有的依赖文件;
                dependsAssetbundle[index] = AssetBundle.CreateFromFile(Application.dataPath + "/Resources/BundleFiles/"
                                                                     + cubedepends[index]);


            }


            //加载我们需要的文件;
            AssetBundle cubeBundle = AssetBundle.CreateFromFile(Application.dataPath
                                                              + "/Resources/BundleFiles/bagsprite");
            GameObject cube = cubeBundle.LoadAsset("Assets/_Images/icons/bag/1124.png") as GameObject;
            if (cube != null)
            {
                Instantiate(cube);
            }
        }

	*/
    }



    IEnumerator Start1()
    {
        // Download the file from the URL. It will not be saved in the Cache
        Debug.Log("xxxxxx:" + Application.dataPath);
        //yield return new  WaitForSeconds(10);
        BundleURL = "file:\\" + Application.dataPath + "/Resources/BundleFiles/bagicon";
        using (WWW www = new WWW(BundleURL))
        {
            yield return www;
            if (www.error != null)
                Debug.Log("WWW download had an error:" + www.error);
            AssetBundle bundle = www.assetBundle;
            GameObject tx = bundle.LoadAsset<GameObject>("Assets/_Images/icons/bag/1083.png");
           Renderer renderer = GetComponent<Renderer>();
       //    renderer.material.mainTexture = tx as Texture;

            /*
            if (AssetName == "")
                Instantiate(bundle.mainAsset);
            else
                Instantiate(bundle.LoadAsset(AssetName));
             * */
            // Unload the AssetBundles compressed contents to conserve memory
            bundle.Unload(false);

        } // memory is freed from the web stream (www.Dispose() gets called implicitly)
    }
}
