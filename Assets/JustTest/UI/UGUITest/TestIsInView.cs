using UnityEngine;
using System.Collections;

public class TestIsInView : MonoBehaviour {

    public bool isRendering=false;
    private float lastTime=0;
    private float curtTime=0;
    Vector3 posiLeftBottom;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //方法1 貌似不靠谱 角度技术不准确
        Vector3 directionVector = this.gameObject.transform.position - Camera.main.transform.position;
        float angleValue = Vector3.Angle(Camera.main.transform.forward, directionVector);

        // 检测是否在视野内（摄像机镜头中）
        //Debug.Log("angleValue:" + angleValue);
        if (angleValue < Camera.main.fieldOfView)
        {
           //Debug.Log("yes");
        }
        else
        {
            Debug.Log("out");
        }

        //方法2 6面检查ok
        Debug.Log(IsVisibleFrom(gameObject.GetComponent<Renderer>(), Camera.main));


        //方法3 通过onbecamevisible可行 性能好
        isRendering = curtTime != lastTime ? true : false;
        lastTime = curtTime;

        if (!isRendering)
            Debug.Log("OUT By OnWillRenderObject");

        //方法4 通过矩形嵌套检测 可行。貌似正交摄像模式更适合
        posiLeftBottom = Camera.main.WorldToScreenPoint(gameObject.transform.position) + new Vector3(-50,-50,0) ;
        Rect rect1 = new Rect(posiLeftBottom.x, posiLeftBottom.y, 100, 100);

        

        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        Debug.Log("Rect Check " + rect.Overlaps(rect1));
	}


    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(Camera.main.ScreenToWorldPoint(posiLeftBottom), Vector3.one);
    }

    public bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    
         

 
    void OnWillRenderObject()
    {
         curtTime=Time.time;
    }

    void OnBecameVisible()
    {//系统调用
        Debug.Log("OnBecameVisible");
    }

    void OnBecameInvisible()
    {//系统调用 
        Debug.Log("OnBecameInVisible");
    }
 

}
