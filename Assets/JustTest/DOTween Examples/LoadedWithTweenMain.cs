using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class LoadedWithTweenMain : MonoBehaviour {

	// Use this for initialization

    public Transform GoTf;
    public UIGrid GridObj;
	void Start () {

        if (GoTf != null)
            GoTf.DOLocalMoveY(5, 0.5f);

        if (GridObj != null)
        {
            GridObj.onReposition = onReposition;
        }
           
	}

    private void onReposition()
    {
        PlayGridItemEffect(GridObj);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public static void PlayGridItemEffect(UIGrid grid, bool isY = true)
    {
        if (grid == null)
            return;

        grid.onReposition = delegate()
        {
            float speed = 0.3f;
            int movelen = -30;
            float delayTime = 0.2f;
            List<Transform> list = grid.GetChildList();
            for (int i = 0; i < list.Count; i++)
            {
                if (isY)
                {
                    list[i].localPosition = new Vector3(list[i].localPosition.x, list[i].localPosition.y + (i + 1) * movelen, list[i].localPosition.z);
                    list[i].DOLocalMoveY(0, speed).SetEase(Ease.OutCubic).SetDelay(delayTime);
                }
                else
                {
                    list[i].localPosition = new Vector3(list[i].localPosition.x + (i + 1) * movelen, list[i].localPosition.y, list[i].localPosition.z);
                    list[i].DOLocalMoveX(list[i].localPosition.x - (float)((i + 1) * movelen), speed).SetEase(Ease.OutCubic).SetDelay(delayTime + 0.05f * (float)i);
                }

                if (i >= 7)
                    break;
            };
        };
    }   


}
