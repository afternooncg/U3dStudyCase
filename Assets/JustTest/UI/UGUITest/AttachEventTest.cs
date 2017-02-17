using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AttachEventTest: MonoBehaviour {

	// Use this for initialization
	void Start () {

	//UnityAction<Button> btnActions = new UnityAction<Button>(onMyClick);
	//this.onClick.AddListener(btnActions);

	/*
	EventTrigger trigger = transform.gameObject.GetComponent<EventTrigger>();
	if (trigger == null)
	 trigger = transform.gameObject.AddComponent<EventTrigger>();

	// 实例化delegates
	trigger.triggers = new List<EventTrigger.Entry>();

	// 定义需要绑定的事件类型。并设置回调函数
	EventTrigger.Entry entry = new EventTrigger.Entry();
	// 设置 事件类型
	entry.eventID = EventTriggerType.PointerClick;
	// 设置回调函数
	entry.callback = new EventTrigger.TriggerEvent();
	UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(OnScriptControll);
	entry.callback.AddListener(callback);
	// 添加事件触发记录到GameObject的事件触发组件
	trigger.triggers.Add(entry);
*/

	EventTrigger eventTrigger = this.GetComponent<EventTrigger> ();
	if (eventTrigger == null)
	 eventTrigger = this.gameObject.AddComponent<EventTrigger> ();

	eventTrigger.triggers = new List<EventTrigger.Entry> ();

	EventTrigger.Entry entry = new EventTrigger.Entry ();
	entry.eventID = EventTriggerType.PointerClick;

	UnityAction<BaseEventData> action = new UnityAction<BaseEventData> (OnScriptControll);
	entry.callback.AddListener(action);

	eventTrigger.triggers.Add (entry);

	//this.GetComponent<Camera>().pixelRect

	}
	
	// Update is called once per frame
	void Update () {

        Debug.DrawLine(Vector3.zero, new Vector3(10, 10, 10), Color.red);
        Debug.DrawRay(Vector3.zero, new Vector3(-20, 20, 20), Color.green, 10);
	}


	 public void OnEditorControll()
	 {
	Debug.Log ("Attach Event With OnEditorControll");	
	 }

 private void OnScriptControll(BaseEventData arg0) 
 {
	//arg0.selectedObject
	Debug.Log ("Attach Event With OnScriptControll");


    Debug.Log("EventSystem.current.IsPointerOverGameObject:" + EventSystem.current.IsPointerOverGameObject());
}

  void OnGUI()
 {
      /*
     if (Input.GetMouseButtonDown(0))
     {
         Debug.Log("Clicked");
     }
       */

      
 }

}
