using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadResTestMain : MonoBehaviour {


    public enum E_LoadSetup
    { 
        NONE =0,
        LoadMoudle_1,
        LoadMoudle_2,
        LoadMoudle_3,
        
        END,
    }




    public class LoaderCallBack
    {
        public delegate bool LoaderCallbackDelegate(ref int count);
        public int Total;
        public int CurrentCount;
        public LoaderCallbackDelegate CallBack;
        public E_LoadSetup LoadType;
    }

   List<LoaderCallBack> m_listLoaders;
    E_LoadSetup m_currentLoaded = E_LoadSetup.NONE;
    float m_currLoadProgress = 0f;
	// Use this for initialization
	void Start () {
        m_listLoaders = new List<LoaderCallBack>();

        Moudule1 m1 = new Moudule1();        

        register_load(E_LoadSetup.LoadMoudle_1, m1.Loading, m1.total);

        Moudule2 m2 = new Moudule2();
        register_load(E_LoadSetup.LoadMoudle_2, m2.Loading, m2.total);

        register_load(E_LoadSetup.LoadMoudle_1, m2.Loading, m2.total);

        Moudule3 m3 = new Moudule3();
        register_load(E_LoadSetup.LoadMoudle_3, m3.Loading, m3.total);

        register_load(E_LoadSetup.END, null, 0);

        GameObject.Find("Slider").GetComponent<Slider>().value = m_currLoadProgress = 0f;
        Debug.Log("test");
	}
	
	// Update is called once per frame
	void Update () {

        bool flag = loading();    

	}

    bool loading()
    {
        if (m_currentLoaded == E_LoadSetup.NONE)
        {
            m_currentLoaded++;
            GameObject.Find("Slider").GetComponent<Slider>().value = 0;
            return false;
        }

        if (m_currentLoaded < E_LoadSetup.END)
        {
            if (m_listLoaders.Count > 0)
            {
                LoaderCallBack obj = m_listLoaders[0];

                float oldPro = obj.CurrentCount / (float)obj.Total;//上一刻的百分比
                int curValue = obj.CurrentCount;

                obj.CallBack(ref curValue);
                obj.CurrentCount = curValue;

                float curPro = obj.CurrentCount / (float)obj.Total; //此刻的百分比
                float addPro = (curPro - oldPro) / (float)(E_LoadSetup.END - 1);
                Debug.Log("addPro:" + addPro);
                m_currLoadProgress += addPro;

                GameObject.Find("Slider").GetComponent<Slider>().value = m_currLoadProgress;

                if (obj.CurrentCount == obj.Total)
                {
                    m_listLoaders.RemoveAt(0);
                    m_currentLoaded++;

                }
            }
            return false;
        }
        else if ((m_currentLoaded == E_LoadSetup.END))
        {

            m_listLoaders.RemoveAt(0);
            m_currentLoaded++;
            GameObject.Find("Slider").GetComponent<Slider>().value = m_currLoadProgress = 1f;
            Debug.Log("LoadComplete");
            return true;
        }
        else
            return true;
    }



    void register_load(E_LoadSetup setup, LoaderCallBack.LoaderCallbackDelegate callback, int max)
    {
        LoaderCallBack cbobj = m_listLoaders.Find(x => x.LoadType == setup);
        if (cbobj != null)
        {
            Debug.Log("Exist Same");
            return;
        }


        cbobj = new LoaderCallBack();
        cbobj.LoadType = setup;
        cbobj.CallBack = callback;
        cbobj.Total = max;

        m_listLoaders.Add(cbobj);


    }
   

}


class BaseMoudle
{
    public int total = 0;    
    protected string moduleName = "";
    public bool Loading(ref int loadCount)
    {
        if (loadCount < total)
        {
            loadCount++;
            Debug.Log(moduleName + " loading " + loadCount);
            
            return false;
        }
        else
        {

            Debug.Log(moduleName + " loading end ");
            return true;
        }
    }
}


class Moudule1 : BaseMoudle
{
    public Moudule1()
    {
        total = 113;
        moduleName = "Moudule1";
    }
}


class Moudule2 : BaseMoudle
{
    public Moudule2()
    {
        total = 115;
        moduleName = "Moudule2";
    }
}

class Moudule3 : BaseMoudle
{
    public Moudule3()
    {
        total = 2;
        moduleName = "Moudule3";
    }
}