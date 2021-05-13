using UnityEngine;

public class RockScript : MonoBehaviour
{
    private Vector3 desPt = new Vector3(-200f, 0f, -280f);
    private Vector3 vh;
    private Vector3 vv;
    private bool disable;
    private float g = 500f;
    private float speed = 800f;

    private void Start()
    {
        base.transform.position = new Vector3(0f, 0f, 676f);
        vh = desPt - base.transform.position;
        vv = new Vector3(0f, g * vh.magnitude / (2f * speed), 0f);
        vh.Normalize();
        vh *= speed;
    }

    private void Update()
    {
        if (disable)
        {
            return;
        }
        vv += -Vector3.up * g * Time.deltaTime;
        base.transform.position += vv * Time.deltaTime;
        base.transform.position += vh * Time.deltaTime;
        if (!(Vector3.Distance(desPt, base.transform.position) < 20f))
        {
            Vector3 position = base.transform.position;
            if (!(position.y < 0f))
            {
                return;
            }
        }
        base.transform.position = desPt;
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Instantiate("FX/boom1_CT_KICK", base.transform.position + Vector3.up * 30f, Quaternion.Euler(270f, 0f, 0f), 0);
        }
        else
        {
            Object.Instantiate(Resources.Load("FX/boom1_CT_KICK"), base.transform.position + Vector3.up * 30f, Quaternion.Euler(270f, 0f, 0f));
        }
        disable = true;
    }
}
