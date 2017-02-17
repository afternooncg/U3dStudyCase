using UnityEngine;
using System.Collections;

public class CombineInstanceTest : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;            
            //combine[i]. = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.active = false;
        }

        Debug.Log(mr[0]);
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        gameObject.GetComponent<MeshRenderer>().materials[0] = mr[0].sharedMaterial;
        
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
