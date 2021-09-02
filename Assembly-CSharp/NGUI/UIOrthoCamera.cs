using UnityEngine;

[AddComponentMenu("NGUI/UI/Orthographic Camera")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class UIOrthoCamera : MonoBehaviour
{
	private Camera mCam;

	private Transform mTrans;

	private void Start()
	{
		mCam = base.camera;
		mTrans = base.transform;
		mCam.orthographic = true;
	}

	private void Update()
	{
		float num = mCam.rect.yMin * (float)Screen.height;
		float num2 = mCam.rect.yMax * (float)Screen.height;
		float num3 = (num2 - num) * 0.5f;
		Vector3 lossyScale = mTrans.lossyScale;
		float num4 = num3 * lossyScale.y;
		if (!Mathf.Approximately(mCam.orthographicSize, num4))
		{
			mCam.orthographicSize = num4;
		}
	}
}
