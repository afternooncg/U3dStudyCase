using UnityEngine;
using System.Collections;

public class RocateMain : MonoBehaviour {

    private GameObject m_cube;
    private GameObject m_sphere;

	// Use this for initialization
	void Start () {

        m_cube = GameObject.Find("Cube");
        m_sphere = GameObject.Find("Sphere");
	}
	
	// Update is called once per frame
	void Update () {
        if (m_cube && m_sphere)
        {
            //m_sphere.transform.RotateAround(m_cube.transform.position, Vector3.up , Time.deltaTime * 10);
            //m_sphere.transform.RotateAround(m_cube.transform.position, m_cube.transform.rotation.eulerAngles , Time.deltaTime * 10);
            m_sphere.transform.RotateAround(m_cube.transform.position,  m_cube.transform.up, Time.deltaTime * 20);
            //m_sphere.transform.Rotate(Vector3.up, Space())
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A DOWN");
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("A UP");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("A HOLD DOWN");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Left DOWN");        
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            Debug.Log("Left UP");  
        }
        else if (Input.GetButton("Fire1"))
        {
            Debug.Log("Left HOLD DOWN");  
        }
	}
}
