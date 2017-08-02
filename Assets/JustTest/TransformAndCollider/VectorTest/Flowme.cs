using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowme : MonoBehaviour {

    public   Transform hunter;
    public Transform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float vx = target.localPosition.x - hunter.localPosition.x;
        float vy = target.localPosition.y - hunter.localPosition.y;


        hunter.localPosition += new Vector3(vx, vy, 0f) * 0.001f;
		
	}
}
