using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class TestLock : MonoBehaviour {

    private static Object lockObject = null;
    private Queue<int> m_queue;
    private List<Thread> m_ths;
    private int i = 0;
	// Use this for initialization
	void Start () {

        if (lockObject == null)
        {
            lockObject = new Object();
            Debug.Log("lockObject init");
        }

        m_queue = new Queue<int>();
        m_ths = new List<Thread>();
        Debug.Log("TestLock start");
	}


    public void StartThread()
    {
        for (int i = 0; i < 5; i++)
        {
            Thread th = new Thread(new ThreadStart(Push));
            m_ths.Add(th);
            th.Start();
        }
    }

    public void KillThread()
    {
        for (int i = 0; i < 5; i++)
        {
            m_ths[i].Abort();
            m_ths[i] = null;
        }
        m_ths.Clear();
    }


	// Update is called once per frame
	void Update () {

     
	}

    public  void Push()
    {
        while (true)
        {
            lock (lockObject)
            {
                i++;
                m_queue.Enqueue(i);
                Debug.Log("current thread id " + Thread.CurrentThread.ManagedThreadId.ToString() + "  :  " + i);
            }
            Thread.Sleep(1000);        
        }        
    }

    public void Pop()
    {
        lock (lockObject)
        {
           while (m_queue.Count > 0)
                Debug.Log("pop:" + m_queue.Dequeue());
        }  
    }

}
