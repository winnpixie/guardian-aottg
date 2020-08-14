using UnityEngine;

[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
	private static UITooltip mInstance;

	public Camera uiCamera;

	public UILabel text;

	public UISprite background;

	public float appearSpeed = 10f;

	public bool scalingTransitions = true;

	private Transform mTrans;

	private float mTarget;

	private float mCurrent;

	private Vector3 mPos;

	private Vector3 mSize;

	private UIWidget[] mWidgets;

	private void Awake()
	{
		mInstance = this;
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	private void Start()
	{
		mTrans = base.transform;
		mWidgets = GetComponentsInChildren<UIWidget>();
		mPos = mTrans.localPosition;
		mSize = mTrans.localScale;
		if (uiCamera == null)
		{
			uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		SetAlpha(0f);
	}

	private void Update()
	{
		if (mCurrent != mTarget)
		{
			mCurrent = Mathf.Lerp(mCurrent, mTarget, Time.deltaTime * appearSpeed);
			if (Mathf.Abs(mCurrent - mTarget) < 0.001f)
			{
				mCurrent = mTarget;
			}
			SetAlpha(mCurrent * mCurrent);
			if (scalingTransitions)
			{
				Vector3 b = mSize * 0.25f;
				b.y = 0f - b.y;
				Vector3 localScale = Vector3.one * (1.5f - mCurrent * 0.5f);
				Vector3 localPosition = Vector3.Lerp(mPos - b, mPos, mCurrent);
				mTrans.localPosition = localPosition;
				mTrans.localScale = localScale;
			}
		}
	}

	private void SetAlpha(float val)
	{
		int i = 0;
		for (int num = mWidgets.Length; i < num; i++)
		{
			UIWidget uIWidget = mWidgets[i];
			Color color = uIWidget.color;
			color.a = val;
			uIWidget.color = color;
		}
	}

	private void SetText(string tooltipText)
	{
		if (text != null && !string.IsNullOrEmpty(tooltipText))
		{
			mTarget = 1f;
			if (text != null)
			{
				text.text = tooltipText;
			}
			mPos = Input.mousePosition;
			if (background != null)
			{
				Transform transform = background.transform;
				Transform transform2 = text.transform;
				Vector3 localPosition = transform2.localPosition;
				Vector3 localScale = transform2.localScale;
				mSize = text.relativeSize;
				mSize.x *= localScale.x;
				mSize.y *= localScale.y;
				ref Vector3 reference = ref mSize;
				float x = reference.x;
				Vector4 border = background.border;
				float x2 = border.x;
				Vector4 border2 = background.border;
				float num = x2 + border2.z;
				float x3 = localPosition.x;
				Vector4 border3 = background.border;
				reference.x = x + (num + (x3 - border3.x) * 2f);
				ref Vector3 reference2 = ref mSize;
				float y = reference2.y;
				Vector4 border4 = background.border;
				float y2 = border4.y;
				Vector4 border5 = background.border;
				float num2 = y2 + border5.w;
				float num3 = 0f - localPosition.y;
				Vector4 border6 = background.border;
				reference2.y = y + (num2 + (num3 - border6.y) * 2f);
				mSize.z = 1f;
				transform.localScale = mSize;
			}
			if (uiCamera != null)
			{
				mPos.x = Mathf.Clamp01(mPos.x / (float)Screen.width);
				mPos.y = Mathf.Clamp01(mPos.y / (float)Screen.height);
				float orthographicSize = uiCamera.orthographicSize;
				Vector3 lossyScale = mTrans.parent.lossyScale;
				float num4 = orthographicSize / lossyScale.y;
				float num5 = (float)Screen.height * 0.5f / num4;
				Vector2 vector = new Vector2(num5 * mSize.x / (float)Screen.width, num5 * mSize.y / (float)Screen.height);
				mPos.x = Mathf.Min(mPos.x, 1f - vector.x);
				mPos.y = Mathf.Max(mPos.y, vector.y);
				mTrans.position = uiCamera.ViewportToWorldPoint(mPos);
				mPos = mTrans.localPosition;
				mPos.x = Mathf.Round(mPos.x);
				mPos.y = Mathf.Round(mPos.y);
				mTrans.localPosition = mPos;
			}
			else
			{
				if (mPos.x + mSize.x > (float)Screen.width)
				{
					mPos.x = (float)Screen.width - mSize.x;
				}
				if (mPos.y - mSize.y < 0f)
				{
					mPos.y = mSize.y;
				}
				mPos.x -= (float)Screen.width * 0.5f;
				mPos.y -= (float)Screen.height * 0.5f;
			}
		}
		else
		{
			mTarget = 0f;
		}
	}

	public static void ShowText(string tooltipText)
	{
		if (mInstance != null)
		{
			mInstance.SetText(tooltipText);
		}
	}
}
