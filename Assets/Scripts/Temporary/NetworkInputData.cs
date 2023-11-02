using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
    public NetworkBool jumpButtonPressed;
    public NetworkBool talkButtonPressed;
}