using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class ChildSence : MonoBehaviour {

    delegate void Fun();
    Fun m_CallBack ;
	// Use this for initialization
    List<GameObject> m_list;
	void Start () {

        m_list = new List<GameObject>();

        m_CallBack += MemTest.TestCallBack;


        GameObject go = new GameObject();
        go.AddComponent<UpdateLog>();
        go.transform.parent = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddMem()
    {
        for (int i = 0; i < 10000; i++)
            m_list.Add(new GameObject());

        if (m_CallBack != null)
            m_CallBack();
    }

    void OnDisable()
    {
        while(m_list.Count>0)
        {
            Destroy(m_list[0], 0);
            m_list.RemoveAt(0);
        }
    }
}
