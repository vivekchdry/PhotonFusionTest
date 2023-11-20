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
    [SerializeField]
    private LobbySceneHandler lobbySceneHandler;
    [SerializeField]
    private SessionListHandler sessionListHandler;

    private NetworkObject networkPlayerObject;

    public string playerInGameName;



    private void Awake()
    {
        //!notneedednow Debug.Log("Awake " + nameof(NetworkRunnerHandler));

        //networkRunner = gameObject.AddComponent<NetworkRunner>();
        lobbySceneHandler = FindAnyObjectByType<LobbySceneHandler>();
        sessionListHandler = FindAnyObjectByType<SessionListHandler>();
        if (lobbySceneHandler != null)
        {
            //StartCoroutine(lobbySceneHandler.StartFakeLoading(true));
            StartCoroutine(lobbySceneHandler.StartFakeLoading("Connecting", true));

            lobbySceneHandler.Button_createNewSession.onClick.AddListener(() =>
            {
                lobbySceneHandler.ControlCreateSessionPanel(true);
            });
            lobbySceneHandler.Button_hostNewGame.onClick.AddListener(() =>
            {
                CreateGame(lobbySceneHandler.customSessionName, "GameScene", lobbySceneHandler.customSessionPlayerCount);
            });

            lobbySceneHandler.Button_enterSessionBrowser.onClick.AddListener(() =>
            {
                lobbySceneHandler.ShowPanel_ListAllSession();
            });

            if (sessionListHandler != null)
            {
                sessionListHandler.goback.onClick.AddListener(() =>
                {
                    lobbySceneHandler.ShowPanel_HostOrJoinSession();
                });
            }
        }
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
            var clientTask = InitializeNetworkRunner(networkRunner, GameMode.AutoHostOrClient, "TestSession", SceneManager.GetActiveScene().buildIndex, NetAddress.Any(), null, 10);

            //var networkPlayerSpawner = Instantiate(Prefab_networkPlayerSpawner);
        }
    }

    protected virtual async Task InitializeNetworkRunner(NetworkRunner networkRunner, GameMode gameMode, string sessionName, SceneRef scene, NetAddress address, Action<NetworkRunner> initialized, int maxPlayerCount)
    {
        //!notneedednow Debug.Log("InitializeNetworkRunner " + nameof(NetworkRunnerHandler));
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
            PlayerCount = maxPlayerCount
        });

        if (result.Ok)
        {
            //!notneedednow Debug.Log("OK StartGame " + nameof(NetworkRunnerHandler));

        }
        else
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }
    }

    INetworkSceneManager GetSceneManager(NetworkRunner networkRunner)
    {
        //!notneedednow Debug.Log("GetSceneManager " + nameof(NetworkRunnerHandler));
        var sceneManager = networkRunner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();
        if (sceneManager == null)
        {
            //!notneedednow Debug.Log("SceneManager Null " + nameof(NetworkRunnerHandler));
            sceneManager = networkRunner.gameObject.AddComponent<NetworkSceneManagerDefault>();

        }
        return sceneManager;
    }

    [ContextMenu("OnJoinLobby")]
    public void OnJoinLobby()
    {
        //!notneedednow Debug.Log("OnJoinLobby " + nameof(NetworkRunnerHandler));
        var clientTask = JoinLobby();
        //var networkPlayerSpawner = Instantiate(Prefab_networkPlayerSpawner);
    }

    public async Task JoinLobby()
    {
        //!notneedednow Debug.Log("JoinLobby " + nameof(NetworkRunnerHandler));
        string lobbyId = "CustomLobbyId_1";
        var result = await networkRunner.JoinSessionLobby(SessionLobby.Custom, lobbyId);

        if (result.Ok)
        {
            //!notneedednow Debug.Log($"Successfully Joined Lobby {lobbyId}");
            if (lobbySceneHandler != null)
            {
                //StartCoroutine(lobbySceneHandler.StartFakeLoading(false));
                StartCoroutine(lobbySceneHandler.StartFakeLoading("Connected", false));
                lobbySceneHandler.holderPrimaryParent.SetActive(true);
                lobbySceneHandler.Init();
            }
        }
        else
        {
            Debug.Log($"Unable to Join Lobby {lobbyId} {result.ShutdownReason}");
            StartCoroutine(lobbySceneHandler.StartFakeLoading("Something went wrong.", true));
        }
    }

    public void CreateGame(string sessionName, string sceneName, int maxPlayerCount)
    {
        PlayerPrefs.SetString("playerInGameName", playerInGameName);
        //!notneedednow Debug.Log($"CreateGame {sessionName} {sceneName} {nameof(NetworkRunnerHandler)}");
        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Host, sessionName, SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}"), NetAddress.Any(), null, maxPlayerCount);
        //var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Host, sessionName, SceneManager.GetActiveScene().buildIndex, NetAddress.Any(), null);
    }

    public void JoinGame(SessionInfo sessionInfo)
    {
        PlayerPrefs.SetString("playerInGameName", playerInGameName);
        Debug.Log($"JoinGame {sessionInfo.Name} {nameof(NetworkRunnerHandler)}");
        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Client, sessionInfo.Name, SceneManager.GetActiveScene().buildIndex, NetAddress.Any(), null, sessionInfo.MaxPlayers);
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
        //!notneedednow Debug.Log($"{nameof(NetworkRunnerHandler)} OnSceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        //!notneedednow Debug.Log($"{nameof(NetworkRunnerHandler)} OnSceneLoadStart");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //!notneedednow Debug.Log($"{nameof(NetworkRunnerHandler)} OnSessionListUpdated");
        if (sessionListHandler == null)
        {
            return;
        }
        sessionListHandler.ClearList();

        if (sessionList.Count > 0)
        {
            foreach (SessionInfo item in sessionList)
            {
                Debug.Log($"{nameof(NetworkRunnerHandler)} {item.Name}");
                if (item.PlayerCount >= item.MaxPlayers)
                {
                    item.IsOpen = false;
                }
                else
                {
                    item.IsOpen = true;
                }
                sessionListHandler.AddToList(item);
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
    [SerializeField] private NetworkPrefabRef playerRpmPrefab;
    private Dictionary<PlayerRef, NetworkObject> allSpawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    //public delegate void OnBeforeSpawned(NetworkRunner runner, NetworkObject obj);


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

        Debug.Log($"{nameof(NetworkRunnerHandler)} OnPlayerJoined {player.PlayerId} {runner.UserId}");

        Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 2, 0);
        networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
        //networkPlayerObject.name = player.PlayerId.ToString();

        //NetworkObject noRpmPrefabObject = runner.Spawn(playerRpmPrefab, spawnPosition, Quaternion.identity, player);

        //RpmCustomAvatarManager customRpm = noRpmPrefabObject.transform.GetComponent<RpmCustomAvatarManager>();

        //customRpm.playerRef = player;
        //customRpm.name = player.PlayerId.ToString();

        // customRpm.mainParent = networkPlayerObject.transform;
        // customRpm.networkObject = noRpmPrefabObject;
        // networkPlayerObject.GetComponent<Player>().rpmCustomAvatarManager = customRpm;
        // noRpmPrefabObject.transform.parent = networkPlayerObject.transform;
        // noRpmPrefabObject.transform.localPosition = Vector3.zero;

        // if (player == runner.LocalPlayer)
        // {
        //     Debug.Log($"{nameof(NetworkRunnerHandler)} LocalPlayer");
        // }
        // else
        // {
        //     Debug.Log($"{nameof(NetworkRunnerHandler)} NOT LocalPlayer");
        // }


        allSpawnedCharacters.Add(player, networkPlayerObject);

    }



    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"{nameof(NetworkRunnerHandler)} OnPlayerLeft");
        //Find and remove the players avatar
        if (allSpawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            allSpawnedCharacters.Remove(player);
            networkPlayerObject = null;
        }
    }



}
