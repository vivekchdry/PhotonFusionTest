using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    public static HudManager instance;

    public TouchDetect touchDetect;
    public FixedJoystick fixedJoystick;
    public CinemachineFreeLook cinemachineFreeLook;
    public Button jumpButton;
    public Button talkButton;
    public Button hostDemoGame;
    public Button joinDemoGame;

    public bool jumpButtonPressed;
    public bool talkButtonPressed;

    public float sensitivityX = 0.005f;
    public float sensitivityY = -0.005f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }


#if UNITY_EDITOR
        sensitivityX = 0.1f;
        sensitivityY = -0.1f;
#else
        sensitivityX = 0.005f;
        sensitivityY = -0.005f;
#endif

        jumpButton.onClick.AddListener(() => { jumpButtonPressed = true; });
        talkButton.onClick.AddListener(() => { talkButtonPressed = true; });


    }



    public Vector3 MovementDirection(Vector2 directionValue)
    {
        Vector3 movementDirection = Camera.main.transform.forward * directionValue.y + Camera.main.transform.right * directionValue.x;
        movementDirection.y = 0f;
        return movementDirection;
    }
    public void MoveCameraUsingTouchPanel()
    {
        cinemachineFreeLook.m_XAxis.Value += touchDetect.TouchDistance.x * 200 * sensitivityX * Time.deltaTime;
        cinemachineFreeLook.m_YAxis.Value += touchDetect.TouchDistance.y * sensitivityY * Time.deltaTime;
    }
}
