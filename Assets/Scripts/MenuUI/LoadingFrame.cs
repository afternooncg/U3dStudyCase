using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingFrame : BaseFrame {

	// Use this for initialization
    UIProgressBar m_progbar;

    void Start()
    {
        m_progbar = this.transform.Find("Progress").GetComponent<UIProgressBar>();

        Reset();
    }

    public void Reset()
    {
        if (m_progbar != null)
            m_progbar.value = 0f;
    }
	
	// Update is called once per frame
	public void UpdateProgress(float progress)
    {
        if (m_progbar != null)
            m_progbar.value = progress;
	}
}
