using System;
using System.Collections;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using UnityEngine;

public class RpmCustomAvatarManager : MonoBehaviour
{
    private readonly Vector3 avatarPositionOffset = new Vector3(0, 0, 0);
    private readonly Vector3 avatarScaleOffset = new Vector3(2, 2, 2);
    private const float FALL_TIMEOUT = 0.15f;
    private float fallTimeoutDelta;

    private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private static readonly int JumpHash = Animator.StringToHash("JumpTrigger");
    private static readonly int FreeFallHash = Animator.StringToHash("FreeFall");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");

    [SerializeField]
    [Tooltip("RPM avatar ID")]
    private string avatarId;
    [SerializeField]
    [Tooltip("RPM avatar URL or shortcode to load")]
    private string avatarUrl;
    private GameObject avatar;
    private AvatarObjectLoader avatarObjectLoader;
    [SerializeField]
    [Tooltip("Animator to use on loaded avatar")]
    private RuntimeAnimatorController animatorController;
    [SerializeField]
    // [Tooltip("If true it will try to load avatar from avatarUrl on start")]
    // private bool loadOnStart = true;
    private Animator animator;
    public bool jumpTrigger;

    [SerializeField]
    private NetworkCharacterControllerPrototype networkCharacterControllerPrototype;

    public event Action OnLoadComplete;
    public bool avatarReady = false;
    public float playerCurrentMagnitude;

    public void Init()
    {

        //StartCoroutine(FetchAvatarSavedInformation.instance.CustomStart());
        FetchAvatarSavedInformation.instance.CheckAvatarExists();

        avatarReady = false;
        if (networkCharacterControllerPrototype == null)
        {
            networkCharacterControllerPrototype = GetComponent<NetworkCharacterControllerPrototype>();
        }

        avatarObjectLoader = new AvatarObjectLoader();
        avatarObjectLoader.OnCompleted += OnLoadCompleted;
        avatarObjectLoader.OnFailed += OnLoadFailed;

        avatarId = FetchAvatarSavedInformation.instance.avatarCreatorData.AvatarProperties.Id;
        avatarUrl = $"https://models.readyplayer.me/{avatarId}.glb";

        // if (loadOnStart)
        // {
        // }
        LoadAvatar(avatarUrl);
    }
    private void OnLoadFailed(object sender, FailureEventArgs args)
    {
        OnLoadComplete?.Invoke();
    }

    private void OnLoadCompleted(object sender, CompletionEventArgs args)
    {
        SetupAvatar(args.Avatar);
        OnLoadComplete?.Invoke();
    }

    private void SetupAvatar(GameObject targetAvatar)
    {
        if (avatar != null)
        {
            Destroy(avatar);
        }

        avatar = targetAvatar;

        avatar.transform.parent = transform;
        avatar.transform.localScale = avatarScaleOffset;
        avatar.transform.localPosition = avatarPositionOffset;
        avatar.transform.localRotation = Quaternion.Euler(0, 0, 0);

        animator = avatar.GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorController;

        animator.applyRootMotion = false;

        avatarReady = true;
    }

    public void LoadAvatar(string url)
    {
        //remove any leading or trailing spaces
        avatarUrl = url.Trim(' ');
        avatarObjectLoader.LoadAvatar(avatarUrl);
    }

    private void Update()
    {
        if (!avatarReady)
        {
            return;
        }
        if (animator == null)
        {
            return;
        }
        UpdateAnimator();
    }

    public void ControlMoveAnimation(float value)
    {
        if (animator != null)
        {
            Debug.Log(value);
            animator.SetFloat(MoveSpeedHash, value);
        }
    }

    public void UpdateAnimator()
    {
        var isGrounded = networkCharacterControllerPrototype.IsGrounded;
        animator.SetFloat(MoveSpeedHash, playerCurrentMagnitude * networkCharacterControllerPrototype.maxSpeed);
        animator.SetBool(IsGroundedHash, isGrounded);
        if (isGrounded)
        {
            fallTimeoutDelta = FALL_TIMEOUT;
            animator.SetBool(FreeFallHash, false);
        }
        else
        {
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                animator.SetBool(FreeFallHash, true);
            }
        }
    }
    public void OnJump()
    {
        if (TryJump() && animator != null)
        {
            animator.SetTrigger(JumpHash);
        }
    }

    public bool TryJump()
    {
        jumpTrigger = false;
        if (networkCharacterControllerPrototype.IsGrounded)
        {
            jumpTrigger = true;
        }
        return jumpTrigger;
    }


}
