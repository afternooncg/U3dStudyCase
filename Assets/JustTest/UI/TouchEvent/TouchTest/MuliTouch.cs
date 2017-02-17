using UnityEngine;
using System.Collections;

public class MuliTouch : MonoBehaviour {

	public Texture2D imageMenu ;
	
	public  Texture2D imageItem;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI  () {
	
		int touchCount = Input.touchCount;
		Debug.Log("touchCount:" + touchCount);
		for(int i = 0; i < touchCount; i++)
		{
			Vector2 iPos = Input.GetTouch(i).position;
			float x = iPos.x;
			float y = iPos.y;
			
			GUI.DrawTexture(new Rect(x,960 - y ,120,120),imageItem);
			
			GUI.Label(new Rect(x, 960 - y,120,120),"Touch position is  " + iPos);
		}

	}
}
