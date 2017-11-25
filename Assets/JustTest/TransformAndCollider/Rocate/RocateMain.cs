using UnityEngine;
using System.Collections;

public class RocateMain : MonoBehaviour {

    private GameObject m_cube;
    private GameObject m_sphere;

	// Use this for initialization
	void Start () {

        m_cube = GameObject.Find("Cube");
        m_sphere = GameObject.Find("Sphere");

        //创建平移 旋转 缩放矩阵 可以理解为一个坐标系（不知道对不对。。）
        Matrix4x4 mat = Matrix4x4.TRS(new Vector3(1, 1, 1), Quaternion.Euler(0, 90, 0), Vector3.one);
        //得到在这个坐标系点（2,2,2）在世界坐标系的坐标
        print(mat.MultiplyPoint(new Vector3(2, 2, 2)));
        //在世界坐标系点（2,2,2）在mat变换下的坐标
        //局部坐标*mat = 世界坐标
        //世界坐标*mat的逆 = 局部坐标
        print(mat.inverse.MultiplyPoint(new Vector3(3.0f, 3.0f, -1.0f)));

        print(mat.inverse.MultiplyPoint(new Vector3(2, 2, 2)));
        //MultiplyVector方法 感觉没啥用
        //把方向向量dir 做了一个旋转
        Vector3 dir = new Vector3(3, 2, 3);
        print(mat.MultiplyVector(dir) == Quaternion.Euler(0, 90, 0) * dir);
	}
	
	// Update is called once per frame
	void Update () {
        if (m_cube && m_sphere)
        {
            //m_sphere.transform.RotateAround(m_cube.transform.position, Vector3.up , Time.deltaTime * 10);
            //m_sphere.transform.RotateAround(m_cube.transform.position, m_cube.transform.rotation.eulerAngles , Time.deltaTime * 10);
            m_sphere.transform.RotateAround(m_cube.transform.position,  m_cube.transform.up, Time.deltaTime * 20);
            //m_sphere.transform.Rotate(Vector3.up, Space())
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A DOWN");
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("A UP");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("A HOLD DOWN");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Left DOWN");        
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            Debug.Log("Left UP");  
        }
        else if (Input.GetButton("Fire1"))
        {
            Debug.Log("Left HOLD DOWN");  
        }
	}
}
