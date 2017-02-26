using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 小结
/// 同1个场景里的涉及的
/// 
/// Awake 里 instantiate  出来的 go 上的 Awake 优先执行
/// 
/// </summary>
public class ScriptExecuteOrder : Cube1 {



    void Awake()
    {

        GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Cube1"));

        Debug.Log("ScriptExecuteOrder:  Awake");
    }

	// Use this for initialization
	void Start () {
        Debug.Log("ScriptExecuteOrder:  Start");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
