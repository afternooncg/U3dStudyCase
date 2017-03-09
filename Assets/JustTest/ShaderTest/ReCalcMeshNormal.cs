using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReCalcMeshNormal : MonoBehaviour {

	// Use this for initialization
    void Start()
    {

        Mesh ms = GetComponent<MeshFilter>().sharedMesh;
        GetComponent<MeshFilter>().mesh.RecalculateNormals();

	


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDrawGizmos()
    {
        return;
        
        Mesh ms = GetComponent<MeshFilter>().sharedMesh;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < ms.normals.Length; i++)
        {

            Gizmos.DrawLine(ms.vertices[i], ms.normals[i]);
            //Debug.Log(ms.vertices[i] + " " + ms.normals[i]);
        }
        
    }
}
