using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField]
    private InputHandler inputHandler;
    [SerializeField]
    private MovementRotationHandler movementRotationHandler;

    private void Start()
    {
        if (inputHandler == null)
        {
            if (transform.TryGetComponent<InputHandler>(out InputHandler out_inputHandler))
            {
                inputHandler = out_inputHandler;
            }
        }
        if (movementRotationHandler == null)
        {
            if (transform.TryGetComponent<MovementRotationHandler>(out MovementRotationHandler out_movementRotationHandler))
            {
                movementRotationHandler = out_movementRotationHandler;
            }
        }
    }


    private void Update()
    {
        if (inputHandler == null)
        {
            return;
        }
        if (movementRotationHandler == null)
        {
            return;
        }

        transform.position += movementRotationHandler.MoveTheObject(inputHandler.horizontalValue, inputHandler.verticalValue);
        transform.rotation = movementRotationHandler.RotateTheObject(inputHandler.directionValue, inputHandler.horizontalValue, inputHandler.verticalValue);
    }
}
