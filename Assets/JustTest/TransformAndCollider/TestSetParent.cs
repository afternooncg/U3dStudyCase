using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSetParent : MonoBehaviour {

    public Transform parentobj;
    public Transform child;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnGUI () {

        if (GUI.Button(new Rect(100, 100, 100, 30), "parent"))
        {
            child.parent = parentobj;
        }

        if (GUI.Button(new Rect(220, 100, 150, 30), "setparent_true"))
        {
            child.SetParent(parentobj, true); //等同直接设置parent
        }

        if (GUI.Button(new Rect(420, 100, 150, 30), "setparent_false"))
        {
            child.SetParent(parentobj, false);
        }

	}
}
