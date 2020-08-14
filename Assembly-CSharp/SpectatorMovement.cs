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
            float num = speed;
            if (inputManager.isInput[InputCode.jump])
            {
                num *= 3f;
            }
            float num2 = inputManager.isInput[InputCode.up] ? 1f : ((!inputManager.isInput[InputCode.down]) ? 0f : (-1f));
            float num3 = inputManager.isInput[InputCode.left] ? (-1f) : ((!inputManager.isInput[InputCode.right]) ? 0f : 1f);
            Transform transform = base.transform;
            if (num2 > 0f)
            {
                transform.position += base.transform.forward * num * Time.deltaTime;
            }
            else if (num2 < 0f)
            {
                transform.position -= base.transform.forward * num * Time.deltaTime;
            }
            if (num3 > 0f)
            {
                transform.position += base.transform.right * num * Time.deltaTime;
            }
            else if (num3 < 0f)
            {
                transform.position -= base.transform.right * num * Time.deltaTime;
            }
            if (inputManager.isInput[InputCode.leftRope])
            {
                transform.position -= base.transform.up * num * Time.deltaTime;
            }
            else if (inputManager.isInput[InputCode.rightRope])
            {
                transform.position += base.transform.up * num * Time.deltaTime;
            }
        }
    }
}
