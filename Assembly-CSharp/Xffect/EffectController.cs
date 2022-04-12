using UnityEngine;

public class EffectController : MonoBehaviour
{
    public Transform ObjectCache;
    protected XffectCache EffectCache;

    private void Start()
    {
        EffectCache = ObjectCache.GetComponent<XffectCache>();
    }

    protected Vector3 GetFaceDirection()
    {
        return base.transform.TransformDirection(Vector3.forward);
    }

    private void OnEffect(string eftname)
    {
        if (eftname == "lightning")
        {
            for (int i = 0; i < 9; i++)
            {
                Xffect component = EffectCache.GetObject(eftname).GetComponent<Xffect>();
                Vector3 zero = Vector3.zero;
                zero.x = Random.Range(-2.2f, 2.3f);
                zero.z = Random.Range(-2.1f, 2.1f);
                component.SetEmitPosition(zero);
                component.Active();
            }
        }
        else if (eftname == "cyclone")
        {
            Xffect component = EffectCache.GetObject(eftname).GetComponent<Xffect>();
            component.SetDirectionAxis(GetFaceDirection().normalized);
            component.Active();
        }
        else if (eftname == "crystal")
        {
            Xffect component = EffectCache.GetObject("crystal_surround").GetComponent<Xffect>();
            component.Active();
            component = EffectCache.GetObject("crystal").GetComponent<Xffect>();
            component.SetEmitPosition(new Vector3(0f, 1.9f, 1.4f));
            component.Active();
            component = EffectCache.GetObject("crystal_lightn").GetComponent<Xffect>();
            component.SetDirectionAxis(new Vector3(-1.5f, 1.8f, 0f));
            component.Active();
            component = EffectCache.GetObject("crystal").GetComponent<Xffect>();
            component.SetEmitPosition(new Vector3(0f, 1.5f, -1.2f));
            component.Active();
            component = EffectCache.GetObject("crystal_lightn").GetComponent<Xffect>();
            component.SetDirectionAxis(new Vector3(1.4f, 1.4f, 0f));
            component.Active();
        }
        else
        {
            Xffect component = EffectCache.GetObject(eftname).GetComponent<Xffect>();
            component.Active();
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0f, 0f, 100f, 225f), "Effect List");
        GUI.Label(new Rect(150f, 0f, 350f, 25f), "alt+left mouse button to rotation.  mouse wheel to zoom.");
        if (GUI.Button(new Rect(10f, 20f, 80f, 20f), "Effect1"))
        {
            OnEffect("crystal");
        }
        if (GUI.Button(new Rect(10f, 45f, 80f, 20f), "Effect2"))
        {
            OnEffect("rage_explode");
        }
        if (GUI.Button(new Rect(10f, 70f, 80f, 20f), "Effect3"))
        {
            OnEffect("cyclone");
        }
        if (GUI.Button(new Rect(10f, 95f, 80f, 20f), "Effect4"))
        {
            OnEffect("lightning");
        }
        if (GUI.Button(new Rect(10f, 120f, 80f, 20f), "Effect5"))
        {
            OnEffect("hit");
        }
        if (GUI.Button(new Rect(10f, 145f, 80f, 20f), "Effect6"))
        {
            OnEffect("firebody");
        }
        if (GUI.Button(new Rect(10f, 170f, 80f, 20f), "Effect7"))
        {
            OnEffect("explode");
        }
        if (GUI.Button(new Rect(10f, 195f, 80f, 20f), "Effect8"))
        {
            OnEffect("rain");
        }
    }
}
