using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{

    [SerializeField]
    private NetworkPlayerSpawner Prefab_networkPlayerSpawner;
    [SerializeField]
    private NetworkRunner networkRunner;

    // private void OnGUI()
    // {
    //     if (networkRunner == null)
    //     {
    //         if (GUI.Button(new Rect(20, 20, 240, 80), "Host"))
    //         {
    //             StartGame(GameMode.Host);
    //         }
    //         if (GUI.Button(new Rect(20, 120, 240, 80), "Join"))
    //         {
    //             StartGame(GameMode.Client);
    //         }
    //     }
    // }

    private void Awake()
    {
        Debug.Log("Awake " + nameof(NetworkRunnerHandler));

        //networkRunner = gameObject.AddComponent<NetworkRunner>();
    }

    private void Start()
    {
        OnJoinLobby();
    }

    [ContextMenu("CustomStart")]
    private void CustomStart()
    {
        Debug.Log("Start " + nameof(NetworkRunnerHandler));

        // networkRunner = gameObject.AddComponent<NetworkRunner>();
        // networkRunner.name = "Network Runner";


        if (SceneManager.GetActiveScene().name != "GameScene")
        {
            Debug.Log("LobbyScene " + nameof(NetworkRunnerHandler));
            var clientTask = InitializeNetworkRunner(networkRunner, GameMode.AutoHostOrClient, "TestSession", SceneManager.GetActiveScene().buildIndex, NetAddress.Any(), null);

            //var networkPlayerSpawner = Instantiate(Prefab_networkPlayerSpawner);
        }
    }

    protected virtual async Task InitializeNetworkRunner(NetworkRunner networkRunner, GameMode gameMode, string sessionName, SceneRef scene, NetAddress address, Action<NetworkRunner> initialized)
    {
        Debug.Log("InitializeNetworkRunner " + nameof(NetworkRunnerHandler));
        var sceneManager = GetSceneManager(networkRunner);
        networkRunner.ProvideInput = true;

        var result = await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = sessionName,
            CustomLobbyName = "CustomLobbyId",
            Scene = scene,
            Address = address,
            Initialized = initialized,
            SceneManager = sceneManager,

        });

        if (result.Ok)
        {
            Debug.Log("OK StartGame " + nameof(NetworkRunnerHandler));
        }
        else
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }
    }

    INetworkSceneManager GetSceneManager(NetworkRunner networkRunner)
    {
        Debug.Log("GetSceneManager " + nameof(NetworkRunnerHandler));
        var sceneManager = networkRunner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();
        if (sceneManager == null)
        {
            Debug.Log("SceneManager Null " + nameof(NetworkRunnerHandler));
            sceneManager = networkRunner.gameObject.AddComponent<NetworkSceneManagerDefault>();

        }
        return sceneManager;
    }

    [ContextMenu("OnJoinLobby")]
    public void OnJoinLobby()
    {
        Debug.Log("OnJoinLobby " + nameof(NetworkRunnerHandler));
        //var clientTask = JoinLobby();
        //var networkPlayerSpawner = Instantiate(Prefab_networkPlayerSpawner);
    }

    public async Task JoinLobby()
    {
        Debug.Log("JoinLobby " + nameof(NetworkRunnerHandler));
        string lobbyId = "CustomLobbyId";
        var result = await networkRunner.JoinSessionLobby(SessionLobby.Custom, lobbyId);

        if (result.Ok)
        {
            Debug.Log($"Successfully Joined Lobby {lobbyId}");
        }
        else
        {
            Debug.Log($"Unable to Join Lobby {lobbyId} {result.ShutdownReason}");
        }
    }

    public void CreateGame(string sessionName, string sceneName)
    {
        Debug.Log($"CreateGame {sessionName} {sceneName} {nameof(NetworkRunnerHandler)}");
        //var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Host, sessionName, SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}"), NetAddress.Any(), null);
        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Host, sessionName, SceneManager.GetActiveScene().buildIndex, NetAddress.Any(), null);
    }

    public void JoinGame(SessionInfo sessionInfo)
    {
        Debug.Log($"JoinGame {sessionInfo.Name} {nameof(NetworkRunnerHandler)}");
        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Client, sessionInfo.Name, SceneManager.GetActiveScene().buildIndex, NetAddress.Any(), null);
    }

    public void HostButtonTemp()
    {
        Debug.Log($"HostButtonTemp");
        CreateGame("TestSession 1", "LobbyScene");
    }
    public void JoinButtonTemp()
    {
        Debug.Log($"JoinButtonTemp");
        OnJoinLobby();
    }


    public void OnConnectedToServer(NetworkRunner runner)
    {
        // Debug.Log($"{nameof(BasicSpawner)} OnConnectedToServer");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        // Debug.Log($"{nameof(BasicSpawner)} OnConnectFailed");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        // Debug.Log($"{nameof(BasicSpawner)} OnConnectRequest");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        // Debug.Log($"{nameof(BasicSpawner)} OnCustomAuthenticationResponse");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        // Debug.Log($"{nameof(BasicSpawner)} OnDisconnectedFromServer");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        // Debug.Log($"{nameof(BasicSpawner)} OnHostMigration");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // Debug.Log($"{nameof(BasicSpawner)} OnInput");
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        // Debug.Log($"{nameof(BasicSpawner)} OnInputMissing");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        // Debug.Log($"{nameof(BasicSpawner)} OnReliableDataReceived");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        // Debug.Log($"{nameof(BasicSpawner)} OnSceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        // Debug.Log($"{nameof(BasicSpawner)} OnSceneLoadStart");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log($"{nameof(NetworkRunnerHandler)} OnSessionListUpdated");

        if (sessionList.Count > 0)
        {
            foreach (SessionInfo item in sessionList)
            {
                Debug.Log($"{nameof(NetworkRunnerHandler)} {item.Name}");
            }
        }

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        // Debug.Log($"{nameof(BasicSpawner)} OnShutdown");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        // Debug.Log($"{nameof(BasicSpawner)} OnUserSimulationMessage");
    }

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();



    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"{nameof(NetworkRunnerHandler)} OnPlayerJoined");

        // if (runner.IsServer)
        // {
        //     Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 2, 0);
        //     NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
        //     networkPlayerObject.transform.name = networkPlayerObject.Name;
        //     _spawnedCharacters.Add(player, networkPlayerObject);
        // }

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"{nameof(NetworkRunnerHandler)} OnPlayerLeft");
        // Find and remove the players avatar
        // if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        // {
        //     runner.Despawn(networkObject);
        //     _spawnedCharacters.Remove(player);
        // }
    }

}
