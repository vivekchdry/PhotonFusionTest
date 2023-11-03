using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System;


public class SessionInfoListUiItem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI sessionDisplayName;
    [SerializeField]
    private TextMeshProUGUI sessionDisplayPlayerCount;
    [SerializeField]
    private Button joinSessionButton;

    private SessionInfo sessionInfo;

    public event Action<SessionInfo> OnJoinSession;

    public void SetSessionInformation(SessionInfo sessionInfo)
    {
        this.sessionInfo = sessionInfo;

        sessionDisplayName.text = sessionInfo.Name;
        sessionDisplayPlayerCount.text = $"{sessionInfo.PlayerCount.ToString()}/{sessionInfo.MaxPlayers.ToString()}";

        if (sessionInfo.PlayerCount >= sessionInfo.MaxPlayers)
        {
            joinSessionButton.interactable = false;
        }
        else
        {
            joinSessionButton.interactable = true;
        }
    }

    public void OnClick()
    {
        OnJoinSession?.Invoke(sessionInfo);
    }
}
