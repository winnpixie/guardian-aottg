using UnityEngine;

[AddComponentMenu("NGUI/UI/Input (Basic)")]
public class UIInput : MonoBehaviour
{
	public delegate char Validator(string currentText, char nextChar);

	public enum KeyboardType
	{
		Default,
		ASCIICapable,
		NumbersAndPunctuation,
		URL,
		NumberPad,
		PhonePad,
		NamePhonePad,
		EmailAddress
	}

	public delegate void OnSubmit(string inputString);

	public static UIInput current;

	public UILabel label;

	public int maxChars;

	public string caratChar = "|";

	public Validator validator;

	public KeyboardType type;

	public bool isPassword;

	public bool autoCorrect;

	public bool useLabelTextAtStart;

	public Color activeColor = Color.white;

	public GameObject selectOnTab;

	public GameObject eventReceiver;

	public string functionName = "OnSubmit";

	public OnSubmit onSubmit;

	private string mText = string.Empty;

	private string mDefaultText = string.Empty;

	private Color mDefaultColor = Color.white;

	private UIWidget.Pivot mPivot = UIWidget.Pivot.Left;

	private float mPosition;

	private string mLastIME = string.Empty;

	private bool mDoInit = true;

	public virtual string text
	{
		get
		{
			if (mDoInit)
			{
				initMain();
			}
			return mText;
		}
		set
		{
			if (mDoInit)
			{
				initMain();
			}
			mText = value;
			if (label != null)
			{
				if (string.IsNullOrEmpty(value))
				{
					value = mDefaultText;
				}
				label.supportEncoding = false;
				label.text = ((!selected) ? value : (value + caratChar));
				label.showLastPasswordChar = selected;
				label.color = ((!selected && !(value != mDefaultText)) ? mDefaultColor : activeColor);
			}
		}
	}

	public bool selected
	{
		get
		{
			return UICamera.selectedObject == base.gameObject;
		}
		set
		{
			if (!value && UICamera.selectedObject == base.gameObject)
			{
				UICamera.selectedObject = null;
			}
			else if (value)
			{
				UICamera.selectedObject = base.gameObject;
			}
		}
	}

	public string defaultText
	{
		get
		{
			return mDefaultText;
		}
		set
		{
			if (label.text == mDefaultText)
			{
				label.text = value;
			}
			mDefaultText = value;
		}
	}

	protected void initMain()
	{
		maxChars = 100;
		if (!mDoInit)
		{
			return;
		}
		mDoInit = false;
		if (label == null)
		{
			label = GetComponentInChildren<UILabel>();
		}
		if (label != null)
		{
			if (useLabelTextAtStart)
			{
				mText = label.text;
			}
			mDefaultText = label.text;
			mDefaultColor = label.color;
			label.supportEncoding = false;
			label.password = isPassword;
			mPivot = label.pivot;
			Vector3 localPosition = label.cachedTransform.localPosition;
			mPosition = localPosition.x;
		}
		else
		{
			base.enabled = false;
		}
	}

	private void OnEnable()
	{
		if (UICamera.IsHighlighted(base.gameObject))
		{
			OnSelect(isSelected: true);
		}
	}

	private void OnDisable()
	{
		if (UICamera.IsHighlighted(base.gameObject))
		{
			OnSelect(isSelected: false);
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (mDoInit)
		{
			initMain();
		}
		if (!(label != null) || !base.enabled || !NGUITools.GetActive(base.gameObject))
		{
			return;
		}
		if (isSelected)
		{
			mText = ((useLabelTextAtStart || !(label.text == mDefaultText)) ? label.text : string.Empty);
			label.color = activeColor;
			if (isPassword)
			{
				label.password = true;
			}
			Input.imeCompositionMode = IMECompositionMode.On;
			Transform cachedTransform = label.cachedTransform;
			Vector3 position = label.pivotOffset;
			float y = position.y;
			Vector2 relativeSize = label.relativeSize;
			position.y = y + relativeSize.y;
			position = cachedTransform.TransformPoint(position);
			Input.compositionCursorPos = UICamera.currentCamera.WorldToScreenPoint(position);
			UpdateLabel();
			return;
		}
		if (string.IsNullOrEmpty(mText))
		{
			label.text = mDefaultText;
			label.color = mDefaultColor;
			if (isPassword)
			{
				label.password = false;
			}
		}
		else
		{
			label.text = mText;
		}
		label.showLastPasswordChar = false;
		Input.imeCompositionMode = IMECompositionMode.Off;
		RestoreLabel();
	}

	private void Update()
	{
		if (selected)
		{
			if (selectOnTab != null && Input.GetKeyDown(KeyCode.Tab))
			{
				UICamera.selectedObject = selectOnTab;
			}
			if (Input.GetKeyDown(KeyCode.V) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
			{
				Append(NGUITools.clipboard);
			}
			if (mLastIME != Input.compositionString)
			{
				mLastIME = Input.compositionString;
				UpdateLabel();
			}
		}
	}

	private void OnInput(string input)
	{
		if (mDoInit)
		{
			initMain();
		}
		if (selected && base.enabled && NGUITools.GetActive(base.gameObject) && Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
		{
			Append(input);
		}
	}

	private void Append(string input)
	{
		int i = 0;
		for (int length = input.Length; i < length; i++)
		{
			char c = input[i];
			if (c == '\b')
			{
				if (mText.Length > 0)
				{
					mText = mText.Substring(0, mText.Length - 1);
					SendMessage("OnInputChanged", this, SendMessageOptions.DontRequireReceiver);
				}
			}
			else if (c == '\r' || c == '\n')
			{
				if ((UICamera.current.submitKey0 == KeyCode.Return || UICamera.current.submitKey1 == KeyCode.Return) && (!label.multiLine || (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))))
				{
					current = this;
					if (onSubmit != null)
					{
						onSubmit(mText);
					}
					if (eventReceiver == null)
					{
						eventReceiver = base.gameObject;
					}
					eventReceiver.SendMessage(functionName, mText, SendMessageOptions.DontRequireReceiver);
					current = null;
					selected = false;
					return;
				}
				if (validator != null)
				{
					c = validator(mText, c);
				}
				if (c == '\0')
				{
					continue;
				}
				if (c == '\n' || c == '\r')
				{
					if (label.multiLine)
					{
						mText += "\n";
					}
				}
				else
				{
					mText += c;
				}
				SendMessage("OnInputChanged", this, SendMessageOptions.DontRequireReceiver);
			}
			else if (c >= ' ')
			{
				if (validator != null)
				{
					c = validator(mText, c);
				}
				if (c != 0)
				{
					mText += c;
					SendMessage("OnInputChanged", this, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		UpdateLabel();
	}

	private void UpdateLabel()
	{
		if (mDoInit)
		{
			initMain();
		}
		if (maxChars > 0 && mText.Length > maxChars)
		{
			mText = mText.Substring(0, maxChars);
		}
		if (!(label.font != null))
		{
			return;
		}
		string str;
		if (isPassword && selected)
		{
			str = string.Empty;
			int i = 0;
			for (int length = mText.Length; i < length; i++)
			{
				str += "*";
			}
			str = str + Input.compositionString + caratChar;
		}
		else
		{
			str = ((!selected) ? mText : (mText + Input.compositionString + caratChar));
		}
		label.supportEncoding = false;
		if (!label.shrinkToFit)
		{
			if (label.multiLine)
			{
				UIFont font = label.font;
				string text = str;
				float num = label.lineWidth;
				Vector3 localScale = label.cachedTransform.localScale;
				str = font.WrapText(text, num / localScale.x, 0, encoding: false, UIFont.SymbolStyle.None);
			}
			else
			{
				UIFont font2 = label.font;
				string text2 = str;
				float num2 = label.lineWidth;
				Vector3 localScale2 = label.cachedTransform.localScale;
				string endOfLineThatFits = font2.GetEndOfLineThatFits(text2, num2 / localScale2.x, encoding: false, UIFont.SymbolStyle.None);
				if (endOfLineThatFits != str)
				{
					str = endOfLineThatFits;
					Vector3 localPosition = label.cachedTransform.localPosition;
					localPosition.x = mPosition + (float)label.lineWidth;
					if (mPivot == UIWidget.Pivot.Left)
					{
						label.pivot = UIWidget.Pivot.Right;
					}
					else if (mPivot == UIWidget.Pivot.TopLeft)
					{
						label.pivot = UIWidget.Pivot.TopRight;
					}
					else if (mPivot == UIWidget.Pivot.BottomLeft)
					{
						label.pivot = UIWidget.Pivot.BottomRight;
					}
					label.cachedTransform.localPosition = localPosition;
				}
				else
				{
					RestoreLabel();
				}
			}
		}
		label.text = str;
		label.showLastPasswordChar = selected;
	}

	private void RestoreLabel()
	{
		if (label != null)
		{
			label.pivot = mPivot;
			Vector3 localPosition = label.cachedTransform.localPosition;
			localPosition.x = mPosition;
			label.cachedTransform.localPosition = localPosition;
		}
	}

	protected void Init()
	{
		maxChars = 100;
		initMain();
	}
}
