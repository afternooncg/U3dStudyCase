using UnityEngine;
using System.Collections;


public class CameraController1 : MonoBehaviour
{
    #region Private References
    [SerializeField, Range(0.0f, 1.0f)]
    private float _lerpRate;
    private float _xRotation;
    private float _yYRotation;
    #endregion
    #region Private Methods
    private void Rotate(float xMovement, float yMovement)
    {
        
        _xRotation += xMovement;
        _yYRotation += yMovement;

        bool flag = false;
        float temp = Mathf.Lerp(_xRotation, 0, _lerpRate);
        if (Mathf.Abs(temp - _xRotation) > 0.02)
        {
            _xRotation = temp;
            flag = true;
        }
        
       temp = Mathf.Lerp(_yYRotation, 0, _lerpRate);
       if (Mathf.Abs(temp - _yYRotation) > 0.02)
       {
           _yYRotation = temp;
           flag = true;
       }

        if(flag)
            transform.eulerAngles += new Vector3(-_yYRotation, _xRotation, 0);
    }
    #endregion
    #region Unity CallBacks
    void Start()
    {
        InputManager.MouseMovedEvent += Rotate;
    }
    void Update()
    {
      
    }
    void OnDestroy()
    {
        InputManager.MouseMovedEvent -= Rotate;
    }
    #endregion
}