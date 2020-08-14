using UnityEngine;

[AddComponentMenu("NGUI/Tween/Field of View")]
[RequireComponent(typeof(Camera))]
public class TweenFOV : UITweener
{
    public float from;
    public float to;
    private Camera mCam;

    public Camera cachedCamera
    {
        get
        {
            if (mCam == null)
            {
                mCam = base.camera;
            }
            return mCam;
        }
    }

    public float fov
    {
        get
        {
            return cachedCamera.fieldOfView;
        }
        set
        {
            cachedCamera.fieldOfView = value;
        }
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        cachedCamera.fieldOfView = from * (1f - factor) + to * factor;
    }

    public static TweenFOV Begin(GameObject go, float duration, float to)
    {
        TweenFOV tweenFOV = UITweener.Begin<TweenFOV>(go, duration);
        tweenFOV.from = tweenFOV.fov;
        tweenFOV.to = to;
        if (duration <= 0f)
        {
            tweenFOV.Sample(1f, isFinished: true);
            tweenFOV.enabled = false;
        }
        return tweenFOV;
    }
}
