using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
    public FengCustomInputs inputManager;
    private float speed = 100f;
    public bool disable;

    private void Start()
    {
        inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
    }

    private void Update()
    {
        if (!disable)
        {
            float moveSpeed = inputManager.isInput[InputCode.Jump] ? (speed * 3f) : speed;

            float forward = inputManager.isInput[InputCode.Up] ? 1f : (!inputManager.isInput[InputCode.Down] ? 0f : -1f);
            float strafe = inputManager.isInput[InputCode.Left] ? -1f : (!inputManager.isInput[InputCode.Right] ? 0f : 1f);
            Transform transform = base.transform;

            if (forward > 0f)
            {
                transform.position += base.transform.forward * moveSpeed * Time.deltaTime;
            }
            else if (forward < 0f)
            {
                transform.position -= base.transform.forward * moveSpeed * Time.deltaTime;
            }

            if (strafe > 0f)
            {
                transform.position += base.transform.right * moveSpeed * Time.deltaTime;
            }
            else if (strafe < 0f)
            {
                transform.position -= base.transform.right * moveSpeed * Time.deltaTime;
            }

            if (inputManager.isInput[InputCode.HookLeft])
            {
                transform.position -= base.transform.up * moveSpeed * Time.deltaTime;
            }
            else if (inputManager.isInput[InputCode.HookRight])
            {
                transform.position += base.transform.up * moveSpeed * Time.deltaTime;
            }
        }
    }
}
