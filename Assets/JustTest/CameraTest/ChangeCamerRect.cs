using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeCamerRect : MonoBehaviour {

 public InputField txtX;
 public InputField txtY;
 public InputField txtW;
 public InputField txtH;
 public Camera  SecondCamera;
 public Camera  MainCamera;
 public Camera  SwitchCamera;

	// Use this for initialization
	void Start () 
 {	
	float screenWidth = Screen.width;
	float screenHeight = Screen.height;
	Debug.Log ("screenWidth:" + screenWidth + "  screenHeight:" + screenHeight);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

 public void  handleBtnUpdate()
 {	 
	Debug.Log (float.Parse (txtX.text) + "," + float.Parse (txtY.text) + "," + float.Parse (txtW.text) + "," + float.Parse (txtH.text));
	SecondCamera.pixelRect = new Rect (float.Parse(txtX.text), float.Parse(txtY.text), float.Parse(txtW.text), float.Parse(txtH.text));

    
 }

 public void  handleBtnSwatch()
 {	 

	Debug.Log (Camera.main);
	if (MainCamera.enabled) {
	 Debug.Log ("Disable main");
	 SwitchCamera.enabled = true;
	 MainCamera.enabled = false;
	} else {
	
	 Debug.Log ("enable main");
	 SwitchCamera.enabled = false;
	 MainCamera.enabled = true;
	
	}


 }


}


