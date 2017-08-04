using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 
public class EasyMoveGo : MonoBehaviour {

    // 主角
    public Transform target;

    // 缩放系数
    private float distance = 10.0f;

    // 左右滑动移动速度
    private float xSpeed = 250.0f;
    private float ySpeed = 120.0f;

    // 缩放限制系数
    private float yMinLimit = -20;
    private float yMaxLimit = 80;

    // 摄像头的位置
    private float x = 0.0f;
    private float y = 0.0f;
    float sensitivity = 0.2f; //滚轮缩放速度

	// Use this for initialization
	void Start () {
		
	}

    void Update()
    {

        //鼠标滚轮缩放

        float otrthsize = -Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        if (Mathf.Abs(otrthsize) > 0.01)
        {

        }
       
        if (Input.GetMouseButton(0))
        {
            //根据触摸点计算X与Y位置
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y")  *ySpeed * 0.02f;

            target.localPosition = new Vector3(x, y, 0);
        }

        



    }

}
