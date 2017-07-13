using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoAndDebugMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


       // DrawLine_static();


        DrawLine_st();
        


	}


    void DrawLine_static()
    {
        //Vector3 forward = transform.TransformDirection(Vector3.forward) * 20;
        //Debug.DrawRay (transform.position, forward, Color.green);

        Debug.DrawLine(Vector3.zero, new Vector3(10f, 0f, 0f), Color.red);

        Debug.DrawLine(Vector3.zero, new Vector3(0, 10f, 0f), Color.green);

        Debug.DrawLine(Vector3.zero, new Vector3(0, 0f, 10f), Color.blue);


        
    }

    void DrawLine_st()
    { 
       // DrawArrow.ForDebug(Vector3.zero, new Vector3(10f, 10f, 10f), Color.green,0.5f);


        Vector3 v1 = new Vector3(10f, 0f, 0);
        DrawArrow.ForDebug(Vector3.zero, v1, Color.green, 0.5f);

        Vector3 v2 = new Vector3(0f, 10f, 0);
        DrawArrow.ForDebug(Vector3.zero, v2, Color.blue, 0.5f);

        


        Vector3 sub1 = v1 - v2;

        Vector3 sub2 = v2 - v1;


        DrawArrow.ForDebug(Vector3.zero, sub1, Color.black, 0.5f);
        DrawArrow.ForDebug(Vector3.zero, sub2, Color.red, 0.5f);




        //结论 v1-v2 方向是v2到v1
    
    }
}
