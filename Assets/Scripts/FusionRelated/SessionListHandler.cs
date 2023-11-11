using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class SessionListHandler : MonoBehaviour
{
    public TextMeshProUGUI sessionListStatus;
    public SessionInfoListUiItem prefab;
    public UnityEngine.UI.Button goback;
    public ScrollView scrollView;
    public Transform scrollViewContent;


    public void ClearList(bool showMessage = false)
    {
        foreach (Transform item in scrollViewContent)
        {
            Destroy(item.gameObject);
        }
        sessionListStatus.text = $"No Session Found";
    }

    public void AddToList(SessionInfo sessionInfo)
    {
        sessionListStatus.text = "";

        SessionInfoListUiItem sessionInfoListUiItem = Instantiate(prefab, scrollViewContent);
        sessionInfoListUiItem.SetSessionInformation(sessionInfo);
        sessionInfoListUiItem.OnJoinSession += Even_OnJoinSession;
    }

    public void Even_OnJoinSession(SessionInfo info)
    {
        NetworkRunnerHandler networkRunnerHandler = FindAnyObjectByType<NetworkRunnerHandler>();
        networkRunnerHandler.JoinGame(info);
    }
}
