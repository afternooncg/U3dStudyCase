using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFrame : MonoBehaviour {

	
	
    public void Close()
    {
        SetLayerRecursively(this.gameObject, LayerMask.NameToLayer("NGUIHIDDEN"));        

        //发现 ngui scrollview和 ngui grid 并不会自动更新layer,所以还是需要调整frame 位置
        //this.transform.localPosition = new Vector3(10000f, 10000f, 0); 
        //递归解决
    }

    public void Open()
    {
        SetLayerRecursively(this.gameObject, PubConfig.NGUILayer);        
        
        this.transform.localPosition = Vector3.zero;
    }


    public static void SetLayerRecursively(GameObject go, int layer)
    {
       
        go.layer = layer;

        foreach (Transform child in go.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
