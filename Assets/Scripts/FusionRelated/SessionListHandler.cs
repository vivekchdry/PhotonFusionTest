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
    [SerializeField]
    private TextMeshProUGUI sessionListStatus;
    [SerializeField]
    private SessionInfoListUiItem prefab;
    [SerializeField]
    private UnityEngine.UI.Button goback;
    [SerializeField]
    private ScrollView scrollView;
    [SerializeField]
    private Transform scrollViewContent;


    public void ClearList()
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

    }
}
