using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Star : MonoBehaviour {

    public Vector3 point = Vector3.up;
    public int numberOfPoints = 10;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

        
    public int frequency = 1;

    public ColorPoint center;
    public ColorPoint[] points;

    private Color[] colors;

    void Start()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Star Mesh";

        if (frequency < 1)
        {
            frequency = 1;
        }
        if (points == null)
        {
            points = new ColorPoint[0];
        }
        int numberOfPoints = frequency * points.Length;
        vertices = new Vector3[numberOfPoints + 1];
        colors = new Color[numberOfPoints + 1];
        triangles = new int[numberOfPoints * 3];

        if (numberOfPoints >= 3)
        {
            vertices[0] = center.position;
            colors[0] = center.color;
            float angle = -360f / numberOfPoints;
            for (int repetitions = 0, v = 1, t = 1; repetitions < frequency; repetitions++)
            {
                for (int p = 0; p < points.Length; p += 1, v += 1, t += 3)
                {
                    vertices[v] = Quaternion.Euler(0f, 0f, angle * (v - 1)) * points[p].position;
                    colors[v] = points[p].color;
                    triangles[t] = v;
                    triangles[t + 1] = v + 1;
                }
            }
            triangles[triangles.Length - 1] = 1;
        }
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.triangles = triangles;
    }
}
