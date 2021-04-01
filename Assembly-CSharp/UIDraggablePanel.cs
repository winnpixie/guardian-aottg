using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Interaction/Draggable Panel")]
public class UIDraggablePanel : IgnoreTimeScale
{
	public delegate void OnDragFinished();

	public enum DragEffect
	{
		None,
		Momentum,
		MomentumAndSpring
	}

	public enum ShowCondition
	{
		Always,
		OnlyIfNeeded,
		WhenDragging
	}

	public bool restrictWithinPanel = true;

	public bool disableDragIfFits;

	public DragEffect dragEffect = DragEffect.MomentumAndSpring;

	public bool smoothDragStart = true;

	public Vector3 scale = Vector3.one;

	public float scrollWheelFactor;

	public float momentumAmount = 35f;

	public Vector2 relativePositionOnReset = Vector2.zero;

	public bool repositionClipping;

	public bool iOSDragEmulation = true;

	public UIScrollBar horizontalScrollBar;

	public UIScrollBar verticalScrollBar;

	public ShowCondition showScrollBars = ShowCondition.OnlyIfNeeded;

	public OnDragFinished onDragFinished;

	private Transform mTrans;

	private UIPanel mPanel;

	private Plane mPlane;

	private Vector3 mLastPos;

	private bool mPressed;

	private Vector3 mMomentum = Vector3.zero;

	private float mScroll;

	private Bounds mBounds;

	private bool mCalculatedBounds;

	private bool mShouldMove;

	private bool mIgnoreCallbacks;

	private int mDragID = -10;

	private Vector2 mDragStartOffset = Vector2.zero;

	private bool mDragStarted;

	public UIPanel panel => mPanel;

	public Bounds bounds
	{
		get
		{
			if (!mCalculatedBounds)
			{
				mCalculatedBounds = true;
				mBounds = NGUIMath.CalculateRelativeWidgetBounds(mTrans, mTrans);
			}
			return mBounds;
		}
	}

	public bool shouldMoveHorizontally
	{
		get
		{
			Vector3 size = bounds.size;
			float num = size.x;
			if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				float num2 = num;
				Vector2 clipSoftness = mPanel.clipSoftness;
				num = num2 + clipSoftness.x * 2f;
			}
			float num3 = num;
			Vector4 clipRange = mPanel.clipRange;
			return num3 > clipRange.z;
		}
	}

	public bool shouldMoveVertically
	{
		get
		{
			Vector3 size = bounds.size;
			float num = size.y;
			if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				float num2 = num;
				Vector2 clipSoftness = mPanel.clipSoftness;
				num = num2 + clipSoftness.y * 2f;
			}
			float num3 = num;
			Vector4 clipRange = mPanel.clipRange;
			return num3 > clipRange.w;
		}
	}

	private bool shouldMove
	{
		get
		{
			if (!disableDragIfFits)
			{
				return true;
			}
			if (mPanel == null)
			{
				mPanel = GetComponent<UIPanel>();
			}
			Vector4 clipRange = mPanel.clipRange;
			Bounds bounds = this.bounds;
			float num = (clipRange.z != 0f) ? (clipRange.z * 0.5f) : ((float)Screen.width);
			float num2 = (clipRange.w != 0f) ? (clipRange.w * 0.5f) : ((float)Screen.height);
			if (!Mathf.Approximately(scale.x, 0f))
			{
				Vector3 min = bounds.min;
				if (min.x < clipRange.x - num)
				{
					return true;
				}
				Vector3 max = bounds.max;
				if (max.x > clipRange.x + num)
				{
					return true;
				}
			}
			if (!Mathf.Approximately(scale.y, 0f))
			{
				Vector3 min2 = bounds.min;
				if (min2.y < clipRange.y - num2)
				{
					return true;
				}
				Vector3 max2 = bounds.max;
				if (max2.y > clipRange.y + num2)
				{
					return true;
				}
			}
			return false;
		}
	}

	public Vector3 currentMomentum
	{
		get
		{
			return mMomentum;
		}
		set
		{
			mMomentum = value;
			mShouldMove = true;
		}
	}

	private void Awake()
	{
		mTrans = base.transform;
		mPanel = GetComponent<UIPanel>();
		UIPanel uIPanel = mPanel;
		uIPanel.onChange = (UIPanel.OnChangeDelegate)Delegate.Combine(uIPanel.onChange, new UIPanel.OnChangeDelegate(OnPanelChange));
	}

	private void OnDestroy()
	{
		if (mPanel != null)
		{
			UIPanel uIPanel = mPanel;
			uIPanel.onChange = (UIPanel.OnChangeDelegate)Delegate.Remove(uIPanel.onChange, new UIPanel.OnChangeDelegate(OnPanelChange));
		}
	}

	private void OnPanelChange()
	{
		UpdateScrollbars(recalculateBounds: true);
	}

	private void Start()
	{
		UpdateScrollbars(recalculateBounds: true);
		if (horizontalScrollBar != null)
		{
			UIScrollBar uIScrollBar = horizontalScrollBar;
			uIScrollBar.onChange = (UIScrollBar.OnScrollBarChange)Delegate.Combine(uIScrollBar.onChange, new UIScrollBar.OnScrollBarChange(OnHorizontalBar));
			horizontalScrollBar.alpha = ((showScrollBars != 0 && !shouldMoveHorizontally) ? 0f : 1f);
		}
		if (verticalScrollBar != null)
		{
			UIScrollBar uIScrollBar2 = verticalScrollBar;
			uIScrollBar2.onChange = (UIScrollBar.OnScrollBarChange)Delegate.Combine(uIScrollBar2.onChange, new UIScrollBar.OnScrollBarChange(OnVerticalBar));
			verticalScrollBar.alpha = ((showScrollBars != 0 && !shouldMoveVertically) ? 0f : 1f);
		}
	}

	public bool RestrictWithinBounds(bool instant)
	{
		Vector3 vector = mPanel.CalculateConstrainOffset(bounds.min, bounds.max);
		if (vector.magnitude > 0.001f)
		{
			if (!instant && dragEffect == DragEffect.MomentumAndSpring)
			{
				SpringPanel.Begin(mPanel.gameObject, mTrans.localPosition + vector, 13f);
			}
			else
			{
				MoveRelative(vector);
				mMomentum = Vector3.zero;
				mScroll = 0f;
			}
			return true;
		}
		return false;
	}

	public void DisableSpring()
	{
		SpringPanel component = GetComponent<SpringPanel>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	public void UpdateScrollbars(bool recalculateBounds)
	{
		if (mPanel == null)
		{
			return;
		}
		if (horizontalScrollBar != null || verticalScrollBar != null)
		{
			if (recalculateBounds)
			{
				mCalculatedBounds = false;
				mShouldMove = shouldMove;
			}
			Bounds bounds = this.bounds;
			Vector2 vector = bounds.min;
			Vector2 vector2 = bounds.max;
			if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				Vector2 clipSoftness = mPanel.clipSoftness;
				vector -= clipSoftness;
				vector2 += clipSoftness;
			}
			if (horizontalScrollBar != null && vector2.x > vector.x)
			{
				Vector4 clipRange = mPanel.clipRange;
				float num = clipRange.z * 0.5f;
				float num2 = clipRange.x - num;
				Vector3 min = bounds.min;
				float num3 = num2 - min.x;
				Vector3 max = bounds.max;
				float num4 = max.x - num - clipRange.x;
				float num5 = vector2.x - vector.x;
				num3 = Mathf.Clamp01(num3 / num5);
				num4 = Mathf.Clamp01(num4 / num5);
				float num6 = num3 + num4;
				mIgnoreCallbacks = true;
				horizontalScrollBar.barSize = 1f - num6;
				horizontalScrollBar.scrollValue = ((!(num6 > 0.001f)) ? 0f : (num3 / num6));
				mIgnoreCallbacks = false;
			}
			if (verticalScrollBar != null && vector2.y > vector.y)
			{
				Vector4 clipRange2 = mPanel.clipRange;
				float num7 = clipRange2.w * 0.5f;
				float num8 = clipRange2.y - num7 - vector.y;
				float num9 = vector2.y - num7 - clipRange2.y;
				float num10 = vector2.y - vector.y;
				num8 = Mathf.Clamp01(num8 / num10);
				num9 = Mathf.Clamp01(num9 / num10);
				float num11 = num8 + num9;
				mIgnoreCallbacks = true;
				verticalScrollBar.barSize = 1f - num11;
				verticalScrollBar.scrollValue = ((!(num11 > 0.001f)) ? 0f : (1f - num8 / num11));
				mIgnoreCallbacks = false;
			}
		}
		else if (recalculateBounds)
		{
			mCalculatedBounds = false;
		}
	}

	public void SetDragAmount(float x, float y, bool updateScrollbars)
	{
		DisableSpring();
		Bounds bounds = this.bounds;
		Vector3 min = bounds.min;
		float x2 = min.x;
		Vector3 max = bounds.max;
		if (x2 == max.x)
		{
			return;
		}
		Vector3 min2 = bounds.min;
		float y2 = min2.y;
		Vector3 max2 = bounds.max;
		if (y2 == max2.y)
		{
			return;
		}
		Vector4 clipRange = mPanel.clipRange;
		float num = clipRange.z * 0.5f;
		float num2 = clipRange.w * 0.5f;
		Vector3 min3 = bounds.min;
		float num3 = min3.x + num;
		Vector3 max3 = bounds.max;
		float num4 = max3.x - num;
		Vector3 min4 = bounds.min;
		float num5 = min4.y + num2;
		Vector3 max4 = bounds.max;
		float num6 = max4.y - num2;
		if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			float num7 = num3;
			Vector2 clipSoftness = mPanel.clipSoftness;
			num3 = num7 - clipSoftness.x;
			float num8 = num4;
			Vector2 clipSoftness2 = mPanel.clipSoftness;
			num4 = num8 + clipSoftness2.x;
			float num9 = num5;
			Vector2 clipSoftness3 = mPanel.clipSoftness;
			num5 = num9 - clipSoftness3.y;
			float num10 = num6;
			Vector2 clipSoftness4 = mPanel.clipSoftness;
			num6 = num10 + clipSoftness4.y;
		}
		float num11 = Mathf.Lerp(num3, num4, x);
		float num12 = Mathf.Lerp(num6, num5, y);
		if (!updateScrollbars)
		{
			Vector3 localPosition = mTrans.localPosition;
			if (scale.x != 0f)
			{
				localPosition.x += clipRange.x - num11;
			}
			if (scale.y != 0f)
			{
				localPosition.y += clipRange.y - num12;
			}
			mTrans.localPosition = localPosition;
		}
		clipRange.x = num11;
		clipRange.y = num12;
		mPanel.clipRange = clipRange;
		if (updateScrollbars)
		{
			UpdateScrollbars(recalculateBounds: false);
		}
	}

	public void ResetPosition()
	{
		mCalculatedBounds = false;
		SetDragAmount(relativePositionOnReset.x, relativePositionOnReset.y, updateScrollbars: false);
		SetDragAmount(relativePositionOnReset.x, relativePositionOnReset.y, updateScrollbars: true);
	}

	private void OnHorizontalBar(UIScrollBar sb)
	{
		if (!mIgnoreCallbacks)
		{
			float x = horizontalScrollBar == null ? 0f : horizontalScrollBar.scrollValue;
			float y = verticalScrollBar == null ? 0f : verticalScrollBar.scrollValue;
			SetDragAmount(x, y, updateScrollbars: false);
		}
	}

	private void OnVerticalBar(UIScrollBar sb)
	{
		if (!mIgnoreCallbacks)
		{
			float x = horizontalScrollBar == null ? 0f : horizontalScrollBar.scrollValue;
			float y = verticalScrollBar == null ? 0f : verticalScrollBar.scrollValue;
			SetDragAmount(x, y, updateScrollbars: false);
		}
	}

	public void MoveRelative(Vector3 relative)
	{
		mTrans.localPosition += relative;
		Vector4 clipRange = mPanel.clipRange;
		clipRange.x -= relative.x;
		clipRange.y -= relative.y;
		mPanel.clipRange = clipRange;
		UpdateScrollbars(recalculateBounds: false);
	}

	public void MoveAbsolute(Vector3 absolute)
	{
		Vector3 a = mTrans.InverseTransformPoint(absolute);
		Vector3 b = mTrans.InverseTransformPoint(Vector3.zero);
		MoveRelative(a - b);
	}

	public void Press(bool pressed)
	{
		if (smoothDragStart && pressed)
		{
			mDragStarted = false;
			mDragStartOffset = Vector2.zero;
		}
		if (!base.enabled || !NGUITools.GetActive(base.gameObject))
		{
			return;
		}
		if (!pressed && mDragID == UICamera.currentTouchID)
		{
			mDragID = -10;
		}
		mCalculatedBounds = false;
		mShouldMove = shouldMove;
		if (!mShouldMove)
		{
			return;
		}
		mPressed = pressed;
		if (pressed)
		{
			mMomentum = Vector3.zero;
			mScroll = 0f;
			DisableSpring();
			mLastPos = UICamera.lastHit.point;
			mPlane = new Plane(mTrans.rotation * Vector3.back, mLastPos);
			return;
		}
		if (restrictWithinPanel && mPanel.clipping != 0 && dragEffect == DragEffect.MomentumAndSpring)
		{
			RestrictWithinBounds(instant: false);
		}
		if (onDragFinished != null)
		{
			onDragFinished();
		}
	}

	public void Drag()
	{
		if (!base.enabled || !NGUITools.GetActive(base.gameObject) || !mShouldMove)
		{
			return;
		}
		if (mDragID == -10)
		{
			mDragID = UICamera.currentTouchID;
		}
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
		if (smoothDragStart && !mDragStarted)
		{
			mDragStarted = true;
			mDragStartOffset = UICamera.currentTouch.totalDelta;
		}
		Ray ray = (!smoothDragStart) ? UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos) : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos - mDragStartOffset);
		float enter = 0f;
		if (mPlane.Raycast(ray, out enter))
		{
			Vector3 point = ray.GetPoint(enter);
			Vector3 vector = point - mLastPos;
			mLastPos = point;
			if (vector.x != 0f || vector.y != 0f)
			{
				vector = mTrans.InverseTransformDirection(vector);
				vector.Scale(scale);
				vector = mTrans.TransformDirection(vector);
			}
			mMomentum = Vector3.Lerp(mMomentum, mMomentum + vector * (0.01f * momentumAmount), 0.67f);
			if (!iOSDragEmulation)
			{
				MoveAbsolute(vector);
			}
			else if (mPanel.CalculateConstrainOffset(bounds.min, bounds.max).magnitude > 0.001f)
			{
				MoveAbsolute(vector * 0.5f);
				mMomentum *= 0.5f;
			}
			else
			{
				MoveAbsolute(vector);
			}
			if (restrictWithinPanel && mPanel.clipping != 0 && dragEffect != DragEffect.MomentumAndSpring)
			{
				RestrictWithinBounds(instant: true);
			}
		}
	}

	public void Scroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && scrollWheelFactor != 0f)
		{
			DisableSpring();
			mShouldMove = shouldMove;
			if (Mathf.Sign(mScroll) != Mathf.Sign(delta))
			{
				mScroll = 0f;
			}
			mScroll += delta * scrollWheelFactor;
		}
	}

	private void LateUpdate()
	{
		if (repositionClipping)
		{
			repositionClipping = false;
			mCalculatedBounds = false;
			SetDragAmount(relativePositionOnReset.x, relativePositionOnReset.y, updateScrollbars: true);
		}
		if (!Application.isPlaying)
		{
			return;
		}
		float num = UpdateRealTimeDelta();
		if (showScrollBars != 0)
		{
			bool flag = false;
			bool flag2 = false;
			if (showScrollBars != ShowCondition.WhenDragging || mDragID != -10 || mMomentum.magnitude > 0.01f)
			{
				flag = shouldMoveVertically;
				flag2 = shouldMoveHorizontally;
			}
			if ((bool)verticalScrollBar)
			{
				float alpha = verticalScrollBar.alpha;
				alpha += ((!flag) ? ((0f - num) * 3f) : (num * 6f));
				alpha = Mathf.Clamp01(alpha);
				if (verticalScrollBar.alpha != alpha)
				{
					verticalScrollBar.alpha = alpha;
				}
			}
			if ((bool)horizontalScrollBar)
			{
				float alpha2 = horizontalScrollBar.alpha;
				alpha2 += ((!flag2) ? ((0f - num) * 3f) : (num * 6f));
				alpha2 = Mathf.Clamp01(alpha2);
				if (horizontalScrollBar.alpha != alpha2)
				{
					horizontalScrollBar.alpha = alpha2;
				}
			}
		}
		if (mShouldMove && !mPressed)
		{
			mMomentum -= scale * (mScroll * 0.05f);
			if (mMomentum.magnitude > 0.0001f)
			{
				mScroll = NGUIMath.SpringLerp(mScroll, 0f, 20f, num);
				Vector3 absolute = NGUIMath.SpringDampen(ref mMomentum, 9f, num);
				MoveAbsolute(absolute);
				if (restrictWithinPanel && mPanel.clipping != 0)
				{
					RestrictWithinBounds(instant: false);
				}
				if (mMomentum.magnitude < 0.0001f && onDragFinished != null)
				{
					onDragFinished();
				}
				return;
			}
			mScroll = 0f;
			mMomentum = Vector3.zero;
		}
		else
		{
			mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref mMomentum, 9f, num);
	}
}
