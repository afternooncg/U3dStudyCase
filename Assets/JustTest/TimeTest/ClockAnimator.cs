using UnityEngine;
using System.Collections;
using System;

public class ClockAnimator : MonoBehaviour
{

		public Transform ObjCube;
		public Transform Hours, Miniutes, Seconds;
		public bool IsTimeSpan = false;
		private const float	hoursToDegrees = 360f / 12f,
				minutesToDegrees = 360f / 60f,
				secondsToDegrees = 360f / 60f;

		// Use this for initialization
		void Start ()
		{

				if (ObjCube)
						ObjCube.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
				
		GameObject.CreatePrimitive(PrimitiveType.Cube); //动态直接加入场景树
		GameObject obj = new GameObject("aa");
		obj.gameObject.AddComponent<MeshFilter>().mesh = new Mesh();
		obj.gameObject.AddComponent<MeshRenderer>();

	//	this.gameObject.AddComponent<GameObject.CreatePrimitive(PrimitiveType.Cube)>();
		//new GameObject("aa");
		}

		// Update is called once per frame
		void Update ()
		{

				if (!IsTimeSpan) {
						TimeSpan timespan = DateTime.Now.TimeOfDay;
						Hours.localRotation = Quaternion.Euler (0f, 0f, (float)timespan.TotalHours * -hoursToDegrees);
						Miniutes.localRotation = Quaternion.Euler (0f, 0f, (float)timespan.TotalMinutes * -minutesToDegrees);
						Seconds.localRotation = Quaternion.Euler (0f, 0f, (float)timespan.TotalSeconds * -secondsToDegrees);

				} else {
						DateTime time = DateTime.Now;
						//Hours.localRotation = Quaternion.Euler (new Vector3(0,0,time.Hour * -hoursToDegrees));
						//Miniutes.localRotation = Quaternion.Euler (new Vector3(0,0,time.Minute * -minutesToDegrees));
						//Seconds.localRotation = Quaternion.Euler (new Vector3(0,0,time.Second * -secondsToDegrees));

						Hours.rotation = Quaternion.Euler (new Vector3 (0, 0, time.Hour * -hoursToDegrees));
						Miniutes.rotation = Quaternion.Euler (new Vector3 (0, 0, time.Minute * -minutesToDegrees));
						Seconds.rotation = Quaternion.Euler (new Vector3 (0, 0, time.Second * -secondsToDegrees));
				}
		}
}
