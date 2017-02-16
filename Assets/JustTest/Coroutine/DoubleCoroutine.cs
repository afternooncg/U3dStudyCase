using UnityEngine;
using System.Collections;
using System.Threading;
public class DoubleCoroutine : MonoBehaviour {


    //Coroutine是在同一线程内
	// Use this for initialization
	void Start () {

        Debug.Log("主程序执行开始 CurrentId :  " + Thread.CurrentThread.ManagedThreadId);
        //StartCoroutine("enterframe");
        StartCoroutine(enterframe());
		StartCoroutine(enterframe1());		
        Debug.Log("主程序执行结束 CurrentId :  " + Thread.CurrentThread.ManagedThreadId);

	
	}

    bool flag = true;
	// Update is called once per frame
	void Update () {
        if (flag)
            Debug.Log("主程序updating CurrentId :  " + Thread.CurrentThread.ManagedThreadId);
	}

    IEnumerator enterframe()
    {
        Debug.Log("开始Eenterframe CurrentId :  " + Thread.CurrentThread.ManagedThreadId);
        yield return new WaitForSeconds(4);
        flag = false;
        Debug.Log("这里结束Eenterframe CurrentId :  " + Thread.CurrentThread.ManagedThreadId);
    }

	 IEnumerator enterframe1()
    {		
        Debug.Log("开始Eenterframe1 CurrentId :  " + Thread.CurrentThread.ManagedThreadId);
        yield return new WaitForSeconds(5);
        flag = false;
		Debug.Log("开始Eenterframe1 第2阶段 CurrentId :  " + Thread.CurrentThread.ManagedThreadId);
		yield return new WaitForSeconds(5);
		Debug.Log("开始Eenterframe1 第3阶段 CurrentId :  " + Thread.CurrentThread.ManagedThreadId);
		StartCoroutine(SubCoroutine());
		yield return new WaitForSeconds(5);
        Debug.Log("这里结束Eenterframe1 CurrentId :  " + Thread.CurrentThread.ManagedThreadId);
    }
	
	IEnumerator SubCoroutine()
	{
		Debug.Log("开始SubCoroutine  CurrentId :  " + Thread.CurrentThread.ManagedThreadId);
		yield return new WaitForSeconds(5);
		Debug.Log("结束SubCoroutine CurrentId :  " + Thread.CurrentThread.ManagedThreadId);
	}

}
