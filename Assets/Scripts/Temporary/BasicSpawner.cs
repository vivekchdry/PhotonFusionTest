using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectedToServer");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("OnConnectFailed");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("OnConnectRequest");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("OnDisconnectedFromServer");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("OnHostMigration");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Debug.Log("OnInput");
        var data = new NetworkInputData();


        // if (Input.GetKey(KeyCode.W))
        //     data.direction += Vector3.forward;

        // if (Input.GetKey(KeyCode.S))
        //     data.direction += Vector3.back;

        // if (Input.GetKey(KeyCode.A))
        //     data.direction += Vector3.left;

        // if (Input.GetKey(KeyCode.D))
        //     data.direction += Vector3.right;

        if (HudManager.instance != null)
        {
            data.direction = HudManager.instance.MovementDirection(HudManager.instance.fixedJoystick.Direction);
            data.jumpButtonPressed = HudManager.instance.jumpButtonPressed;
        }
        // if (Input.GetKey(KeyCode.Space))
        //     data.jumpButtonPressed = true;

        input.Set(data);

        // if (HudManager.instance != null)
        // {
        //     HudManager.instance.MoveCameraUsingTouchPanel();
        // }
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("OnInputMissing");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        Debug.Log("OnReliableDataReceived");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("OnSceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("OnSceneLoadStart");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("OnSessionListUpdated");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("OnShutdown");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log("OnUserSimulationMessage");
    }

    private NetworkRunner _runner;

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(20, 20, 240, 80), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(20, 120, 240, 80), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    private void Awake()
    {
        Debug.Log("Awake " + nameof(BasicSpawner));
    }

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();



    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerJoined");
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 2, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedCharacters.Add(player, networkPlayerObject);
            // if (HudManager.instance != null)
            // {
            //     HudManager.instance.cinemachineFreeLook.m_Follow = networkPlayerObject.transform.GetChild(0).transform;
            //     HudManager.instance.cinemachineFreeLook.m_LookAt = networkPlayerObject.transform.GetChild(0).transform;
            // }
            // if (networkPlayerObject.HasInputAuthority)
            // {
            //     if (HudManager.instance != null)
            //     {
            //         HudManager.instance.cinemachineFreeLook.m_Follow = networkPlayerObject.transform.GetChild(0).transform;
            //         HudManager.instance.cinemachineFreeLook.m_LookAt = networkPlayerObject.transform.GetChild(0).transform;
            //     }
            // }

        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerLeft");
        // Find and remove the players avatar
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
}
