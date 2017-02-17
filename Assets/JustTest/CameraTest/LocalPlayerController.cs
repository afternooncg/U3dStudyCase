using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 
public class LocalPlayerController : MonoBehaviour
{
//	public Warrior activeWarrior;

	private float lastClickEffectTime = 0;
    LookTargetCamera cameraController;

	IEnumerator Start()
	{
		yield return null;
		yield return null;
	
        cameraController = Camera.main.GetComponent<LookTargetCamera>();
	}

	void LateUpdate()
	{
		
		
		if (Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			
			if (Physics.Raycast(ray, out hitInfo, 500, 1<<LayerMask.NameToLayer("Floor")))
			{
				//activeWarrior.GetWarriorController().SetDestPoint(hitInfo.point);

				/*if (Time.time - lastClickEffectTime > 0.2f)
				{
					EffectCreator.CreateEffectAt(22200101, hitInfo.point, Vector3.zero, null);
					lastClickEffectTime = Time.time;
				}*/
			}
		}
		else
		{
			lastClickEffectTime = 0;
		}


		
        if (cameraController != null)
        {
            Vector3 vPos = this.transform.position;
            //vPos.z = cameraController.targetPosition.z;
            cameraController.targetPosition = vPos;
        }
	}
}