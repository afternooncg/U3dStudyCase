using UnityEngine;
using System.Collections;

public class CameraPosiTran : MonoBehaviour
{
	public GameObject go;
		// Use this for initialization
		void Start ()
		{	
				
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetMouseButtonDown (0)) {

			Vector3 ScreenSpace = Camera.main.WorldToScreenPoint(go.transform.position);  
			Debug.Log ("ScreenSpace x:" + ScreenSpace.x + " ScreenSpace y:" + ScreenSpace.y + " ScreenSpace z:" +  ScreenSpace.z);
						//屏幕坐标 0,0 左下角
			Debug.Log ("Mouse x:" + Input.mousePosition.x + " Mouse y:" + Input.mousePosition.y + " Mouse z:" +  Input.mousePosition.z);
			Vector3 worldPosi = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,ScreenSpace.z));
			Debug.Log ("World x:" + worldPosi.x + " World y:" + worldPosi.y + " World z:" +  worldPosi.y);
			go.transform.localPosition = worldPosi;
			Vector3 viewPortPosi = Camera.main.ScreenToViewportPoint(Input.mousePosition);
			Debug.Log ("viewPortPosi x:" + viewPortPosi.x + " viewPortPosi y:" + viewPortPosi.y);

			Vector3 ScreenPosi = Camera.main.WorldToScreenPoint(new Vector3(0,0,0));
			Debug.Log ("ScreenPosi x:" + ScreenPosi.x + " ScreenPosi y:" + ScreenPosi.y);
				}


		}

	// Draw a yellow sphere in the scene view at the position
	// on the near plane of the selected camera that is
	// 100 pixels from lower-left.
	void OnDrawGizmosSelected() {
		Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(100, 100, Camera.main.nearClipPlane));
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(p, 0.1F);
	}

	void OnDrawGizmos()
	{
		
		Vector3 ScreenSpace = Camera.main.WorldToScreenPoint(go.transform.position);  
		Debug.Log ("ScreenSpace x:" + ScreenSpace.x + " ScreenSpace y:" + ScreenSpace.y + " ScreenSpace z:" +  ScreenSpace.z);
		//屏幕坐标 0,0 左下角
		Debug.Log ("Mouse x:" + Input.mousePosition.x + " Mouse y:" + Input.mousePosition.y + " Mouse z:" +  Input.mousePosition.z);
		Vector3 worldPosi = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,ScreenSpace.z));
		;
		Gizmos.DrawIcon (this.transform.localPosition, "item.png", true);
		Gizmos.color = new Color (255, 0, 0);
		Gizmos.DrawLine (new Vector3 (0, 0, 0), worldPosi);
	}

}
