using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour, IAfterSpawned
{
    public NetworkCharacterControllerPrototype networkCharacterControllerPrototype;
    public NetworkObject networkObject;
    public Transform myWorldText;
    public TextLookAtCameraLocally textLookAtCameraLocally;

    private void Awake()
    {
        Debug.Log("Awake PlayerCode");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable PlayerCode");
        InitialSetup();
    }


    private void InitialSetup()
    {
        if (networkObject.HasInputAuthority)
        {
            if (HudManager.instance != null)
            {
                HudManager.instance.cinemachineFreeLook.m_Follow = networkObject.transform.GetChild(0).transform;
                HudManager.instance.cinemachineFreeLook.m_LookAt = networkObject.transform.GetChild(0).transform;
            }

            myWorldText = networkObject.transform.GetChild(1).transform;
        }
    }

    // private void MakePlayerJumpOrFallAFterJump()
    // {
    //     playerVelocity.y += networkCharacterControllerPrototype.gravity * Time.deltaTime;
    //     networkCharacterControllerPrototype.Move(playerVelocity * Runner.DeltaTime);
    // }

    // private void CalcutalePlayerJump()
    // {
    //     if (groundedPlayer)
    //     {
    //         playerVelocity.y += Mathf.Sqrt(networkCharacterControllerPrototype.jumpImpulse * -3.0f * networkCharacterControllerPrototype.gravity);
    //     }
    // }

    public override void FixedUpdateNetwork()
    {
        // groundedPlayer = networkCharacterControllerPrototype.IsGrounded;
        // if (groundedPlayer && playerVelocity.y < 0)
        // {
        //     playerVelocity.y = -0.5f;
        // }

        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            networkCharacterControllerPrototype.Move(5 * data.direction * Runner.DeltaTime);
            if (data.jumpButtonPressed)
            {
                data.jumpButtonPressed = false;
                if (networkObject.HasInputAuthority && HudManager.instance != null)
                {
                    HudManager.instance.jumpButtonPressed = false;
                }
                networkCharacterControllerPrototype.Jump(false);
            }
        }


        if (networkObject.HasInputAuthority)
        {
            // if (HudManager.instance != null)
            // {
            //     if (HudManager.instance.cinemachineFreeLook.m_Follow == null)
            //     {
            //         HudManager.instance.cinemachineFreeLook.m_Follow = transform.GetChild(0).transform;
            //     }
            //     if (HudManager.instance.cinemachineFreeLook.m_LookAt == null)
            //     {
            //         HudManager.instance.cinemachineFreeLook.m_LookAt = transform.GetChild(0).transform;
            //     }
            // }

            if (HudManager.instance != null)
            {
                HudManager.instance.MoveCameraUsingTouchPanel();
                //HudManager.instance.cinemachineFreeLook.m_XAxis.Value += HudManager.instance.touchDetect.TouchDistance.x * 200 * HudManager.instance.sensitivityX * Time.deltaTime;
                //HudManager.instance.cinemachineFreeLook.m_YAxis.Value += HudManager.instance.touchDetect.TouchDistance.y * HudManager.instance.sensitivityY * Time.deltaTime;
            }
        }
        else
        {
            Vector3 targetPosition = Camera.main.transform.position;
            targetPosition.y = myWorldText.transform.position.y;
            myWorldText.transform.LookAt(targetPosition);
            myWorldText.transform.Rotate(0, 180, 0);
        }



    }

    public void AfterSpawned()
    {
        Debug.Log("AfterSpawned PlayerCode");
        InitialSetup();
        if (networkObject.HasInputAuthority)
        {
            textLookAtCameraLocally.myLocalCamera = Camera.main.transform;
            textLookAtCameraLocally.textLookAtCamera = true;
        }
    }
}