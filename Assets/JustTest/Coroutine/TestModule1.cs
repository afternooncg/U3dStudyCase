using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestModule1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator Test()
    {
        int i = 0;
        while (i < 100)
        {
            i++;

            Debug.Log("hello");
            yield return null;
        }


    }
}
