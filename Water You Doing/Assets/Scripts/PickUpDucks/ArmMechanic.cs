using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArmMechanic : MonoBehaviour
{
    [SerializeField] private Vector3 h_endPoint = Vector3.zero;
    [SerializeField] private Vector3 v_endPoint = Vector3.zero;
    private Vector3 startPosition = Vector3.zero;
    private Vector3 endVerticalPosistion = Vector3.zero;
    private Vector3 objPosition = Vector3.zero;
    public Sprite open;
    private bool moveTowardsHorizontalEndPoint = true;
    private bool moveTowardsVerticalEndPoint = true;
    private bool horizontalMove = true;
    private bool isTouched = false;
    private bool isSoapPickedUp = false;
    private float h_interpolateValue = 0.0f;
    private float v_interpolateValue = 0.0f;

    public float verticalMoveSpeed = 1f;
    public float horizontalMoveSpeed = 0.5f;

    //used for scaling difficulty
    float difficulty;
    float difficultyScaling = 0.2f;

    public GameObject gameManagement;

    private TouchController controls;
    bool isFirstTouch = true;

    private void Awake()
    {
        //gameManagement = GameObject.FindGameObjectWithTag("gamemanager");
        this.startPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        controls = new TouchController();
        //controls.Touch.TouchPosition.performed += ctx => ScreenIsTouch();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        difficulty = GlobalGameManager.Instance.difficulty;

        this.startPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        this.h_endPoint = this.GetRelativeEndpoint();
    }

    // Update is called once per frame
    void Update()
    {
        //Is er gedrukt op het scherm, laat dan object verticaal gaan
        if (isTouched)
        {
            //Moet het object naar beneden toe
            if (this.moveTowardsVerticalEndPoint)
            {
                //Is object niet op verticaal eindpunt, breng dan object verder naar beneden
                if (!this.ObjectIsAtVerticalEndPoint())
                {
                    horizontalMove = false;
                    this.MoveObjectTowardsVerticalEndPoint();
                    this.IncreaseV_InterpolateValue();
                }
                //Object is bij eindpunt en reset alles waardoor object weer naar boven kan
                else
                {
                    this.moveTowardsVerticalEndPoint = false;
                    this.ResetV_InterpolateValue();
                }
            }
            //Object moet naar boven toe
            else
            {
                //Is object niet op verticaal startpunt, breng dan object verder naar boven
                if (!this.ObjectIsAtVerticalStartPoint())
                {
                    this.MoveObjectTowardsVerticalStartPoint();
                    this.IncreaseV_InterpolateValue();
                }
                //Object is bij startpunt en reset alles waardoor object weer horizontaal kan gaan
                else
                {
                    Debug.Log(isSoapPickedUp);
                    if (isSoapPickedUp)
                    {
                        GetComponent<SpriteRenderer>().sprite = open;
                        this.ResetBools();
                        this.ResetV_InterpolateValue();
                    }
                    else
                    {
                        this.ResetBools();
                        this.ResetV_InterpolateValue();
                        //gameManagement.GetComponent<GameManagement>().LoseGame();                       //Game over screen
                        GlobalGameManager.Instance.LoseGame();
                    }
                }
            }
        }
        if (horizontalMove)
        {
            if (this.moveTowardsHorizontalEndPoint)
            {
                //Bekijk of het object niet op het eindpunt is
                if (!this.ObjectIsAtHorizontalEndPoint())
                {
                    this.MoveObjectTowardsHorizontalEndPoint();
                    this.IncreaseH_InterpolateValue();
                }
                else
                {
                    this.moveTowardsHorizontalEndPoint = false;
                    this.ResetH_InterpolateValue();
                }
            }
            else
            {
                if (!this.ObjectIsAtHorizontalStartPoint())
                {
                    this.MoveObjectTowardsHorizontalStartPoint();
                    this.IncreaseH_InterpolateValue();
                }
                else
                {
                    this.moveTowardsHorizontalEndPoint = true;
                    this.ResetH_InterpolateValue();
                }
            }
        }
    }


    public void ScreenIsTouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isFirstTouch)
            {
                isFirstTouch = false;
            }
            else
            {
                isTouched = true;
                this.objPosition = this.GetObjectPosition();
                this.endVerticalPosistion = this.GetRelativeVerticalEndpoint();
            }
        }
    }
    public void ResetBools()
    {
        this.moveTowardsVerticalEndPoint = true;
        this.horizontalMove = true;
        isSoapPickedUp = false;
        isTouched = false;
    }

    public void ResetStartPosition()
    {

    }

    public bool IsSoapPickedUp()
    {
        isSoapPickedUp = true;
        return isSoapPickedUp;
    }

    #region Move object
    //Verplaats het object richting het eindpunt.
    private void MoveObjectTowardsHorizontalEndPoint()
    {
        Vector2 newPos = this.LerpPositions(this.startPosition, this.h_endPoint, h_interpolateValue);
        this.transform.position = newPos;
    }

    //Zet het object terug naar het startpunt.
    private void MoveObjectTowardsHorizontalStartPoint()
    {
        Vector3 newPos = this.LerpPositions(this.h_endPoint, this.startPosition, h_interpolateValue);
        this.transform.position = newPos;
    }

    //Verplaats het object verticaal naar beneden
    private void MoveObjectTowardsVerticalEndPoint()
    {
        Vector2 newPos = this.LerpPositions(this.objPosition, this.endVerticalPosistion, v_interpolateValue);
        this.transform.position = newPos;
    }

    //Verplaats het object verticaal naar boven
    private void MoveObjectTowardsVerticalStartPoint()
    {
        Vector2 newPos = this.LerpPositions(this.endVerticalPosistion, this.objPosition, v_interpolateValue);
        if (v_interpolateValue > 1.1)
        {
            Debug.Log("Handmatige reset");
            newPos = new Vector2((float)-11.95, (float)8.43);
        }
        this.transform.position = newPos;
    }
    #endregion

    #region Start- endposition
    //Staat het object bij het startpunt?
    private bool ObjectIsAtHorizontalStartPoint()
    {
        return this.transform.position == this.startPosition;
    }

    //Heeft het object het eindpunt gehaald?
    private bool ObjectIsAtHorizontalEndPoint()
    {
        return this.transform.position == this.h_endPoint;
    }

    //Staat het object bij het startpunt?
    private bool ObjectIsAtVerticalStartPoint()
    {
        //Hier nog naar kijken
        return this.transform.position.y >= this.startPosition.y;
    }

    //Heeft het object het eindpunt gehaald?
    private bool ObjectIsAtVerticalEndPoint()
    {
        return this.transform.position == this.endVerticalPosistion;
    }

    //Bereken eindpunt vanuit start positie.
    private Vector2 GetRelativeEndpoint()
    {
        return this.startPosition + this.h_endPoint;
    }

    //Bereken eindpunt vanuit start positie.
    private Vector3 GetRelativeVerticalEndpoint()
    {
        Vector3 relativePosition = this.objPosition;
        relativePosition.y = this.objPosition.y + this.v_endPoint.y;
        return relativePosition;
    }

    private Vector3 GetObjectPosition()
    {
        return this.transform.position;
    }
    #endregion

    #region Interpolation
    //Bereken nieuwe positie object
    private Vector3 LerpPositions(Vector3 start, Vector3 end, float t)
    {
        return new Vector3(
            Mathf.Lerp(start.x, end.x, t),
            Mathf.Lerp(start.y, end.y, t),
            Mathf.Lerp(start.z, end.z, t)
        );
    }

    //Bereken de horizontaal interpolateValue.
    private void IncreaseH_InterpolateValue()
    {
        this.h_interpolateValue += Time.deltaTime * (horizontalMoveSpeed + Mathf.Clamp(difficulty * difficultyScaling, 0, horizontalMoveSpeed * 3));
    }

    //Bereken de verticaal interpolateValue.
    private void IncreaseV_InterpolateValue()
    {
        this.v_interpolateValue += Time.deltaTime * verticalMoveSpeed;
    }

    //Zet interpolateValue terug naar 0.
    private void ResetH_InterpolateValue()
    {
        this.h_interpolateValue = 0;
    }

    //Zet interpolateValue terug naar 0.
    private void ResetV_InterpolateValue()
    {
        this.v_interpolateValue = 0;
    }
    #endregion
}
