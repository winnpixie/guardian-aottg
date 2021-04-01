using UnityEngine;

public class UVAffector : Affector
{
	protected UVAnimation Frames;

	protected float ElapsedTime;

	protected float UVTime;

	public UVAffector(UVAnimation frame, float time, EffectNode node)
		: base(node)
	{
		Frames = frame;
		UVTime = time;
	}

	public override void Reset()
	{
		ElapsedTime = 0f;
		Frames.curFrame = 0;
	}

	public override void Update()
	{
		ElapsedTime += Time.deltaTime;
		float num = (!(UVTime <= 0f)) ? (UVTime / (float)Frames.frames.Length) : (Node.GetLifeTime() / (float)Frames.frames.Length);
		if (ElapsedTime >= num)
		{
			Vector2 uv = Vector2.zero;
			Vector2 dm = Vector2.zero;
			Frames.GetNextFrame(ref uv, ref dm);
			Node.LowerLeftUV = uv;
			Node.UVDimensions = dm;
			ElapsedTime -= num;
		}
	}
}
