using UnityEngine;
using System.Collections;

public class TestScale : MonoBehaviour {

    public GameObject go;

	// Use this for initialization
	void Start () {

       // go = GameObject.CreatePrimitive(PrimitiveType.Cube);
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(go.transform.localScale + "----" + go.transform.lossyScale);        
	}

    void OnMouseDown()
    {
        Debug.Log("test");

        
        if (go.transform.parent)
        {
            Debug.Log("test: " + go.transform.parent.lossyScale + "-----" + go.transform.parent.localScale);

            go.transform.localScale = Vector3.one;// go.transform.parent.lossyScale;// new Vector3(go.transform.parent.lossyScale.x / go.transform.parent.localScale.x, go.transform.parent.lossyScale.y / go.transform.parent.localScale.y, go.transform.parent.lossyScale.z / go.transform.parent.localScale.z);
        }
    }
}
