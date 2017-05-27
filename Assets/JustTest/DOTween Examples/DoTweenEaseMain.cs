using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoTweenEaseMain : MonoBehaviour {


    enum TweenType
    { 
        Move,
        Scale,
        Rotate,
    }

    public Dropdown DpTweenType;
    public Dropdown DpEaseType;

    public InputField StartX;
    public InputField StartY;
    public InputField StartZ;

    public InputField TargetX;
    public InputField TargetY;
    public InputField TargetZ;

    public InputField txtTime;

    public Text txtUpdate;
    public Text txtComplete;
    public InputField txtCode;

    public Transform Ball;


    [Range(30,60)]
    public int frameRate = 30;

	// Use this for initialization
	void Start () {


        if (DpTweenType != null)
        {
            DpTweenType.ClearOptions();
            string[] names = Enum.GetNames(typeof(TweenType));
            for (int i = 0; i < names.Length; i++)
            {
                Dropdown.OptionData op1 = new Dropdown.OptionData();
                op1.text = names[i];                
                DpTweenType.options.Add(op1);               
            }

            DpTweenType.captionText.text = names[0];
            DpTweenType.value = 0;
        }


        if (DpEaseType != null)
        {
            DpEaseType.ClearOptions();
            string[] names = Enum.GetNames(typeof(Ease));
            for (int i = 0; i < names.Length; i++)
            {
                Dropdown.OptionData op1 = new Dropdown.OptionData();
                op1.text = names[i];
                DpEaseType.options.Add(op1);
            }

            DpEaseType.captionText.text = names[0];
            DpEaseType.value = 0;
        }


        Ball.DOScale(new Vector3(2f,2f,2f), 0.5f).SetEase(Ease.Flash);
	}
	
	// Update is called once per frame
	void Update () {

        Application.targetFrameRate = frameRate;
	}


    public void HandleTwennTypeChange()
    {
        Debug.Log(DpTweenType.value );
    }

    public void HandleEaseTypeChange()
    {
        Debug.Log(DpEaseType.value);
    }


    public void HandleBtnPlay()
    {
        if (Ball == null)
            return;

        Vector3 vs = new Vector3(float.Parse(StartX.text),  float.Parse(StartY.text), float.Parse(StartZ.text));
        Vector3 vt = new Vector3(float.Parse(TargetX.text),  float.Parse(TargetY.text), float.Parse(TargetZ.text));

        Ball.localPosition = Vector3.zero;
        Ball.localScale = Vector3.one;
        Ball.rotation = Quaternion.Euler(Vector3.zero);

        float time = float.Parse(txtTime.text);

        Tweener tw = null;

        switch ((TweenType)DpTweenType.value)
        { 
            case TweenType.Move:

                tw = Ball.DOMove(vt, time).SetEase((Ease)DpEaseType.value);

                 tw.OnUpdate(delegate()
                    {
                        txtUpdate.text = Ball.localPosition.ToString();
                    }
                );
              
                break;

            case TweenType.Rotate:

                tw = Ball.DORotate(vt *  Mathf.Rad2Deg, time).SetEase((Ease)DpEaseType.value);

                 tw.OnUpdate(delegate()
                    {
                        txtUpdate.text = Ball.localRotation.ToString();
                    }
                );
                 break;    

            case TweenType.Scale:
                tw = Ball.DOScale(vt, time).SetEase((Ease)DpEaseType.value);

                 tw.OnUpdate(delegate()
                    {
                        txtUpdate.text = Ball.localScale.ToString();
                    }
                );
               
                break;
        
        }

        if (tw != null)
        {
            tw.OnComplete(
                     delegate()
                     {
                         txtComplete.text = (TweenType)DpTweenType.value + " " +  (Ease)DpEaseType.value + "  over";

                         DOTween.Kill(tw);
                     }
                     );


            txtCode.text = "Tweener tw = Ball.DORotate(new Vector3(" + vt.ToString() + ")," + time.ToString() + ").SetEase(Ease." + ((Ease)DpEaseType.value).ToString() + ");\n\n";
            txtCode.text += "tw.OnUpdate(delegate(){ });\n\r";
            txtCode.text += "tw.OnComplete(delegate(){ });";
                                
        }
       



      

    }

    public void Load()
    {
       // SceneManager.LoadScene("LoadedWithTween", LoadSceneMode.Additive);
        //SceneManager.LoadSceneAsync("LoadedWithTween", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("LoadedWithTween");
    }
}

