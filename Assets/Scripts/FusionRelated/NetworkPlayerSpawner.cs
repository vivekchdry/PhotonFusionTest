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

public class NetworkPlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
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
        // Debug.Log($"{nameof(BasicSpawner)} OnSessionListUpdated");
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
