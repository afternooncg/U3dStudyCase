using UnityEngine;
using System.Collections;
//using UnityEditor;
using System;

public class TestJson : MonoBehaviour {

	// Use this for initialization
	void Start () {

        MyObject myObject = new MyObject();
        myObject.name = "雨松MOMO";
        myObject.newOjbect = new MyNewObject() { level = 100 };

        string json = JsonUtility.ToJson(myObject);
        Debug.Log(json);

        myObject = JsonUtility.FromJson<MyObject>(json);
        Debug.Log(myObject.name + " " + myObject.newOjbect.level);

        JsonUtility.FromJsonOverwrite(json, myObject);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}


[Serializable]
public class MyObject
{
    public string name;
    public MyNewObject newOjbect;
}
[Serializable]
public class MyNewObject
{
    public int level;
}

