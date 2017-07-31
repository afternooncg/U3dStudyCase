using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUguiPosi : MonoBehaviour {

    Canvas canvas;
    RectTransform rectTransform;
    // Use this for initialization
    void Start()
    {
        rectTransform = transform as RectTransform;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos))
        {
            rectTransform.anchoredPosition = pos;
        }
    }
}
