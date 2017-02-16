using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class TcpTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        string srvIP = "10.0.2.152";
        int srvPort = 5999;

        clientSock.Connect(srvIP, srvPort);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
