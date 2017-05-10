using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestThreadMain : MonoBehaviour {


    LockDemo1 m_lockDemo;

    UnityAction m_callBack;

    public Text OutputText;

	// Use this for initialization
	void Start () {

        m_lockDemo = new LockDemo1();
	}
	
	// Update is called once per frame

    int count = 0;
    int total = 0;
	void Update () {


        if (m_callBack != null)
            m_callBack();

        count++;
        if(count >= 50)
        {
           //string str = m_lockDemo.PrintThreadState();
           total = m_lockDemo.GetTotal();
           count = 0;
        }

       // Debug.Log("total:" + total);
       
	}



    public void HandleBtnTestLock()
    {
        m_lockDemo.Start();
        m_callBack = callBackForLockDemo;
    }

    void callBackForLockDemo()
    {
        if (OutputText != null)
        {
            string str = "";

            for (int i = 0; i < m_lockDemo.ListOutPut.Count; i++)
            {
                
                str += "i:" + i + "    " +  m_lockDemo.ListOutPut[i] + "\n";
            }
            OutputText.text = str;
        }
            
    
    }
}
