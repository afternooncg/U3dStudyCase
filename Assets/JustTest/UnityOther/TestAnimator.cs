using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayOverEffect()
    {
        Debug.Log("test");
        GameObject.Find("Item").GetComponent<Animator>().SetBool("isover", true);
    }
}
