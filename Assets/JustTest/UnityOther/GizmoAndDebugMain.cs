using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoAndDebugMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        //Vector3 forward = transform.TransformDirection(Vector3.forward) * 20;
	    //Debug.DrawRay (transform.position, forward, Color.green);

        Debug.DrawLine(Vector3.zero, new Vector3(10f,10f,10f),Color.black);


        Debug.DrawRay(Vector3.zero, new Vector3(10f, 10f, 10f), Color.green);


        DrawArrow.ForDebug(Vector3.zero, new Vector3(10f, 10f, 10f), Color.green,0.5f);


        Vector3 v1 = new Vector3(10f, 0f, 0);


        Vector3 v2 = new Vector3(0f, 10f, 0);


        Vector3 sub1 = v1 - v2;

        Vector3 sub2 = v2 - v1;



        Debug.DrawLine(Vector3.zero,sub1, Color.black);    

        Debug.DrawLine(Vector3.zero,sub2, Color.red);


        //结论 v1-v2 方向是v2到v1

        


	}
}
