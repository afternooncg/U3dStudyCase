using UnityEngine;
using System.Collections;

public class TestSingleton : UntiySingleton<TestSingleton>
{

    public int sum;
	// Use this for initialization
	void Start () {
        Debug.Log("GetHashCode:" + this.GetHashCode());
        sum = (int)Random.Range(10.0f,100.0f);

        Debug.Log("Sum " + sum);

        //DictionaryBase a;
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
