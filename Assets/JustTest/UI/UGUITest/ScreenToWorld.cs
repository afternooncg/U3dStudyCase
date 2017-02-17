using UnityEngine;
using System.Collections;

public class ScreenToWorld : MonoBehaviour {

	
	private float camHalfHeight;
	private float camHalfWidth; 
	
	void Awake()
	{
		this.camHalfHeight = Camera.main.orthographicSize;
		this.camHalfWidth = Camera.main.aspect * camHalfHeight; 
	}
	
	void Update () {
		//手指 或者 鼠标的坐标传进去
		Move(Input.mousePosition);
	}
	
	public void Move(Vector2 postions){
		Move(new Vector3(postions.x,postions.y,0));
	}
	public void Move(Vector3 postions){
		//在这里进行坐标的换算

		Vector3 v = ScreenToWorld1(postions);
		v.z = 0;

		transform.position = v;
		Debug.Log(transform.position);
	}
	
	private Vector3 ScreenToWorld1(Vector3 postion)
	{
		
		return Camera.main.ScreenToWorldPoint(postion);
	}


}
