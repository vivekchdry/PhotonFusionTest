using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EasyUI.Toast;

public class LobbySceneHandler : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI Text_gameTitle;
    [SerializeField]
    private TextMeshProUGUI Text_playerDisplayName;
    [SerializeField]
    private TMP_InputField Input_playerName;
    [SerializeField]
    private TMP_InputField Input_sessionName;
    [SerializeField]
    private Button Button_hostNewGame;
    [SerializeField]
    private Button Button_enterGameLobby;
    [SerializeField]
    private GameObject Panel_hostOrJoinSession;
    [SerializeField]
    private GameObject Panel_lisAllSessions;

    public string playerInGameName { get; private set; }



    private void OnEnable()
    {
        // Subscribe to the EndEdit event.
        Input_playerName.onEndEdit.AddListener(PlayerName_OnEndEdit);



    }
    private void OnDisable()
    {
        // Un-Subscribe to the EndEdit event.
        Input_playerName.onEndEdit.RemoveListener(PlayerName_OnEndEdit);
    }

    private void PlayerName_OnEndEdit(string inputText)
    {
        //Debug.Log("User finished typing: " + inputText);
        playerInGameName = inputText;
        Text_playerDisplayName.text = playerInGameName;

        if (!CheckPlayerName())
        {
            Toast.Show("Please enter <b><color=yellow>PLAYER NAME</color></b>", 1.5f, ToastPosition.BottomCenter);
            Button_hostNewGame.interactable = false;
            Button_enterGameLobby.interactable = false;

        }
        else
        {
            Button_hostNewGame.interactable = true;
            Button_enterGameLobby.interactable = true;
        }

        SwitchGameHeader();
    }

    bool CheckPlayerName()
    {
        return playerInGameName != "" && playerInGameName != null && playerInGameName != string.Empty;
    }

    public void ShowPanel_HostOrJoinSession()
    {
        Panel_hostOrJoinSession.gameObject.SetActive(true);
        Panel_lisAllSessions.gameObject.SetActive(false);
    }

    public void ShowPanel_ListAllSession()
    {
        Panel_hostOrJoinSession.gameObject.SetActive(false);
        Panel_lisAllSessions.gameObject.SetActive(true);

    }

    public void SwitchGameHeader()
    {
        if (CheckPlayerName())
        {
            Text_gameTitle.gameObject.SetActive(false);
            Text_playerDisplayName.gameObject.SetActive(true);
        }
        else
        {
            Text_gameTitle.gameObject.SetActive(true);
            Text_playerDisplayName.gameObject.SetActive(false);
        }
    }


}
