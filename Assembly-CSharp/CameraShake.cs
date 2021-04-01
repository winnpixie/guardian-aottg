using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float R;
    private float duration;
    private float decay;
    private bool flip;

    private void shakeUpdate()
    {
        if (duration > 0f)
        {
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
    }

    public void startShake(float R, float duration, float decay = 0.95f)
    {
        if (this.duration < duration)
        {
            this.R = R;
            this.duration = duration;
            this.decay = decay;
        }
    }
}
