using UnityEngine;
using System.Collections;

public class PositionTransformation : Transformation {

    public Vector3 position = new Vector3(1,0,0);

    public override Vector3 Apply(Vector3 point)
    {
        return point + position;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
