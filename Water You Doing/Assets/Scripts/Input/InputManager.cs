using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;

    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;

    private TouchController touchContoller;

    private void Awake()
    {
        touchContoller = new TouchController();
    }

    private void OnEnable()
    {
        touchContoller.Enable();
    }

    private void OnDisable()
    {
        touchContoller.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        touchContoller.Touch.TouchPress.started += ctx => StartTouch(ctx);
        touchContoller.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch started " + touchContoller.Touch.TouchPosition.ReadValue<Vector2>());
        if(OnStartTouch != null)
        {
            OnStartTouch(touchContoller.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
        }
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch ended");
        if (OnEndTouch != null)
        {
            OnEndTouch(touchContoller.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
        }
    }
}
