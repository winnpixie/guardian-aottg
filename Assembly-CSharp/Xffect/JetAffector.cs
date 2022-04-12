using UnityEngine;

public class JetAffector : Affector
{
    protected float MinAcceleration;
    protected float MaxAcceleration;

    public JetAffector(float min, float max, EffectNode node)
        : base(node)
    {
        MinAcceleration = min;
        MaxAcceleration = max;
    }

    public override void Update()
    {
        if ((double)Mathf.Abs(Node.Acceleration) < 1E-06)
        {
            float acceleration = Random.Range(MinAcceleration, MaxAcceleration);
            Node.Acceleration = acceleration;
        }
    }
}
