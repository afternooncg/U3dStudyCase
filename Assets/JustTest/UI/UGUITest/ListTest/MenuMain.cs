using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MenuMain : MonoBehaviour
{

	public struct MenuItem
	{
		public string name ;
		public int index;
		public GameObject go;

		public MenuItem(string name, int index, GameObject go)
		{
			this.name = name;
			this.index = index;
			this.go = go;
		}
	}


	static private  List<MenuItem> _list; 
	public GameObject panel; 
	public UnityEngine.Object Pre;
	// Use this for initialization
	void Start () {
		_inits();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void _inits ()
	{
		_list = new List<MenuItem>();
		panel.transform.DetachChildren();

		_createItem("clock",0,"clock");
		for(int i=1;i<100;i++)
		{
			_createItem("MuliTouch",i,"MuliTouch");
		}
	}

	void _createItem(string name, int index, string levelName)
	{

		//Button btn;
		//btn.GetComponent
		//panel.GetComponent<RectTransform>().
		//VerticalLayoutGroup group = panel.GetComponent<VerticalLayoutGroup>();
		//UnityEngine.Object.Destroy(panel.Get<>());
		//Instantiate(Pre);
		GameObject obj = Instantiate(Pre) as GameObject;
		obj.transform.SetParent(panel.transform,false);

		RectTransform tranBtn = obj.transform.Find("Button") as RectTransform;

		EventTrigger trigger = tranBtn.gameObject.AddComponent<EventTrigger>();

    /*
        EventTrigger eventTrigger = this.GetComponent<EventTrigger>();
        if (eventTrigger == null)
            eventTrigger = this.gameObject.AddComponent<EventTrigger>();

        eventTrigger.triggers = new List<EventTrigger.Entry>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;

        UnityAction<BaseEventData> action = new UnityAction<BaseEventData>(OnScriptControll);
        entry.callback.AddListener(action);

        eventTrigger.triggers.Add(entry);
     */


		// 实例化delegates
        trigger.triggers = new List<EventTrigger.Entry>();
		// 定义需要绑定的事件类型。并设置回调函数
		EventTrigger.Entry entry = new EventTrigger.Entry();
		// 设置 事件类型
		entry.eventID = EventTriggerType.PointerClick;
		// 设置回调函数
		entry.callback = new EventTrigger.TriggerEvent();
		UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(_handleBtnClick);
		entry.callback.AddListener(callback);
		// 添加事件触发记录到GameObject的事件触发组件
        trigger.triggers.Add(entry);



		RectTransform tranTxt = (obj.transform.Find("Button/Text")) as RectTransform;
		Text txt = tranTxt.GetComponent<Text>();
		//Text txt = btn1.transform.Find("Text") as Text;
		//Debug.Log("Txt" + txt.text + "," + _list[0].name);
		txt.text = name;

		_list.Add(new MenuItem(name, index, tranBtn.gameObject));
	}

	void _handleBtnClick (BaseEventData data)
	{
	
		foreach(MenuItem item in _list)
		{
			if(item.go == data.selectedObject)
			{
				Debug.Log(item.go.name);
				Application.LoadLevel(item.name);
				return;
			}
		}

			//data.selectedObject;
		//throw new System.NotImplementedException ();
	}

	public void HandleBtnClose()
	{
		Application.Quit();
	}
}
