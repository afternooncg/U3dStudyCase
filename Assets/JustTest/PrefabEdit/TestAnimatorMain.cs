using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        if (isAttack)
        {
            AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
            if ((stateInfo.shortNameHash == Animator.StringToHash("Attack")))
            {
                //isAttack = false;
                //Debug.Log(m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime); 
                int a = (int)m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                float f = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime - (float)a;

                if (f > 0.9f && f < 1f)
                {
                    Debug.Log("Catch:" + m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime + " " + f);
                    m_animator.speed = 0;
                   // m_animator.Rebind();
                    isAttack = false;
                }
                
            
            }

            



 
        }
            
	}


    public void HandleBtnIdle()
    {

       m_animator.SetBool("isIdle", true);

     
    }

    bool isAttack = false;

    public void HandleBtnAttck()
    {
        m_animator.SetBool("isAttack", true);
        m_animator.Play("Attack", 0, 0.5f);
        
        
        isAttack = true;
        
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
//m_animator.Stop();  //5.6不再支持
        m_animator.enabled = false;
    }

    public void HandleBtnPlay()
    {
        m_animator.enabled = true;
        m_animator.Rebind();
        m_animator.Play("Attack");  //如果上次动作已经是Attack就无效了
    }


    public void HandleBtnMix()
    {
        m_animator.SetBool("isMixAttack", true);
        m_animator.SetLayerWeight(1, 0.5f);    
    }


    public void PlayByTime()
    {
        



    }
    
}
