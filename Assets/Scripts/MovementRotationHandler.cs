using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRotationHandler : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 720.0f;

    public Quaternion RotateTheObject(Transform objectToRotate, Vector3 moveDirection)
    {
        Quaternion newRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        return Quaternion.RotateTowards(objectToRotate.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    // public void RotateTheObject(Vector2 directionValue, Transform objectToRotate)
    // {

    //     if (directionValue != Vector2.zero)
    //     {
    //         Vector3 targetRotation = new Vector3(objectToRotate.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, objectToRotate.localEulerAngles.z);
    //         Quaternion newRotation = Quaternion.Euler(targetRotation);

    //         objectToRotate.rotation = Quaternion.Lerp(objectToRotate.rotation, newRotation, Time.deltaTime * rotationSpeed);
    //     }
    // }

    public Vector3 MovementDirection(Vector2 directionValue)
    {
        Vector3 movementDirection = Camera.main.transform.forward * directionValue.y + Camera.main.transform.right * directionValue.x;
        movementDirection.y = 0f;
        return movementDirection;
    }
}
