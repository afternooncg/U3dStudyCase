﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class JustTestEditor   {

	// Use this for initialization
    [MenuItem("JustTest/CreateMenu")]
    static void JustTestCreateMenu()
    {
        Object obj = AssetDatabase.LoadAssetAtPath<MenuConfig>("Assets/Resources/MenuConfig.asset");
        if (obj == null)
            ScriptableObjectUtility.CreateAsset<MenuConfig>("Assets/Resources/MenuConfig.asset");

        
	}
	
	
}
