using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class InputHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    private bool forceJoystickInput;
    [SerializeField]
    private FixedJoystick fixedJoystick;
    public bool setupDone;

    public float horizontalValue { get; private set; }
    public float verticalValue { get; private set; }
    public Vector2 directionValue { get; private set; }

    public void Init()
    {
        setupDone = false;
        if (HudManager.instance != null)
        {
            fixedJoystick = HudManager.instance.fixedJoystick;
            setupDone = true;
        }
    }


    private void Update()
    {
        if (fixedJoystick == null || setupDone != true)
        {
            return;
        }

        if (forceJoystickInput)
        {
            GetInputDataFromJoystick();
        }
        else
        {
            GetInputDataFromKeyboard();
        }


    }

    public void GetInputDataFromJoystick()
    {
        directionValue = fixedJoystick.Direction;
        horizontalValue = fixedJoystick.Horizontal;
        verticalValue = fixedJoystick.Vertical;
    }
    public void GetInputDataFromKeyboard()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");
        verticalValue = Input.GetAxisRaw("Vertical");
        directionValue = new Vector2(horizontalValue, verticalValue);
    }
    public Vector3 MovementDirection(Vector2 directionValue)
    {
        Vector3 movementDirection = Camera.main.transform.forward * directionValue.y + Camera.main.transform.right * directionValue.x;
        movementDirection.y = 0f;
        return movementDirection;
    }
    public Vector3 MovementDirection()
    {
        Vector3 movementDirection = Camera.main.transform.forward * directionValue.y + Camera.main.transform.right * directionValue.x;
        movementDirection.y = 0f;
        return movementDirection;
    }

    // public NetworkInputData GetNetworkInputData()
    // {
    //     NetworkInputData networkInputData = new NetworkInputData();
    //     networkInputData.direction = MovementDirection(directionValue);
    //     return networkInputData;
    // }

    public void OnEnable()
    {
        var myNetworkRunner = FindObjectOfType<NetworkRunner>();
        myNetworkRunner.AddCallbacks(this);
    }

    public void OnDisable()
    {
        var myNetworkRunner = FindObjectOfType<NetworkRunner>();
        myNetworkRunner.RemoveCallbacks(this);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!setupDone)
        {
            return;
        }
        Debug.Log($"{nameof(InputHandler)} OnInput");
        NetworkInputData myInput = new NetworkInputData();

        myInput.OnScreenButtons.Set(HudButtons.JUMP_BUTTON, HudManager.instance.jumpButtonPressed);
        myInput.OnScreenButtons.Set(HudButtons.TALK_BUTTON, HudManager.instance.talkButtonPressed);
        //myInput.jumpButtonPressed = HudManager.instance.jumpButtonPressed;

        myInput.direction = MovementDirection();

        input.Set(myInput);
    }


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // Debug.Log($"{nameof(InputHandler)} OnPlayerJoined");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // Debug.Log($"{nameof(InputHandler)} OnPlayerLeft");
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        // Debug.Log($"{nameof(InputHandler)} OnInputMissing");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        // Debug.Log($"{nameof(InputHandler)} OnShutdown");
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        // Debug.Log($"{nameof(InputHandler)} OnConnectedToServer");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        // Debug.Log($"{nameof(InputHandler)} OnDisconnectedFromServer");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        // Debug.Log($"{nameof(InputHandler)} OnConnectRequest");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        // Debug.Log($"{nameof(InputHandler)} OnConnectFailed");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        // Debug.Log($"{nameof(InputHandler)} OnUserSimulationMessage");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        // Debug.Log($"{nameof(InputHandler)} OnSessionListUpdated");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        // Debug.Log($"{nameof(InputHandler)} OnCustomAuthenticationResponse");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        // Debug.Log($"{nameof(InputHandler)} OnHostMigration");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        // Debug.Log($"{nameof(InputHandler)} OnReliableDataReceived");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        // Debug.Log($"{nameof(InputHandler)} OnSceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        // Debug.Log($"{nameof(InputHandler)} OnSceneLoadStart");
    }
}
