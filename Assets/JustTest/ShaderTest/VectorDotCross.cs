using UnityEngine;
using System.Collections;

public class VectorDotCross : MonoBehaviour {

	// Use this for initialization

    public Vector3 pv1;
    public Vector3 pv2;

    public Vector3 lerpData;

	void Start () {

        Dot1();

        Cross1();


        TestMartrixTransform();
	}

    private void TestMartrixTransform()
    {
        Matrix4x4 m = Camera.main.cameraToWorldMatrix;
        Vector3 p = m.MultiplyPoint(new Vector3(0, 0, 10));
        Debug.Log(p);
    }
	
	// Update is called once per fr
	void Update () 
    {

        return;

        //lerp在min-max中线性插  z 取0-1
        //smoothlerp在两端非线性插值。有过渡效果 z 0-1

        Debug.Log(string.Format("Lerp({0},{1},{2}) = {3}" , lerpData.x, lerpData.y, lerpData.z , Mathf.Lerp(lerpData.x, lerpData.y, lerpData.z)));
        Debug.Log(string.Format("Mathf.SmoothStep({0},{1},{2}) = {3}", lerpData.x, lerpData.y, lerpData.z, Mathf.SmoothStep(lerpData.x, lerpData.y, lerpData.z)));


        Debug.Log(Vector3.Distance(pv1,pv2));
        

        Debug.Log(Vector3.Angle(pv1, pv2) + "  " + Vector3.Angle(pv2, pv1) + " " + Vector3.Cross(pv1,pv2));
        GetAngle(pv1, pv2);

        Debug.Log(pv1.magnitude + "   " + pv1.normalized +  "   " + Vector3.Distance(Vector3.zero, pv1));


       

	}


    void Dot1()
    {        

        Vector3 v1 = new Vector3(1, 1, 1);
        Vector3 v2 = new Vector3(1, 5, 1);

        float dotvar = Vector3.Dot(v1, v2);        

        float dotvar1 = Vector3.Dot(v1.normalized, v2.normalized);

        Debug.Log("Dot:" +  dotvar + "___"  + dotvar1);

        // 通过向量直接获取两个向量的夹角（默认为 角度）， 此方法范围 [0 - 180]
        float angle = Vector3.Angle(v1, v2);
        float angle1 = Vector3.Angle(v1.normalized, v2.normalized);
        Debug.Log("Angle:" + angle + "___" + angle1);


        float angle2  = Mathf.Acos(dotvar1) * Mathf.Rad2Deg;
        Debug.Log("angle2: " + angle2);

    }


    void Cross1()
    {
        Vector3 v1 = new Vector3(1, 1, 1);
        Vector3 v2 = new Vector3(1, 5, 1);

        Vector3 v3 = Vector3.Cross(v1, v2);
        Vector3 v4 = Vector3.Cross(v1.normalized, v2.normalized);
        Debug.Log("v3:" + v3 + " v4:" + v4);

        // 下面获取夹角的方法，只是展示用法，太麻烦不必使用
        // 通过反正弦函数获取向量 a、b 夹角（默认为弧度）
        float radians = Mathf.Asin(Vector3.Distance(Vector3.zero, Vector3.Cross(v1.normalized, v2.normalized)));
        float angle = radians * Mathf.Rad2Deg;

        Debug.Log("a1:" + angle);

        // 通过反正弦函数获取向量 a、b 夹角（默认为弧度） 和上行相同结果
        radians = Mathf.Asin(Vector3.Cross(v1.normalized, v2.normalized).magnitude);
        angle = radians * Mathf.Rad2Deg;
        Debug.Log("a2:" + angle);

    }

    // 关于点积
    private void Dot()
    {
        /*
        点积 
        点积的计算方式为:  a·b=|a|·|b|cos<a,b>  其中|a|和|b|表示向量的模，
        <a,b>表示两个向量的夹角。 通过点积判断当两个向量的方向向是否相同
        （大致相同即两个向量的夹角在 90 度范围内）
        两个向量的 点积 大于 0 则两个向量夹角小于 90 度， 否则 两个向量的
        夹角大于 90 度，
        */
        // 定义两个向量 a、b
        Vector3 a = new Vector3(1, 1, 1);
        Vector3 b = new Vector3(1, 5, 1);

        // 计算 a、b 点积结果
        float result = Vector3.Dot(a, b);

        // 通过向量直接获取两个向量的夹角（默认为 角度）， 此方法范围 [0 - 180]
        float angle = Vector3.Angle(a, b);

        // 下面获取夹角的方法，只是展示用法，太麻烦不必使用
        // 通过向量点积获取向量夹角，需要注意，必须将向量转换为单位向量才行
        // 计算 a、b 单位向量的点积
        result = Vector3.Dot(a.normalized, b.normalized);
        // 通过反余弦函数获取 向量 a、b 夹角（默认为 弧度）
        float radians = Mathf.Acos(result);
        // 将弧度转换为 角度
        angle = radians * Mathf.Rad2Deg;
    }


    // 获取两个向量的夹角  Vector3.Angle 只能返回 [0, 180] 的值
    // 如真实情况下向量 a 到 b 的夹角（80 度）则 b 到 a 的夹角是（-80）
    // 通过 Dot、Cross 结合获取到 a 到 b， b 到 a 的不同夹角
    private void GetAngle(Vector3 a, Vector3 b)
    {
        Vector3 c = Vector3.Cross(a, b);
        float angle = Vector3.Angle(a, b);

        // b 到 a 的夹角

        //Mathf.Sign 求正负
        float sign = Mathf.Sign(Vector3.Dot(c.normalized, Vector3.Cross(a.normalized, b.normalized)));
        float signed_angle = angle * sign;

        Debug.Log("b -> a :" + signed_angle);

        // a 到 b 的夹角
        sign = Mathf.Sign(Vector3.Dot(c.normalized, Vector3.Cross(b.normalized, a.normalized)));
        signed_angle = angle * sign;

        Debug.Log("a -> b :" + signed_angle);
    }


}
