using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
    public NetworkBool jumpButtonPressed;
    public NetworkBool talkButtonPressed;
    public NetworkButtons OnScreenButtons;
}
enum HudButtons
{
    JUMP_BUTTON = 0,
    TALK_BUTTON = 1,
}