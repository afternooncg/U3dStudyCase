using UnityEngine;
using System.Collections;
using System.Threading;

public class TestResetEvent : MonoBehaviour {


    private static AutoResetEvent autoConnectEvent = new AutoResetEvent(false);
    Thread th;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per fram
	void Update () {
        if (th != null)
            Debug.Log("th.IsAlive " + th.IsAlive);
	}

    public void CallCreateThread()
    {
        Debug.Log("CallCreateThread is supsend");
        th = new Thread(new ThreadStart(SubThreadCall));
        th.IsBackground = true;
        th.Start();
        
        autoConnectEvent.WaitOne();
        Debug.Log("CallCreateThread is resume");
    }


    public void SubThreadCall()
    {
        int i = 10;
        autoConnectEvent.Set();  //释放不一定会马上切换，要等系统切换
        while(i>0)
        {
            i--;
            Debug.Log("SubThreadCall " + i);
            Thread.Sleep(1000);

        }
        

    }

    void OnDestroy()
    {
        th.Abort();
    }
}
