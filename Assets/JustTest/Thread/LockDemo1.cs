using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LockDemo1   {


    private object lockObj;

    public List<string> ListOutPut;

    private int m_total = 100;

    private int m_step = 1;


    private Thread[] m_thread;

    public LockDemo1()
    {
        lockObj = new object();

        ListOutPut = new List<string>();


        m_thread = new Thread[20];


        for (int i = 0; i < m_thread.Length; i++ )
        {
            Thread thread = new Thread(new ThreadStart(count));
            thread.Name = "countthread_" + i.ToString();
            m_thread[i] = thread;

        }

    }


    public void Start()
    {

        for (int i = 0; i < m_thread.Length; i++)
        {
            m_thread[i].Start();
        }
    
    }


    public string PrintThreadState()
    {

        lock (lockObj)
        {            
            string str = "";

            for (int i = 0; i < m_thread.Length; i++)
            {
                if (m_thread[i] == null || !m_thread[i].IsAlive)
                    continue;
                str += m_thread[i].Name + "  :  " + m_thread[i].ThreadState + "\n";
            }

            return str;
        }
        
    }


    void count()
    {
        lock (lockObj)
        {
            while (true)
            {
                if (m_total <= 0)
                {
                    ListOutPut.Add(Thread.CurrentThread.Name + " 结束");

                    break;

                }
                else
                {

                    string str = Thread.CurrentThread.Name + "  " + Thread.CurrentThread.ThreadState + "  m_total = " + m_total.ToString();
                    str += " - " + m_step.ToString();
                    m_total -= m_step;
                    str += " = " + m_total.ToString();

                    ListOutPut.Add(str);
                    Thread.Sleep(10);
                }
            }
        
        }

        
    }


}
