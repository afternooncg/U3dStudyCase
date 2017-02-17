using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class TestExecuteInEditMode : MonoBehaviour {

	// Use this for initialization
    public int id = 0;
    private int oldid = 0;
	void Start () {
        Debug.Log("Start  " + Application.isPlaying);
        oldid = id;
	}

    void Awake()
    {
        Debug.Log("Awake  " + Application.isPlaying);
    }
	

#if UNITY_EDITOR

	// Update is called once per frame
	void Update () {
	    if(oldid != id)
        {
            Debug.Log("change");
            oldid = id;
        }
	}
#endif
}
