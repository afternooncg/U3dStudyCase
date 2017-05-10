using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 这个脚本实现的点击单个点的效果
/// </summary>
public class WuziqiShader : MonoBehaviour
{
    public Material mat;
    //设置点中的最小误差
    public float clickMinError;

    //网格点的坐标集
    private List<Vector2> list_gridIntersectionPos = new List<Vector2>();

    //网格点的数量
    private int gridIntersectionNums;
    private float gridSpace;
    private Vector2 vec_mouseBtnPos;
    // Use this for initialization
    void Start()
    {

        gridSpace = mat.GetFloat("_tickWidth");

        //单个坐标轴上网格点的数量等于横轴坐标间距除以网格间距
        gridIntersectionNums = (int)Mathf.Floor(1.0f / gridSpace); //这里不能只用强制类型转换，如果使用强制类型转换会丢失数据，比如1.0/0.1最后的结果是9

        for (int i = -gridIntersectionNums; i <= gridIntersectionNums; i++)
        {
            float x = gridSpace * i;


            for (int j = -gridIntersectionNums; j <= gridIntersectionNums; j++)
            {
                float y = gridSpace * j;
                list_gridIntersectionPos.Add(new Vector2(x, y));

            }

        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            vec_mouseBtnPos = Input.mousePosition;

            //将鼠标的位置除以屏幕参数得到范围为0~1的坐标范围
            vec_mouseBtnPos = new Vector2(vec_mouseBtnPos.x / Screen.width, vec_mouseBtnPos.y / Screen.height);
            //设定坐标原点为中点
            vec_mouseBtnPos -= new Vector2(0.5f, 0.5f);
            vec_mouseBtnPos *= 2;
            vec_mouseBtnPos.y = -vec_mouseBtnPos.y;
            
            /*
              mat.SetFloat("_MouseBtnPosX", vec_mouseBtnPos.x);
              mat.SetFloat("_MouseBtnPosY", vec_mouseBtnPos.y);
              Debug.Log("x:" + vec_mouseBtnPos.x + "y:" + vec_mouseBtnPos.y);
    return;*/

            //如果点中了网格的交叉点出就显示圆点
            int index = CheckClikedIntersection(vec_mouseBtnPos);
            if (index != -1)
            {
                //将准确的网格点的位置赋值给vec_mouseBtnPos
                vec_mouseBtnPos = list_gridIntersectionPos[index];
                mat.SetFloat("_MouseBtnPosX", vec_mouseBtnPos.x);
                mat.SetFloat("_MouseBtnPosY", vec_mouseBtnPos.y);
            }
            Debug.Log("x:" + vec_mouseBtnPos.x + "y:" + vec_mouseBtnPos.y);


        }

    }
    /// <summary>
    /// 判断鼠标点中的地方是否在网格的交叉点处
    /// </summary>
    /// <param name="vec2"></param>
    /// <returns></returns>
    private int CheckClikedIntersection(Vector2 vec2)
    {
        int clickIndex = -1;
        for (int i = 0; i < list_gridIntersectionPos.Count; i++)
        {
            float errorx = Mathf.Abs(vec2.x - list_gridIntersectionPos[i].x);
            float errory = Mathf.Abs(vec2.y - list_gridIntersectionPos[i].y);
            //如果误差的值小于预设的值则判定点中了
            float error = Mathf.Sqrt(errorx * errorx + errory * errory);
            if (error < clickMinError)
            {
                clickIndex = i;
                break;
            }
        }
        return clickIndex;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (mat != null)
            Graphics.Blit(src, dest, mat);
    }
}