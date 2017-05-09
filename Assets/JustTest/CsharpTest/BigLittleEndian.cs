using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLittleEndian : MonoBehaviour {

	// Use this for initialization
	void Start () {
        test();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void test()
    {
        short x = 6;
        byte[] little = BitConverter.GetBytes(x);
        Debug.Log(little);
        Array.Reverse(little);
        Debug.Log(little);


        short b = System.Net.IPAddress.HostToNetworkOrder(x); //把x转成相应的大端字节数
        byte[] bb = System.BitConverter.GetBytes(b);//这样直接取到的就是大端字节序字节数组。
        Debug.Log(bb);
        /*
            UInt16 pInt = 0x1234;
            byte[] a = BitConverter.GetBytes(pInt);
            byte[] b = new byte[4];
            b[1] = (byte)((pInt & 0xFF00) >> 8);
            b[0] = (byte)((pInt & 0xFF));
         *        Debug.Log(a);
         */


    }
}
