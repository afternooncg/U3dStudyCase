using UnityEngine;
using System.Collections;

public class WrapContent : MonoBehaviour
{
    public GameObject _wrap;
    public UIScrollView scrollViewUI;
    UIWrapContent _wrapScript;
    public GameObject _box;

    // Use this for initialization
    void Awake()
    {
        scrollViewUI = GameObject.Find("Scroll View").GetComponent<UIScrollView>();
        //获取脚本
        _wrapScript = _wrap.GetComponent<UIWrapContent>();

        //绑定方法
        _wrapScript.onInitializeItem = OnUpdateItem;

        scrollViewUI.onStoppedMoving = onStoppedMoving;



        //scrollview 滚动原理
        //transform.localPosition = new Vector3(110f, 220f, 0f);
        //mPanel.clipOffset = new Vector3(-110f, -220f, 0f);

    }

    private void onStoppedMoving()
    {

        if (Mathf.Abs(scrollViewUI.gameObject.transform.localPosition.x) < 2f || Mathf.Abs(Mathf.Abs(scrollViewUI.gameObject.transform.localPosition.x) + scrollViewUI.panel.width - 700f) < 2F)
            Debug.Log("边缘");
        else
        {

            float posi = Mathf.Abs(scrollViewUI.gameObject.transform.localPosition.x % 100f);
            Debug.Log(posi);
            
            if(posi<=50f)
            {//右移
                SpringPanel.Begin(scrollViewUI.gameObject, new Vector3(scrollViewUI.gameObject.transform.localPosition.x + posi, 0, 0), 3f);
            }
            else
            {//左移
                SpringPanel.Begin(scrollViewUI.gameObject, new Vector3(scrollViewUI.gameObject.transform.localPosition.x - (100-posi), 0, 0),3f);
            }
            

            Debug.Log("onStoppedMoving");

        }



       // SpringPanel.Begin(scrollViewUI.gameObject, new Vector3(30,0,0), 1f);
    }


    // Update is called once per frame
    void Update()
    {
        //Test
        if (Input.GetKeyDown(KeyCode.A))
        {
            _wrapScript.minIndex = -10;
            _wrapScript.maxIndex = 10;

            //启用脚本
            _wrapScript.enabled = true;


        }
    }

    void OnUpdateItem(GameObject go, int index, int realIndex)
    {
        //Debug.Log("index = " + index);
        Debug.Log("realIndex = " + realIndex);
        ItemNum tb = go.GetComponent<ItemNum>();
        tb.SetNumber(realIndex.ToString());


        //NGUIMath.CalculateAbsoluteWidgetBounds
        //NGUIMath.CalculateAbsoluteWidgetBounds
        //NGUIMath.
    }


    /*
    public bool RestrictWithinView(Transform child, bool instant, bool horizontal, bool vertical)
    {

        //      Debug.Log (child);

        Bounds childBounds = NGUIMath.CalculateRelativeWidgetBounds(mPanel.transform, child, false);
        Vector3 min = childBounds.min;
        Vector3 max = childBounds.max;
        Vector4 cr = mPanel.finalClipRegion;

        float offsetX = cr.z * 0.5f;
        float offsetY = cr.w * 0.5f;

        Vector2 minRect = new Vector2(min.x, min.y);
        Vector2 maxRect = new Vector2(max.x, max.y);
        Vector2 minArea = new Vector2(cr.x - offsetX, cr.y - offsetY);
        Vector2 maxArea = new Vector2(cr.x + offsetX, cr.y + offsetY);

        if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
        {
            minArea.x += mPanel.clipSoftness.x;
            minArea.y += mPanel.clipSoftness.y;
            maxArea.x -= mPanel.clipSoftness.x;
            maxArea.y -= mPanel.clipSoftness.y;
        }
        Vector3 constraint = NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
        //      Debug.Log (constraint);
        if (constraint.magnitude > 1f)
        {
            if (!instant && dragEffect == DragEffect.MomentumAndSpring)
            {
                // Spring back into place
                Vector3 pos = mTrans.localPosition + constraint;
                pos.x = Mathf.Round(pos.x);
                pos.y = Mathf.Round(pos.y);
                SpringPanel.Begin(mPanel.gameObject, pos, 13f);
            }
            else
            {
                // Jump back into place
                MoveRelative(constraint);
                mMomentum = Vector3.zero;
                mScroll = 0f;
            }
            return true;
        }
        return false;

    }
     */
}


