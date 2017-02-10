using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PubCloseFrame : BaseFrame
{

    public GameObject BtnOk;
    public GameObject BtnCancel;
    public UITweener[] tweenAni;
	// Use this for initialization
	void Start () {
        for (var i = 0; i < tweenAni.Length; ++i)
        {
            if (!tweenAni[i].enabled)
                tweenAni[i].enabled = true;
            //tweenAni[i].PlayForward();
        }

        UIEventListener.Get(BtnOk).onClick = onBtnClick;
        UIEventListener.Get(BtnCancel).onClick = onBtnClick;
	}
	
	// Update is called once per frame
    void onBtnClick(GameObject go)
    {
        if (BtnOk == go)
            Application.Quit();
        else
            this.Close();

	}

   
}
