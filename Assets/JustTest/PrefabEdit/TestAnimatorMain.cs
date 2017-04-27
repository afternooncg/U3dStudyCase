using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimatorMain : MonoBehaviour {


    public GameObject go;
    private Animator m_animator;
	// Use this for initialization
	void Start () {
        if (go != null)
            m_animator = go.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void HandleBtnIdle()
    {

        m_animator.SetBool("isIdle", true);
    }

    public void HandleBtnAttck()
    {
        m_animator.SetBool("isAttack", true);
    }


    public void HandleBtnRun()
    {
        m_animator.SetBool("isRun", true);
    }

    public void HandleBtnResponse()
    {
      //  m_animator.SetTrigger("res"); //无过渡  优先度高
        m_animator.SetBool("isResponse", true);
    }

    public void HandleBtnPause()
    {
        m_animator.speed = 0f;
    }

    public void HandleBtnResume()
    {
        m_animator.speed = 1f;
    }

    public void HandleBtnStop()
    {
        m_animator.Stop();
    }

    public void HandleBtnPlay()
    {
        m_animator.Rebind();
        m_animator.Play("Attack");
    }


    public void HandleBtnMix()
    {
        m_animator.SetBool("isMixAttack", true);
        m_animator.SetLayerWeight(1, 0.5f);    
    }

    
}
