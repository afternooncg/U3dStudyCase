using UnityEngine;
using System.Collections;

public class S3 : MonoBehaviour {
                                                                                            
    public GameObject rootgo;

	// Use this for initialization
	void Start () {
        rootgo = this.gameObject;

        Transform t = rootgo.transform;
        while (t.parent != null)
            t = t.parent;
        rootgo =  t.gameObject;

        NotDestory.PubAdd = this;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("S3 runing...");
	}
}
