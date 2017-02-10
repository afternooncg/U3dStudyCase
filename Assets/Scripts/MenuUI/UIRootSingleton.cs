using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRootSingleton : UntiySingleton<UIRootSingleton>
{

	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(this.gameObject);


        GameObject gomain = GameObject.Instantiate(Resources.Load<GameObject>("MainFrame"));
        gomain.transform.parent = GameObject.Find("UIRoot").transform;
        MainFrame.ResetGameObjectTrans(gomain);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
