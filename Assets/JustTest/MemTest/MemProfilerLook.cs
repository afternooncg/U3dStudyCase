using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MemProfilerLook : MonoBehaviour
{


    string m_savePath = string.Empty;
    void Awake()
    {
        Profiler.enabled = true;
#if UNITY_EDITOR
        m_savePath = Application.dataPath + "/StreamingAssets";

        //  Profiler.logFile =  Application.dataPath.Replace("Assets","") + "/mylog.log";  
#else
        m_savePath = Application.streamingAssetsPath;
      //  Profiler.logFile = Application.persistentDataPath + "/mylog.log";  
#endif
        m_savePath += "/assetbundles";
    }


    // Use this for initialization
    void Start()
    {





        OutputInfo();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public InputField text;
    AssetBundle m_ab;
    GameObject m_prefab;
    GameObject m_terr;
    public void Load()
    {

        if (m_terr != null)
            return;

        float time = Time.realtimeSinceStartup;

        m_ab = AssetBundle.LoadFromFile(m_savePath + "/terrain.unity3d");


        text.text = "load ab:" + ((Time.realtimeSinceStartup - time)).ToString() + "秒\n";
        time = Time.realtimeSinceStartup;

        m_prefab = m_ab.LoadAsset<GameObject>(m_ab.GetAllAssetNames()[0]);
        text.text += "load asset:" + ((Time.realtimeSinceStartup - time)).ToString() + "秒\n";

        time = Time.realtimeSinceStartup;
        m_terr = GameObject.Instantiate<GameObject>(m_prefab);
        text.text += "clone asset:" + ((Time.realtimeSinceStartup - time)).ToString() + "秒\n";




        m_ab.Unload(false);
        m_ab = null;
        OutputInfo(false);

    }

    public void Unload()
    {
        if (m_terr != null)
        {
            GameObject.DestroyImmediate(m_terr, true);
            GameObject.DestroyImmediate(m_prefab, true);
            m_prefab = null;
            m_terr = null;

            Resources.UnloadUnusedAssets();
        }


        OutputInfo();

    }

    string formatByte(long num)
    {
#if UNITY_EDITOR
        return UnityEditor.EditorUtility.FormatBytes(num);
#else
        if(num < 1024)
            return  num.ToString() + "b";
        else if(num < 1024*1024)
            return  (num/1024).ToString() + "k";
        else
            return  (num/(1024*1024)).ToString() + "m";
#endif
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("MemProfiler");


    }

    private void OutputInfo(bool isclear = true)
    {
        int m = 1024 * 1024;
        if (isclear)
            text.text = string.Empty;

        text.text += "==============================================================================\n";

        text.text += "程序占用内存(引擎+资源) usedHeapSizeLong: " + formatByte(Profiler.usedHeapSizeLong).ToString() + "\n";
        text.text += "GetMonoHeapSizeLong: " + formatByte(Profiler.GetMonoHeapSizeLong()).ToString() + "\n";
        text.text += "GetMonoUsedSizeLong: " + formatByte(Profiler.GetMonoUsedSizeLong()).ToString() + "\n";

        text.text += "==============================================================================\n";

        text.text += "Unity申请的全部 GetTotalReservedMemoryLong: " + formatByte(Profiler.GetTotalReservedMemoryLong()).ToString() + "\n";
        text.text += "Unity用的 GetTotalAllocatedMemoryLong: " + formatByte(Profiler.GetTotalAllocatedMemoryLong()).ToString() + "\n";
        text.text += "总剩余:GetTotalUnusedReservedMemoryLong: " + formatByte(Profiler.GetTotalUnusedReservedMemoryLong()).ToString() + "\n";

        text.text += "==============================================================================\n";

        text.text += "GetTempAllocatorSize: " + formatByte(Profiler.GetTempAllocatorSize() / m).ToString() + "\n";

        text.text += "==============================================================================\n";

        if (m_prefab != null)
            text.text += "m_prefab_GetRuntimeMemorySizeLong: " + formatByte(Profiler.GetRuntimeMemorySizeLong(m_prefab)).ToString() + "\n";

        if (m_terr != null)
            text.text += "m_terr_GetRuntimeMemorySizeLong: " + formatByte(Profiler.GetRuntimeMemorySizeLong(m_terr)).ToString() + "\n";


        text.text += "==============================================================================\n";
        text.text += ("All " + Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)).Length + "\n");
        text.text += ("Textures " + Resources.FindObjectsOfTypeAll(typeof(Texture)).Length + "\n");
        //text.text += ("AudioClips " + Resources.FindObjectsOfTypeAll(typeof(AudioClip)).Length + "\n");
        text.text += ("Meshes " + Resources.FindObjectsOfTypeAll(typeof(Mesh)).Length + "\n");
        text.text += ("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length + "\n");
        text.text += ("GameObjects " + Resources.FindObjectsOfTypeAll(typeof(GameObject)).Length + "\n");
        text.text += ("Components " + Resources.FindObjectsOfTypeAll(typeof(Component)).Length + "\n");

        text.text += "==============================================================================\n";
        Texture[] textures = Resources.FindObjectsOfTypeAll<Texture>();
        foreach (Texture t in textures)
        {
#if !UNITY_EDITOR
            text.text += ("Texture object " + t.name + " using: " + formatByte(Profiler.GetRuntimeMemorySizeLong(t)) + "\n");
#else
            Debug.Log("Texture object " + t.name + " using: " + formatByte(Profiler.GetRuntimeMemorySizeLong(t)));
#endif
        }

        Mesh[] meshes = Resources.FindObjectsOfTypeAll<Mesh>();
        foreach (Mesh t in meshes)
        {
#if !UNITY_EDITOR
            text.text += ("Mesh object " + t.name + " using: " + formatByte(Profiler.GetRuntimeMemorySizeLong(t)) + "\n");
#else
            Debug.Log("Mesh object " + t.name + " using: " + formatByte(Profiler.GetRuntimeMemorySizeLong(t)));
#endif
        }


        Material[] mats = Resources.FindObjectsOfTypeAll<Material>();
        foreach (Material t in mats)
        {
#if !UNITY_EDITOR
            text.text += ("Material object " + t.name + " using: " + formatByte(Profiler.GetRuntimeMemorySizeLong(t)) + "\n");
#else
            Debug.Log("Material object " + t.name + " using: " + formatByte(Profiler.GetRuntimeMemorySizeLong(t)));
#endif
        }

        GameObject[] gos = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject t in gos)
        {
#if !UNITY_EDITOR
            text.text += ("GameObject object " + t.name + " using: " + formatByte(Profiler.GetRuntimeMemorySizeLong(t)) + "\n");
#else
            Debug.Log("GameObject object " + t.name + " using: " + formatByte(Profiler.GetRuntimeMemorySizeLong(t)));
#endif
        }


        Component[] Components = Resources.FindObjectsOfTypeAll<Component>();
        foreach (Component t in Components)
        {
#if !UNITY_EDITOR
            text.text += ("Component object " + t.name + " using: " + formatByte(Profiler.GetRuntimeMemorySizeLong(t)) + "\n");
#else
            Debug.Log("Component object " + t.name + " using: " + formatByte(Profiler.GetRuntimeMemorySizeLong(t)));
#endif
        }

        System.GC.Collect();
    }
}
