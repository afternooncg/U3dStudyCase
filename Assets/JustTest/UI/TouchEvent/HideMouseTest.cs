using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMouseTest : MonoBehaviour {


    public Texture2D mouseicon;
	// Use this for initialization
	void Start () {
        //Screen.showCursor = false; //隐藏mouse
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnGUI()
    {
        if (mouseicon != null)
        {
            float mx = Input.mousePosition.x - mouseicon.width / 2;
            float my = Input.mousePosition.y + mouseicon.height / 2;

            GUI.DrawTexture(new Rect(mx, Screen.height - my, mouseicon.width, mouseicon.height), mouseicon);
        }
    }
}
