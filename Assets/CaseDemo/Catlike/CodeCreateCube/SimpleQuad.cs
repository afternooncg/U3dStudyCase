using UnityEngine;
using System.Collections;

public class SimpleQuad : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        this.GetComponent<MeshFilter>().mesh = CreateMesh();
	}

    Mesh CreateMesh()
    {

        Mesh mesh = new Mesh();

        Vector3[] vs = new Vector3[6];
        vs[0] = new Vector3(-1, 0, 1);
        vs[1] = new Vector3(1, 0, 1);
        vs[2] = new Vector3(1, 0, -1);
        vs[3] = new Vector3(-2, 0, -2);
        vs[4] = new Vector3(-2, 0, 1);
        vs[5] = new Vector3(-2, 0, -1);

        int[] ts = new int[12];
        ts[0] = 0;
        ts[1] = 1;
        ts[2] = 2;
        ts[3] = 0;
        ts[4] = 2;
        ts[5] = 3;
        ts[6] = 4;
        ts[7] = 0;
        ts[8] = 3;
        ts[9] = 4;
        ts[10] = 3;
        ts[11] = 5;

        mesh.vertices = vs;
        mesh.triangles =ts;

        return mesh;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(0, 0, 0), new Vector3(0.5f, 0.5f, 0.5f));
    }
	
}
