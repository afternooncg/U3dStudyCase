using UnityEngine;
using System.Collections;

public class CubeAI : MonoBehaviour {

	private float m_backTime = 0;
	public Transform target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(m_backTime==0)
			m_backTime = Time.time;
		else if(Time.time - m_backTime > 3f)
		{
			Debug.Log("Begin");
			m_backTime = Time.time;

			Quaternion rotate = Quaternion.Euler(0,Random.Range(1,5)*30f,0);
			//this.transform.rotation = Quaternion.Slerp(this.transform.rotation,rotate,Time.deltaTime * 3000);
			//this.transform.Translate(Vector3.right * Mathf.Slerp(

		}

		if(target)
		{
			//this.transform.position = new Vector3(target.position.x, this.transform.position.y, this.transform.position.z);
			//this.transform.Translate(Vector3.right * Mathf.Lerp(this.transform.position.x, target.position.x,Time.deltaTime * 1000));
			//this.transform.Translate(Vector3.right * Mathf.Lerp(this.transform.position.x, target.position.x,Time.deltaTime * 1000));
			this.transform.position += new Vector3(Mathf.Lerp(0, target.position.x -this.transform.position.x ,Time.deltaTime * 1000),0,0);
		}
	}  
}
