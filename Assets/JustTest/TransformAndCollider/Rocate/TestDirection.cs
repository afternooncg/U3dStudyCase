using UnityEngine;
using System.Collections;

public class TestDirection : MonoBehaviour {
	
	int count = 0;
	GameObject m_obj;
	Vector3 m_dir;
	public Texture2D mouseicon;
	public GameObject cubeEye;
	// Use this for initialization
	void Start () {
		m_obj = GameObject.Find("Cube");
		//Screen.showCursor = false; //隐藏mouse
	}
	
	// Update is called once per frame
	void Update () 
	{
		count++;
		if(count==100)
		{
			m_obj.transform.Rotate(new Vector3(0,90,0));
			m_dir = (new Vector3(200,0,300)- m_obj.transform.position).normalized;
		}
		else
			m_dir = (new Vector3(0,0,300)- m_obj.transform.position).normalized;
		
		//m_obj.transform.Translate(Vector3.forward*Time.deltaTime); //会自动记的本地的z方向
		//m_obj.transform.position += new Vector3(0,0,1*Time.deltaTime);//只会按世界坐标走
	 	//m_obj.transform.position += m_obj.transform.TransformDirection(new Vector3(0, 0, 1*Time.deltaTime));//效果等同第1句
		
		m_obj.transform.position += m_obj.transform.TransformDirection(m_dir*Time.deltaTime);//可辨认方向

		if(cubeEye)
			cubeEye.transform.LookAt(m_obj.transform.position);
	}

	void OnGUI()
	{
		if(mouseicon!=null)
		{
			float mx = Input.mousePosition.x - mouseicon.width /2;
			float my = Input.mousePosition.y + mouseicon.height /2;

			GUI.DrawTexture(new Rect(mx,Screen.height - my, mouseicon.width,mouseicon.height),mouseicon);
		}
	}
	void OnMouseDown()
	{
		print("mmmm");
		//m_obj.transform.Translate(Vector3.back*Time.deltaTime); //会自动记的本地的z方向
	}
}
