using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private bool forceJoystickInput;
    [SerializeField]
    private FixedJoystick fixedJoystick;

    public float horizontalValue { get; private set; }
    public float verticalValue { get; private set; }
    public Vector2 directionValue { get; private set; }


    private void Update()
    {
        if (fixedJoystick == null)
        {
            return;
        }

        // if (fixedJoystick.Direction != Vector2.zero)
        //     Debug.Log($"{fixedJoystick.Horizontal} {fixedJoystick.Vertical} {fixedJoystick.Direction}");

#if UNITY_EDITOR

        if (forceJoystickInput)
        {
            directionValue = fixedJoystick.Direction;
            horizontalValue = fixedJoystick.Horizontal;
            verticalValue = fixedJoystick.Vertical;
        }
        else
        {
            horizontalValue = Input.GetAxis("Horizontal");
            verticalValue = Input.GetAxis("Vertical");
            directionValue = new Vector2(horizontalValue, verticalValue);
        }
#else

        directionValue = fixedJoystick.Direction;
        horizontalValue = fixedJoystick.Horizontal;
        verticalValue = fixedJoystick.Vertical;
#endif

    }
}
