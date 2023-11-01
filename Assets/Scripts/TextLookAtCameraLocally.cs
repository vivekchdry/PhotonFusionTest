using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLookAtCameraLocally : MonoBehaviour
{
    public Transform myLocalCamera;
    public bool textLookAtCamera;
    void Update()
    {
        if (!textLookAtCamera)
        {
            return;
        }
        Vector3 targetPosition = myLocalCamera.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
        transform.Rotate(0, 180, 0);
    }
}
