using UnityEngine;
using System.Collections;

public class DirectionCount : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Vector3 V1;

    public Vector3 V2;

    void Count()
    {
        // 为了方便理解便于计算，将向量在 Y 轴上的偏移量设置为 0
        V1 = new Vector3( 3, 0, 4);
        V2 = new Vector3( -4, 0, 3);

        // 分别取 V1，V2 方向上的 单位向量（只是为了方便下面计算）
        V1 = V1.normalized;
        V2 = V2.normalized;

        // 计算向量 V1，V2 点乘结果
        // 即获取 V1,V2夹角余弦    cos(夹角)
        float direction = Vector3.Dot(V1, V2);
        Debug.LogError("direction : " + direction);

        // 夹角方向一般取（0 - 180 度）
        // 如果取(0 - 360 度)
        // direction >= 0 则夹角在 （0 - 90] 和 [270 - 360] 度之间
        // direction < 0 则夹角在 （90 - 270) 度之间
        // direction 无法确定具体角度

        // 反余弦求V1，V2 夹角的弧度
        float rad = Mathf.Acos(direction);
        // 再将弧度转换为角度
        float deg = rad * Mathf.Rad2Deg;
        // 得到的 deg 为 V1，V2 在（0 - 180 度的夹角）还无法确定V1，V2 的相对夹角
        // deg 还是无法确定具体角度

        // 计算向量 V1， V2 的叉乘结果
        // 得到垂直于 V1， V2 的向量， Vector3(0, sin(V1,V2夹角), 0)
        // 即 u.y = sin(V1,V2夹角)
        Vector3 u = Vector3.Cross(V1, V2);
        Debug.LogError("u.y  : " + u.y);

        // u.y >= 0 则夹角在 ( 0 - 180] 度之间
        // u.y < 0 则夹角在 (180 - 360) 度之间
        // u.y 依然无法确定具体角度

        // 结合 direction >0 、 u.y > 0 和 deg 的值
        // 即可确定 V2 相对于 V1 的夹角
        if (u.y >= 0) // (0 - 180]
        {
            if (direction >= 0)
            {
                // (0 - 90] 度
            }
            else
            {
                // (90 - 180] 度
            }
        }
        else    // (180 - 360]
        {
            if (direction >= 0)
            {
                // [270 - 360]
                // 360 + (-1)deg
            }
            else
            {
                // (180 - 270)
            }
        }

        Debug.LogError(deg);
    }
}

