using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuConfig : ScriptableObject {
    [System.Serializable]
    public class SubMenu
    {
        public string ButtnText;
        public string SceneName;
    }

    [System.Serializable]
    public class MenuObject
    {
        public string TypeName;


        public SubMenu[] SubMenuItem;
    }

	// Use this for initialization
    public MenuObject[] Menus;
}
