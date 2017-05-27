using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TextureImportTest
{

    [MenuItem("QuickTest/Texture/CheckFormat")]
    public static void CheckFormat()
    {
        string[] ids = AssetDatabase.FindAssets("t:texture2D", new string[] { "Assets/_Images/icons/bag" });

        for (int i = 0; i < ids.Length; i++)
        {
            string txpath = AssetDatabase.GUIDToAssetPath(ids[i]);
            TextureImporter ti = AssetImporter.GetAtPath(txpath) as TextureImporter;

            Texture2D sourcetex = AssetDatabase.LoadAssetAtPath<Texture2D>(txpath);

           // Debug.Log(ti.assetPath + " " + ti.compressionQuality + "  " + sourcetex.format);
        }


        Debug.Log(GetSelectedTextures().Length);
    }


    static Object[] GetSelectedTextures()
    {

        return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);

    }

    [MenuItem("QuickTest/Texture/Remove Texture Alpha Chanel")]
    static void ModifyTextures()
    {
        Debug.Log("Start Removing Alpha Chanel.");
        string[] ids = AssetDatabase.FindAssets("t:Texture2D", new string[] { "Assets/_Images/icons/formattest" });
       for (int i = 0; i < ids.Length; i++)
       {
           RemoveTextureAlphaChanel(AssetDatabase.GUIDToAssetPath(ids[i]));
       }      
       AssetDatabase.Refresh();    //Refresh to ensure new generated RBA and Alpha textures shown in Unity as well as the meta file
       Debug.Log("Finish Removing Alpha Chanel.");
    }

    [MenuItem("QuickTest/Texture/LimitTextureSizeTo128")]
    static void LimitTextures()
    {
        Debug.Log("Start Limit Textures.");

        
        string[] paths = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);
        foreach (string path in paths)
        {
            if (!string.IsNullOrEmpty(path) && IsTextureFile(path))   //full name  
            {
                try
                {
                    string assetRelativePath = GetRelativeAssetPath(path);
                    ReImportAsset(assetRelativePath);
                    Debug.Log("Limit Texture: " + assetRelativePath);
                }
                catch
                {
                    Debug.LogError("ReImport Texture failed: " + GetRelativeAssetPath(path));
                }
            }
        }
        AssetDatabase.Refresh();    //Refresh to ensure new generated RBA and Alpha textures shown in Unity as well as the meta file
        Debug.Log("Finish Limit Textures.");
    }

    #region process texture

    static void RemoveTextureAlphaChanel(string _texPath)
    {
        string assetRelativePath = _texPath;
        SetTextureReadableEx(assetRelativePath);    //set readable flag and set textureFormat TrueColor


        Texture2D sourcetex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetRelativePath);  //not just the textures under Resources file  
        if (!sourcetex)
        {
            Debug.LogError("Load Texture Failed : " + assetRelativePath);
            return;
        }
        
        if (IsNoAlphaTexture(sourcetex))
        {
            Debug.Log("pass. no Alpha texture: " + assetRelativePath);
            return;
        }
        /*
        #region Get origion Mipmap Setting

        TextureImporter ti = null;
        try
        {
            ti = (TextureImporter)TextureImporter.GetAtPath(assetRelativePath);
        }
        catch
        {
            Debug.LogError("Load Texture failed: " + assetRelativePath);
            return;
        }
        if (ti == null)
        {
            return;
        }
        bool bGenerateMipMap = ti.mipmapEnabled;    //same with the texture import setting      

        #endregion

        Texture2D rgbTex = new Texture2D(sourcetex.width, sourcetex.height, TextureFormat.RGB24, bGenerateMipMap);
        rgbTex.SetPixels(sourcetex.GetPixels());
        rgbTex.Apply();

        byte[] bytes = rgbTex.EncodeToPNG();
        File.WriteAllBytes(assetRelativePath, bytes);
        ReImportAsset(assetRelativePath, sourcetex.width, sourcetex.height);

        Debug.Log("Succeed Removing Alpha : " + assetRelativePath);
         */
    }

    static bool IsNoAlphaTexture(Texture2D texture)
    {
        //只能判断特定
        return texture.format == TextureFormat.RGB24;
    }

    static void SetTextureReadableEx(string _relativeAssetPath)    //set readable flag and set textureFormat TrueColor
    {
        TextureImporter ti = null;
        try
        {
            ti = (TextureImporter)TextureImporter.GetAtPath(_relativeAssetPath);
        }
        catch
        {
            Debug.LogError("Load Texture failed: " + _relativeAssetPath);
            return;
        }
        if (ti == null)
        {
            return;
        }
        ti.isReadable = true;
       // ti.textureFormat = TextureImporterFormat.AutomaticTruecolor;      //this is essential for departing Textures for ETC1. No compression format for following operation.

        Debug.Log(_relativeAssetPath + " _  ");
        AssetDatabase.ImportAsset(_relativeAssetPath);
    }

    static void ReImportAsset(string path)
    {
        TextureImporter importer = null;
        try
        {
            importer = (TextureImporter)TextureImporter.GetAtPath(path);
        }
        catch
        {
            Debug.LogError("Load Texture failed: " + path);
            return;
        }
        if (importer == null)
        {
            return;
        }
        importer.maxTextureSize = 128;
        importer.anisoLevel = 0;
        importer.isReadable = false;  //increase memory cost if readable is true
        importer.textureFormat = TextureImporterFormat.AutomaticCompressed;
        AssetDatabase.ImportAsset(path);
    }

    static void ReImportAsset(string path, int width, int height)
    {
        try
        {
            AssetDatabase.ImportAsset(path);
        }
        catch
        {
            Debug.LogError("Import Texture failed: " + path);
            return;
        }
        TextureImporter importer = null;
        try
        {
            importer = (TextureImporter)TextureImporter.GetAtPath(path);
        }
        catch
        {
            Debug.LogError("Load Texture failed: " + path);
            return;
        }
        if (importer == null)
        {
            return;
        }
        importer.maxTextureSize = Mathf.Max(width, height);
        importer.anisoLevel = 0;
        importer.isReadable = false;  //increase memory cost if readable is true
        importer.textureFormat = TextureImporterFormat.AutomaticCompressed;
        importer.textureType = TextureImporterType.Default;
        if (path.Contains("/UI/"))
        {
            importer.textureType = TextureImporterType.GUI;
        }
        AssetDatabase.ImportAsset(path);
    }

    #endregion

    #region string or path helper

    static bool IsTextureFile(string _path)
    {
        string path = _path.ToLower();
        return path.EndsWith(".psd") || path.EndsWith(".tga") || path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".bmp") || path.EndsWith(".tif") || path.EndsWith(".gif");
    }

    static string GetRelativeAssetPath(string _fullPath)
    {
      
        return _fullPath;
    }



    #endregion     
	
}
