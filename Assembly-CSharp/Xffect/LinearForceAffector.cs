using UnityEngine;

public class LinearForceAffector : Affector
{
    protected Vector3 Force;

    public LinearForceAffector(Vector3 force, EffectNode node)
        : base(node)
    {
        Force = force;
    }

    public override void Update()
    {
        Node.Velocity += Force * Time.deltaTime;
    }
}
