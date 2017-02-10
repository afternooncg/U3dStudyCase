using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMainMono : MonoBehaviour {

	// Use this for initialization

    void Awake()
    { 
       Camera.main.cullingMask = Camera.main.cullingMask  &   ~(1 << PubConfig.NGUILayer);     
    }

}
