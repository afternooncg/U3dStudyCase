using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControll : MonoBehaviour {

	// Use this for initialization
    public float Speed = 10;
    private Rigidbody m_rigbody;

    public Transform BoomEffect;
    public Material Mat;

    private List<Transform> m_boomEffect;
    void Start()
    {
        m_rigbody = this.GetComponent<Rigidbody>();
        m_boomEffect = new List<Transform>();
    }

	
	// Update is called once per frame
	void Update () {

        float moveh = Input.GetAxis("Horizontal");
        float movev = Input.GetAxis("Vertical");

        m_rigbody.AddForce(new Vector3(moveh * Speed, 0, movev * Speed));


        foreach (Transform go in m_boomEffect)
        {
            ParticleSystem[]  particleSystems = go.GetComponentsInChildren<ParticleSystem>();
            bool allStopped = true;

            foreach (ParticleSystem ps in particleSystems)
            {
                if (!ps.isStopped)
                {
                    allStopped = false;
                }                
            }

            if (allStopped)
            {
                m_boomEffect.Remove(go);
                GameObject.Destroy(go.gameObject);                
            }
        }
	}

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag ("Pick Up"))
        {
            other.gameObject.SetActive (false);
            //修改颜色
            this.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
           //Material m =  this.GetComponent<Material>();
           //m.color = 
           //this.gameObject.GetComponent<Material>()

          //GameObject fire =   Instantiate(Resources.Load("_Prefabs/ScrollBall/fx_fire_ball_b")) as GameObject;
            if (BoomEffect)
            {
                Transform fire = Instantiate(BoomEffect, transform.position, Quaternion.identity) as Transform;
             //fire.GetComponent<Renderer>().sharedMaterial = Mat;
             //fire.GetComponent<ParticleRenderer>().material = Mat;
            // fire.GetComponent<ParticleSystemRenderer>().material = Mat;
             m_boomEffect.Add(fire);             

            }
        //  fire.transform.parent = transform.parent;
          //fire.transform.position = transform.position;
        }
    }
}
