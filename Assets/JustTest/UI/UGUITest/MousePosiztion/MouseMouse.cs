using UnityEngine;
using System.Collections;

public class MoveMouse : MonoBehaviour {

	[System.Serializable]
	public struct TestStruct
	{
		public int a;
		public int b;
	}



	public TestStruct posi;
	public GameObject go;
	public GameObject prefab;
	// Use this for initialization
	void Start () {
		if (go != null)
				StartGame ();
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log (Time.time);

	}

	void StartGame()
	{
		StartCoroutine ("EnterFrame");
	}

	IEnumerator EnterFrame()
	{
		for (int i=0; i<200; i++) {

			yield return new  WaitForSeconds(0.2f);
			
		GameObject go1 = GameObject.CreatePrimitive (PrimitiveType.Cube);
		go1.transform.SetParent (this.transform);
		go1.transform.Rotate (new Vector3 (0, Time.time, 0));

        GameObject go2 = Instantiate(prefab, new Vector3(i * 1, 0, i * 1),  Quaternion.Euler(new Vector3(1, Time.deltaTime*5, 1))) as GameObject;

			go2.transform.SetParent (this.transform);
			Debug.Log("i:=" + i);
		//	go.transform.Rotate(new Vector3(0,5,0));
		}

	}
}
