using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//在Game模式下不运行也会跑。update只会执行一次，除非一直移动Game窗口
 [ExecuteInEditMode]  
public class UnityTips : MonoBehaviour {

	// Use this for initialization

    [Tooltip("hp")]//鼠标hover的时候显示一个tooltip
    public float Offset;

    [Range(0, 10)]
    public float Offset1;

    [Header("分组1")]

    [Range(0,100)]
    public int v1;
    public int v2;

    [Header("分组2")]
    public int v3;
    public string v4;

   [HideInInspector]
    public string v5;

   [Space(30)]//可以与上面形成一个空隙
    public string v6;

   [System.NonSerialized]//使public属性不能序列化
    public float CloudNonSerialized = 1f;

    [SerializeField]//使private属性可以被序列化，在面板上显示并且可以读取保存
    private bool CloudSerializeField = true;


    public GizmoAndDebugMain m_test ;

	void Start () {

        gameObject.AddComponent<GizmoAndDebugMain>();

	}

     void OnEnable()
     {
#if UNITY_EDITOR
         UnityEditor.EditorApplication.update += Update;  //
#endif
     }

     void OnDisable()
     { 
     #if UNITY_EDITOR
        UnityEditor.EditorApplication.update -= Update;  //
#endif
     }


    [ContextMenu("Execute in Editor Mode ")]
    void executeTest()
    {
        Debug.Log("test");
    }

	// Update is called once per frame
	void Update () {
        Debug.Log("h");
	}
}
