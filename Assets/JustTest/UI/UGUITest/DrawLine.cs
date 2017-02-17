using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour {

	// Use this for initialization
	Material aMaterial;

    private LineRenderer mLine;

void Start(){

    mLine = this.gameObject.AddComponent <LineRenderer>();

    mLine.SetWidth(5, 5);

    mLine.SetVertexCount(3000);

    mLine.SetColors (Color.yellow,Color.yellow);

    mLine.material = aMaterial;

    mLine.material.color = new Color(0f, 1f, 0f, 0.25f);

    
    //mLine.renderer.enabled = true;

}

int i = 0;

void Update()
{

    if (i < 300)
    {

        mLine.SetPosition(i, Input.mousePosition);

    }

    i++;

}
}
