using UnityEngine;
using System.Collections;

public class TestResourceLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GameObject go = Resources.Load("icon/1060") as GameObject;
        GameObject insgo = Instantiate(go);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
