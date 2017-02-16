using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadResTestCoroutine : MonoBehaviour {

	// Use this for initialization

    float m_currLoadProgress = 0f;

    int m_moudel4_depentCount = 500;
    int m_moudel5_count = 0;
    bool m_flag = false;
	void Start () {
        GameObject.Find("Slider").GetComponent<Slider>().value = m_currLoadProgress = 0f;
	}
	
	// Update is called once per frame
	void Update () {

        if (m_flag)
        {
            if(m_moudel4_depentCount > m_moudel5_count)
                m_moudel5_count++;
            GameObject.Find("Slider").GetComponent<Slider>().value = m_currLoadProgress;
        }
           



        

	}

    public void BeginLoad()
    {
        m_flag = true;
        m_moudel5_count = 0;
        StartCoroutine("loading");    
    }

    IEnumerator loading()
    {
        GameObject.Find("Slider").GetComponent<Slider>().value = m_currLoadProgress = 0f;

        yield return LoadModudel1();

        yield return LoadModudel2();

        yield return LoadModudel3();

        yield return LoadModudel4();
        m_currLoadProgress = 1f;

        Debug.Log("LoadComplete");
        m_flag = false;
        yield return null;
    }

    float countAddPro(int max)
    {
        return 1f / (4f*max) ;
    }

    IEnumerator LoadModudel1()
    {
        Moudule1 m1 = new Moudule1();
        int count = 0;
        while (m1.total > count)
        {
            m1.Loading(ref count);
            m_currLoadProgress += countAddPro(m1.total);
            yield return null;
        }

        yield return null;

    }

    IEnumerator LoadModudel2()
    {
        Moudule2 m1 = new Moudule2();
        int count = 0;
        while (m1.total > count)
        {
            m1.Loading(ref count);
            m_currLoadProgress += countAddPro(m1.total);
            yield return null;
        }

        yield return null;

    }


    IEnumerator LoadModudel3()
    {
        Moudule3 m1 = new Moudule3();
        int count = 0;
        while (m1.total > count)
        {
            m1.Loading(ref count);
            m_currLoadProgress += countAddPro(m1.total);
            
            yield return null;
        }

        yield return null;

    }

    IEnumerator LoadModudel5()
    {

        while (m_moudel4_depentCount > m_moudel5_count)
        {            
            m_currLoadProgress += (float)(m_moudel5_count / m_moudel4_depentCount) * countAddPro(m_moudel4_depentCount);
            Debug.Log("LoadModudel5 " + m_moudel5_count);
            yield return null;
        }

        Debug.Log("Module5 end");
    }
    IEnumerator LoadModudel4()
    {
        yield return LoadModudel5();
        m_currLoadProgress += countAddPro(1);
        Debug.Log("Module4 end");
    }


}
