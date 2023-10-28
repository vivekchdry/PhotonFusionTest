using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
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

        if (fixedJoystick.Direction != Vector2.zero)
            Debug.Log($"{fixedJoystick.Horizontal} {fixedJoystick.Vertical} {fixedJoystick.Direction}");

        directionValue = fixedJoystick.Direction;
        horizontalValue = fixedJoystick.Horizontal;
        verticalValue = fixedJoystick.Vertical;
    }
}
