using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BalancingScript : MonoBehaviour
{
    private SoapBalancingControlMaster controlMaster;
    private Accelerometer accelerometer;

    private float xRotation;
    private float characterZRotation;
    private bool slowFallActive;

    bool gameOver = false;

    [Range(0, 1)]
    public float MinimumRotation; //should be between 0 and 1

    [Range(0, 1)]
    public float DeathRotationValue;
    [Range(0, 5)]
    public float RandomRotationOffsetRange;
    public float RotateSpeed;
    public float FallSpeed;
    public GameObject Character;
    public AudioSource audioSource;

    public GameManagement gameManagement;

    private void Awake()
    {
        controlMaster = new SoapBalancingControlMaster();
        if (UnityEngine.InputSystem.Accelerometer.current != null)
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Accelerometer.current);
        }

        SetRandomFallDirection();

    }

    private void Update()
    {
        if (UnityEngine.InputSystem.Accelerometer.current != null)
        {
            Vector3 Rotation = UnityEngine.InputSystem.Accelerometer.current.acceleration.ReadValue();
            xRotation = Rotation.x;
            if (xRotation >= MinimumRotation || xRotation <= -MinimumRotation)
            {
                //Debug.Log(xRotation);
                if (Character != null)
                {
                    Character.transform.Rotate(0, 0, -(xRotation * RotateSpeed * Time.deltaTime));
                }
                else Debug.Log("Could not find character");
            }
        }

        if (Character.transform.rotation.z >= DeathRotationValue || Character.transform.rotation.z <= -DeathRotationValue)
        {
            if (!gameOver)
            {
                gameOver = true;
                GlobalGameManager.Instance.LoseGame();
            }
        }

        string falldirection = DetermineFallDirection();
        //Debug.Log(falldirection);

        if (Character != null)
        {
            if (falldirection == "Right" && slowFallActive != true)
            {
                Character.transform.Rotate(0, 0, (FallSpeed * Time.deltaTime));
            }

            else if (falldirection == "Left" && slowFallActive != true)
            {
                Character.transform.Rotate(0, 0, -(FallSpeed * Time.deltaTime));
            }
        }
        PitchSound();
    }

    private void PitchSound()
    {
        audioSource.pitch = Mathf.Abs(Character.transform.rotation.z) / DeathRotationValue * 2;
    }

    private string DetermineFallDirection()
    {
        characterZRotation = Character.transform.rotation.z;

        if (characterZRotation == 0)
        {
            SetRandomFallDirection();
            return "Set fall direction";
        }

        if (characterZRotation > 0) return "Right";
        else if (characterZRotation < 0) return "Left";
        else return "Error: no fall direction";
    }

    private void SetRandomFallDirection()
    {
        //float minoffset = -RandomRotationOffsetRange;
        //float maxoffset = RandomRotationOffsetRange;

        //float rotationoffset = 0;
        ////ensure that rotation offset never ends up as 0
        //while (rotationoffset == 0)
        //{
        //    rotationoffset = Random.Range(maxoffset, minoffset);
        //}

        //if (Character != null)
        //{
        //    Character.transform.Rotate(0, 0, rotationoffset);
        //}
        //Debug.Log($"set rotation offset to: {rotationoffset}");

        //return Character.transform.rotation.x;

        if (Character != null)
        {
            int randomdirenctionint = Random.Range(1, 3); // max is exclusive so this returns eiter 1 or 2.

            slowFallActive = true;

            for (int i = 0; i < 20; i++)
            {
                if (randomdirenctionint == 1) Character.transform.Rotate(0, 0, (FallSpeed / 2 * Time.deltaTime));
                if (randomdirenctionint == 1) Character.transform.Rotate(0, 0, -(FallSpeed / 2 * Time.deltaTime));
            }
            slowFallActive = false;

        }
    }

    private void OnEnable()
    {
        controlMaster.Enable();
    }

    private void OnDisable()
    {
        controlMaster.Disable();
    }
}
