using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LookTargetCamera : MonoBehaviour
{
//	[HideInInspector]
	public float distance = 20f;
//	[HideInInspector]
	public float heightOffset = 2.5f;
//	[HideInInspector]
	public float fov = 20f;
//	[HideInInspector]
	public float viewAngle = 15f;
    [HideInInspector]
    public float lastviewAngle = 15f;

	public float yAngle = 0;

	public Camera targetCamera;
	public Vector3 targetPosition;
    public float fObstacleOffset = 5.0f;
	[HideInInspector]
	public Vector3? lastTargetPosition = null;
	public float dampSpeed = 5f;

    public BoxCollider boxbound;
    [HideInInspector]
    public bool IsBreakLimit = false;
	public virtual void Start()
	{
		if (targetCamera == null)
		{
			targetCamera = Camera.main;
		}
	}

//	void LateUpdate()
//	{
//		targetCamera.transform.rotation = Quaternion.Euler(viewAngle,yAngle,0);
//
//		Vector3 dampedTargetPosition = targetPosition;
//
//		if (lastTargetPosition == null)
//			lastTargetPosition = targetPosition;
//
//		dampedTargetPosition = Vector3.Lerp(lastTargetPosition.Value, targetPosition, Time.deltaTime * dampSpeed);
//			
//
//		targetCamera.transform.position = new Vector3(dampedTargetPosition.x - Mathf.Cos(viewAngle * Mathf.Deg2Rad) * distance * Mathf.Sin(yAngle * Mathf.Deg2Rad),
//			                                              dampedTargetPosition.y + Mathf.Sin(viewAngle * Mathf.Deg2Rad) * distance + heightOffset,
//			                                              dampedTargetPosition.z - Mathf.Cos(viewAngle * Mathf.Deg2Rad) * distance * Mathf.Cos(yAngle * Mathf.Deg2Rad));
//
//		lastTargetPosition = dampedTargetPosition;
//
//		targetCamera.fieldOfView = fov;
//	}

	public virtual void Update()
	{
		targetCamera.transform.rotation = Quaternion.Euler(viewAngle,yAngle,0);
        //targetPosition.z = 0;
		Vector3 dampedTargetPosition = targetPosition;

		if (lastTargetPosition == null)
			lastTargetPosition = targetPosition;

		dampedTargetPosition = Vector3.Lerp(lastTargetPosition.Value, targetPosition, Time.smoothDeltaTime * dampSpeed);

        //targetCamera.transform.position = new Vector3(dampedTargetPosition.x - Mathf.Cos(viewAngle * Mathf.Deg2Rad) * distance * Mathf.Sin(yAngle * Mathf.Deg2Rad),
        //                                                  dampedTargetPosition.y + Mathf.Sin(viewAngle * Mathf.Deg2Rad) * distance + heightOffset,
        //                                                  dampedTargetPosition.z - Mathf.Cos(viewAngle * Mathf.Deg2Rad) * distance * Mathf.Cos(yAngle * Mathf.Deg2Rad));
        Vector3 vPos = new Vector3(dampedTargetPosition.x - Mathf.Cos(viewAngle * Mathf.Deg2Rad) * distance * Mathf.Sin(yAngle * Mathf.Deg2Rad),
                                                          dampedTargetPosition.y + Mathf.Sin(viewAngle * Mathf.Deg2Rad) * distance + heightOffset,
                                                          dampedTargetPosition.z - Mathf.Cos(viewAngle * Mathf.Deg2Rad) * distance * Mathf.Cos(yAngle * Mathf.Deg2Rad));

        if (boxbound == null)
            targetCamera.transform.position = vPos;
        else
        {
            if (boxbound.bounds.Contains(vPos))
            {
                targetCamera.transform.position = vPos;
            }
            else
            {
                if (vPos.x > boxbound.bounds.max.x)
                    vPos.x = boxbound.bounds.max.x;
                if (vPos.x < boxbound.bounds.min.x)
                    vPos.x = boxbound.bounds.min.x;

                if (vPos.y > boxbound.bounds.max.y)
                    vPos.y = boxbound.bounds.max.y;
                if (vPos.y < boxbound.bounds.min.y)
                    vPos.y = boxbound.bounds.min.y;

                if (vPos.z > boxbound.bounds.max.z)
                    vPos.z = boxbound.bounds.max.z;
                if (vPos.z < boxbound.bounds.min.z)
                    vPos.z = boxbound.bounds.min.z;

                targetCamera.transform.position = vPos;
            }
        } 
        
		lastTargetPosition = dampedTargetPosition;
		targetCamera.fieldOfView = fov;
	}
}
