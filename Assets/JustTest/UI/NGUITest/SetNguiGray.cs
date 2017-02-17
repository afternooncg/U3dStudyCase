using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SetNguiGray : MonoBehaviour
{
    // UISprite : 更换图集(正常和置灰图集切换)
    // UITexture：更换Shader为正常Shader和置灰使用的Shader
    /*
      1.将同一个图集中的图片单独的变灰操作
      2.将UITexture变灰去色
     */
    // 维护一个图集List
    // 置灰图集
    // 正常图集
    public List<UIAtlas> grayAtlas;
    public List<UIAtlas> normalAtlas;
    public Hashtable shaderTable = new Hashtable();

    public UIAtlas grayUIAtlas;
    public UIAtlas normalUIAtlas;
    public enum AtlasName
    {
        EM_AtlasName_None = -1,
        EM_AtlasName_CommonAtlas,
    }
    private static SetNguiGray instance = null;
    public static SetNguiGray GetInstance()
    {
        return instance;
    }
    void Awake()
    {
        instance = this;
    }
    ///
    /// 将制定GameObject存在的UITexture和UISprite去色，变灰
    ///
    ///  目标物体
    ///  是否变灰
    ///  制定图集
    public void ChangeObjectsToGray(GameObject targetObj, bool gray, AtlasName atlasName = AtlasName.EM_AtlasName_None)
    {
        /*
        UISprite[] sprites = targetObj.GetComponentsInChildren();
        UITexture[] textures = targetObj.GetComponentsInChildren();
        for (int i = 0; i < sprites.Length; i++)
            ChangeUISpriteToGray(sprites, gray, atlasName);
        for (int i = 0; i < textures.Length; i++)
            ChangeUITextureToGary(textures, gray);
         */
    }

    bool b = false;
    public void Test(GameObject go)
    {
        GameObject go1 = go;

        SetUISpriteToGray(go1.GetComponent<UISprite>(), !b);
        b = !b;

    }

    public void SetUISpriteToGray(UISprite targetSprite, bool toGray)
    {

        if (targetSprite != null)
        {
            if (toGray)
            {
                if (targetSprite.atlas.name.LastIndexOf("gray") < 0)
                {
                    UIAtlas tmp = targetSprite.atlas;
                    targetSprite.atlas = null;
                    targetSprite.atlas = grayUIAtlas;
                }
            }
            else
            {
                if (targetSprite.atlas.name.LastIndexOf("gray") >= 0)
                {
                    UIAtlas tmp = targetSprite.atlas;
                    targetSprite.atlas = null;
                    targetSprite.atlas = normalUIAtlas;
                }
            }
            
        }


       

        /*
        bool toGarySucess = false;
        if (targetSprite != null)
        {
            List findAtlas = new List();
            if (toGray)
                findAtlas = grayAtlas;
            else
                findAtlas = normalAtlas;
            UISpriteData findData = null;
            string spriteName = targetSprite.spriteName;
            // 如果指定图集，不用查找直接更换图集即可(高效方式)
            if (atlasName != AtlasName.EM_AtlasName_None)
            {
                findData = findAtlas[(int)atlasName].GetSprite(spriteName);
                if (findData != null)
                {
                    targetSprite.atlas = findAtlas[(int)atlasName];
                    toGarySucess = true;
                }
            }
            else
            {
                // 未指定图集，需要从现有的图集中查找对应的图集
                for (int i = 0; i < findAtlas.Count; i++)
                {
                    findData = findAtlas.GetSprite(spriteName);
                    if (findData != null)
                    {
                        targetSprite.atlas = findAtlas;
                        toGarySucess = true;
                        break;
                    }
                }
            }
        }
        return toGarySucess;
         */
    }
    ///
    /// 将UITexture置灰操作
    ///
    ///
    ///
    /*
    public void ChangeUITextureToGary(UITexture texture, bool toGray)
    {
        Shader newShader = null;
        if (toGray)
        {
            // 保存UITexture原Shader
            if (!shaderTable.ContainsKey(texture))
                shaderTable[texture] = texture.shader;
            newShader = Shader.Find("Unlit/GrayShader");
        }
        else
        {
            // 恢复原Shader
            if (shaderTable.ContainsKey(texture))
                newShader = (Shader)shaderTable[texture];
            else
                newShader = Shader.Find("Unlit/Transparent Colored");
        }
        texture.shader = newShader;
    }
    private bool changeToGray = true;
    public GameObject targets;
    public void TestChangeGray()
    {
        ChangeObjectsToGray(targets, changeToGray, AtlasName.EM_AtlasName_CommonAtlas);
        changeToGray = !changeToGray;
    }*/
}