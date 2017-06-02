using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TestAutoResetEventMain : MonoBehaviour {

    //声明AutoResetEvent变量
    //如果参数为true的，则初始值第一次WaitOne是直接通过的，不会阻塞。 如果是false的话，直接阻塞直到得到Set()信号通知
    static AutoResetEvent autoReset = new AutoResetEvent(false);


	// Use this for initialization
	void Start () {
        //创建线程
        Thread th = new Thread(new ThreadStart(MyMethod));
        th.Start();


        //发送阻塞完成信号，让线程中的代码开始执行
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("主线程代码..");
        //    autoReset = new AutoResetEvent(false);
            autoReset.Set();    //发送信号，通知阻塞完成
            Thread.Sleep(1000);
        }
	}
    


    //线程执行方法
    static void MyMethod()
    {
        while (true)
        {
            autoReset.WaitOne();    //阻塞，直到调用了 autoReset.Set(); 进行通知。
            Debug.Log("WaitOne阻塞完成了");
        }
    }
}
