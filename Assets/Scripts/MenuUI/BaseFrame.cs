using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFrame : MonoBehaviour {

	
	
    public void Close()
    {
        this.gameObject.layer = 30;

        //发现 ngui scrollview和 ngui grid 并不会自动更新layer,所以还是需要调整frame 位置
        this.transform.localPosition = new Vector3(10000f, 10000f, 0);
    }

    public void Open()
    {
        this.gameObject.layer = PubConfig.NGUILayer;
        this.transform.localPosition = Vector3.zero;
    }
}
