using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroScreenMenu : MonoBehaviour
{
    public RectTransform HolderIntroButtons;
    public RectTransform HolderAvatarButtons;
    public Button Button_playGame;
    public Button Button_quitGame;
    public Button Button_continueWithOldAvatar;
    public Button Button_createNewAvatar;

    private void OnEnable()
    {
        FetchAvatarSavedInformation.instance.onAvatarFoundOrNot += Event_OnAvatarStatus;


        Button_playGame.onClick.AddListener(OnBeginPlayGame);
        Button_quitGame.onClick.AddListener(Application.Quit);

        HolderAvatarButtons.localScale = Vector3.zero;
        HolderIntroButtons.localScale = Vector3.zero;

        HolderIntroButtons.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCirc).SetDelay(0.25f).SetAutoKill(true);
    }

    private void OnDisable()
    {
        FetchAvatarSavedInformation.instance.onAvatarFoundOrNot -= Event_OnAvatarStatus;

        Button_playGame.onClick.RemoveAllListeners();
        Button_quitGame.onClick.RemoveAllListeners();
        Button_continueWithOldAvatar.onClick.RemoveAllListeners();
        Button_createNewAvatar.onClick.RemoveAllListeners();
    }

    private void Event_OnAvatarStatus(bool status)
    {
        if (status)
        {
            //HolderAvatarButtons.gameObject.SetActive(true);
            HolderAvatarButtons.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCirc).SetAutoKill(true);

            Button_continueWithOldAvatar.onClick.AddListener(() =>
            {
                HolderAvatarButtons.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutCubic).SetAutoKill(true);

                StartCoroutine(ControlledSceneLoading("LobbyScene", 0.1f, status));
            });

            Button_createNewAvatar.onClick.AddListener(() =>
            {
                HolderAvatarButtons.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutCubic).SetAutoKill(true);

                FetchAvatarSavedInformation.instance.ResetSavedAvatarInformation();
                StartCoroutine(ControlledSceneLoading("AvatarCreatorSample", 0.1f, status));
            });
        }
        else
        {
            StartCoroutine(ControlledSceneLoading("AvatarCreatorSample", 0.1f, status));
        }
    }

    private void OnBeginPlayGame()
    {
        //HolderIntroButtons.gameObject.SetActive(false);
        HolderIntroButtons.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutCubic).SetAutoKill(true);
        StartCoroutine(FetchAvatarSavedInformation.instance.CustomStart());
    }
    public IEnumerator ControlledSceneLoading(string sceneName, float loadDelay, bool avatarStatus)
    {
        yield return new WaitForSeconds(0.5f);
        if (avatarStatus)
        {
            FetchAvatarSavedInformation.instance.loadingManager.EnableLoading("Loading into lobby.", ReadyPlayerMe.LoadingManager.LoadingType.Fullscreen, true);
        }
        else
        {
            FetchAvatarSavedInformation.instance.loadingManager.EnableLoading("Loading avatar creator", ReadyPlayerMe.LoadingManager.LoadingType.Fullscreen, true);
        }

        yield return new WaitForSeconds(1f);
        FetchAvatarSavedInformation.instance.loadingManager.DisableLoading();
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(sceneName);
    }

    public void OnCloseHolderAvatarButtons()
    {
        HolderAvatarButtons.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutCubic).SetAutoKill(true);
        HolderIntroButtons.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCirc).SetDelay(0.15f).SetAutoKill(true);
    }
}
