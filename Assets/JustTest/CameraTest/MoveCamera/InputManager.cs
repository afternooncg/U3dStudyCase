using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    
    
    public delegate void MouseMoved(float xMovement, float yMovement);
    
    #region Private References
    private float _xMovement;
    private float _yMovement;
    #endregion
    
    
    #region Events
    public static event MouseMoved MouseMovedEvent;
    #endregion
    
    #region Event Invoker Methods
    private static void OnMouseMoved(float xmovement, float ymovement)
    {

      //  Debug.Log(xmovement + "  " + ymovement);
        var handler = MouseMovedEvent;
        if (handler != null) handler(xmovement, ymovement);
    }
    #endregion


    #region Private Methods
    private void InvokeActionOnInput()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("hold");

            _xMovement = Input.GetAxis("Mouse X") - _xMovement;
            _yMovement = Input.GetAxis("Mouse Y") - _yMovement;
            OnMouseMoved(_xMovement, _yMovement);
        }
    }
    #endregion
    #region Unity CallBacks
    void Update()
    {
        InvokeActionOnInput();
    }
    #endregion
}
