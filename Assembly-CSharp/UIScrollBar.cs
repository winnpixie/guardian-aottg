using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Scroll Bar")]
[ExecuteInEditMode]
public class UIScrollBar : MonoBehaviour
{
	public delegate void OnScrollBarChange(UIScrollBar sb);

	public delegate void OnDragFinished();

	public enum Direction
	{
		Horizontal,
		Vertical
	}

	[HideInInspector]
	[SerializeField]
	private UISprite mBG;

	[HideInInspector]
	[SerializeField]
	private UISprite mFG;

	[HideInInspector]
	[SerializeField]
	private Direction mDir;

	[SerializeField]
	[HideInInspector]
	private bool mInverted;

	[SerializeField]
	[HideInInspector]
	private float mScroll;

	[HideInInspector]
	[SerializeField]
	private float mSize = 1f;

	private Transform mTrans;

	private bool mIsDirty;

	private Camera mCam;

	private Vector2 mScreenPos = Vector2.zero;

	public OnScrollBarChange onChange;

	public OnDragFinished onDragFinished;

	public Transform cachedTransform
	{
		get
		{
			if (mTrans == null)
			{
				mTrans = base.transform;
			}
			return mTrans;
		}
	}

	public Camera cachedCamera
	{
		get
		{
			if (mCam == null)
			{
				mCam = NGUITools.FindCameraForLayer(base.gameObject.layer);
			}
			return mCam;
		}
	}

	public UISprite background
	{
		get
		{
			return mBG;
		}
		set
		{
			if (mBG != value)
			{
				mBG = value;
				mIsDirty = true;
			}
		}
	}

	public UISprite foreground
	{
		get
		{
			return mFG;
		}
		set
		{
			if (mFG != value)
			{
				mFG = value;
				mIsDirty = true;
			}
		}
	}

	public Direction direction
	{
		get
		{
			return mDir;
		}
		set
		{
			if (mDir == value)
			{
				return;
			}
			mDir = value;
			mIsDirty = true;
			if (!(mBG != null))
			{
				return;
			}
			Transform cachedTransform = mBG.cachedTransform;
			Vector3 localScale = cachedTransform.localScale;
			if ((mDir == Direction.Vertical && localScale.x > localScale.y) || (mDir == Direction.Horizontal && localScale.x < localScale.y))
			{
				float x = localScale.x;
				localScale.x = localScale.y;
				localScale.y = x;
				cachedTransform.localScale = localScale;
				ForceUpdate();
				if (mBG.collider != null)
				{
					NGUITools.AddWidgetCollider(mBG.gameObject);
				}
				if (mFG.collider != null)
				{
					NGUITools.AddWidgetCollider(mFG.gameObject);
				}
			}
		}
	}

	public bool inverted
	{
		get
		{
			return mInverted;
		}
		set
		{
			if (mInverted != value)
			{
				mInverted = value;
				mIsDirty = true;
			}
		}
	}

	public float scrollValue
	{
		get
		{
			return mScroll;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (mScroll != num)
			{
				mScroll = num;
				mIsDirty = true;
				if (onChange != null)
				{
					onChange(this);
				}
			}
		}
	}

	public float barSize
	{
		get
		{
			return mSize;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (mSize != num)
			{
				mSize = num;
				mIsDirty = true;
				if (onChange != null)
				{
					onChange(this);
				}
			}
		}
	}

	public float alpha
	{
		get
		{
			if (mFG != null)
			{
				return mFG.alpha;
			}
			if (mBG != null)
			{
				return mBG.alpha;
			}
			return 0f;
		}
		set
		{
			if (mFG != null)
			{
				mFG.alpha = value;
				NGUITools.SetActiveSelf(mFG.gameObject, mFG.alpha > 0.001f);
			}
			if (mBG != null)
			{
				mBG.alpha = value;
				NGUITools.SetActiveSelf(mBG.gameObject, mBG.alpha > 0.001f);
			}
		}
	}

	private void CenterOnPos(Vector2 localPos)
	{
		if (!(mBG == null) && !(mFG == null))
		{
			Bounds bounds = NGUIMath.CalculateRelativeInnerBounds(cachedTransform, mBG);
			Bounds bounds2 = NGUIMath.CalculateRelativeInnerBounds(cachedTransform, mFG);
			if (mDir == Direction.Horizontal)
			{
				Vector3 size = bounds.size;
				float x = size.x;
				Vector3 size2 = bounds2.size;
				float num = x - size2.x;
				float num2 = num * 0.5f;
				Vector3 center = bounds.center;
				float num3 = center.x - num2;
				float num4 = (!(num > 0f)) ? 0f : ((localPos.x - num3) / num);
				scrollValue = ((!mInverted) ? num4 : (1f - num4));
			}
			else
			{
				Vector3 size3 = bounds.size;
				float y = size3.y;
				Vector3 size4 = bounds2.size;
				float num5 = y - size4.y;
				float num6 = num5 * 0.5f;
				Vector3 center2 = bounds.center;
				float num7 = center2.y - num6;
				float num8 = (!(num5 > 0f)) ? 0f : (1f - (localPos.y - num7) / num5);
				scrollValue = ((!mInverted) ? num8 : (1f - num8));
			}
		}
	}

	private void Reposition(Vector2 screenPos)
	{
		Transform cachedTransform = this.cachedTransform;
		Plane plane = new Plane(cachedTransform.rotation * Vector3.back, cachedTransform.position);
		Ray ray = cachedCamera.ScreenPointToRay(screenPos);
		if (plane.Raycast(ray, out float enter))
		{
			CenterOnPos(cachedTransform.InverseTransformPoint(ray.GetPoint(enter)));
		}
	}

	private void OnPressBackground(GameObject go, bool isPressed)
	{
		mCam = UICamera.currentCamera;
		Reposition(UICamera.lastTouchPosition);
		if (!isPressed && onDragFinished != null)
		{
			onDragFinished();
		}
	}

	private void OnDragBackground(GameObject go, Vector2 delta)
	{
		mCam = UICamera.currentCamera;
		Reposition(UICamera.lastTouchPosition);
	}

	private void OnPressForeground(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			mCam = UICamera.currentCamera;
			Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(mFG.cachedTransform);
			mScreenPos = mCam.WorldToScreenPoint(bounds.center);
		}
		else if (onDragFinished != null)
		{
			onDragFinished();
		}
	}

	private void OnDragForeground(GameObject go, Vector2 delta)
	{
		mCam = UICamera.currentCamera;
		Reposition(mScreenPos + UICamera.currentTouch.totalDelta);
	}

	private void Start()
	{
		if (background != null && background.collider != null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(background.gameObject);
			uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressBackground));
			uIEventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener.onDrag, new UIEventListener.VectorDelegate(OnDragBackground));
		}
		if (foreground != null && foreground.collider != null)
		{
			UIEventListener uIEventListener2 = UIEventListener.Get(foreground.gameObject);
			uIEventListener2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener2.onPress, new UIEventListener.BoolDelegate(OnPressForeground));
			uIEventListener2.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener2.onDrag, new UIEventListener.VectorDelegate(OnDragForeground));
		}
		ForceUpdate();
	}

	private void Update()
	{
		if (mIsDirty)
		{
			ForceUpdate();
		}
	}

	public void ForceUpdate()
	{
		mIsDirty = false;
		if (mBG == null || mFG == null)
		{
			return;
		}
		mSize = Mathf.Clamp01(mSize);
		mScroll = Mathf.Clamp01(mScroll);
		Vector4 border = mBG.border;
		Vector4 border2 = mFG.border;
		Vector3 localScale = mBG.cachedTransform.localScale;
		float x = Mathf.Max(0f, localScale.x - border.x - border.z);
		Vector3 localScale2 = mBG.cachedTransform.localScale;
		Vector2 vector = new Vector2(x, Mathf.Max(0f, localScale2.y - border.y - border.w));
		float num = (!mInverted) ? mScroll : (1f - mScroll);
		if (mDir == Direction.Horizontal)
		{
			Vector2 vector2 = new Vector2(vector.x * mSize, vector.y);
			mFG.pivot = UIWidget.Pivot.Left;
			mBG.pivot = UIWidget.Pivot.Left;
			mBG.cachedTransform.localPosition = Vector3.zero;
			mFG.cachedTransform.localPosition = new Vector3(border.x - border2.x + (vector.x - vector2.x) * num, 0f, 0f);
			mFG.cachedTransform.localScale = new Vector3(vector2.x + border2.x + border2.z, vector2.y + border2.y + border2.w, 1f);
			if (num < 0.999f && num > 0.001f)
			{
				mFG.MakePixelPerfect();
			}
		}
		else
		{
			Vector2 vector3 = new Vector2(vector.x, vector.y * mSize);
			mFG.pivot = UIWidget.Pivot.Top;
			mBG.pivot = UIWidget.Pivot.Top;
			mBG.cachedTransform.localPosition = Vector3.zero;
			mFG.cachedTransform.localPosition = new Vector3(0f, 0f - border.y + border2.y - (vector.y - vector3.y) * num, 0f);
			mFG.cachedTransform.localScale = new Vector3(vector3.x + border2.x + border2.z, vector3.y + border2.y + border2.w, 1f);
			if (num < 0.999f && num > 0.001f)
			{
				mFG.MakePixelPerfect();
			}
		}
	}
}
