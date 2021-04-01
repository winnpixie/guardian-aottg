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
		int num;
		int num2;
		float num5;
		if (isHorse)
		{
			num = (FengGameManagerMKII.InputRC.isInputHorse(InputCodeRC.HorseForward) ? 1 : (FengGameManagerMKII.InputRC.isInputHorse(InputCodeRC.HorseBack) ? (-1) : 0));
			num2 = (FengGameManagerMKII.InputRC.isInputHorse(InputCodeRC.HorseLeft) ? (-1) : (FengGameManagerMKII.InputRC.isInputHorse(InputCodeRC.HorseRight) ? 1 : 0));
			if (num2 != 0 || num != 0)
			{
				float y = currentCamera.transform.rotation.eulerAngles.y;
				float num3 = Mathf.Atan2(num, num2) * 57.29578f;
				num3 = 0f - num3 + 90f;
				float num4 = targetDirection = y + num3;
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
			num5 = currentCamera.transform.rotation.eulerAngles.y - currentDirection;
			if (num5 >= 180f)
			{
				num5 -= 360f;
			}
			if (FengGameManagerMKII.InputRC.isInputHorse(InputCodeRC.HorseJump))
			{
				isAttackDown = true;
			}
			isWALKDown = FengGameManagerMKII.InputRC.isInputHorse(InputCodeRC.HorseWalk);
			return;
		}
		num = (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanForward) ? 1 : (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanBack) ? (-1) : 0));
		num2 = (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanLeft) ? (-1) : (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanRight) ? 1 : 0));
		if (num2 != 0 || num != 0)
		{
			float y = currentCamera.transform.rotation.eulerAngles.y;
			float num3 = Mathf.Atan2(num, num2) * 57.29578f;
			num3 = 0f - num3 + 90f;
			float num4 = targetDirection = y + num3;
		}
		else
		{
			targetDirection = -874f;
		}
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
		num5 = currentCamera.transform.rotation.eulerAngles.y - currentDirection;
		if (num5 >= 180f)
		{
			num5 -= 360f;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanPunch))
		{
			isAttackDown = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanSlam))
		{
			isAttackIIDown = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanJump))
		{
			isJumpDown = true;
		}
		if (inputManager.GetComponent<FengCustomInputs>().isInputDown[InputCode.Restart])
		{
			isSuicide = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanCover))
		{
			cover = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanSit))
		{
			sit = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanGrabFront) && num5 >= 0f)
		{
			grabfrontr = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanGrabFront) && num5 < 0f)
		{
			grabfrontl = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanGrabBack) && num5 >= 0f)
		{
			grabbackr = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanGrabBack) && num5 < 0f)
		{
			grabbackl = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanGrabNape) && num5 >= 0f)
		{
			grabnaper = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanGrabNape) && num5 < 0f)
		{
			grabnapel = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.titanAntiAE) && num5 >= 0f)
		{
			choptr = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.titanAntiAE) && num5 < 0f)
		{
			choptl = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanBite) && num5 > 7.5f)
		{
			biter = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanBite) && num5 < -7.5f)
		{
			bitel = true;
		}
		if (FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanBite) && num5 >= -7.5f && num5 <= 7.5f)
		{
			bite = true;
		}
		isWALKDown = FengGameManagerMKII.InputRC.isInputTitan(InputCodeRC.TitanWalk);
	}
}
