using UnityEngine;
using System.Collections;

public class TestDrawIcon : MonoBehaviour {

	public float explosionRadius = 5.0F;
	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {
	

	
	
	}



	void OnGUI () {

		Vector2 pointA = new Vector2(Screen.width/2, Screen.height/2);

		Vector2 pointB = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);



	}


	void OnDrawGizmosSelected() {
		Gizmos.color = Color.black;
		Gizmos.DrawSphere(transform.position, 1);
		Gizmos.DrawWireSphere (transform.position, explosionRadius);
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, 1);

		Debug.DrawLine (Vector3.zero, new Vector3 (10, 10, 10), Color.blue);

		//	Debug.DrawLine (Vector3.zero, new Vector3 (10, 10, 10), Color.blue);
		//Debug.
		Debug.DrawLine (Vector3.zero, new Vector3 (4, 0, 0), Color.red);
	}
}
