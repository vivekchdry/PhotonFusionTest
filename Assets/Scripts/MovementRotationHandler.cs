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



    public Quaternion RotateTheObject(Vector2 directionValue, float horizontalValue, float verticalValue)
    {
        if (directionValue != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(horizontalValue, verticalValue) * Mathf.Rad2Deg;
            Quaternion newRotation = Quaternion.Euler(0, targetRotation, 0);

            return transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
        return Quaternion.identity;
    }

    public Vector3 MoveTheObject(float horizontalValue, float verticalValue)
    {
        Vector3 movement = new Vector3(horizontalValue, 0, verticalValue) * moveSpeed * Time.deltaTime;
        return movement;
    }
}
