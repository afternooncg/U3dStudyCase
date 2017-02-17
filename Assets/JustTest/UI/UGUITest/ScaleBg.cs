
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScaleBg : MonoBehaviour {

 public GameObject go;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	Debug.Log (Camera.main.orthographicSize.ToString() +  " ____ " + Camera.main.aspect);
	gameObject.GetComponent<Image> ().SetNativeSize ();

	Vector3 s = (gameObject.GetComponent<Image> ().sprite.bounds.size);
	//Rect s = gameObject.GetComponent<Image> ().sprite.rect;
	float h = Camera.main.orthographicSize * 2;

	float w = h * Camera.main.aspect;


	Debug.Log ("w:" + w + " h:" + h + " " + s);
	transform.localScale = new Vector3 (s.x/w, s.y/h, 1);
	//Screen.width
	this.GetComponent<RectTransform>().sizeDelta = new Vector2( Screen.width, Screen.height);
	this.GetComponent<RectTransform>().localScale = new Vector3 (s.x/w, s.y/h, 1);
        /*
	Debug.Log("mm:" + go.GetComponent<Renderer>().material.mainTexture.width/100);
	Debug.Log (go.GetComponent<Renderer> ().material.GetTexture (1));
	
        */
    go.transform.localScale = new Vector3(go.GetComponent<Renderer>().material.mainTexture.width / 100 / w, s.y / h, 1);
	}

    void OnWillRenderObject()
    {
        Debug.Log("out");
    }
}
