using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Diagnostics;
using System;

public class SystemTest : MonoBehaviour {



    public class A : IEnumerable
    {

        private int[] array = new int[10];

        public IEnumerator GetEnumerator()
        {

            for (int i = 0; i < 10; i++)
            {

//                if (i < 8)

                    yield return array[i];

                //else

                  //  yield break;

            }

           // yield break;
        }

    }
    public class MyTest
    {
        int a = 0;

        public MyTest()
        {

        }

        public MyTest(int pa)
        {
            a = pa;
        }

        public void Call()
        {
            UnityEngine.Debug.Log("hello");
        }

        public void Call1(string str)
        {
            UnityEngine.Debug.Log(str);
        }
    }

    System.Diagnostics.Process getDateProcess;

    LinkedList<int> list;
	// Use this for initialization
    void Start()
    {
        A a = new A();

        foreach(object i in a)
        {
            if(i!=null)
                UnityEngine.Debug.Log(i);
        }

    }


    void TestActivator()
    {
        //Activator.CreateInstance(Type.GetType());
    }


	void ExecCmd () {
        list = new LinkedList<int>();
        Process process = new Process();
        //设定程序名
        process.StartInfo.FileName = "cmd.exe";
        //关闭Shell的使用
        process.StartInfo.UseShellExecute = false;
        //重新定向标准输入，输入，错误输出
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        //设置cmd窗口不显示
        process.StartInfo.CreateNoWindow = true;
        //开始
        process.Start();
        //输入命令，退出
     //   process.StandardInput.WriteLine("ping 192.168.0.1");
        //process.StandardInput.WriteLine("netstat");
   //     process.StandardInput.WriteLine("exit");
        //获取结果
        string strRst = process.StandardOutput.ReadToEnd();
        UnityEngine.Debug.Log(strRst);

        return;
        /*
        Process notePad = new Process();
        // FileName 是要執行的檔案
        notePad.StartInfo.FileName = "notepad.exe";
        notePad.Start();
        notePad.Close();
         */
        return;
        getDateProcess = new System.Diagnostics.Process();
        getDateProcess.StartInfo.UseShellExecute = false;
     //   getDateProcess.StartInfo.RedirectStandardOutput = true;
        getDateProcess.StartInfo.RedirectStandardInput = true;
        getDateProcess.StartInfo.RedirectStandardOutput = true; 

        getDateProcess.StartInfo.FileName = "cmd";
        //getDateProcess.StartInfo.Arguments = @"log -1 --date=default --format=%cd";
     //   process.Arguments = "/c " + cmd;
        getDateProcess.StartInfo.Arguments = "/c dir";
        
     //   getDateProcess.StartInfo.WorkingDirectory = @"e:\\";
        getDateProcess.StartInfo.CreateNoWindow = true;//不显示程序窗口
        getDateProcess.Start();
        string str = getDateProcess.StandardOutput.ReadToEnd();
       // Debug.Log("str");
      //  getDateProcess.WaitForExit();
        
	}
	
   
	// Update is called once per frame
	void Update () {
       // list.AddLast(1);
	}
}

