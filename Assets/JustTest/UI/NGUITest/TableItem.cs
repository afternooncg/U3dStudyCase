using UnityEngine;
using System.Collections;

public class TableItem : MonoBehaviour {

    Vector3 center;
	// Use this for initialization
	void Start () {

        center = GameObject.Find("Scroll View").transform.position;

       
	}
	
	// Update is called once per frame
	void Update () {

        //if (gameObject.name == "TableItem")
        {
            //Debug.Log(gameObject.transform.localPosition + " _ " + gameObject.transform.position);
            if (Mathf.Abs(gameObject.transform.position.x - center.x) < 0.1f)
            {
                float a = 1f + (0.5f-Mathf.Abs(gameObject.transform.position.x - center.x) * 0.5f / 0.1f);
                Debug.Log("scale");
                TweenScale.Begin(gameObject, 0.1f, new Vector3(a, a, 1f));
            }
            else
            {
                TweenScale.Begin(gameObject, 0.1f, new Vector3(1f, 1f, 1f));
            }
        }

        Debug.DrawLine(new Vector3(0,0,-1),center, Color.red);

	}

    
}
