using UnityEngine;
using System.Collections;

public class EditorCameraRotate : MonoBehaviour
{
	public float m_fNear = 20.0f;
	public float m_fFar = 1000.0f;

	public float m_fYaw = -40.0f;
	public float m_fPitch = 220.0f;
	public Vector3 m_vInitPostion = new Vector3(80.0f, 60.0f, 80.0f);

	public float m_fRotateSpeed = 2.0f;
	public float m_fMoveSpeed = 1.1f;
	public float m_fMouseWheel = 20.0f;
    //---------------------------------------------
    private void Awake()
    {
    }
    //---------------------------------------------
    private void Start()
	{
		transform.position = m_vInitPostion;
	}
    //---------------------------------------------
    void LateUpdate()
	{
        if (Input.GetMouseButton(1))
        {
            m_fYaw += Input.GetAxis("Mouse Y") * m_fRotateSpeed;
            m_fPitch += Input.GetAxis("Mouse X") * m_fRotateSpeed;
        }
        transform.eulerAngles = new Vector3(-m_fYaw, m_fPitch, 0.0f);


        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * m_fMoveSpeed;
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * m_fMoveSpeed;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * m_fMoveSpeed;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * m_fMoveSpeed;
        }
    }
}