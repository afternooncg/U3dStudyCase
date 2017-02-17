using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading;

public class TestAssert : MonoBehaviour {

    public GameObject txt;
    private int a = 0;

    private Thread t;
    private Mutex mutex;
    private Thread[] myThread;
    private int aryLen = 3;

    public delegate void CallBack(int param);

    private Dictionary<int, CallBack> m_Dict = new Dictionary<int, CallBack>();
    private List<int> m_List = new List<int>();

    void Start()
    {
        mutex = new Mutex();
        myThread = new Thread[aryLen];

        m_Dict.Add(1, CallBack1);
        m_Dict.Add(2, CallBack2);

        for(int i=0;i<aryLen;i++)
        {
            Thread th = new Thread(new ThreadStart(SubThreadCall));
            th.Name = "sub_thread" + i.ToString();
            th.Start();
            myThread[i] = th;
        }
    }

	// Use this for initialization
    public void WatchAssetPass()
    {
        Debug.Log("WatchAssetPass Begin");

        bool test = true;
        Debug.Assert(test, "I need test is true");

        Debug.Log("WatchAssetPass End");

        if (t == null)
        {
            t = new Thread(new ThreadStart(SubThreadCall1));
            t.Start();
        }
        else
        {
            if (t.ThreadState == ThreadState.Suspended)
            {
                Debug.Log("恢复");
                t.Resume();
              
            }
            else
            {
                Debug.Log("没挂起?");
            }
        }
        
    }

    public void WatchAssetNotPass()
    {

        Debug.Log("WatchAssetNotPass Begin");

        bool test = false;
        Debug.Assert(test, "I need test is true");

        Debug.LogError("Error Hapen");

        Debug.Log("WatchAssetNotPass End");

        if(t!=null)
        {
            if (t.IsAlive)
            {
                Debug.Log("挂起");
                t.Suspend();
            }
        }
        
    }


    void Update()
    {
        txt.GetComponent<Text>().text = a.ToString();

        if(m_List.Count>0)
        {
            int tmpa = m_List[0];
            m_List.RemoveAt(0);

            if (tmpa % 2 == 0)
                m_Dict[1](tmpa);
            else
                m_Dict[2](tmpa);
        }

    }

    void  SubThreadCall()
    {
        
        while(true)        
        {
            mutex.WaitOne();
            a++;
            Debug.Log(Thread.CurrentThread.Name + ":" + a);
            m_List.Add(a);
            mutex.ReleaseMutex();
            Thread.Sleep(1000);
        }
        
    }


    void SubThreadCall1()
    {
        while (true)
        {
            mutex.WaitOne();
            a += 100;
            int j = 0;
            for (int i = 0; i < 1000000;i++ )
            {
                j += i;
            }
                Debug.Log("SubThreadCall1:" + j);
            mutex.ReleaseMutex();
            Thread.Sleep(2000);
        }
       
    }


    void CallBack1(int param)
    {
        Debug.Log("i am CallBack1");
    }


    void CallBack2(int param)
    {
        Debug.Log("i am CallBack2");
    }
}
