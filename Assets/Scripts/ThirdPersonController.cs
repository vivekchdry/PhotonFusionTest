using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Activation;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField]
    private InputHandler inputHandler;
    [SerializeField]
    private MovementRotationHandler movementRotationHandler;
    [SerializeField]
    private TouchDetect touchDetect;
    [SerializeField]
    private CinemachineFreeLook cinemachineFreeLook;

    [SerializeField]
    private float sensitivityX = 0.05f;
    [SerializeField]
    private float sensitivityY = -0.05f;


    [SerializeField]
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    public Button jumpButton;
    public Button talkButton;
    public float _rotationSpeed;

    private void OnEnable()
    {
        if (controller == null)
        {
            if (transform.TryGetComponent<CharacterController>(out CharacterController out_controller))
            {
                controller = out_controller;
            }
        }

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

        if (jumpButton != null)
        {
            jumpButton.onClick.AddListener(CalcutalePlayerJump);
        }
        if (talkButton != null)
        {
            talkButton.onClick.AddListener(MakePlayerTalk);
        }

    }

    private void OnDisable()
    {
        if (jumpButton != null)
        {
            jumpButton.onClick.RemoveListener(CalcutalePlayerJump);
        }
        if (talkButton != null)
        {
            talkButton.onClick.RemoveListener(MakePlayerTalk);
        }
    }


    void Update()
    {

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -0.5f;
        }


        MoveThePlayer();

        MakePlayerJumpOrFallAFterJump();

        MoveCameraUsingTouchPanel();

#if UNITY_EDITOR
        if (Input.GetButtonDown("Jump"))
        {
            CalcutalePlayerJump();
        }
#endif
    }

    private void MakePlayerJumpOrFallAFterJump()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void CalcutalePlayerJump()
    {
        if (groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }

    private void MoveThePlayer()
    {
        Vector3 movement = movementRotationHandler.MovementDirection(inputHandler.directionValue);
        controller.Move(movement * Time.deltaTime * playerSpeed);

        RotateThePlayer(movement);

        // if (movement != Vector3.zero)
        // {
        //     gameObject.transform.forward = movement;
        // }

    }

    private void RotateThePlayer(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            transform.rotation = movementRotationHandler.RotateTheObject(transform, movement);
        }
    }

    private void MoveCameraUsingTouchPanel()
    {
        cinemachineFreeLook.m_XAxis.Value += touchDetect.TouchDistance.x * 200 * sensitivityX * Time.deltaTime;
        cinemachineFreeLook.m_YAxis.Value += touchDetect.TouchDistance.y * sensitivityY * Time.deltaTime;
    }

    private void MakePlayerTalk()
    {
        Debug.Log($"I'm Talking.");
    }
}
