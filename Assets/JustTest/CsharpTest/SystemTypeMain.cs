using UnityEngine;
using System.Collections;
using System;

public class SystemTypeMain : MonoBehaviour {

    

	// Use this for initialization
	void Start () {

        TestDelegateType();
        TestDelegateInHashSet();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    #region  辨别Delegate型别
  public delegate void TestDelegate();
    public delegate void TestDelegate<T>(T p0);
    public delegate void TestDelegate1();

    private TestDelegate callback1;
    private TestDelegate<int> callback2;
    private TestDelegate1 callback3;

    void TestDelegateType()
    {
        Debug.Log(typeof(TestDelegate));
        Debug.Log(typeof(TestDelegate1));

        callback1 = func1;
        callback2 = func2;
        callback3 = func1;
        Debug.Log(callback1.GetType());
        Debug.Log(callback2.GetType());
        Debug.Log(callback3.GetType());

        Debug.Log("是否相等? " + (callback2.GetType() == callback1.GetType()));

        Debug.Log(typeof(int));
        int a = 1;
        Debug.Log(a.GetType());
        
    }

    void func1() { }
    void func2(int a) { }

    void func4() { }
    void TestDelegateInHashSet()
    {     
        System.Collections.Generic.HashSet<TestDelegate> hash = new System.Collections.Generic.HashSet<TestDelegate>();
        System.Collections.Generic.HashSet<Delegate> hashBase = new System.Collections.Generic.HashSet<Delegate>();
        hash.Add(callback1);
        TestDelegate callbacktemp = func4;
        hash.Add(callbacktemp);

        hashBase.Add(callback1);
        hashBase.Add(callback2);
        hashBase.Add(callback3);
        hashBase.Add(callbacktemp);

        Debug.Log("callback1 == callbacktemp? " + (callback1 == callbacktemp));
        
    }

  #endregion


  
}
