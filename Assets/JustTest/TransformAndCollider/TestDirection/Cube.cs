using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		print("Click Cube");
		this.transform.Translate(Vector3.back*Time.deltaTime*20); //会自动记的本地的z方向
	}
}
