using UnityEngine;
using System.Collections;

public class Grapher2 : MonoBehaviour {
	[Range(10, 100)]
	public int resolution = 100;
	private int currentResolution;
	private ParticleSystem.Particle[] points;
	
	public enum FunctionOption {
		Linear,
		Exponential,
		Parabola,
		Sine
	}
	
	public FunctionOption function;
	
	
	private delegate float FunctionDelegate (Vector3 p,float x);
	private static FunctionDelegate[] functionDelegates = {
		Linear,
		Exponential,
		Parabola,
		Sine
	};
	
	
	void Start () {
		
		CreatePoints();
		
	}
	
	private void CreatePoints () {
		if (resolution < 10 || resolution > 100) {
			Debug.LogWarning("Grapher resolution out of bounds, resetting to minimum.", this);
			resolution = 10;
		}
		currentResolution = resolution;
		points = new ParticleSystem.Particle[resolution * resolution];
		float increment = 1f / (resolution - 1);
		int i = 0;
		for (int x = 0; x < resolution; x++) {
			for (int z = 0; z < resolution; z++) {
				Vector3 p = new Vector3(x * increment, 0f, z * increment);
				points[i].position = p;
				points[i].color = new Color(p.x, 0f, p.z);
				points[i++].size = 0.1f;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if (currentResolution != resolution) {
			CreatePoints();
		}
		FunctionDelegate f = functionDelegates[(int)function];
		float t = Time.timeSinceLevelLoad;
		for (int i = 0; i < points.Length; i++) {
			Vector3 p = points[i].position;
			p.y = f(p, t);
			points[i].position = p;
			Color c = points[i].color;
			c.g = p.y;
			points[i].color = c;
		}
		
		this.GetComponent<ParticleSystem>().SetParticles(points, points.Length);
	}
	
	private static float Linear (Vector3 p, float t) {
		return p.x;
	}
	
	private static float Exponential (Vector3 p, float t) {
		return p.x * p.x;
	}
	
	private static float Parabola (Vector3 p, float t){
		p.x = 2f * p.x - 1f;
		return p.x * p.x;
	}
	

	
	private static float Sine (Vector3 p,float t){
		//return 0.5f + 0.5f * Mathf.Sin(2 * Mathf.PI * x + Time.timeSinceLevelLoad);

		return 0.50f +	0.25f * Mathf.Sin(4f * Mathf.PI * p.x + 4f * t) * Mathf.Sin(2f * Mathf.PI * p.z + t) +
				0.10f * Mathf.Cos(3f * Mathf.PI * p.x + 5f * t) * Mathf.Cos(5f * Mathf.PI * p.z + 3f * t) +
				0.15f * Mathf.Sin(Mathf.PI * p.x + 0.6f * t);
	}
}


