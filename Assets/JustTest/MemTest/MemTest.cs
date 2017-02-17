using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MemTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Load()
    {
        SceneManager.LoadScene("ChildScene",LoadSceneMode.Additive);
    }

    public void UnLoad()
    {
        SceneManager.UnloadScene("ChildScene");
    }


    static public void TestCallBack()
    {
        Debug.Log("test call back ...");
    }
}
