using UnityEngine;

public class GameCameraRotate : MonoBehaviour
{
    public Vector3 InitRotate = new Vector3(50,-140,0);
	public Vector3 m_vInitPostion = new Vector3(60.0f, 60.0f, 60.0f);

	public float m_fRotateSpeed = 2.0f;
	public float m_fMoveSpeed = 2.0f;
	public float m_fMouseWheel = 30.0f;

    public bool bLimitDistance = true;
	public float m_fUpperDistance = 60.0f;
	public float m_fLowerDistance = 30.0f;

    private float m_fMinView = -75f;
    public float MinViewSize
    {
        get { return m_fMinView; }
        set { m_fMinView = value; }
    }
    private float m_fMaxView = 75f;
    public float MaxViewSize
    {
        get { return m_fMaxView; }
        set { m_fMaxView = value; }
    }

    private Ray			m_pRay = new Ray();
	private Vector3		m_vPosition = new Vector3();
	private float 		m_fTweenMove = -1.0f;

    private float m_fYaw = 0.0f;
    private float m_fPitch = 0.0f;
    private float m_fRaw = 0f;
    //---------------------------------------------
    private CameraCloseUp m_pCloseUp;
    public CameraCloseUp cameraCloseUp
    {
        get { return m_pCloseUp; }
        set { m_pCloseUp = value; }
    }
    //---------------------------------------------
    private void Awake()
    {
        m_pCloseUp = transform.gameObject.GetComponent<CameraCloseUp>();
        if (m_pCloseUp == null)
            m_pCloseUp = transform.gameObject.AddComponent<CameraCloseUp>();
    }
    //--------------------------------------------- 
    private void Start()
	{
		transform.position = m_vInitPostion;
        m_fYaw = -InitRotate.x;
        m_fPitch = InitRotate.y;
        m_fRaw = InitRotate.z;
    }
	//-------------------------------------------
	private void OnDrawGizmos()
	{
        Gizmos.color = Color.red;
        Vector3 vPos = CameraControl.CalcCameraHitPlane(transform.position, transform.forward);
        vPos.x = Mathf.Clamp(vPos.x, m_fMinView, m_fMaxView);
        vPos.z = Mathf.Clamp(vPos.z, m_fMinView, m_fMaxView);
        Gizmos.DrawLine(transform.position, vPos);
    }
	//-------------------------------------------
	public void SetViewSize(float min, float max)
	{
		m_fMinView = min;
		m_fMaxView = max;
	}
    //-------------------------------------------
    private bool IsLookInRegion(Vector3 pos, Vector3 dir)
    {
        Vector3 vPlanePos = Vector3.zero;
        vPlanePos.y = 0.0f;

        Vector3 vPlaneNor = Vector3.up;

        float fdot = Vector3.Dot(dir, vPlaneNor);
        if (fdot == 0.0f)
            return false;

        float fRage = ((vPlanePos.x - pos.x) * vPlaneNor.x + (vPlanePos.y - pos.y) * vPlaneNor.y + (vPlanePos.z - pos.z) * vPlaneNor.z) / fdot;

        Vector3 vPos = pos + dir * fRage;
        if (vPos.x < m_fMinView || vPos.x > m_fMaxView || vPos.z < m_fMinView || vPos.z > m_fMaxView )
            return false;

        return true;
    }
    //-------------------------------------------
    public Vector3 CalcLimitRegion(Vector3 pos, Vector3 dir)
    {
        Vector3 vPlanePos = Vector3.zero;
        vPlanePos.y = 0.0f;

        Vector3 vPlaneNor = Vector3.up;

        float fdot = Vector3.Dot(dir, vPlaneNor);
        if (fdot == 0.0f)
            return Vector3.zero;

        float fRage = ((vPlanePos.x - pos.x) * vPlaneNor.x + (vPlanePos.y - pos.y) * vPlaneNor.y + (vPlanePos.z - pos.z) * vPlaneNor.z) / fdot;

        Vector3 vPos = pos + dir * fRage;
        vPos.x = Mathf.Clamp(vPos.x, m_fMinView, m_fMaxView);
        vPos.z = Mathf.Clamp(vPos.z, m_fMinView, m_fMaxView);

        Vector3 vCameraPos = vPos - dir* fRage;

        return vCameraPos;
    }
    //-------------------------------------------
    private float CalcCameraDistancePlane(Vector3 pos, Vector3 dir)
	{
		return Vector3.Distance(CameraControl.CalcCameraHitPlane(pos, dir), pos);
	}
	//-------------------------------------------
	public void SetLookAt( Vector3 vPos, float fTween )
	{
        vPos = CalcLimitRegion(vPos, transform.forward);

        m_fTweenMove = fTween;
		m_vPosition = vPos - transform.forward * CalcCameraDistancePlane (transform.position, transform.forward);

		if(m_fTweenMove<=0f)
			transform.position = m_vPosition;
	}
	//-------------------------------------------
	void LateUpdate()
	{
		//TEST Begin---------------------by happli
		if (Input.GetKey (KeyCode.O)) 
		{
			GameObject obj = GameObject.Find("1_build_model");
			m_pCloseUp.CloseUp("0.5,1,0.1,0,0,0,0,8,-5", obj);
		}
		else if (Input.GetKey (KeyCode.P)) 
		{
            /// SetLookAt(new Vector3(49.0f, 0.0f, -49.0f), 0.3f);
            SetLookAt(new Vector3(106.0f, 0.0f, 31), 0.3f);
        }
        //TEST End---------------------by happli

        if (m_pCloseUp.isCloseUp ())
		{
			Vector3 vTrans = Vector3.zero;
			Vector3 vDir = Vector3.zero;
			m_pCloseUp.GetCloseUpParameter (ref vTrans, ref vDir, 1.0f / Application.targetFrameRate);

			transform.position = vTrans;
			transform.forward = vDir;
		}
		else if (m_fTweenMove>0f) 
		{
			transform.position = Vector3.Lerp(transform.position, m_vPosition, m_fTweenMove);
			if (Vector3.Distance (transform.position, m_vPosition) <= 0.01f) 
			{
				transform.position = m_vPosition;
				m_fTweenMove = -1f;
			}
		}
		else 
		{
            float OldYaw = m_fYaw;
            if (Input.GetMouseButton(1))
            {
                m_fYaw += Input.GetAxis("Mouse Y") * m_fRotateSpeed;
                m_fPitch += Input.GetAxis("Mouse X") * m_fRotateSpeed;
                m_fYaw = Mathf.Clamp(m_fYaw, -90f, 0f);
            }
            transform.eulerAngles = new Vector3(-m_fYaw, m_fPitch, 0.0f);
            if (!IsLookInRegion(m_vPosition, transform.forward))
            {
                transform.eulerAngles = new Vector3(-OldYaw, m_fPitch, 0.0f);
                m_fYaw = OldYaw;
            }


            m_vPosition = transform.position;
            if (Input.GetAxis ("Mouse ScrollWheel") != 0) 
			{
				m_vPosition += transform.forward *  Input.GetAxis("Mouse ScrollWheel")*m_fMouseWheel;	
			}

			Vector3 vForward = transform.forward;
			vForward.y = 0.0f;
            vForward = vForward.normalized;


            if (Input.GetKey (KeyCode.UpArrow))
				m_vPosition.y += m_fMoveSpeed;
			if (Input.GetKey (KeyCode.DownArrow))
				m_vPosition.y -= m_fMoveSpeed;
			if (Input.GetKey (KeyCode.W)) 
				m_vPosition += vForward * m_fMoveSpeed;
			if (Input.GetKey (KeyCode.S)) 
				m_vPosition -= vForward * m_fMoveSpeed;
			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) 
				m_vPosition -= transform.right * m_fMoveSpeed;
			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) 
				m_vPosition += transform.right * m_fMoveSpeed;

            if(bLimitDistance)
                m_vPosition.y = Mathf.Clamp(m_vPosition.y, m_fLowerDistance, m_fUpperDistance);
            m_vPosition = CalcLimitRegion(m_vPosition, transform.forward);

            if(!Vector3.Equals(m_vPosition,transform.position))
			    transform.position = m_vPosition;
        }
    }
}