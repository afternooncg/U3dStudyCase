using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public GameObject LoopBg;
    Material m_bgmat;

    float m_bgMovSpeed = 1.2f;

	// Use this for initialization
	void Start () {
        if (LoopBg != null)
            m_bgmat = LoopBg.GetComponent<Renderer>().material;


        Application.targetFrameRate = 60;   //fps设置
        Screen.sleepTimeout = SleepTimeout.NeverSleep;  //不灭屏?

	}
	
	// Update is called once per frame
	void Update () {

        float v = Time.deltaTime * m_bgMovSpeed * 0.02f;

        if (m_bgmat != null)
            m_bgmat.mainTextureOffset += new Vector2(v, v);
       
	}

    void OnDestroy()
    {
        m_bgmat = null;
        LoopBg = null;
    }
}
