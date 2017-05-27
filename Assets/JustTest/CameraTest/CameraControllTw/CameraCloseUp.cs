using UnityEngine;
using System.Collections;

public class CameraCloseUp : MonoBehaviour
{

	private Vector3 m_vDuration;
	private Vector3 m_vLookAtOffset;
	private Vector3 m_vTranslationOffset;

	private GameObject m_pLookAtObj;

	//runing time data
	private bool	m_bEnable = false;
	private Vector3	m_vDurationDelta;
	private Vector3 m_vCloseUpTransOffset;
	private Vector3 m_vCloseUpDirOffset;
	private Vector3 m_vCurrentTrans;
	private Vector3 m_vCurrentDir;

	private void Start()
	{

	}

	public bool isCloseUp()
	{
		return m_bEnable;
	}

	public void CloseUp(string strParam, GameObject Obj)
	{
		string[] param = strParam.Split(",".ToCharArray());
		if (param.Length != 9) 
		{
			Debug.LogWarning ("CameraCloseUp::CloseUp  param[" + strParam + "] error!");
			return;
		}

		m_vDuration.x = float.Parse(param[0]);
		m_vDuration.y = float.Parse(param[1]);
		m_vDuration.z = float.Parse(param[2]);

		m_vLookAtOffset.x = float.Parse(param[3]);
		m_vLookAtOffset.y = float.Parse(param[4]);
		m_vLookAtOffset.z = float.Parse(param[5]);

		m_vTranslationOffset.x = float.Parse(param[6]);
		m_vTranslationOffset.y = float.Parse(param[7]);
		m_vTranslationOffset.z = float.Parse(param[8]);

		m_pLookAtObj = Obj;

		if (!m_bEnable)
		{
			m_vCurrentTrans = transform.position;
			m_vCurrentDir = transform.forward;
		}
		if (m_pLookAtObj && m_bEnable) 
		{
			Vector3 vTrans = Vector3.zero;
			Vector3 vDir = Vector3.zero;
			GetCloseUpParameter(ref vTrans, ref vDir, 0.0f);
			m_vCloseUpTransOffset = vTrans - transform.position;
			m_vCloseUpDirOffset = vDir - transform.forward;
		}

		m_bEnable = true;
		m_vDurationDelta = m_vDuration;

	}

	public void GetCloseUpParameter(ref Vector3 vTranslation,ref Vector3 vDirection, float fFrameTime)
	{
		if (m_pLookAtObj) 
		{
			Vector3 vLookAt = m_pLookAtObj.transform.position
			                  + m_vLookAtOffset.x * m_pLookAtObj.transform.forward
			                  + m_vLookAtOffset.y * m_pLookAtObj.transform.up
			                  + m_vLookAtOffset.z * m_pLookAtObj.transform.right;

			Vector3 vTrans = m_pLookAtObj.transform.position
			                 + m_vTranslationOffset.x * m_pLookAtObj.transform.forward
			                 + m_vTranslationOffset.y * m_pLookAtObj.transform.up
			                 + m_vTranslationOffset.z * m_pLookAtObj.transform.right;

			Vector3 vDir = vLookAt - vTrans;
			vDir = vDir.normalized;
			if (m_vDurationDelta.x > 0.0f) 
			{
				m_vDurationDelta.x -= fFrameTime;
				m_vDurationDelta.x = Mathf.Max (m_vDurationDelta.x, 0);
				float fFactor = m_vDurationDelta.x / m_vDuration.x;
				vTranslation = vTrans * (1.0f - fFactor) + (m_vCurrentTrans + m_vCloseUpTransOffset) * fFactor;
				vDirection = vDir * (1.0f - fFactor) + (m_vCurrentDir + m_vCloseUpDirOffset) * fFactor;
			} 
			else if (m_vDurationDelta.y > 0.0f) 
			{
				m_vDurationDelta.y -= fFrameTime;
				m_vDurationDelta.y = Mathf.Max (m_vDurationDelta.y, 0.0f);
				vTranslation = vTrans;
				vDirection = vDir;
			}
			else if( m_vDurationDelta.z > 0.0f )
			{
				m_vDurationDelta.z -= fFrameTime;
				m_vDurationDelta.z = Mathf.Max (m_vDurationDelta.z, 0.0f);	
				if (m_vDurationDelta.z <= 0.0f)
					m_bEnable = false;

				float fFactor = m_vDurationDelta.z / m_vDuration.z;
				vTranslation = vTrans * fFactor + m_vCurrentTrans * (1.0f - fFactor);
				vDirection = vDir * fFactor + m_vCurrentDir * (1.0f - fFactor);
			}
			vDirection = vDirection.normalized;
		}
		else
		{
			vTranslation = m_vCurrentTrans;
			vDirection = m_vCurrentDir;
		}

	//	transform.position = vTranslation;
	//	transform.forward = vDirection;
	}

	void LateUpdate()
	{
	}
}