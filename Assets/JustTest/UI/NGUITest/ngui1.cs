using UnityEngine;
using System.Collections;


public class ngui1 : MonoBehaviour {

	// Use this for initialization
    GameObject btn;
    GameObject target;
	void Start () {
	   //UIEventListener
        btn = GameObject.Find("BtnOk");
        target = GameObject.Find("Target");
        if (btn)
        { 
            Debug.Log("Get Btn Ok");
            UIEventListener.Get(btn).onClick += handleBtnOkClick;
            UIEventListener.Get(btn).onHover += handleBtnOkHover;
            UIEventListener.Get(btn).onTooltip += handleBtnOkTooltip;
            UIEventListener.Get(btn).onDragOver += handleBtnOkDragOver;

            UIEventListener.Get(target).onDrop += handleTargetOnDrap;
            
        }
	}


    public void OnInput1Change()
    {
        GameObject.Find("Label1").GetComponent<UILabel>().text = GameObject.Find("Input1").GetComponent<UIInput>().value;
    }


    public void OnCheckBox1Change()
    {
        Debug.Log("CheckBox1 " + GameObject.Find("Checkbox1").GetComponent<UIToggle>().value);

        this.transform.FindChild("Camera").gameObject.GetComponent<UICamera>().enabled = GameObject.Find("Checkbox1").GetComponent<UIToggle>().value;
        
    }


    public void OnScrollBar1Change()
    {
        Debug.Log("OnScrollBar1Change " + GameObject.Find("ScrollBar1").GetComponent<UIScrollBar>().value);

        GameObject.Find("ScrollBar1").GetComponent<UIScrollBar>().barSize = 0.5f;

    }


    private void handleBtnOkDragOver(GameObject go)
    {
        Debug.Log("i am DragOver!");
    }

    private void handleTargetOnDrap(GameObject go, GameObject obj)
    {
        Debug.Log("i am drap!");
    }

    private void handleBtnOkTooltip(GameObject go, bool state)
    {
        Debug.Log("handleBtnOkTooltip " + state);
    }

    private void handleBtnOkHover(GameObject go, bool state)
    {
        Debug.Log("hello " + state);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void handleBtnOkClick(GameObject go)
    {
        Debug.Log("click:" + go.name);
        NGUITools.GetRoot(go).SetActive(false);
    }
}
