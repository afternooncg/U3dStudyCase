using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class NotDestory : UntiySingleton<NotDestory>, IPointerClickHandler
{

    public int a = 0;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    static public MonoBehaviour PubAdd;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("hello");

        if (PubAdd != null)
            (PubAdd as S3).rootgo.transform.parent = this.transform;
    }
}
