using UnityEngine;
using System.Collections;
using System.Net.Sockets;

public class Fractal : MonoBehaviour {

    public Mesh[] meshes;

    public Mesh mesh;
    public Material material;

    public int maxDepth;

    private int depth;

    public float childScale;

    private Material[,] materials;

    public float spawnProbability;

    public float maxRotationSpeed;

    private float rotationSpeed;

    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1,2];
        for (int i = 0; i <= maxDepth; i++)
        {
            materials[i,0] = new Material(material);
            float t = i / (maxDepth - 1f);
            t *= t;
            materials[i,0].color = Color.Lerp(Color.white, Color.yellow, t);
                //Color.Lerp(Color.white, Color.yellow, (float)i / maxDepth);


            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
        }

        materials[maxDepth,0].color = Color.magenta;
        materials[maxDepth, 1].color = Color.red;
    }

    private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
       	Vector3.forward,
		Vector3.back
	};

    private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, -90f),
		Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
		Quaternion.Euler(-90f, 0f, 0f)
	};

	// Use this for initialization
	void Start () {

        if (materials == null)
        {
            InitializeMaterials();
        }

        this.gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];// mesh;
        this.gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0, 2)];
        rotationSpeed = Random.Range(rotationSpeed, maxRotationSpeed);

     //   GetComponent<MeshRenderer>().material.color =   Color.Lerp(Color.white, Color.yellow, (float)depth / maxDepth);
        if (depth < maxDepth)
        {
            StartCoroutine("CreateChildren");
        }
	}
	
	// Update is called once per frame
    private void Initialize(Fractal parent, int childIndex) 
    {
        if (parent != null)
        {
            this.mesh = parent.mesh;
            this.meshes = parent.meshes;
            this.material = parent.material;
            this.maxDepth = parent.maxDepth;
            this.depth = parent.depth+1;
            transform.parent = parent.transform;
            
            childScale = parent.childScale;
            this.spawnProbability = parent.spawnProbability;
            maxRotationSpeed = parent.maxRotationSpeed;

            this.transform.localScale = Vector3.one * childScale;
            this.transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
            this.transform.localRotation = childOrientations[childIndex];
            
        }
	}

    private IEnumerator CreateChildren()
    {
        //WaitForSeconds w = new WaitForSeconds(0.5f);

        for (int i = 0; i < childDirections.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            if (spawnProbability > Random.value)
            {
                new GameObject("Fractal Child").AddComponent<Fractal>().
                Initialize(this, i);
            }
            SocketAsyncEventArgs a;
        }        
    }

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
