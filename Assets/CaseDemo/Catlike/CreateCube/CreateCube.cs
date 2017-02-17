using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class CreateCube : MonoBehaviour {


    public int xSize, ySize, zSize;        //非长度,是最后1个点的索引
    private Mesh mesh;
    private Vector3[] vertices;
    int[] triangles;
	// Use this for initialization
	void Awake () {

        StartCoroutine(Generate());

	}
	
	// Update is called once per frame
    IEnumerator Generate()
    {
        this.GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "hello";
     
        WaitForSeconds wait = new WaitForSeconds(1f);

        int cornerVertices = 8;                                             //8个角的点

        int edgeVertices = (xSize + ySize  + zSize - 3) * 4;                //12条边 去除头尾重复点的点数(xSize - 1 + ySize - 1 + zSize - 1) * 4; 

        //去除角 边，其他面上的全部点
        int faceVertices = ((xSize-1)*(ySize-1) + (ySize-1)*(zSize-1) + (xSize-1)*(zSize-1))*2;                                              
        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];


        //画点
        int v =0;
        for (int i = 0; i <= ySize; i++)
        {

            for (int x = 0; x <= xSize; x++)
            {
                vertices[v++] = new Vector3(x, i, 0);
              //  yield return wait;
            }

            for (int z = 1; z <= zSize; z++)
            {
                vertices[v++] = new Vector3(xSize, i, z);
               // yield return wait;
            }

            for (int x = xSize - 1; x >= 0; x--)
            {
                vertices[v++] = new Vector3(x, i, zSize);
               // yield return wait;
            }

            for (int z = zSize - 1; z > 0; z--)
            {
                vertices[v++] = new Vector3(0, i, z);
              //  yield return wait;
            }

            
        }



        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                vertices[v++] = new Vector3(x, ySize, z);
                //   yield return wait;
            }
        }


        for (int z = 1; z < zSize; z++ )
        {
            for (int x = 1; x < xSize; x++)
            {
                vertices[v++] = new Vector3(x, 0, z);
            //    yield return wait;
            }
        }


        mesh.vertices = vertices;
        
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2 * 6;
        triangles = new int[quads];

        int tIndex = 0; int vIndex = 0;
        int levelLen = (xSize + zSize) * 2;

        for (int y = 0; y < zSize; y++, vIndex++)
        {
            
            for (int xbegin = 0; xbegin < levelLen - 1; xbegin++, vIndex++)
            {
                int v00 = vIndex;
                int v10 = vIndex + 1;
                int v01 = levelLen + vIndex;
                int v11 = v01 + 1;

                tIndex = SetQuadTri(triangles, tIndex, v00, v10, v01, v11);
                Debug.Log("tIndex:" + tIndex);
              //  yield return wait;

            }

            tIndex = SetQuadTri(triangles, tIndex, vIndex, vIndex - levelLen + 1, levelLen + vIndex, vIndex + 1);
            yield return wait;
        }

        int vMin = levelLen * (ySize + 1) - 1;
        int vMid = vMin + 1;
        int vMax = vIndex + 2;

        tIndex = SetQuadTri(triangles, tIndex, vMin, vMid, vMin - 1, vMid + xSize - 1);
        yield return wait;
        for (int x = 1; x < xSize - 1; x++, vMid++)
        {
            tIndex = SetQuadTri(
                triangles, tIndex,
                vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
            yield return wait;
        }
        tIndex = SetQuadTri(triangles, tIndex, vMid, vMax, vMid + xSize - 1, vMax + 1);


            mesh.triangles = triangles;
        
	}

    void OnDrawGizmos()
    {
        
        if (vertices == null)
            return;

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
            Gizmos.DrawSphere(vertices[i], 0.1f);
        

        if (triangles == null)
            return;

        Gizmos.color = Color.red;
        for (int i = 0; i < triangles.Length; i++)
            Gizmos.DrawSphere(vertices[triangles[i]], 0.2f);
    }


    /*
     * 4个顶点位置编码
     * v01  v11
     * 
     * v00  v01
     */

    static int SetQuadTri(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = v01;
        triangles[i + 2] = v10;

        triangles[i + 3] = v01;
        triangles[i + 4] = v11;
        triangles[i + 5] = v10;

        return i + 6;
    }
}
