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
    //[Networked] public NetworkString<_64> playerInGameName { get; set; }
    private IEnumerator coroutineTimed;


    public void AfterSpawned()
    {
        Debug.Log("AfterSpawned PlayerCode");
        InitialSetup();
        // if (networkObject.HasInputAuthority)
        // {
        //     textLookAtCameraLocally.myLocalCamera = Camera.main.transform;
        //     textLookAtCameraLocally.textLookAtCamera = true;
        // }
        // _messagesText = myWorldText.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        // RPC_SendMessage($"Player_{networkObject.Id}");
    }

    public void InitialSetup()
    {
        //if (networkObject.HasInputAuthority)
        //{
        Debug.Log("InitialSetup PlayerCode");
        coroutineTimed = null;
        if (HudManager.instance != null)
        {
            HudManager.instance.cinemachineFreeLook.m_Follow = networkObject.transform.GetChild(0).transform;
            HudManager.instance.cinemachineFreeLook.m_LookAt = networkObject.transform.GetChild(0).transform;
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
        //RPC_SendMessage($"Player_{networkObject.Id}");
        //RPC_SendMessage($"{playerInGameName}");
        //}
    }

    // private void Update()
    // {
    //     if (networkObject.HasInputAuthority && HudManager.instance != null)
    //     {
    //         HudManager.instance.MoveCameraUsingTouchPanel();
    //     }
    //     if (!networkObject.HasInputAuthority)
    //     {
    //         Vector3 targetPosition = Camera.main.transform.position;
    //         targetPosition.y = myWorldText.transform.position.y;
    //         myWorldText.transform.LookAt(targetPosition);
    //         myWorldText.transform.Rotate(0, 180, 0);
    //     }
    // }

    public override void FixedUpdateNetwork()
    {

        if (GetInput(out NetworkInputData input))
        {


            input.direction.Normalize();
            networkCharacterControllerPrototype.Move(5 * input.direction * Runner.DeltaTime);
            // jump (check for pressed)


            if (input.OnScreenButtons.IsSet(HudButtons.JUMP_BUTTON))
            {
                networkCharacterControllerPrototype.Jump(false);
                HudManager.instance.jumpButtonPressed = false;
                if (networkObject.HasInputAuthority && HudManager.instance != null)
                {
                    input.OnScreenButtons.Set(HudButtons.JUMP_BUTTON, HudManager.instance.jumpButtonPressed);
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

            // if (input.jumpButtonPressed)
            // {
            //     input.jumpButtonPressed = false;
            //     if (networkObject.HasInputAuthority && HudManager.instance != null)
            //     {
            //         HudManager.instance.jumpButtonPressed = false;
            //     }
            //     networkCharacterControllerPrototype.Jump(false);
            // }


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
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        Debug.Log("RPC_SendMessage PlayerCode");

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
        changed.Behaviour.OnSettingPlayerInGameName();
        //playerNameInGame = changed.Behaviour.playerNameInGame;
    }
    private void OnSettingPlayerInGameName()
    {
        if (playerName == null)
        {
            playerName = myWorldCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }
        playerName.text = playerInGameName.ToString();
    }

    public IEnumerator ClearChatMessageTempSolution()
    {
        yield return new WaitForSeconds(2);
        RPC_SendMessage(string.Empty);
        coroutineTimed = null;
    }
}