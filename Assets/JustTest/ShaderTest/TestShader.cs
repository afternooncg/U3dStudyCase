using UnityEngine;
using System.Collections;
using System;

public class TestShader : MonoBehaviour {


    Action act;
    Action<int, string> act1;
	// Use this for initialization
    float r = 0f;
    int a = 0;
	void Start () {

        act = test;
        act();

        act = delegate()
        {
            Debug.Log("world");
        };

        act();

        act1 = test1;
        act1(1, "string");

        bool r;
        bool ret = bool.TryParse("true", out r);
        Debug.Log("r:" + r + "  ret:" + ret);
	}

    void test()
    {
        Debug.Log("hello");
    }

    void test1(int a, string b)
    {
        Debug.Log("test1:" + a + " " + b);
    }
	
	// Update is called once per frame
	void Update () {
        a++;
        if (a < 30)
            return;
        else
            a = 0;
        r += 0.01f;
        if (r >= 1.0f)
            r = 0f;
        this.GetComponent<MeshRenderer>().material.SetFloat("_centerX", r);
        this.GetComponent<MeshRenderer>().material.SetFloat("_time", Time.realtimeSinceStartup);
        //this.GetComponent<MeshRenderer>().material.SetFloat("_centerX", Random.value);
      //  this.GetComponent<MeshRenderer>().material.SetFloat("_centerY", Random.value);        

	}
}
