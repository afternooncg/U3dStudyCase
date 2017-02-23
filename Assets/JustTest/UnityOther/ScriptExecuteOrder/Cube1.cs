using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube1 : MonoBehaviour {

    public int Id = 0;
    void Awake()
    {
        Debug.Log("Cube Awake " + Id.ToString());
    }

	// Use this for initialization
	void Start () {

        Debug.Log("Cube Start" + Id.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
