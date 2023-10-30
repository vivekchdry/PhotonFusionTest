using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRotationHandler : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private float rotationSpeed = 720.0f;
    private float turnSmoothVelocity;

    public Quaternion RotateTheObject(Vector2 directionValue, float horizontalValue, float verticalValue)
    {
        // if (directionValue != Vector2.zero)
        // {
        //     float targetAngle = Mathf.Atan2(horizontalValue, verticalValue) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        //     float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
        //     return Quaternion.Euler(0, angle, 0);

        //     //return Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        // }
        // return Quaternion.identity;
        if (directionValue != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(horizontalValue, verticalValue) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y; ;
            Quaternion newRotation = Quaternion.Euler(0, targetRotation, 0);

            return Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
        return Quaternion.identity;
    }

    // public Vector3 MoveTheObject(float horizontalValue, float verticalValue)
    // {
    //     Vector3 movement = new Vector3(horizontalValue, 0, verticalValue) * moveSpeed * Time.deltaTime;
    //     return movement;
    // }
    public Vector3 MoveTheObject(Vector2 directionValue)
    {
        Vector3 movementDirection = Camera.main.transform.forward * directionValue.y + Camera.main.transform.right * directionValue.x;
        movementDirection.y = 0f;
        return movementDirection;
    }
}
