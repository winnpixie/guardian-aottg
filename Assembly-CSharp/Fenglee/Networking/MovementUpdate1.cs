using UnityEngine;

public class MovementUpdate1 : MonoBehaviour
{
    public bool disabled;

    private void Start()
    {
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
        {
            disabled = true;
            base.enabled = false;
        }
        else if (base.networkView.isMine)
        {
            base.networkView.RPC("updateMovement1", RPCMode.OthersBuffered, base.transform.position, base.transform.rotation, base.transform.lossyScale);
        }
        else
        {
            base.enabled = false;
        }
    }

    private void Update()
    {
        if (!disabled)
        {
            base.networkView.RPC("updateMovement1", RPCMode.Others, base.transform.position, base.transform.rotation, base.transform.lossyScale);
        }
    }

    [Guardian.Networking.RPC(Name = "updateMovement1")]
    private void UpdateMovement1(Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
    {
        base.transform.position = newPosition;
        base.transform.rotation = newRotation;
        base.transform.localScale = newScale;
    }
}
