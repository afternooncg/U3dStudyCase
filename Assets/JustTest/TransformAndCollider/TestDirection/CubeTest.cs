using UnityEngine;
using System.Collections;

public class CubeTest : MonoBehaviour {

    public AudioSource hit;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown("w"))
        {
            hit.Play();
            //Debug.Log("w key");
           // hit.play();
            GameObject sp = GameObject.FindGameObjectWithTag("Sp0");
            Vector3 posi = sp.transform.position;
            posi += new Vector3(0, 0, Time.deltaTime * 1);
            sp.transform.position = posi;
         //   sp.transform.forward = sp.transform.position +
            Debug.Log(GameObject.Find("Cube"));

            if (Input.GetMouseButton(0))
                Debug.Log("Pressed left click.");

            if (Input.GetMouseButton(1))
                Debug.Log("Pressed right click.");

            if (Input.GetMouseButton(2))
                Debug.Log("Pressed middle click.");

           
        }

      

	}

    void OnMouseDown()
    {
        Debug.Log("Pressed click.");
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(200, 200, 0));
        Debug.DrawRay(Vector3.zero, Vector3.one, Color.yellow);
    }
}
