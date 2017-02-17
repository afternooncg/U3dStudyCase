using UnityEngine;
using System.Collections;

public class TestSingeltonMain : MonoBehaviour {

    public GameObject cube1;
    public GameObject sphere1;
	void Start () {

    //    cube1.AddComponent<TestSingleton>();
     //   cube1.AddComponent<TestSingleton>();

       
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("TestSingleton.GetInstance().sum: " + TestSingleton.GetInstance().sum);
	}
}
