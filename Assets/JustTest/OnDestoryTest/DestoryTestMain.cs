using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryTestMain : MonoBehaviour {

   public  GameObject Ball;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void HandleAttach()
    {
        Ball.AddComponent<MonoInstance>();

  
    }

    public void HandleDestroy()
    {
        if(Ball.GetComponent<MonoInstance>()!=null)
            Destroy(Ball.GetComponent<MonoInstance>());
    }
}
