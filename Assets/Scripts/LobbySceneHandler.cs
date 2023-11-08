using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EasyUI.Toast;

public class LobbySceneHandler : MonoBehaviour
{

    public TextMeshProUGUI Text_gameTitle;
    public TextMeshProUGUI Text_playerDisplayName;

    public TMP_InputField Input_playerName;
    public TMP_InputField Input_sessionName;
    public Button Button_hostNewGame;
    public Button Button_enterGameLobby;
    public GameObject Panel_hostOrJoinSession;
    public GameObject Panel_lisAllSessions;


    public GameObject LoadingAnimationObject;
    private NetworkRunnerHandler networkRunnerHandler;



    private void OnEnable()
    {
        // Subscribe to the EndEdit event.
        Input_playerName.onEndEdit.AddListener(PlayerName_OnEndEdit);
        Button_enterGameLobby.onClick.AddListener(ShowPanel_ListAllSession);

    }
    private void OnDisable()
    {
        // Un-Subscribe to the EndEdit event.
        Input_playerName.onEndEdit.RemoveListener(PlayerName_OnEndEdit);
        Button_enterGameLobby.onClick.RemoveListener(ShowPanel_ListAllSession);
    }

    private void PlayerName_OnEndEdit(string inputText)
    {
        //Debug.Log("User finished typing: " + inputText);
        networkRunnerHandler.playerInGameName = inputText;
        Text_playerDisplayName.text = networkRunnerHandler.playerInGameName;

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

        //SwitchGameHeader();
    }

    bool CheckPlayerName()
    {
        return networkRunnerHandler.playerInGameName != "" && networkRunnerHandler.playerInGameName != null && networkRunnerHandler.playerInGameName != string.Empty;
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

    public void Init()
    {
        Text_gameTitle.gameObject.SetActive(true);
        Text_playerDisplayName.gameObject.SetActive(false);
        Button_hostNewGame.interactable = false;
        Button_enterGameLobby.interactable = false;
        Panel_hostOrJoinSession.gameObject.SetActive(true);
        Panel_lisAllSessions.gameObject.SetActive(false);
        Text_playerDisplayName.text = string.Empty;
        //playerInGameName = string.Empty;
        networkRunnerHandler = FindAnyObjectByType<NetworkRunnerHandler>();

        networkRunnerHandler.playerInGameName = string.Empty;

    }

    public IEnumerator StartFakeLoading(bool showAnimation)
    {
        yield return new WaitForSeconds(0.1f);
        LoadingAnimationObject.SetActive(showAnimation);
        // Text_showLoading.text = "<b><uppercase>Loading</b></uppercase>";
        // yield return new WaitForSeconds(0.5f);
        // Text_showLoading.text = "<b><uppercase>Loading</b></uppercase><size=150%>.</size>";
        // yield return new WaitForSeconds(0.5f);
        // Text_showLoading.text = "<b><uppercase>Loading</b></uppercase><size=150%>..</size>";
        // yield return new WaitForSeconds(0.5f);
        // Text_showLoading.text = "<b><uppercase>Loading</b></uppercase><size=150%>...</size>";
    }

}
