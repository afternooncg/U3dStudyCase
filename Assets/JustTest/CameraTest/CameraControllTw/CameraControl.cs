using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;


public class CameraControl : MonoBehaviour
{
    public enum ECamereMode
    {
        Editor,
        Game,
        Null,
    }

    public enum ECamereType
    {
        Background = 0,
        Middle,
        Foreground,
        Effect,
        Num,
    }

    public static CameraControl Instance { protected set; get; }

    public ECamereMode controlerMode = ECamereMode.Game;

    public Camera[] m_ppCamera = new Camera[(int)ECamereType.Num];

    //---------------------------------------------
    private GameCameraRotate m_pGameRotate;
    public GameCameraRotate GameRotate
    {
        get{ return m_pGameRotate; }
        set{ m_pGameRotate = value; }
    }
  
    //---------------------------------------------
    public bool bAutoGetSize = true;
    private float m_minViewSize = -75f;
    public float MinViewSize
    {
        get { return m_minViewSize; }
        set { m_minViewSize = value; }
    }
    private float m_maxViewSize = 75f;
    public float MaxViewSize
    {
        get { return m_maxViewSize; }
        set { m_maxViewSize = value; }
    }
    //---------------------------------------------
    private void Awake()
    {
        m_pGameRotate = gameObject.GetComponent<GameCameraRotate>();
       // m_pEditorRotate = gameObject.GetComponent<EditorCameraRotate>();

        ChangeControllerMode(controlerMode, true);

        Instance = this;
    }
    //---------------------------------------------
    private void Start()
    {
        if (bAutoGetSize)
        {
            GameObject Grid = GameObject.Find("Grid");
            if (Grid != null)
            {
                float size = Mathf.Min(Grid.transform.localScale.x / 2, Grid.transform.localScale.z / 2);
                OnChangeMap(-size, size);
            }
        }
    }
    //---------------------------------------------
    private void OnDestroy()
    {
        Instance = null;
    }
    //---------------------------------------------
    public void OnChangeMap(float minSize, float maxSize)
    {
        if(bAutoGetSize)
        {
            m_maxViewSize = Mathf.Min(minSize / 2, maxSize / 2);
            m_minViewSize = -m_maxViewSize;
        }

        if (m_pGameRotate)
        {
            m_pGameRotate.MinViewSize = m_minViewSize;
            m_pGameRotate.MaxViewSize = m_maxViewSize;
        }
    }
    //---------------------------------------------
    public void ChangeControllerMode(ECamereMode mode, bool bForce = false)
    {
        if (!bForce && controlerMode == mode)
            return;

        controlerMode = mode;
        if (controlerMode == ECamereMode.Editor)
        {
           // m_pEditorRotate.enabled = true;
            m_pGameRotate.enabled = false;
        }
        else if (controlerMode == ECamereMode.Game)
        {
          //  m_pEditorRotate.enabled = false;
            m_pGameRotate.enabled = true;
        }
        else
        {
          //  m_pEditorRotate.enabled = false;
            m_pGameRotate.enabled = false;
        }
    }
    //---------------------------------------------
    public Camera ChangeCamera(ECamereType type)
    {
        if (type >= ECamereType.Num || type < 0)
            return null;

        for(int i = 0; i < (int)ECamereType.Num; ++i)
        {
            m_ppCamera[i].enabled = (int)type == i;
        }

        return m_ppCamera[(int)type];
    }
    //-------------------------------------------
    static public Vector3 CalcCameraHitPlane(Vector3 pos, Vector3 dir)
    {
        Vector3 vPlanePos = Vector3.zero;
        vPlanePos.y = 0.0f;

        Vector3 vPlaneNor = Vector3.up;

        float fdot = Vector3.Dot(dir, vPlaneNor);
        if (fdot == 0.0f)
            return Vector3.zero;

        float fRage = ((vPlanePos.x - pos.x) * vPlaneNor.x + (vPlanePos.y - pos.y) * vPlaneNor.y + (vPlanePos.z - pos.z) * vPlaneNor.z) / fdot;

        return pos + dir * fRage;
    }
    //---------------------------------------------
    public Vector3 GetCameraLookAtLand()
    {
        return CalcCameraHitPlane(transform.position, transform.forward);
    }
    //---------------------------------------------
#if UNITY_EDITOR
    Vector3 a1, a2, a3, a4, viewPort;
    [SerializeField]
    private bool m_isShowCameraViewRange = true;
    private void OnDrawGizmos()
    {
        if (m_isShowCameraViewRange)
        {
            RaycastHit hit;
            viewPort = Vector3.zero;
            Ray ray = m_ppCamera[(int)ECamereType.Background].ViewportPointToRay(viewPort);
            if (Physics.Raycast(ray, out hit))
            {
                a1 = hit.point;
            }
            viewPort.Set(1, 0, 0);
            ray = m_ppCamera[(int)ECamereType.Background].ViewportPointToRay(viewPort);
            if (Physics.Raycast(ray, out hit))
            {
                a2 = hit.point;
            }
            viewPort.Set(1, 1, 0);
            ray = m_ppCamera[(int)ECamereType.Background].ViewportPointToRay(viewPort);
            if (Physics.Raycast(ray, out hit))
            {
                a3 = hit.point;
            }
            viewPort.Set(0, 1, 0);
            ray = m_ppCamera[(int)ECamereType.Background].ViewportPointToRay(viewPort);
            if (Physics.Raycast(ray, out hit))
            {
                a4 = hit.point;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(a1, a2);
            Gizmos.DrawLine(a2, a3);
            Gizmos.DrawLine(a3, a4);
            Gizmos.DrawLine(a4, a1);
        }

        Gizmos.color = Color.yellow;
        a1.x = m_minViewSize;
        a1.z = m_minViewSize;
        a2.x = m_minViewSize;
        a2.z = m_maxViewSize;
        a3.x = m_maxViewSize;
        a3.z = m_maxViewSize;
        a4.x = m_maxViewSize;
        a4.z = m_minViewSize;
        Gizmos.DrawLine(a1, a2);
        Gizmos.DrawLine(a2, a3);
        Gizmos.DrawLine(a3, a4);
        Gizmos.DrawLine(a4, a1);

        Gizmos.color = Color.red;
        Vector3 target = CameraControl.CalcCameraHitPlane(transform.position, transform.forward);
        Gizmos.DrawLine(transform.position, target);
    }
#endif
}
