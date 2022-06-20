using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public float upwardsForce = 1; 
    public float sidewaysForce = 1f;
    float maxSidewaysForce = 500;
    bool movingRight = true;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    public void Move(float loudness)
    {
        if (sidewaysForce > 200)
        {
            movingRight = false;
        }
        if (sidewaysForce < -200)
            movingRight = true;

        if (movingRight)
            sidewaysForce += Random.Range(0f, 120f * Time.deltaTime);
        else
            sidewaysForce -= Random.Range(0f, 120f * Time.deltaTime);
        Mathf.Clamp(sidewaysForce, -maxSidewaysForce, maxSidewaysForce + 1);
        rigidbody.AddForce(Vector2.up * upwardsForce * Time.deltaTime * loudness*1.5f);
        rigidbody.AddForce(Vector2.left * sidewaysForce * Time.deltaTime);
    }
}
