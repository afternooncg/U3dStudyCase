using UnityEngine;
using System.Collections;

public class ScaleTransformation : Transformation {

    public Vector3 scale;

    public override Vector3 Apply(Vector3 point)
    {
        point.x *= scale.x;
        point.y *= scale.y;
        point.z *= scale.z;

        return point;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
