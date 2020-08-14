using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Slider")]
public class UISlider : IgnoreTimeScale
{
	public enum Direction
	{
		Horizontal,
		Vertical
	}

	public delegate void OnValueChange(float val);

	public static UISlider current;

	public Transform foreground;

	public Transform thumb;

	public Direction direction;

	public GameObject eventReceiver;

	public string functionName = "OnSliderChange";

	public OnValueChange onValueChange;

	public int numberOfSteps;

	[HideInInspector]
	[SerializeField]
	private float rawValue = 1f;

	private BoxCollider mCol;

	private Transform mTrans;

	private Transform mFGTrans;

	private UIWidget mFGWidget;

	private UISprite mFGFilled;

	private bool mInitDone;

	private Vector2 mSize = Vector2.zero;

	private Vector2 mCenter = Vector3.zero;

	public float sliderValue
	{
		get
		{
			float num = rawValue;
			if (numberOfSteps > 1)
			{
				num = Mathf.Round(num * (float)(numberOfSteps - 1)) / (float)(numberOfSteps - 1);
			}
			return num;
		}
		set
		{
			Set(value, force: false);
		}
	}

	public Vector2 fullSize
	{
		get
		{
			return mSize;
		}
		set
		{
			if (mSize != value)
			{
				mSize = value;
				ForceUpdate();
			}
		}
	}

	private void Init()
	{
		mInitDone = true;
		if (foreground != null)
		{
			mFGWidget = foreground.GetComponent<UIWidget>();
			mFGFilled = ((!(mFGWidget != null)) ? null : (mFGWidget as UISprite));
			mFGTrans = foreground.transform;
			if (mSize == Vector2.zero)
			{
				mSize = foreground.localScale;
			}
			if (mCenter == Vector2.zero)
			{
				mCenter = foreground.localPosition + foreground.localScale * 0.5f;
			}
		}
		else if (mCol != null)
		{
			if (mSize == Vector2.zero)
			{
				mSize = mCol.size;
			}
			if (mCenter == Vector2.zero)
			{
				mCenter = mCol.center;
			}
		}
		else
		{
			Debug.LogWarning("UISlider expected to find a foreground object or a box collider to work with", this);
		}
	}

	private void Awake()
	{
		mTrans = base.transform;
		mCol = (base.collider as BoxCollider);
	}

	private void Start()
	{
		Init();
		if (Application.isPlaying && thumb != null && thumb.collider != null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(thumb.gameObject);
			uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressThumb));
			uIEventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener.onDrag, new UIEventListener.VectorDelegate(OnDragThumb));
		}
		Set(rawValue, force: true);
	}

	private void OnPress(bool pressed)
	{
		if (pressed && UICamera.currentTouchID != -100)
		{
			UpdateDrag();
		}
	}

	private void OnDrag(Vector2 delta)
	{
		UpdateDrag();
	}

	private void OnPressThumb(GameObject go, bool pressed)
	{
		if (pressed)
		{
			UpdateDrag();
		}
	}

	private void OnDragThumb(GameObject go, Vector2 delta)
	{
		UpdateDrag();
	}

	private void OnKey(KeyCode key)
	{
		float num = (!((float)numberOfSteps > 1f)) ? 0.125f : (1f / (float)(numberOfSteps - 1));
		if (direction == Direction.Horizontal)
		{
			switch (key)
			{
			case KeyCode.LeftArrow:
				Set(rawValue - num, force: false);
				break;
			case KeyCode.RightArrow:
				Set(rawValue + num, force: false);
				break;
			}
		}
		else
		{
			switch (key)
			{
			case KeyCode.DownArrow:
				Set(rawValue - num, force: false);
				break;
			case KeyCode.UpArrow:
				Set(rawValue + num, force: false);
				break;
			}
		}
	}

	private void UpdateDrag()
	{
		if (!(mCol == null) && !(UICamera.currentCamera == null) && UICamera.currentTouch != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			if (new Plane(mTrans.rotation * Vector3.back, mTrans.position).Raycast(ray, out float enter))
			{
				Vector3 b = mTrans.localPosition + (Vector3)(mCenter - mSize * 0.5f);
				Vector3 b2 = mTrans.localPosition - b;
				Vector3 a = mTrans.InverseTransformPoint(ray.GetPoint(enter));
				Vector3 vector = a + b2;
				Set((direction != 0) ? (vector.y / mSize.y) : (vector.x / mSize.x), force: false);
			}
		}
	}

	private void Set(float input, bool force)
	{
		if (!mInitDone)
		{
			Init();
		}
		float num = Mathf.Clamp01(input);
		if (num < 0.001f)
		{
			num = 0f;
		}
		float sliderValue = this.sliderValue;
		rawValue = num;
		float sliderValue2 = this.sliderValue;
		if (!force && sliderValue == sliderValue2)
		{
			return;
		}
		Vector3 localScale = mSize;
		if (direction == Direction.Horizontal)
		{
			localScale.x *= sliderValue2;
		}
		else
		{
			localScale.y *= sliderValue2;
		}
		if (mFGFilled != null && mFGFilled.type == UISprite.Type.Filled)
		{
			mFGFilled.fillAmount = sliderValue2;
		}
		else if (foreground != null)
		{
			mFGTrans.localScale = localScale;
			if (mFGWidget != null)
			{
				if (sliderValue2 > 0.001f)
				{
					mFGWidget.enabled = true;
					mFGWidget.MarkAsChanged();
				}
				else
				{
					mFGWidget.enabled = false;
				}
			}
		}
		if (thumb != null)
		{
			Vector3 localPosition = thumb.localPosition;
			if (mFGFilled != null && mFGFilled.type == UISprite.Type.Filled)
			{
				if (mFGFilled.fillDirection == UISprite.FillDirection.Horizontal)
				{
					localPosition.x = ((!mFGFilled.invert) ? localScale.x : (mSize.x - localScale.x));
				}
				else if (mFGFilled.fillDirection == UISprite.FillDirection.Vertical)
				{
					localPosition.y = ((!mFGFilled.invert) ? localScale.y : (mSize.y - localScale.y));
				}
				else
				{
					Debug.LogWarning("Slider thumb is only supported with Horizontal or Vertical fill direction", this);
				}
			}
			else if (direction == Direction.Horizontal)
			{
				localPosition.x = localScale.x;
			}
			else
			{
				localPosition.y = localScale.y;
			}
			thumb.localPosition = localPosition;
		}
		current = this;
		if (eventReceiver != null && !string.IsNullOrEmpty(functionName) && Application.isPlaying)
		{
			eventReceiver.SendMessage(functionName, sliderValue2, SendMessageOptions.DontRequireReceiver);
		}
		if (onValueChange != null)
		{
			onValueChange(sliderValue2);
		}
		current = null;
	}

	public void ForceUpdate()
	{
		Set(rawValue, force: true);
	}
}
