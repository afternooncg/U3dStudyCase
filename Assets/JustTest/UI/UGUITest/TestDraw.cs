using UnityEngine;
using System.Collections;

public class TestDraw : MonoBehaviour {

	// Use this for initialization
	void Start () {
	

        //Debug.DrawLine("")

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        for(int i=0;i<10;i++)
        {
            Gizmos.DrawCube(new Vector3(i * 2, 0, 0), new Vector3(1, 1, 1));

            Gizmos.DrawLine(new Vector3(i, i, 0), new Vector3(i + 1, i + 1, 0));

            Debug.DrawLine(new Vector3(i, i, i), new Vector3(i + 1, i + 1, 0),Color.red);
        }

        Debug.Log("OnDrawGizmos");

        
    }
}
