using UnityEngine;
using System.Collections;


public class CameraController : MonoBehaviour
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

        _xRotation = Mathf.Lerp(_xRotation, 0, _lerpRate);
        _yYRotation = Mathf.Lerp(_yYRotation, 0, _lerpRate);
        transform.eulerAngles += new Vector3(0, _xRotation, -_yYRotation);
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