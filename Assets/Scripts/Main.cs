using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    public GameObject BtnPrefab;
    public GameObject BtnPrefabSub;



    MenuConfig m_menuConfig;
    UIGrid m_left;
    UIGrid m_right;

    Material  m_bgmat;
    float m_bgMovSpeed = 1.2f;
	// Use this for initialization

    List<GameObject> m_subBtns;
    int m_currentMenuId = 0;

	void Start () {

        GameObject mainframe = GameObject.Instantiate(Resources.Load<GameObject>("MainFrame"));
        mainframe.transform.parent = GameObject.Find("UIRoot").transform;
        resetGameObjectTrans(mainframe);


        m_bgmat = GameObject.Find("bg").GetComponent<Renderer>().material;

        m_left = GameObject.Find("BtnGrid").GetComponent<UIGrid>();
        m_right = GameObject.Find("SubBtnGrid").GetComponent<UIGrid>();

        NGUITools.DestroyChildren(m_left.transform);
        NGUITools.DestroyChildren(m_right.transform);

        m_subBtns = new List<GameObject>();
        
        //m_right.gameObject.AddChild(BtnPrefabSub.transform);

        m_menuConfig = Resources.Load<MenuConfig>("MenuConfig");
        for (int i = 0; i < m_menuConfig.Menus.Length; i++)
        {
            MenuConfig.MenuObject menu = m_menuConfig.Menus[i];
            
            GameObject btn =  NGUITools.AddChild(m_left.gameObject, BtnPrefab);
            btn.transform.FindChild("Label").GetComponent<UILabel>().text = menu.TypeName;


            UIEventListener.Get(btn).onClick = onMenuClick;

            if (i != 0)
                continue;

            for (int j = 0; j < menu.SubMenuItem.Length; j++)
            {
                GameObject btn1 =  NGUITools.AddChild(m_right.gameObject, BtnPrefabSub);
                UIEventListener.Get(btn1).onClick = onSubMenuClick;
                btn1.transform.FindChild("Label").GetComponent<UILabel>().text = menu.SubMenuItem[j].ButtnText;
            }
        }

        m_left.Reposition();
        m_right.Reposition();
	}

    private void onSubMenuClick(GameObject go)
    {
        int index = m_right.GetIndex(go.transform);

        MenuConfig.SubMenu[] submenus = m_menuConfig.Menus[m_currentMenuId].SubMenuItem;

        SceneManager.LoadScene(submenus[index].SceneName);

    }

    private void onMenuClick(GameObject go)
    {

        swapMenuById(m_left.GetIndex(go.transform));       

    }

    private void swapMenuById(int index)
    {
        if (index == m_currentMenuId)
            return;

        MenuConfig.SubMenu[] submenus = m_menuConfig.Menus[index].SubMenuItem;



        List<Transform> list = m_right.GetChildList();
        for (int i = 0; i < list.Count; i++)
        {
            pushFreeBtn(list[i].gameObject);
            m_right.RemoveChild(list[i]);
        }


        for (int i = 0; i < submenus.Length; i++)
        {
            GameObject btn = popFreeBtn();
            btn.transform.parent = m_right.transform;
            resetGameObjectTrans(btn);
            
            btn.transform.FindChild("Label").GetComponent<UILabel>().text = submenus[i].ButtnText;
            UIEventListener.Get(btn).onClick = onSubMenuClick;
        }
                
        m_right.Reposition();

        m_right.transform.parent.GetComponent<UIScrollView>().ResetPosition();

        m_currentMenuId = index;
    }


    GameObject popFreeBtn()
    {
        GameObject go = null;
        if (m_subBtns.Count > 0)
        {
            go = m_subBtns[m_subBtns.Count - 1];
            m_subBtns.RemoveAt(m_subBtns.Count - 1);
            go.SetActive(true);   
        }
        else
        {
            go = GameObject.Instantiate<GameObject>(BtnPrefabSub);

        }

        resetGameObjectTrans(go);
        
        
        return go;
    }

    void pushFreeBtn(GameObject btn)
    {
        btn.transform.parent = GameObject.Find("UIRoot").transform;
        btn.SetActive(false);
        m_subBtns.Add(btn);
    }
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.Escape))  
            Application.Quit();
        
        float v = Time.deltaTime * m_bgMovSpeed * 0.02f;
        m_bgmat.mainTextureOffset += new Vector2(v, v);
	}


    void resetGameObjectTrans(GameObject go)
    {
        if (go != null)
        {
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
        }
    }

}
