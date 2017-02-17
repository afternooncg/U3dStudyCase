using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SerializeTest : MonoBehaviour {

    [System.Serializable]
    public class MyObj
    {
        public string name;
        public GameObject prefab;
    }

    [SerializeField]
    public List<MyObj> ListObj;

	// Use this for initialization
	void Start () {
	
        

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
