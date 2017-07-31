using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherScene : MonoBehaviour {

    public Camera cam;
    Canvas canvas;
	// Use this for initialization
	void Start () {

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

  
	
	// Update is called once per frame
	void Update () {

        Vector2 v = (RectTransformUtility.WorldToScreenPoint(cam, this.transform.position));

        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, v, canvas.worldCamera, out pos))
        {
            CameraAnotherScene.Instance.img.rectTransform.anchoredPosition = pos;
        }


        
        



	}
}
