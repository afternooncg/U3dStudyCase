using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordTest : MonoBehaviour {

    public Transform GoTsf;
	// Use this for initialization
	void Start () {

        if (GoTsf != null)
        {
            Bounds bd = GoTsf.GetComponent<MeshFilter>().mesh.bounds;
            Debug.Log("bd.min:" + bd.min);
            Debug.Log("bd.max:" + bd.max);
        }
        

	}
	
	// Update is called once per frame
	void OnGUI () {

        //注意quad / plane 坐标区别

       //TestWorldPosi();
        //TestPlaneLocalPosi();
        TestQuadLocalPosi();
	}

    void TestQuadLocalPosi()
    {
        if (GoTsf != null)
        {
            Vector3 v = GoTsf.TransformPoint(new Vector3(2f, 2f, 0f));

            //v.z = 0;
            v = Camera.main.WorldToScreenPoint(v);



            GUI.Label(new Rect(v.x, Screen.height - v.y, 100, 30), "位置");

        }
    }

    void TestPlaneLocalPosi()
    {
        if (GoTsf != null)
        {
            Vector3 v = GoTsf.TransformPoint(new Vector3(5f, 0,5f));

            //v.z = 0;
            v = Camera.main.WorldToScreenPoint(v);



            GUI.Label(new Rect(v.x, Screen.height - v.y, 100, 30), "位置");

        }
    }

    void TestWorldPosi()
    {
        if (GoTsf != null)
        {
            Vector3 v = GoTsf.position;

            v.z = 0;
            v = Camera.main.WorldToScreenPoint(v);



            GUI.Label(new Rect(v.x, Screen.height - v.y, 100, 30), "位置");

        }
    }
}
