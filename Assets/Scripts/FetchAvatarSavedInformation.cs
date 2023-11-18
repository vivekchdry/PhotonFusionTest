using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using ReadyPlayerMe;
using ReadyPlayerMe.Core;
using ReadyPlayerMe.Core.WebView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FetchAvatarSavedInformation : MonoBehaviour
{
    public static FetchAvatarSavedInformation instance;
    public AvatarCreatorData avatarCreatorData;
    public LoadingManager loadingManager;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(this);

    }

    private void Start()
    {
        StartCoroutine(CustomStart());
    }

    public IEnumerator CustomStart()
    {
        if (loadingManager == null)
        {
            loadingManager = FindObjectOfType<LoadingManager>();
            yield return new WaitForSeconds(1f);
        }
        loadingManager.EnableLoading("Checking existing Avatar", LoadingManager.LoadingType.Fullscreen, true);
        if (CheckAvatarExists())
        {
            yield return new WaitForSeconds(1f);
            loadingManager.EnableLoading("Existing avatar found.", LoadingManager.LoadingType.Fullscreen, true);
            yield return new WaitForSeconds(1f);
            loadingManager.EnableLoading("Loading into lobby.", LoadingManager.LoadingType.Fullscreen, true);
            yield return new WaitForSeconds(1f);
            loadingManager.DisableLoading();
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("LobbyScene");

        }
        else
        {
            yield return new WaitForSeconds(1f);
            loadingManager.EnableLoading("No existing avatar found.", LoadingManager.LoadingType.Fullscreen, true);
            yield return new WaitForSeconds(1f);
            loadingManager.EnableLoading("Loading avatar creator", LoadingManager.LoadingType.Fullscreen, true);
            yield return new WaitForSeconds(1f);
            loadingManager.DisableLoading();
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("AvatarCreatorSample");
        }
    }

    [ContextMenu("CheckAvatarExists")]
    public bool CheckAvatarExists()
    {
        if (avatarCreatorData == null)
        {
            return false;
        }

        avatarCreatorData.AvatarProperties.Id = PlayerPrefs.GetString("AvatarProperties_Id");
        avatarCreatorData.AvatarProperties.Partner = PlayerPrefs.GetString("AvatarProperties_Partner");

        switch (PlayerPrefs.GetInt("AvatarProperties_Gender"))
        {
            case (int)OutfitGender.None:
                {
                    avatarCreatorData.AvatarProperties.Gender = OutfitGender.None;
                    break;
                }
            case (int)OutfitGender.Masculine:
                {
                    avatarCreatorData.AvatarProperties.Gender = OutfitGender.Masculine;
                    break;
                }
            case (int)OutfitGender.Feminine:
                {
                    avatarCreatorData.AvatarProperties.Gender = OutfitGender.Feminine;
                    break;
                }
        }
        switch (PlayerPrefs.GetInt("AvatarProperties_BodyType"))
        {
            case (int)BodyType.None:
                {
                    avatarCreatorData.AvatarProperties.BodyType = BodyType.None;
                    break;
                }
            case (int)BodyType.FullBody:
                {
                    avatarCreatorData.AvatarProperties.BodyType = BodyType.FullBody;
                    break;
                }
            case (int)BodyType.HalfBody:
                {
                    avatarCreatorData.AvatarProperties.BodyType = BodyType.HalfBody;
                    break;
                }
        }
        avatarCreatorData.AvatarProperties.Base64Image = PlayerPrefs.GetString("AvatarProperties_Base64Image");
        if (PlayerPrefs.GetString("AvatarCreatorData_IsExistingAvatar") == "True")
        {
            avatarCreatorData.IsExistingAvatar = true;
        }
        else
        {
            avatarCreatorData.IsExistingAvatar = false;
        }
        if (avatarCreatorData.AvatarProperties.Id != string.Empty && avatarCreatorData.AvatarProperties.Id != "" && avatarCreatorData.AvatarProperties.Id != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
