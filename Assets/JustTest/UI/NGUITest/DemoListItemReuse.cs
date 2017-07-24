using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoListItemReuse : MonoBehaviour {


    List<GameObject> m_Items = new List<GameObject>();
    int m_max = 0;
    float m_leftPosi = -150f;
    float m_halfItemW = 50f;
	// Use this for initialization
	void Start () {

        m_Items.Add(GameObject.Find("Item1"));
        m_Items.Add(GameObject.Find("Item2"));
        m_Items.Add(GameObject.Find("Item3"));
        m_Items.Add(GameObject.Find("Item4"));
        m_max = m_Items.Count;
	}
	
	// Update is called once per frame
	void Update () {


        for (int i = 0; i < m_Items.Count; i++)
        {

            m_Items[i].transform.Translate(Vector3.left*Time.deltaTime);
        
        }


        if (m_Items.Count > 0 && m_Items[0].transform.localPosition.x <= (m_leftPosi - m_halfItemW))
        {
            m_Items[0].transform.localPosition = new Vector3(-m_leftPosi + m_halfItemW, 0, 0);
            Debug.Log(m_Items[0].transform.localPosition);
            GameObject go = m_Items[0];

            m_Items.RemoveAt(0);
            m_Items.Add(go);
            go.GetComponentInChildren<UILabel>().text =( ++m_max ).ToString();
        }
        

	}
}
