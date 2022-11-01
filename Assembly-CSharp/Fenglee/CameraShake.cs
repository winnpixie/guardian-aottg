using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float R;
    private float duration;
    private float decay;
    private bool flip;

    private void UpdateShake()
    {
        if (duration <= 0f) return;

        duration -= Time.deltaTime;
        if (flip)
        {
            base.gameObject.transform.position += Vector3.up * R;
        }
        else
        {
            base.gameObject.transform.position -= Vector3.up * R;
        }

        flip = !flip;
        R *= decay;
    }

    public void StartShake(float R, float duration, float decay = 0.95f)
    {
        if (this.duration >= duration) return;

        this.R = R;
        this.duration = duration;
        this.decay = decay;
    }
}
