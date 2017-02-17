using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class FindCurrentCLick: MonoBehaviour {

	public Button btn;
	// Use this for initialization
	void Start () {
	
		Image img = this.GetComponent<Image>();
		btn.onClick.AddListener(onMyClick1);

	}
	
	// Update is called once per frame
	void Update () {
	
		
		if (Input.GetMouseButtonDown(0) )
		{
			//Debug.Log("Select:" + EventSystem.current.currentSelectedGameObject);
			/*
			{
				if(EventSystem.current.currentSelectedGameObject is Button)
				{
					Button btn = EventSystem.current.currentSelectedGameObject as Button ;
				if(btn)
				{
					UnityAction<Button> btnActions = new UnityAction<Button>(onMyClick);
					btn.onClick.AddListener(btnActions);
				}



			}
			*/
			if (EventSystem.current.IsPointerOverGameObject())
				Debug.Log("left-click over a GUI element!");			
			else 
				Debug.Log("just a left-click!");
		}

	}
	void  onMyClick1()
	{
		Debug.Log("button===========onMyClick1");
	}


	void  onMyClick(Object obj)
	{
		Debug.Log("button===========");
		Debug.Log("button-----------" + obj.name);
	}
}
