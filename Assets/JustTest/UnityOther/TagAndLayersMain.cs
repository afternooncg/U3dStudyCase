using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagAndLayersMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (gameObject.tag == "abc")
            Debug.Log("ok");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
