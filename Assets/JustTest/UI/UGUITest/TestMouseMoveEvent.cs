using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestMouseMoveEvent : MonoBehaviour, IMoveHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IUpdateSelectedHandler, IPointerExitHandler,IScrollHandler
{

	// Use this for initialization
	void Start () {

        Debug.Log("?:" + TestOk(true));
	
	}
	
	// Update is called once per frame
	void Update () {

        float h = Input.GetAxis("Horizontal");
        if (h > 0)
            this.GetComponent<Image>().rectTransform.position += new Vector3(1f, 0f, 0f);
	}


    public void OnMove(UnityEngine.EventSystems.AxisEventData eventData)
    {
        Debug.Log("moving..");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click..");
        
    }

    void OnMouseUp()
    {
        Debug.Log("Drag ended!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter..");
        this.GetComponent<Image>().rectTransform.position += new Vector3(1f, 0f,0f);
    }

    public void OnUpdateSelected(UnityEngine.EventSystems.BaseEventData eventData)
    {
        Debug.Log("OnUpdateSelected..");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit..");
    }

    public string TestOk(bool flag)
    {
        if (flag)
            return "abc";

        Debug.Log("test");
        return "";
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp..");
    }

    public void OnScroll(PointerEventData eventData)
    {
        Debug.Log("OnScroll..");
    }
}
