using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    AsyncOperation m_ao;
	// Use this for initialization
	void Start () {

        //Application.LoadLevel("WwwTest");
        float start = Time.time;
        m_ao = SceneManager.LoadSceneAsync("WwwTest");

        Debug.Log("ExecTime: " +  (Time.time -start));
	}
	
	// Update is called once per frame
	void Update () {
	    Debug.Log("m_ao:" + m_ao.progress);
	}
}
