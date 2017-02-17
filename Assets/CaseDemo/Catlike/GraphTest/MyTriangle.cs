using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class MyTriangle : MonoBehaviour {


    private Mesh m_mesh;

    public Vector3 pos1;
    public Vector3 pos2;
    public Vector3 pos3;

    public int index1;
    public int index2;
    public int index3;

    public Matrix4x4 m_martrix;

    public enum Axle { x, y, z };
    public Axle axle;
    public float angle = 15f;//旋转角度

	// Use this for initialization
	void Start () {

        this.GetComponent<MeshFilter>().mesh = m_mesh = new Mesh();

     //   pos1 = new Vector3(0, 1, 0);
      //  pos3 = new Vector3(-1, 0, 0);
     //   pos2 = new Vector3(1, 0, 0);

    //    index1 = 0;
     //   index2 = 1;
     //   index3 = 2;

        Vector3[] vertices = new Vector3[3];
        vertices[0] = pos1;
        vertices[1] = pos2;
        vertices[2] = pos3;
        m_mesh.vertices = vertices;

        int[] tri = new int[3];
        tri[0] = 0;
        tri[1] = 1;
        tri[2] = 2;
        m_mesh.triangles = tri;

        m_mesh.RecalculateNormals();

        
        

       //m_martrix.SetTRS(transform.position, transform.rotation, transform.localScale);
       //return;


        //修改平移
        /*
        Vector4 v = new Vector4(transform.position.x, transform.position.y, transform.position.z, 1);

        m_martrix =  Matrix4x4.identity;
        m_martrix.m03 = 3;
        m_martrix.m13= 4;
        m_martrix.m23 = 5;
        v = m_martrix * v;

        transform.position = new Vector3(v.x, v.y, v.z);
         */

        //缩放矩阵
        Vector4 v1 = new Vector4(transform.localScale.x, transform.localScale.y, transform.localScale.z, 1);
        m_martrix = Matrix4x4.identity;
        m_martrix.m00 = 3;
        m_martrix.m11 = 4;
        m_martrix.m22 = 5;
        v1 = m_martrix * v1;

        transform.localScale = new Vector3(v1.x, v1.y, v1.z);


        
        m_martrix = Matrix4x4.identity;
        //旋转矩阵
        if (axle == Axle.x)
        {
            m_martrix.m11 = Mathf.Cos(angle * Mathf.Deg2Rad);
            m_martrix.m12 = -Mathf.Sin(angle * Mathf.Deg2Rad);
            m_martrix.m21 = Mathf.Sin(angle * Mathf.Deg2Rad);
            m_martrix.m22 = Mathf.Cos(angle * Mathf.Deg2Rad);
        }
        else if (axle == Axle.y)
        {
            m_martrix.m00 = Mathf.Cos(angle * Mathf.Deg2Rad);            
            m_martrix.m02 = Mathf.Sin(angle * Mathf.Deg2Rad);
            m_martrix.m20 = -Mathf.Sin(angle * Mathf.Deg2Rad);
            m_martrix.m22 = Mathf.Cos(angle * Mathf.Deg2Rad);
        
        }
        else if (axle == Axle.z)
        {

            m_martrix.m00 = Mathf.Cos(angle * Mathf.Deg2Rad);
            m_martrix.m01 = -Mathf.Sin(angle * Mathf.Deg2Rad);
            m_martrix.m10 = Mathf.Sin(angle * Mathf.Deg2Rad);
            m_martrix.m11 = Mathf.Cos(angle * Mathf.Deg2Rad);
        }

        float qw = Mathf.Sqrt(1f + m_martrix.m00 + m_martrix.m11 + m_martrix.m22) / 2;
        float w = 4f * qw;
        float qx = (m_martrix.m21 - m_martrix.m12) /w;
        float qy = (m_martrix.m02 - m_martrix.m20) /w;
        float qz = (m_martrix.m10 - m_martrix.m01) /w;

        transform.rotation = new Quaternion(qx, qy, qz, qw);



	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnDrawGizmos()
    {
        if (m_mesh == null)
            return;

        Vector3[] vertices = m_mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
