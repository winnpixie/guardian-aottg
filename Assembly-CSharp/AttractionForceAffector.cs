using UnityEngine;

public class AttractionForceAffector : Affector
{
    private AnimationCurve AttractionCurve;
    protected Vector3 Position;
    private float Magnitude;
    private bool UseCurve;

    public AttractionForceAffector(AnimationCurve curve, Vector3 pos, EffectNode node) : base(node)
    {
        AttractionCurve = curve;
        Position = pos;
        UseCurve = true;
    }

    public AttractionForceAffector(float magnitude, Vector3 pos, EffectNode node) : base(node)
    {
        Magnitude = magnitude;
        Position = pos;
        UseCurve = false;
    }

    public override void Update()
    {
        Vector3 vector = (!Node.SyncClient) ? (Node.ClientTrans.position + Position - Node.GetLocalPosition()) : (Position - Node.GetLocalPosition());
        float elapsedTime = Node.GetElapsedTime();
        float num = (!UseCurve) ? Magnitude : AttractionCurve.Evaluate(elapsedTime);
        float d = num;
        Node.Velocity += vector.normalized * d * Time.deltaTime;
    }
}
