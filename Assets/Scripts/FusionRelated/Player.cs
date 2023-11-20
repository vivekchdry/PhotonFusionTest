using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour, IAfterSpawned
{
    public NetworkCharacterControllerPrototype networkCharacterControllerPrototype;
    public NetworkObject networkObject;
    public Transform myWorldCanvas;
    public TextLookAtCameraLocally textLookAtCameraLocally;
    public InputHandler inputHandler;

    [SerializeField]
    private TextMeshProUGUI playerChat;
    [SerializeField]
    private TextMeshProUGUI playerName;
    [Networked(OnChanged = nameof(OnMessageToShowChanged))] public NetworkString<_128> messageToShow { get; set; }
    [Networked(OnChanged = nameof(OnSettingPlayerInGameName))] public NetworkString<_64> playerInGameName { get; set; }
    private IEnumerator coroutineTimed;

    public RpmCustomAvatarManager rpmCustomAvatarManager;
    [SerializeField]
    private Transform lookAtMe;


    public void AfterSpawned()
    {
        //Debug.Log("AfterSpawned PlayerCode");

        transform.name = networkObject.Id.ToString();
        Debug.Log("AfterSpawned PlayerCode " + transform.name);

        StartCoroutine(InitialSetup());

    }

    public IEnumerator InitialSetup()
    {

        //RPC_SetAvatar();
        // if (rpmCustomAvatarManager == null)
        // {
        //     rpmCustomAvatarManager = transform.GetChild(2).GetComponent<RpmCustomAvatarManager>();
        // }

        //yield return new WaitUntil(() => rpmCustomAvatarManager != null);
        yield return new WaitForSeconds(1);

        rpmCustomAvatarManager.Init();

        if (networkObject.HasInputAuthority)
        {
            //!notneedednow Debug.Log("InitialSetup PlayerCode");
            coroutineTimed = null;
            if (HudManager.instance != null)
            {
                HudManager.instance.cinemachineFreeLook.m_Follow = lookAtMe;// networkObject.transform.GetChild(0).transform;
                HudManager.instance.cinemachineFreeLook.m_LookAt = lookAtMe;// networkObject.transform.GetChild(0).transform;
            }

            myWorldCanvas = networkObject.transform.GetChild(1).transform;
            inputHandler.Init();

            textLookAtCameraLocally.myLocalCamera = Camera.main.transform;
            textLookAtCameraLocally.textLookAtCamera = true;
            if (playerChat == null)
            {
                playerChat = myWorldCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            }
            if (playerName == null)
            {
                playerName = myWorldCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            }

            RPC_SetNickname(PlayerPrefs.GetString("playerInGameName"));

        }
    }



    public override void FixedUpdateNetwork()
    {

        if (GetInput(out NetworkInputData input))
        {

            input.direction.Normalize();
            networkCharacterControllerPrototype.Move(input.direction * Runner.DeltaTime);

            if (rpmCustomAvatarManager != null)
            {
                //rpmCustomAvatarManager.ControlMoveAnimation(input.direction.magnitude * networkCharacterControllerPrototype.maxSpeed);
                rpmCustomAvatarManager.playerCurrentMagnitude = input.direction.magnitude;
                //rpmCustomAvatarManager.UpdateAnimator();
            }

            if (input.OnScreenButtons.IsSet(HudButtons.JUMP_BUTTON))
            {

                networkCharacterControllerPrototype.Jump(false);
                if (rpmCustomAvatarManager != null)
                {
                    rpmCustomAvatarManager.OnJump();
                }
                HudManager.instance.jumpButtonPressed = false;
                if (networkObject.HasInputAuthority && HudManager.instance != null)
                {
                    input.OnScreenButtons.Set(HudButtons.JUMP_BUTTON, HudManager.instance.jumpButtonPressed);
                    if (rpmCustomAvatarManager != null)
                    {
                        rpmCustomAvatarManager.TryJump();
                    }
                }

            }
            if (input.OnScreenButtons.IsSet(HudButtons.TALK_BUTTON))
            {
                //RPC_SendMessage($"Player_{networkObject.Id} Talking.");
                RPC_SendMessage($"{playerInGameName} Talking.");
                HudManager.instance.talkButtonPressed = false;
                if (networkObject.HasInputAuthority && HudManager.instance != null)
                {
                    input.OnScreenButtons.Set(HudButtons.TALK_BUTTON, HudManager.instance.talkButtonPressed);
                }
            }

        }

        if (networkObject.HasInputAuthority && HudManager.instance != null)
        {
            HudManager.instance.MoveCameraUsingTouchPanel();
        }
        if (!networkObject.HasInputAuthority)
        {
            Vector3 targetPosition = Camera.main.transform.position;
            targetPosition.y = myWorldCanvas.transform.position.y;
            myWorldCanvas.transform.LookAt(targetPosition);
            myWorldCanvas.transform.Rotate(0, 180, 0);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetAvatar()
    {
        //!notneedednow Debug.Log("RPC_SetAvatar Player");
        // if (rpmCustomAvatarManager == null)
        // {
        //     rpmCustomAvatarManager = GetComponent<RpmCustomAvatarManager>();
        // }
        rpmCustomAvatarManager.Init();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetNickname(string playerName, RpcInfo info = default)
    {
        //!notneedednow Debug.Log("RPC_SetNickname Player");
        playerInGameName = playerName;

    }




    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        //!notneedednow Debug.Log("RPC_SendMessage PlayerCode");

        messageToShow = message;

    }



    public static void OnMessageToShowChanged(Changed<Player> changed)
    {
        changed.Behaviour.OnMessageToShowChanged();
    }

    private void OnMessageToShowChanged()
    {
        if (playerChat == null)
        {
            playerChat = myWorldCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        playerChat.text = messageToShow.ToString();
        if (coroutineTimed == null)
        {
            coroutineTimed = ClearChatMessageTempSolution();
            StartCoroutine(coroutineTimed);
        }
    }

    public static void OnSettingPlayerInGameName(Changed<Player> changed)
    {
        changed.Behaviour.playerName.text = changed.Behaviour.playerInGameName.ToString();
    }


    public IEnumerator ClearChatMessageTempSolution()
    {
        yield return new WaitForSeconds(2);
        RPC_SendMessage(string.Empty);
        coroutineTimed = null;
    }

}