using UnityEngine;
using System.Collections;


using UnityEngine.SceneManagement;

public class Test : MonoBehaviour {
        
	// Use this for initialization
	void Start () {
        Debug.Log("Test Start");
        
        char a;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ClickCallBack()
    {
        Debug.Log("test");
    }

    public void LoadNextScene()
    {
        //Application.LoadLevel("S2");
        SceneManager.LoadScene("S2");
    }

    public void LoadPrevScene()
    {
        //Application.LoadLevel("S2");
        SceneManager.LoadScene("S1");
    }



    public void LoadAddScene()
    {
        SceneManager.LoadScene("S3", LoadSceneMode.Additive);
    }


    public void UnLoadAddScene()
    {

        SceneManager.UnloadScene("S3");
        if(NotDestory.PubAdd)
        {
            Debug.Log("UnLoadAddScene");

            Destroy((NotDestory.PubAdd as S3).rootgo);
            NotDestory.PubAdd = null;
        }
        
        
    }


    ~Test()
    {
        Debug.Log("Destory");
    }
}
