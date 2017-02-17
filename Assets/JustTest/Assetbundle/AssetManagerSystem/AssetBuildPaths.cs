using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class AssetBuildPathObj
{
    public GameObject prefab;
    public string desc;
}
public class AssetBuildPaths : ScriptableObject
{
    


    static AssetBuildPaths m_instance;

    public AssetBuildPathObj[] AssetPath;



    public static AssetBuildPaths instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new AssetBuildPaths();

            return m_instance;
        }


    }


    private AssetBuildPaths()
    {
     
    }

   
    void OnDestroy()
    {
        m_instance = null;
	}
    
}
