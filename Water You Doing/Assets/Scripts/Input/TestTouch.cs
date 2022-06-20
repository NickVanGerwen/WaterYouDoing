using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestTouch : MonoBehaviour
{
    private TouchController controls;
    private ArmMechanic armController = null;

    private void Awake()
    {
        controls = new TouchController();
        armController = gameObject.AddComponent<ArmMechanic>();
        controls.Touch.TouchPosition.performed += ctx => IsTouch();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void IsTouch()
    {
        if(armController == null)
        {
            Debug.Log("Controller is null");
        }
        Debug.Log("Touch");
        //Connectie met ArmMechanic.cs
        //armController.ScreenIsTouch();
    }
}
