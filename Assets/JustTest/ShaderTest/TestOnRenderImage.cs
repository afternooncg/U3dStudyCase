using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOnRenderImage : MonoBehaviour {

	// Use this for initialization
    public Material mat;

    float mod(float a, float b)
    {
        //floor(x)方法是Cg语言内置的方法，返回小于x的最大的整数
        return a - b * Mathf.Floor(a / b);
    }


    void Start()
    {
       
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if(mat!=null)
            Graphics.Blit(src, dest, mat);
    }
}
