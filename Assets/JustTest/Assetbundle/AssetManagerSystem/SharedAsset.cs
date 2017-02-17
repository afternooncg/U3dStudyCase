using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SharedAsset : ScriptableObject
{

    static SharedAsset m_instance;
    public static SharedAsset instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new SharedAsset();

            return m_instance;
        }
    }

    public string FontsShareName;
    public string AltasesShareName;
    public string TexturesShareName;
    public string ShadersShareName;


    //[SerializeField]
    public List<GameObject> Fonts;

    //[SerializeField]
    public List<GameObject> Altases;

    //[SerializeField]
    public List<Texture> Textures;

    //[SerializeField]
    public List<Shader> Shaders;

    public List<Material> Materials;

    private SharedAsset()
    {
        Fonts = new List<GameObject>();
        Altases = new List<GameObject>();
        Textures = new List<Texture>();
        Shaders = new List<Shader>();
        Materials = new List<Material>();

        FontsShareName = "FontsShare";
        AltasesShareName = "AltasesShare";
        TexturesShareName = "TexturesShare";
        ShadersShareName = "ShadersShare";
    }

  

    void OnDestroy()
    {
        m_instance = null;
    }


}
