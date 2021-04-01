using UnityEngine;

public class LevelTriggerHint : MonoBehaviour
{
    public string content;
    public HintType myhint;
    private bool on;
    private FengGameManagerMKII fengGame;

    private void Start()
    {
        if (!FengGameManagerMKII.Level.Hints)
        {
            base.enabled = false;
        }

        fengGame = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();

        FengCustomInputs inputs = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();

        if (content == string.Empty)
        {
            switch (myhint)
            {
                case HintType.DODGE:
                    content = "Press [F7D358]" + inputs.inputString[InputCode.Dodge] + "[-] to Dodge.";
                    break;
                case HintType.ATTACK:
                    content = "Press [F7D358]" + inputs.inputString[InputCode.Attack0] + "[-] to Attack. \nPress [F7D358]" + inputs.inputString[InputCode.Attack1] + "[-] to use special attack.\n***You can only kill a titan by slashing his [FA5858]NAPE[-].***\n\n";
                    break;
                case HintType.MOVE:
                    content = "Hello soldier!\nWelcome to Attack On Titan Tribute Game!\n Press [F7D358]" + inputs.inputString[InputCode.Up] + inputs.inputString[InputCode.Left] + inputs.inputString[InputCode.Down] + inputs.inputString[InputCode.Right] + "[-] to Move.";
                    break;
                case HintType.TELE:
                    content = "Move to [82FA58]green warp point[-] to proceed.";
                    break;
                case HintType.CAMA:
                    content = "Press [F7D358]" + inputs.inputString[InputCode.ChangeCamera] + "[-] to change camera mode\nPress [F7D358]" + inputs.inputString[InputCode.ToggleCursor] + "[-] to hide or show the cursor.";
                    break;
                case HintType.JUMP:
                    content = "Press [F7D358]" + inputs.inputString[InputCode.Jump] + "[-] to Jump.";
                    break;
                case HintType.JUMP2:
                    content = "Press [F7D358]" + inputs.inputString[InputCode.Up] + "[-] towards a wall to perform a wall-run.";
                    break;
                case HintType.HOOK:
                    content = "Press and Hold[F7D358] " + inputs.inputString[InputCode.HookLeft] + "[-] or [F7D358]" + inputs.inputString[InputCode.HookRight] + "[-] to launch your grapple.\nNow Try hooking to the [>3<] box. ";
                    break;
                case HintType.HOOK2:
                    content = "Press and Hold[F7D358] " + inputs.inputString[InputCode.HookBoth] + "[-] to launch both of your grapples at the same Time.\n\nNow aim between the two black blocks. \nYou will see the mark '<' and '>' appearing on the blocks. \nThen press " + inputs.inputString[InputCode.HookBoth] + " to hook the blocks.";
                    break;
                case HintType.SUPPLY:
                    content = "Press [F7D358]" + inputs.inputString[InputCode.Reload] + "[-] to reload your blades.\n Move to the supply station to refill your gas and blades.";
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            on = true;
        }
    }

    private void Update()
    {
        if (on)
        {
            fengGame.ShowHUDInfoCenter(content + "\n\n\n\n\n");
            on = false;
        }
    }
}
