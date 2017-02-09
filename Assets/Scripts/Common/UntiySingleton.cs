using UnityEngine;
using System.Collections;

//以mono为基类的单例基类
//使用方式 public class TestSingleton : UntiySingleton<TestSingleton>

public class UntiySingleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;

    public static T GetInstance()
    {
        if (_instance == null)
        {
            _instance = UnityEngine.Object.FindObjectOfType<T>();
        }

        return _instance;
    }


    protected virtual void Awake()
    {
        this.CheckInstance(); //觉醒检查
    }

    protected bool CheckInstance()//是否唯一，不是就销毁多余的目标，防止目标有多个挂载在其他gameobject上。
    {
        if (this == UntiySingleton<T>.GetInstance())
        {
            return true;
        }
        Debug.Log("重复挂载GameObject");
        UnityEngine.Object.Destroy(this.gameObject);
        //UnityEngine.Object.Destroy(this);
        return false;
    }
}
