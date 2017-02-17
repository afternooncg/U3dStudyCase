using UnityEngine;
using System.Collections;

public class TestEventAndMemLeak : MonoBehaviour {


    public UIButton btn;

	// Use this for initialization
	void Start () {
        //return;
        

        //System.GC.Collect();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void OnClick()
    {
        for (int i = 0; i < 1000; i++)
        {
            TestEvent t = new TestEvent();
            UIEventListener.Get(btn.gameObject).onClick += t.CallBack;
            UIEventListener.Get(btn.gameObject).onClick = null;
            t.Destroy();
        
        }

       // System.GC.Collect();
    }
}


class TestEvent
{
    public GameObject[] cube;
    

    public TestEvent()
    {
        cube = new GameObject[1];
        for(int i=0;i<cube.Length;i++)
            cube[i] = GameObject.CreatePrimitive(PrimitiveType.Cube); 
    }

    

    public void CallBack(GameObject go)
    {
        Debug.Log("len:" + cube.Length);
    }

    public void Destroy()
    {
        for (int i = 0; i < cube.Length; i++)
            GameObject.DestroyImmediate(cube[i]);
    }
}




