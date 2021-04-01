using UnityEngine;

public class ScaleAffector : Affector
{
    protected AnimationCurve ScaleXCurve;
    protected AnimationCurve ScaleYCurve;
    protected RSTYPE Type;
    protected float DeltaX;
    protected float DeltaY;

    public ScaleAffector(AnimationCurve curveX, AnimationCurve curveY, EffectNode node) : base(node)
    {
        Type = RSTYPE.CURVE;
        ScaleXCurve = curveX;
        ScaleYCurve = curveY;
    }

    public ScaleAffector(float x, float y, EffectNode node) : base(node)
    {
        Type = RSTYPE.SIMPLE;
        DeltaX = x;
        DeltaY = y;
    }

    public override void Update()
    {
        float elapsedTime = Node.GetElapsedTime();
        if (Type == RSTYPE.CURVE)
        {
            if (ScaleXCurve != null)
            {
                Node.Scale.x = ScaleXCurve.Evaluate(elapsedTime);
            }
            if (ScaleYCurve != null)
            {
                Node.Scale.y = ScaleYCurve.Evaluate(elapsedTime);
            }
        }
        else if (Type == RSTYPE.SIMPLE)
        {
            float num = Node.Scale.x + DeltaX * Time.deltaTime;
            float num2 = Node.Scale.y + DeltaY * Time.deltaTime;
            if (num * Node.Scale.x > 0f)
            {
                Node.Scale.x = num;
            }
            if (num2 * Node.Scale.y > 0f)
            {
                Node.Scale.y = num2;
            }
        }
    }
}
