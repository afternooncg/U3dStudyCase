using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.SceneManagement;
public class TestCoroutine : MonoBehaviour {


    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
	// Use this for initialization
	void Start () {
		Thread thread = new Thread(Test);
		thread.Start();

    

	}

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log(arg0.name + " __ " + (LoadSceneMode)arg1);
    }
	
	
	void Test()
	{
		//Debug.Log("============" + Time.realtimeSinceStartup);
	}

   

	void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 50), "测试1：" + result);
        if (GUI.Button(new Rect(0, 100, 100, 50), "开启协程"))
        {
            StartCoroutine(GetResult());
        }

        GUI.Label(new Rect(200, 0, 200, 50), "测试2：" + result1);
        if (GUI.Button(new Rect(200, 100, 100, 50), "无协程测试"))
        {
            GetResult1();
        }

        GUI.Label(new Rect(400, 0, 200, 50), "测试执行同1个协程两次");
        if (GUI.Button(new Rect(400, 100, 100, 50), "开始测试"))
        {
            CallTwoCoroutine();
        }

        GUI.Label(new Rect(600, 0, 200, 50), "测试执行协程和Update");
        if (GUI.Button(new Rect(600, 100, 100, 50), "开始测试"))
        {
            CallCoroutineAndUpdate();
        }        

         GUI.Label(new Rect(800, 0, 200, 50), "测试执行另个mono里的协程");
        if (GUI.Button(new Rect(800, 100, 100, 50), "开始测试"))
        {
            StartCoroutine(this.GetComponent<TestModule1>().Test());
            StopAllCoroutines();
        }
    }

   

    private void CallTwoCoroutine()
    {
        StartCoroutine("CallTest", "a");
        StartCoroutine("CallTest", "b");
    }

    int pubCount = 0;

    IEnumerator CallTest(string t)
    {
        int i = 0;
        pubCount++;
        while (i < 50)
        {
            i++;
            yield return null;
        }
        pubCount--;

        Debug.Log(t + " " + pubCount);
    
    }

	
	float result;
    IEnumerator GetResult()
    {
        for (int i = 0; i < 1000; i++)
        {
            for (int j = 0; j < 1000000; j++)
            {
                result += (i + j);
                if (j % 500000 == 0)
                {
                    yield return null;
                    //yield break;  // 等同上行;
                }   
            }

            if (i % 100 == 0)
            {
                yield return null;
                //yield break;  // 等同上行;
            }   
        }
    }
	
	float result1;
    void GetResult1()
    {
        for (int i = 0; i < 1000; i++)
        {
            for (int j = 0; j < 1000000; j++)
            {
                result1 += (i + j);
            }
        }
    }


    bool m_isCheckUpdate = false;
    int m_total = 100;
    int m_currentIndex = 0 ;
    private void CallCoroutineAndUpdate()
    {
        if (!m_isCheckUpdate)
        {
            m_isCheckUpdate = true;
            StartCoroutine("CoroutineAndUpdate");
        }
        else
            m_isCheckUpdate = false;
    }


    void Update()
    {
        if(m_isCheckUpdate)
        {
            m_currentIndex++;
            if(m_total>=m_currentIndex)
            {
                Debug.Log("Load End");
                m_isCheckUpdate = false;
            }

        }
        
        
    }
    IEnumerator CoroutineAndUpdate()
    {
        while (m_total > m_currentIndex)
        {
            m_currentIndex += 1;
            yield return null;
            
        }

        Debug.Log("CoroutineAndUpdate End");
        
    }
}
