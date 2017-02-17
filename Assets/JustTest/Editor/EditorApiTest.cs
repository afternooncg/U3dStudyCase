using UnityEngine;
#if UNITY_EDITOR	
using UnityEditor;
#endif
using System.Collections;

public class EditorApiTest : MonoBehaviour {

	// Use this for initialization
	void Start () {


        TestEditorPref();

	}
	
	// Update is called once per frame
	void Update () {
	
    }

    private void TestEditorPref()
    {              
        
#if UNITY_EDITOR
        EditorPrefs.SetInt("SoundOff", 1);
        Debug.Log(EditorPrefs.GetInt("SoundOff"));
     //   FileUtil.CopyFileOrDirectory
        
#endif
    }

}
