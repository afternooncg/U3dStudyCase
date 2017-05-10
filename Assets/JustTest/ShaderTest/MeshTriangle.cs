using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshTriangle : MonoBehaviour
{

    private Mesh mesh;
    // Use this for initialization
    void Start()
    {

        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = new Vector3[] { Vector3.zero, new Vector3(0, 1, 0), new Vector3(1, 1, 0) };
        mesh.uv = new Vector2[] { Vector2.zero, Vector2.zero, Vector2.zero };
        mesh.triangles = new int[] { 0, 1, 2 };




    }

    // Update is called once per frame
    void Update()
    {

    }
}