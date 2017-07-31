using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraAnotherScene : MonoBehaviour {

    public Image img;
    public static CameraAnotherScene Instance;
	// Use this for initialization
	void Awake () {

        Instance = this;
        

	}

    void Start()
    {
        SceneManager.LoadScene("AnotherScene", LoadSceneMode.Additive);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
