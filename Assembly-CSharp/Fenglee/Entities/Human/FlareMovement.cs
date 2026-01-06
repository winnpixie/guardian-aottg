using Guardian.Utilities.Resources;
using UnityEngine;

public class FlareMovement : Photon.MonoBehaviour
{
    public GameObject hint;
    private GameObject hero;
    public string color;
    private Vector3 offY;
    private bool nohint;
    private float timer;

    private Light g_light;

    private void Start()
    {
        // Load custom textures and audio clips
        {
            if (ResourceLoader.TryGetAsset("Custom/Textures/flare.png", out Texture2D flareTexture))
            {
                base.GetComponent<ParticleSystem>().renderer.material.mainTexture = flareTexture;
            }
        }

        Minimap.Instance.TrackGameObjectOnMinimap(base.gameObject, base.GetComponent<ParticleSystem>().startColor, true, true);

        if (Guardian.GuardianClient.Properties.EmissiveFlares.Value)
        {
            g_light = base.gameObject.AddComponent<Light>();
            g_light.type = LightType.Point;
            g_light.intensity = 1f;
            g_light.range = 125f;
            g_light.color = base.GetComponent<ParticleSystem>().startColor;
            g_light.renderMode = LightRenderMode.ForcePixel;
        }

        hero = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
        if (!nohint && hero != null)
        {
            hint = (GameObject)Object.Instantiate(Resources.Load("UI/" + color + "FlareHint"));


            if (color == "Black")
            {
                offY = Vector3.up * 0.4f;
            }
            else
            {
                offY = Vector3.up * 0.5f;
            }
            hint.transform.parent = base.transform.root;
            hint.transform.position = hero.transform.position + offY;
            Vector3 vector = base.transform.position - hint.transform.position;
            float num = Mathf.Atan2(0f - vector.z, vector.x) * 57.29578f;
            hint.transform.rotation = Quaternion.Euler(-90f, num + 180f, 0f);
            hint.transform.localScale = Vector3.zero;
            iTween.ScaleTo(hint, iTween.Hash("x", 1f, "y", 1f, "z", 1f, "easetype", iTween.EaseType.easeOutElastic, "time", 1f));
            iTween.ScaleTo(hint, iTween.Hash("x", 0, "y", 0, "z", 0, "easetype", iTween.EaseType.easeInBounce, "time", 0.5f, "delay", 2.5f));
        }
    }

    [Guardian.Networking.RPC]
    public void SetMyColor(float r, float g, float b, float a, PhotonMessageInfo info = null)
    {
        if (info != null && info.sender.Id != base.photonView.owner.Id) return;

        Color customColor = new Color(r, g, b, a);
        base.GetComponent<ParticleSystem>().startColor = customColor;

        if (g_light != null)
        {
            g_light.color = customColor;
        }

        if (hint != null)
        {
            hint.GetComponent<MeshRenderer>().material.color = customColor;
        }
    }

    public void DontShowHint()
    {
        Object.Destroy(hint);
        nohint = true;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (hint != null)
        {
            if (timer < 3f)
            {
                hint.transform.position = hero.transform.position + offY;
                Vector3 vector = base.transform.position - hint.transform.position;
                float num = Mathf.Atan2(0f - vector.z, vector.x) * 57.29578f;
                hint.transform.rotation = Quaternion.Euler(-90f, num + 180f, 0f);
            }
            else
            {
                Object.Destroy(hint);
            }
        }

        if (timer < 4f)
        {
            base.rigidbody.AddForce((base.transform.forward + base.transform.up * 5f) * Time.deltaTime * 5f, ForceMode.VelocityChange);
        }
        else
        {
            base.rigidbody.AddForce(-base.transform.up * Time.deltaTime * 7f, ForceMode.Acceleration);
        }
    }
}