using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParamMain : MonoBehaviour {


    public GameObject Soldier;
	// Use this for initialization
	void Start () {
		
        if(Soldier != null)
        {
            for (int i = 0; i < 100; i++)
            {
                GameObject go = GameObject.Instantiate<GameObject>(Soldier);
                go.transform.localPosition = new Vector3(Random.RandomRange(-5f, 5f), 0, Random.RandomRange(-5f, 5f));
            }
            

        }

	}
	
	// Update is called once per frame
	void Update () {

        Camera.main.transform.LookAt(Vector3.zero);

	}
}
