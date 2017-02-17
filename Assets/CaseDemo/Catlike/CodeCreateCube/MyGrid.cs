using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class MyGrid : MonoBehaviour {

    public int xSize;   //段数 （格子数）
    public int ySize;

    private Vector3[] vertiecs;

    // Use this for initialization
	void Awake () {
        StartCoroutine(createGrid());	
	}

    private IEnumerator createGrid()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        vertiecs = new Vector3[(xSize + 1) * (ySize + 1)];

        for (int y = 0, i = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize ; x++, i++)
            {
                //Debug.Log("i=" + i + " x:" + x + " y:" + y);
                vertiecs[i] = new Vector3(x, y, 0);                
            }
        }

        Mesh ms = new Mesh();
        this.GetComponent<MeshFilter>().mesh = ms;
        ms.vertices = vertiecs;
        

        int[] trs = new int[xSize * ySize * 6];        
        for (int ty = 0, ti=0; ty < ySize; ty++)
        {
            for (int tx = 0; tx < xSize; tx++, ti+=6)
            {
                int start = (xSize + 1) * ty;
                trs[ti] = start + tx;
                trs[ti + 1] = xSize + 1 + start + tx;
                trs[ti + 2] = 1 + start + tx;
                trs[ti + 3] = 1 + start + tx;
                trs[ti + 4] = xSize + 1 + start + tx;
                trs[ti + 5] = xSize + 2 + start + tx;
                ms.triangles = trs;
                this.GetComponent<MeshFilter>().mesh = ms;
                yield return wait;
            }
        }


        Vector2[] uv = new Vector2[vertiecs.Length];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertiecs[i] = new Vector3(x, y);
                uv[i] = new Vector2(x / xSize, y / ySize);
            }
        }
        
        ms.uv = uv;
        ms.RecalculateNormals();      

        yield return wait;

    }


    void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(new Vector3(0,0,0), 0.1f);
        if (vertiecs == null)
            return;
        
        Gizmos.color = Color.blue;

        for (int i = 0; i < vertiecs.Length; i++)
        {
            Gizmos.DrawSphere(vertiecs[i], 0.1f);            
        }


        
    }
	
	
}
