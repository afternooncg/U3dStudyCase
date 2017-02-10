using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainFrame : BaseFrame
{

    public GameObject BtnPrefab;                                //左侧按钮prefab
    public GameObject BtnPrefabSub;                             //右侧按钮prefab


    MenuConfig m_menuConfig;                                    //配置
    UIGrid m_left;
    UIGrid m_right;


    // Use this for initialization

    List<GameObject> m_subBtns;
    int m_currentMenuId = 0;                                    //当前选中大类


    GameObject m_subcloseframe;                                 //共用回退UI
    PubCloseFrame m_closeframe;                                 //关闭UI
    LoadingFrame m_loadingFrame;                                //过场

    bool m_isShowMainState = true;                              //是否显示在主 UI界面

    IEnumerator Start()
    {


        SetMainCamaeraCullMask();

        m_subcloseframe = GameObject.Instantiate(Resources.Load<GameObject>("PubCloseFrame"));
        m_subcloseframe.transform.parent = GameObject.Find("UIRoot").transform;
        ResetGameObjectTrans(m_subcloseframe);


        UIEventListener.Get(GameObject.Find("btnClose").gameObject).onClick = delegate(GameObject go)
        {
            if (m_isShowMainState)
            {
                if (m_closeframe == null)
                {
                    GameObject goclose = GameObject.Instantiate(Resources.Load<GameObject>("CloseFrame"));
                    m_closeframe = goclose.GetComponent<PubCloseFrame>();
                    goclose.transform.parent = GameObject.Find("UIRoot").transform;
                    ResetGameObjectTrans(goclose);
                }

                m_closeframe.Open();

            }
            else
            {
                m_isShowMainState = true;
                this.Open();

                StartCoroutine("LoadSubScene", "Main");


            }
        };


        yield return null;


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

            GameObject btn = NGUITools.AddChild(m_left.gameObject, BtnPrefab);
            btn.transform.FindChild("Label").GetComponent<UILabel>().text = menu.TypeName;


            UIEventListener.Get(btn).onClick = onMenuClick;

            if (i != 0)
                continue;

            for (int j = 0; j < menu.SubMenuItem.Length; j++)
            {
                GameObject btn1 = NGUITools.AddChild(m_right.gameObject, BtnPrefabSub);
                UIEventListener.Get(btn1).onClick = onSubMenuClick;
                btn1.transform.FindChild("Label").GetComponent<UILabel>().text = menu.SubMenuItem[j].ButtnText;
            }
        }

        m_left.Reposition();
        m_right.Reposition();
        m_right.transform.parent.GetComponent<UIScrollView>().ResetPosition();
    }


    #region 加载场景
    IEnumerator LoadSubScene(string name)
    {

        if (m_loadingFrame == null)
        {
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("LoadingFrame"));
            m_loadingFrame = go.GetComponent<LoadingFrame>();
            go.transform.parent = GameObject.Find("UIRoot").transform;
            ResetGameObjectTrans(go);
        }
        else
            m_loadingFrame.Open();

        AsyncOperation op = SceneManager.LoadSceneAsync(name);

        while (!op.isDone)
        {
            m_loadingFrame.UpdateProgress(op.progress);
            yield return null;
        }

        SetMainCamaeraCullMask();

        m_loadingFrame.UpdateProgress(1f);
        yield return null;
        m_loadingFrame.Close();

    }
    #endregion


    #region 子菜单点击
    private void onSubMenuClick(GameObject go)
    {
        int index = m_right.GetIndex(go.transform);

        MenuConfig.SubMenu[] submenus = m_menuConfig.Menus[m_currentMenuId].SubMenuItem;
        this.Close();
        m_isShowMainState = false;

        StartCoroutine("LoadSubScene", submenus[index].SceneName);


    }
    #endregion

    #region 主菜单点击
    private void onMenuClick(GameObject go)
    {

        swapMenuById(m_left.GetIndex(go.transform));

    }
    #endregion

    #region 切换一级类别
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
            ResetGameObjectTrans(btn);
            btn.layer = m_right.gameObject.layer;
            btn.SetActive(true);   //ugui bug? 需要重新显示下
            btn.transform.FindChild("Label").GetComponent<UILabel>().text = submenus[i].ButtnText;
            UIEventListener.Get(btn).onClick = onSubMenuClick;
        }

        m_right.Reposition();

        m_right.transform.parent.GetComponent<UIScrollView>().ResetPosition();

        m_currentMenuId = index;
    }
    #endregion

    #region 从缓冲池取按钮
    GameObject popFreeBtn()
    {
        GameObject go = null;
        if (m_subBtns.Count > 0)
        {
            go = m_subBtns[m_subBtns.Count - 1];
            m_subBtns.RemoveAt(m_subBtns.Count - 1);
            // go.SetActive(true);   
        }
        else
        {
            go = GameObject.Instantiate<GameObject>(BtnPrefabSub);

        }

        ResetGameObjectTrans(go);


        return go;
    }
    #endregion

    #region 暂无用按钮放入缓冲池
    void pushFreeBtn(GameObject btn)
    {
        btn.transform.parent = GameObject.Find("UIRoot").transform;
        btn.SetActive(false);
        m_subBtns.Add(btn);
    }
    #endregion



    #region 重置Transform数据

    public static void ResetGameObjectTrans(GameObject go)
    {
        if (go != null)
        {
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
        }
    }
    #endregion


    #region 主摄像机剔除NGUI层
public static void SetMainCamaeraCullMask()
    {
        Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << PubConfig.NGUILayer);
    }
#endregion
}
