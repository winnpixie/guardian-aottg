using UnityEngine;

public class VortexAffector : Affector
{
	private AnimationCurve VortexCurve;

	protected Vector3 Direction;

	private bool UseCurve;

	private float Magnitude;

	public VortexAffector(AnimationCurve vortexCurve, Vector3 dir, EffectNode node)
		: base(node)
	{
		VortexCurve = vortexCurve;
		Direction = dir;
		UseCurve = true;
	}

	public VortexAffector(float mag, Vector3 dir, EffectNode node)
		: base(node)
	{
		Magnitude = mag;
		Direction = dir;
		UseCurve = false;
	}

	public override void Update()
	{
		Vector3 vector = Node.GetLocalPosition() - Node.Owner.EmitPoint;
		float magnitude = vector.magnitude;
		if (magnitude != 0f)
		{
			float d = Vector3.Dot(Direction, vector);
			vector -= d * Direction;
			Vector3 zero = Vector3.zero;
			zero = ((!(vector == Vector3.zero)) ? Vector3.Cross(Direction, vector).normalized : vector);
			float elapsedTime = Node.GetElapsedTime();
			float num = (!UseCurve) ? Magnitude : VortexCurve.Evaluate(elapsedTime);
			zero *= num * Time.deltaTime;
			Node.Position += zero;
		}
	}
}
