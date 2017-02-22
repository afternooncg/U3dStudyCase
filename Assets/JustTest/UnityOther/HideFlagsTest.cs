using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 
这个HideFlags是一个enum 。而且其中的作用和DontDestroyOnLoad函数的作用是一致的。HideFlags的枚举成员有 : DontSava , HideAndDontSave , HideInHierarchy , HideInInspector , None(默认)，NotEditable。
我的Unity版本是：5.4.0f3。在这个版本测试 ： 
一 ，DontSave和HideAndDontSave的效果是一样的。HideAndDontSave的功能：如果GO被HideAndDontSave表示go.hideFlags = HideFlags.DontSave（在Transform中则无效）。在退出程序时，需要手动销毁，不然会产生内存泄漏：DestroyImmediate函数，在面板中会隐藏 ，但是使用DontSave也会隐藏。
具体：

    public void OnApplicationQuit()
    {
        @go = GameObject.Find("Cube");
        if (@go != null)
        {
            Debug.Log("必须要强制消除上个Scene的Cube！");
            DestroyImmediate(@go);
        }
    }
它们和DontDestroyOnLoad的效果是一样的。会重复制造@go（子对象不会）。这点一定要注意，虽然说在Hierarchy看不见 ， 但在Scene面板中拖动一下，就会发现在复制。

二，HideInHierarchy 在Hierarchy面板中隐藏（子对象有效）。只有在Awake方法中使用才有效。

三，HideInInspector在Inspector面板中隐藏（子对象无效）。若在GO中使用，则GO中所有的组件将隐藏。若对某个组件使用，则只有其组件隐藏。
 */
public class HideFlagsTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

     //   gameObject.hideFlags = HideFlags.DontSave;          //等同DontDestroyOnLoad  Hierarchy 不可见 只对1级对象有用     
//        gameObject.hideFlags = HideFlags.DontSaveInEditor;  //>
        gameObject.hideFlags = HideFlags.NotEditable;       //运行时无法编辑
        gameObject.hideFlags = HideFlags.HideInHierarchy;   //运行时隐藏
        gameObject.hideFlags = HideFlags.None;
        gameObject.hideFlags = HideFlags.HideInInspector;    //除了材质，其他全隐藏
     //   gameObject.hideFlags = HideFlags.HideAndDontSave;
        
        SceneManager.LoadScene("IoTest");
	}
	
	// Update is called once per frame
	void Update () {

        Debug.Log("i am runing");
	}

    //当程序退出时用DestroyImmediate()销毁被HideFlags.DontSave标识的对象  
    //否则即使程序已经退出，被HideFlags.DontSave标识的对象依然在Hierarchy面板中  
    //即每运行一次程序就会产生多余对象，造成内存泄漏  
    void OnApplicationQuit()
    {
        GameObject cube = GameObject.Find("Cube");
        
        if (cube)
        {
            Debug.Log("Cube0 DestroyImmediate");
            DestroyImmediate(cube);
        }
        
    }  
}
