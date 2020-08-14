using UnityEngine;

[AddComponentMenu("NGUI/Tween/Orthographic Size")]
[RequireComponent(typeof(Camera))]
public class TweenOrthoSize : UITweener
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

    public float orthoSize
    {
        get
        {
            return cachedCamera.orthographicSize;
        }
        set
        {
            cachedCamera.orthographicSize = value;
        }
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        cachedCamera.orthographicSize = from * (1f - factor) + to * factor;
    }

    public static TweenOrthoSize Begin(GameObject go, float duration, float to)
    {
        TweenOrthoSize tweenOrthoSize = UITweener.Begin<TweenOrthoSize>(go, duration);
        tweenOrthoSize.from = tweenOrthoSize.orthoSize;
        tweenOrthoSize.to = to;
        if (duration <= 0f)
        {
            tweenOrthoSize.Sample(1f, isFinished: true);
            tweenOrthoSize.enabled = false;
        }
        return tweenOrthoSize;
    }
}
