using UnityEngine;

public class FlareMovement : MonoBehaviour
{
    public GameObject hint;
    private GameObject hero;
    public string color;
    private Vector3 offY;
    private bool nohint;
    private float timer;

    private void Start()
    {
        hero = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
        Color customColor = color switch
        {
            "Green" => Guardian.Mod.Properties.Flare1Color.Value.ToColor(),
            "Red" => Guardian.Mod.Properties.Flare2Color.Value.ToColor(),
            "Black" => Guardian.Mod.Properties.Flare3Color.Value.ToColor(),
            _ => Color.white
        };

        base.GetComponent<ParticleSystem>().startColor = customColor;

        Light light = base.gameObject.AddComponent<Light>();
        light.type = LightType.Point;
        light.intensity = 1f;
        light.range = 125f;
        light.color = base.GetComponent<ParticleSystem>().startColor;
        light.renderMode = LightRenderMode.ForcePixel;

        if (!nohint && hero != null)
        {
            hint = (GameObject)Object.Instantiate(Resources.Load("UI/" + color + "FlareHint"));

            hint.GetComponent<MeshRenderer>().material.color = customColor;

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

        // TODO: Mod, load custom textures and audio clips
        {
            if (Guardian.Utilities.Gesources.TryGetAsset("Custom/Textures/flare.png", out Texture2D flareTexture))
            {
                base.GetComponent<ParticleSystem>().renderer.material.mainTexture = flareTexture;
            }
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