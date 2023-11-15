using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EasyUI.Toast;
using System;
using ReadyPlayerMe;

public class LobbySceneHandler : MonoBehaviour
{
    private NetworkRunnerHandler networkRunnerHandler;
    [SerializeField]
    private TextMeshProUGUI playerCountSliderDisplay;

    public TextMeshProUGUI Text_gameTitle;
    public TextMeshProUGUI Text_playerDisplayName;

    public TMP_InputField Input_playerName;
    public TMP_InputField Input_sessionName;
    public Button Button_createNewSession;
    public Button Button_hostNewGame;
    public Button Button_enterSessionBrowser;
    public Slider Slider_playerCount;
    public GameObject Panel_hostOrJoinSession;
    public GameObject Panel_lisAllSessions;

    public GameObject LoadingAnimationObject;
    public GameObject CreateSessionObject;

    public string customSessionName { get; private set; }
    public int customSessionPlayerCount { get; private set; }
    public LoadingManager loadingManager;
    public GameObject holderPrimaryParent;

    private void OnEnable()
    {
        // Subscribe to the EndEdit event.
        Input_playerName.onEndEdit.AddListener(PlayerName_OnEndEdit);
        Button_enterSessionBrowser.onClick.AddListener(ShowPanel_ListAllSession);
        Input_sessionName.onEndEdit.AddListener(CustomSessionName_OnEndEdit);
        Slider_playerCount.onValueChanged.AddListener(Slider_playerCountValueChanged);

    }



    private void OnDisable()
    {
        // Un-Subscribe to the EndEdit event.
        Input_playerName.onEndEdit.RemoveListener(PlayerName_OnEndEdit);
        Button_enterSessionBrowser.onClick.RemoveListener(ShowPanel_ListAllSession);
        Input_sessionName.onEndEdit.RemoveListener(CustomSessionName_OnEndEdit);
        Slider_playerCount.onValueChanged.RemoveListener(Slider_playerCountValueChanged);
    }

    private void PlayerName_OnEndEdit(string inputText)
    {
        //Debug.Log("User finished typing: " + inputText);
        networkRunnerHandler.playerInGameName = inputText;
        Text_playerDisplayName.text = networkRunnerHandler.playerInGameName;

        if (!CheckPlayerName())
        {
            Toast.Show("Please enter <b><color=yellow>PLAYER NAME</color></b>", 1.5f, ToastPosition.BottomCenter);
            Button_createNewSession.interactable = false;
            Button_enterSessionBrowser.interactable = false;
        }
        else
        {
            Button_createNewSession.interactable = true;
            Button_enterSessionBrowser.interactable = true;
        }

        //SwitchGameHeader();
    }

    bool CheckPlayerName()
    {
        return networkRunnerHandler.playerInGameName != "" && networkRunnerHandler.playerInGameName != null && networkRunnerHandler.playerInGameName != string.Empty;
    }

    bool CheckSessionName()
    {
        return customSessionName != "" && customSessionName != null && customSessionName != string.Empty;
    }

    private void CustomSessionName_OnEndEdit(string inputText)
    {
        customSessionName = inputText;
        if (CheckSessionName())
        {
            Button_hostNewGame.interactable = true;
        }
        else
        {
            Toast.Show("Please enter <b><color=yellow>SESSION NAME</color></b>", 1.5f, ToastPosition.BottomCenter);
            Button_hostNewGame.interactable = false;
        }
    }

    private void Slider_playerCountValueChanged(float value)
    {
        customSessionPlayerCount = (int)value;
        playerCountSliderDisplay.text = $"{customSessionPlayerCount}";
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
        Button_createNewSession.interactable = false;
        Button_hostNewGame.interactable = false;
        Button_enterSessionBrowser.interactable = false;
        Panel_hostOrJoinSession.gameObject.SetActive(true);
        Panel_lisAllSessions.gameObject.SetActive(false);
        Text_playerDisplayName.text = string.Empty;
        Slider_playerCount.minValue = 2;
        Slider_playerCount.maxValue = 10;
        Slider_playerCount.value = Slider_playerCount.minValue;
        customSessionPlayerCount = 2;
        playerCountSliderDisplay.text = $"{customSessionPlayerCount}";
        Input_sessionName.text = string.Empty;
        Input_playerName.text = string.Empty;
        CreateSessionObject.SetActive(false);
        //playerInGameName = string.Empty;
        networkRunnerHandler = FindAnyObjectByType<NetworkRunnerHandler>();

        networkRunnerHandler.playerInGameName = string.Empty;

    }

    public IEnumerator StartFakeLoading(string message, bool showAnimation)
    {
        yield return new WaitForSeconds(0.1f);
        // LoadingAnimationObject.SetActive(showAnimation);
        if (loadingManager == null)
        {
            loadingManager = FindObjectOfType<LoadingManager>();
        }
        if (showAnimation)
        {
            loadingManager.EnableLoading(message, LoadingManager.LoadingType.Fullscreen, true);
        }
        else
        {
            loadingManager.DisableLoading();
        }
    }
    // public IEnumerator StartFakeLoading(bool showAnimation)
    // {
    //     yield return new WaitForSeconds(0.1f);
    //     LoadingAnimationObject.SetActive(showAnimation);
    // }

    public void ControlCreateSessionPanel(bool show)
    {
        Slider_playerCount.minValue = 2;
        Slider_playerCount.maxValue = 10;
        Slider_playerCount.value = Slider_playerCount.minValue;
        customSessionPlayerCount = 2;
        playerCountSliderDisplay.text = $"{customSessionPlayerCount}";
        Button_hostNewGame.interactable = false;
        Input_sessionName.text = string.Empty;
        CreateSessionObject.SetActive(show);
    }

}
