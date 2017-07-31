using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParallaxScroll : MonoBehaviour {

    public Transform[] objs;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        
        var points = new Vector4[3];        
        for (var i = 0; i < objs.Length; i++)
        {
            points[i] = new Vector4(objs[i].position.x, objs[i].position.y, objs[i].position.z, 0);
        }
        
        /*
        var points = new Vector3[3];
        for (var i = 0; i < objs.Length; i++)
        {
            points[i] = new Vector4(objs[i].position.x, objs[i].position.y, objs[i].position.z);
        }
         */

        var render = GetComponentInChildren<MeshRenderer>();
        var material = render.sharedMaterial;
        /*
        //foreach (var material in render.materials)
        {
           material.SetInt("_Points_Num",points.Length);
           //material.SetVectorArray("_Points",points);
           for (var i = 0; i < objs.Length; i++)
           {
               //points[i] = new Vector4(objs[i].position.x, objs[i].position.y, objs[i].position.z);
               material.SetVector("_Points" + i.ToString(), points[i]);
           }

          

        }*/

        MaterialPropertyBlock materialProperty = new MaterialPropertyBlock();
        materialProperty.SetVectorArray("_Points", points);
        materialProperty.SetFloat("_Points_Num", points.Length);
        gameObject.GetComponent<Renderer>().SetPropertyBlock(materialProperty);


	}
}
