using UnityEngine;

public class TITAN_CONTROLLER : MonoBehaviour
{
    public FengCustomInputs inputManager;
    public float targetDirection;
    public Camera currentCamera;
    public bool isAttackDown;
    public bool isJumpDown;
    public bool isAttackIIDown;
    public bool isWALKDown;
    public bool isSuicide;
    public bool grabfrontl;
    public bool grabfrontr;
    public bool grabbackl;
    public bool grabbackr;
    public bool grabnapel;
    public bool grabnaper;
    public bool choptl;
    public bool choptr;
    public bool chopl;
    public bool chopr;
    public bool bitel;
    public bool bite;
    public bool biter;
    public bool cover;
    public bool sit;
    public float currentDirection;
    public bool isHorse;

    private void Start()
    {
        inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
        currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
        {
            base.enabled = false;
        }
    }

    private void Update()
    {
        int forward;
        int strafe;

        // Horse Controls
        if (isHorse)
        {
            forward = (FengGameManagerMKII.InputRC.IsInputHorse(InputCodeRC.HorseForward) ? 1 : (FengGameManagerMKII.InputRC.IsInputHorse(InputCodeRC.HorseBack) ? (-1) : 0));
            strafe = (FengGameManagerMKII.InputRC.IsInputHorse(InputCodeRC.HorseLeft) ? (-1) : (FengGameManagerMKII.InputRC.IsInputHorse(InputCodeRC.HorseRight) ? 1 : 0));
            if (strafe != 0 || forward != 0)
            {
                float currentYaw = currentCamera.transform.rotation.eulerAngles.y;
                float yaw = Mathf.Atan2(forward, strafe) * 57.29578f;
                yaw = 0f - yaw + 90f;
                targetDirection = currentYaw + yaw;
            }
            else
            {
                targetDirection = -874f;
            }
            isAttackDown = false;
            isAttackIIDown = false;
            if (targetDirection != -874f)
            {
                currentDirection = targetDirection;
            }

            if (FengGameManagerMKII.InputRC.IsInputHorse(InputCodeRC.HorseJump))
            {
                isAttackDown = true;
            }
            if (inputManager.isInputDown[InputCode.Salute])
            {
                isAttackIIDown = true;
            }
            isWALKDown = FengGameManagerMKII.InputRC.IsInputHorse(InputCodeRC.HorseWalk);
            return;
        }

        // Titan Controls
        forward = (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanForward) ? 1 : (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanBack) ? (-1) : 0));
        strafe = (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanLeft) ? (-1) : (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanRight) ? 1 : 0));
        if (strafe != 0 || forward != 0)
        {
            float currentYaw = currentCamera.transform.rotation.eulerAngles.y;
            float yaw = Mathf.Atan2(forward, strafe) * 57.29578f;
            yaw = 0f - yaw + 90f;
            targetDirection = currentYaw + yaw;
        }
        else
        {
            targetDirection = -874f;
        }

        float cameraSideToBody;
        isAttackDown = false;
        isJumpDown = false;
        isAttackIIDown = false;
        isSuicide = false;
        grabbackl = false;
        grabbackr = false;
        grabfrontl = false;
        grabfrontr = false;
        grabnapel = false;
        grabnaper = false;
        choptl = false;
        chopr = false;
        chopl = false;
        choptr = false;
        bite = false;
        bitel = false;
        biter = false;
        cover = false;
        sit = false;

        if (targetDirection != -874f)
        {
            currentDirection = targetDirection;
        }

        cameraSideToBody = currentCamera.transform.rotation.eulerAngles.y - currentDirection;
        if (cameraSideToBody >= 180f)
        {
            cameraSideToBody -= 360f;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanPunch))
        {
            isAttackDown = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanSlam))
        {
            isAttackIIDown = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanJump))
        {
            isJumpDown = true;
        }

        if (inputManager.GetComponent<FengCustomInputs>().isInputDown[InputCode.Restart])
        {
            isSuicide = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanCover))
        {
            cover = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanSit))
        {
            sit = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanGrabFront) && cameraSideToBody >= 0f)
        {
            grabfrontr = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanGrabFront) && cameraSideToBody < 0f)
        {
            grabfrontl = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanGrabBack) && cameraSideToBody >= 0f)
        {
            grabbackr = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanGrabBack) && cameraSideToBody < 0f)
        {
            grabbackl = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanGrabNape) && cameraSideToBody >= 0f)
        {
            grabnaper = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanGrabNape) && cameraSideToBody < 0f)
        {
            grabnapel = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.titanAntiAE) && cameraSideToBody >= 0f)
        {
            choptr = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.titanAntiAE) && cameraSideToBody < 0f)
        {
            choptl = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanBite) && cameraSideToBody > 7.5f)
        {
            biter = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanBite) && cameraSideToBody < -7.5f)
        {
            bitel = true;
        }

        if (FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanBite) && cameraSideToBody >= -7.5f && cameraSideToBody <= 7.5f)
        {
            bite = true;
        }

        isWALKDown = FengGameManagerMKII.InputRC.IsInputTitan(InputCodeRC.TitanWalk);
    }
}
