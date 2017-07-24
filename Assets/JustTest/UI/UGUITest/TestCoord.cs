using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoord : MonoBehaviour {


    public GameObject prefab;
	// Use this for initialization
	void Start () {
		
	}

    private bool m_isPress = false;
    private Vector2 m_offset;
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            SetBasicValues();
            m_isPress = true;
            Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_offset.x = v.x - transform.position.x;
            m_offset.y = v.y - transform.position.y;
        }

        if (m_isPress && Input.GetMouseButton(0))
        {

            Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            v.x -= m_offset.x;
            v.y -= m_offset.y;


            if (rightBorder - leftBorder > 20)
            { 
                if (v.x < leftBorder + 10)
                    v.x = leftBorder + 10;

                if (v.x > rightBorder - 10)
                    v.x = rightBorder - 10;
            }

            if (topBorder - downBorder > 20)
            {
                if (v.y > topBorder - 10)
                    v.y = topBorder - 10;

                if (v.y < downBorder + 10)
                    v.y = downBorder + 10;
            }

            transform.position = new Vector3(v.x, v.y, transform.position.z);
        }


        if (Input.GetMouseButtonUp(0))
        {
            m_isPress = false;
            Debug.Log(Camera.main.orthographicSize + " " + Camera.main.aspect + " " + Camera.main.aspect * Camera.main.orthographicSize * 2);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

           	RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.name == "Quad")
                {
                    Debug.Log(hitInfo.point + "  " + mousePosiToGridPosi(hitInfo.point) + " " + mousePosiToGridPosi(transform, hitInfo.point));

                    if (prefab != null)
                    {
                        GameObject g = GameObject.Instantiate<GameObject>(prefab);
                        g.transform.localScale = Vector3.one;
                        g.transform.parent = transform;
                        Vector3 v = mousePosiToGridPosi(transform, hitInfo.point);
                        g.transform.localPosition = gridPosiTolocalPosi(v);
                    }
                }
            }


            Vector3 v1 = transform.position;

            if (rightBorder - leftBorder <= 20)
            {

                if (v1.x > leftBorder + 10)
                {
                   
                    v1.x = leftBorder + 10;
                }

                if (v1.x < rightBorder - 10)
                    v1.x = rightBorder - 10;
        
            }

            if (topBorder - downBorder <= 20)
            {
                if (v1.y < topBorder - 10)
                    v1.y = topBorder - 10;

                if (v1.y > downBorder + 10)
                    v1.y = downBorder + 10;
            }

            transform.position = new Vector3(v1.x, v1.y, transform.position.z);
        }

	}

    Vector3 mousePosiToGridPosi(Vector3 v)
    {
        v.x = Mathf.Floor(v.x)+10;
        v.y = Mathf.Floor(v.y)+10;
        v.z = 0;
        return v;
    }


   // [HideInInspector]
    public float leftBorder;
   // [HideInInspector]
    public float rightBorder;
   // [HideInInspector]
    public float topBorder;
 //   [HideInInspector]
    public float downBorder;
    private float width;
    private float height;

    void SetBasicValues()
    {


        //the up right corner
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f,
                                                                       Mathf.Abs(-Camera.main.transform.position.z)));

        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        rightBorder = cornerPos.x;
        topBorder = cornerPos.y;
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);

        width = rightBorder - leftBorder;
        height = topBorder - downBorder;

    }

    Vector3 mousePosiToGridPosi(Transform local, Vector3 v)
    {
        v = local.InverseTransformPoint(v); //local.InverseTransformPoint(v);
        v = v * 20;
        v = mousePosiToGridPosi(v);
        return v;
    }

    Vector3 gridPosiTolocalPosi(Vector3 v)
    {
        v.x = v.x -10 + 0.5f;
        v.y = v.y - 10 + 0.5f;
        v.z = -1;
        v = v / 20 ;        
        return v;
    }
    
}
