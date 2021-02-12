using UnityEngine;

[AddComponentMenu("NGUI/UI/Label")]
[ExecuteInEditMode]
public class UILabel : UIWidget
{
	public enum Effect
	{
		None,
		Shadow,
		Outline
	}

	[HideInInspector]
	[SerializeField]
	private UIFont mFont;

	[SerializeField]
	[HideInInspector]
	private string mText = string.Empty;

	[SerializeField]
	[HideInInspector]
	private int mMaxLineWidth;

	[HideInInspector]
	[SerializeField]
	private bool mEncoding = true;

	[HideInInspector]
	[SerializeField]
	private int mMaxLineCount;

	[HideInInspector]
	[SerializeField]
	private bool mPassword;

	[HideInInspector]
	[SerializeField]
	private bool mShowLastChar;

	[HideInInspector]
	[SerializeField]
	private Effect mEffectStyle;

	[HideInInspector]
	[SerializeField]
	private Color mEffectColor = Color.black;

	[SerializeField]
	[HideInInspector]
	private UIFont.SymbolStyle mSymbols = UIFont.SymbolStyle.Uncolored;

	[HideInInspector]
	[SerializeField]
	private Vector2 mEffectDistance = Vector2.one;

	[HideInInspector]
	[SerializeField]
	private bool mShrinkToFit;

	[HideInInspector]
	[SerializeField]
	private float mLineWidth;

	[HideInInspector]
	[SerializeField]
	private bool mMultiline = true;

	private bool mShouldBeProcessed = true;

	private string mProcessedText;

	private Vector3 mLastScale = Vector3.one;

	private string mLastText = string.Empty;

	private int mLastWidth;

	private bool mLastEncoding = true;

	private int mLastCount;

	private bool mLastPass;

	private bool mLastShow;

	private Effect mLastEffect;

	private Vector2 mSize = Vector2.zero;

	private bool mPremultiply;

	private bool hasChanged
	{
		get
		{
			return mShouldBeProcessed || mLastText != text || mLastWidth != mMaxLineWidth || mLastEncoding != mEncoding || mLastCount != mMaxLineCount || mLastPass != mPassword || mLastShow != mShowLastChar || mLastEffect != mEffectStyle;
		}
		set
		{
			if (value)
			{
				mChanged = true;
				mShouldBeProcessed = true;
				return;
			}
			mShouldBeProcessed = false;
			mLastText = text;
			mLastWidth = mMaxLineWidth;
			mLastEncoding = mEncoding;
			mLastCount = mMaxLineCount;
			mLastPass = mPassword;
			mLastShow = mShowLastChar;
			mLastEffect = mEffectStyle;
		}
	}

	public UIFont font
	{
		get
		{
			return mFont;
		}
		set
		{
			if (mFont != value)
			{
				mFont = value;
				material = mFont == null ? null : mFont.material;
				mChanged = true;
				hasChanged = true;
				MarkAsChanged();
			}
		}
	}

	public string text
	{
		get
		{
			return mText;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(mText))
				{
					mText = string.Empty;
				}
				hasChanged = true;
			}
			else if (mText != value)
			{
				mText = value;
				hasChanged = true;
				if (shrinkToFit)
				{
					MakePixelPerfect();
				}
			}
		}
	}

	public bool supportEncoding
	{
		get
		{
			return mEncoding;
		}
		set
		{
			if (mEncoding != value)
			{
				mEncoding = value;
				hasChanged = true;
				if (value)
				{
					mPassword = false;
				}
			}
		}
	}

	public UIFont.SymbolStyle symbolStyle
	{
		get
		{
			return mSymbols;
		}
		set
		{
			if (mSymbols != value)
			{
				mSymbols = value;
				hasChanged = true;
			}
		}
	}

	public int lineWidth
	{
		get
		{
			return mMaxLineWidth;
		}
		set
		{
			if (mMaxLineWidth != value)
			{
				mMaxLineWidth = value;
				hasChanged = true;
				if (shrinkToFit)
				{
					MakePixelPerfect();
				}
			}
		}
	}

	public bool multiLine
	{
		get
		{
			return mMaxLineCount != 1;
		}
		set
		{
			if (mMaxLineCount != 1 != value)
			{
				mMaxLineCount = ((!value) ? 1 : 0);
				hasChanged = true;
				if (value)
				{
					mPassword = false;
				}
			}
		}
	}

	public int maxLineCount
	{
		get
		{
			return mMaxLineCount;
		}
		set
		{
			if (mMaxLineCount != value)
			{
				mMaxLineCount = Mathf.Max(value, 0);
				hasChanged = true;
				if (value == 1)
				{
					mPassword = false;
				}
			}
		}
	}

	public bool password
	{
		get
		{
			return mPassword;
		}
		set
		{
			if (mPassword != value)
			{
				if (value)
				{
					mMaxLineCount = 1;
					mEncoding = false;
				}
				mPassword = value;
				hasChanged = true;
			}
		}
	}

	public bool showLastPasswordChar
	{
		get
		{
			return mShowLastChar;
		}
		set
		{
			if (mShowLastChar != value)
			{
				mShowLastChar = value;
				hasChanged = true;
			}
		}
	}

	public Effect effectStyle
	{
		get
		{
			return mEffectStyle;
		}
		set
		{
			if (mEffectStyle != value)
			{
				mEffectStyle = value;
				hasChanged = true;
			}
		}
	}

	public Color effectColor
	{
		get
		{
			return mEffectColor;
		}
		set
		{
			if (!mEffectColor.Equals(value))
			{
				mEffectColor = value;
				if (mEffectStyle != 0)
				{
					hasChanged = true;
				}
			}
		}
	}

	public Vector2 effectDistance
	{
		get
		{
			return mEffectDistance;
		}
		set
		{
			if (mEffectDistance != value)
			{
				mEffectDistance = value;
				hasChanged = true;
			}
		}
	}

	public bool shrinkToFit
	{
		get
		{
			return mShrinkToFit;
		}
		set
		{
			if (mShrinkToFit != value)
			{
				mShrinkToFit = value;
				hasChanged = true;
			}
		}
	}

	public string processedText
	{
		get
		{
			if (mLastScale != base.cachedTransform.localScale)
			{
				mLastScale = base.cachedTransform.localScale;
				mShouldBeProcessed = true;
			}
			if (hasChanged)
			{
				ProcessText();
			}
			return mProcessedText;
		}
	}

	public override Material material
	{
		get
		{
			Material material = base.material;
			if (material == null)
			{
				material = this.material = (mFont == null ? null : mFont.material);
			}
			return material;
		}
	}

	public override Vector2 relativeSize
	{
		get
		{
			if (mFont == null)
			{
				return Vector3.one;
			}
			if (hasChanged)
			{
				ProcessText();
			}
			return mSize;
		}
	}

	protected override void OnStart()
	{
		if (mLineWidth > 0f)
		{
			mMaxLineWidth = Mathf.RoundToInt(mLineWidth);
			mLineWidth = 0f;
		}
		if (!mMultiline)
		{
			mMaxLineCount = 1;
			mMultiline = true;
		}
		mPremultiply = (font != null && font.material != null && font.material.shader.name.Contains("Premultiplied"));
	}

	public override void MarkAsChanged()
	{
		hasChanged = true;
		base.MarkAsChanged();
	}

	private void ProcessText()
	{
		mChanged = true;
		hasChanged = false;
		mLastText = mText;
		Vector3 localScale = base.cachedTransform.localScale;
		float num = Mathf.Abs(localScale.x);
		float num2 = mFont.size * mMaxLineCount;
		if (num > 0f)
		{
			while (true)
			{
				if (mPassword)
				{
					mProcessedText = string.Empty;
					if (mShowLastChar)
					{
						int i = 0;
						for (int num3 = mText.Length - 1; i < num3; i++)
						{
							mProcessedText += "*";
						}
						if (mText.Length > 0)
						{
							mProcessedText += mText[mText.Length - 1];
						}
					}
					else
					{
						int j = 0;
						for (int length = mText.Length; j < length; j++)
						{
							mProcessedText += "*";
						}
					}
					mProcessedText = mFont.WrapText(mProcessedText, (float)mMaxLineWidth / num, mMaxLineCount, encoding: false, UIFont.SymbolStyle.None);
				}
				else if (mMaxLineWidth > 0)
				{
					mProcessedText = mFont.WrapText(mText, (float)mMaxLineWidth / num, (!mShrinkToFit) ? mMaxLineCount : 0, mEncoding, mSymbols);
				}
				else if (!mShrinkToFit && mMaxLineCount > 0)
				{
					mProcessedText = mFont.WrapText(mText, 100000f, mMaxLineCount, mEncoding, mSymbols);
				}
				else
				{
					mProcessedText = mText;
				}
				mSize = (string.IsNullOrEmpty(mProcessedText) ? Vector2.one : mFont.CalculatePrintedSize(mProcessedText, mEncoding, mSymbols));
				if (!mShrinkToFit)
				{
					break;
				}
				if (mMaxLineCount > 0 && mSize.y * num > num2)
				{
					num = Mathf.Round(num - 1f);
					if (num > 1f)
					{
						continue;
					}
				}
				if (mMaxLineWidth > 0)
				{
					float num4 = (float)mMaxLineWidth / num;
					float a = (!(mSize.x * num > num4)) ? num : (num4 / mSize.x * num);
					num = Mathf.Min(a, num);
				}
				num = Mathf.Round(num);
				base.cachedTransform.localScale = new Vector3(num, num, 1f);
				break;
			}
			mSize.x = Mathf.Max(mSize.x, (!(num > 0f)) ? 1f : ((float)lineWidth / num));
		}
		else
		{
			mSize.x = 1f;
			num = mFont.size;
			base.cachedTransform.localScale = new Vector3(0.01f, 0.01f, 1f);
			mProcessedText = string.Empty;
		}
		mSize.y = Mathf.Max(mSize.y, 1f);
	}

	public override void MakePixelPerfect()
	{
		if (mFont != null)
		{
			float pixelSize = font.pixelSize;
			Vector3 localScale = base.cachedTransform.localScale;
			localScale.x = (float)mFont.size * pixelSize;
			localScale.y = localScale.x;
			localScale.z = 1f;
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.x = Mathf.CeilToInt(localPosition.x / pixelSize * 4f) >> 2;
			localPosition.y = Mathf.CeilToInt(localPosition.y / pixelSize * 4f) >> 2;
			localPosition.z = Mathf.RoundToInt(localPosition.z);
			localPosition.x *= pixelSize;
			localPosition.y *= pixelSize;
			base.cachedTransform.localPosition = localPosition;
			base.cachedTransform.localScale = localScale;
			if (shrinkToFit)
			{
				ProcessText();
			}
		}
		else
		{
			base.MakePixelPerfect();
		}
	}

	private void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
	{
		Color color = mEffectColor;
		color.a *= base.alpha * mPanel.alpha;
		Color32 color2 = (!font.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
		for (int i = start; i < end; i++)
		{
			verts.Add(verts.buffer[i]);
			uvs.Add(uvs.buffer[i]);
			cols.Add(cols.buffer[i]);
			Vector3 vector = verts.buffer[i];
			vector.x += x;
			vector.y += y;
			verts.buffer[i] = vector;
			cols.buffer[i] = color2;
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (mFont == null)
		{
			return;
		}
		Pivot pivot = base.pivot;
		int size = verts.size;
		Color c = base.color;
		c.a *= mPanel.alpha;
		if (font.premultipliedAlpha)
		{
			c = NGUITools.ApplyPMA(c);
		}
		switch (pivot)
		{
		case Pivot.TopLeft:
		case Pivot.Left:
		case Pivot.BottomLeft:
			mFont.Print(this.processedText, c, verts, uvs, cols, mEncoding, mSymbols, UIFont.Alignment.Left, 0, mPremultiply);
			break;
		case Pivot.TopRight:
		case Pivot.Right:
		case Pivot.BottomRight:
		{
			UIFont uIFont2 = mFont;
			string processedText2 = this.processedText;
			Color32 color2 = c;
			bool encoding2 = mEncoding;
			UIFont.SymbolStyle symbolStyle2 = mSymbols;
			Vector2 relativeSize2 = this.relativeSize;
			uIFont2.Print(processedText2, color2, verts, uvs, cols, encoding2, symbolStyle2, UIFont.Alignment.Right, Mathf.RoundToInt(relativeSize2.x * (float)mFont.size), mPremultiply);
			break;
		}
		default:
		{
			UIFont uIFont = mFont;
			string processedText = this.processedText;
			Color32 color = c;
			bool encoding = mEncoding;
			UIFont.SymbolStyle symbolStyle = mSymbols;
			Vector2 relativeSize = this.relativeSize;
			uIFont.Print(processedText, color, verts, uvs, cols, encoding, symbolStyle, UIFont.Alignment.Center, Mathf.RoundToInt(relativeSize.x * (float)mFont.size), mPremultiply);
			break;
		}
		}
		if (effectStyle != 0)
		{
			int size2 = verts.size;
			float num = 1f / (float)mFont.size;
			float num2 = num * mEffectDistance.x;
			float num3 = num * mEffectDistance.y;
			ApplyShadow(verts, uvs, cols, size, size2, num2, 0f - num3);
			if (effectStyle == Effect.Outline)
			{
				size = size2;
				size2 = verts.size;
				ApplyShadow(verts, uvs, cols, size, size2, 0f - num2, num3);
				size = size2;
				size2 = verts.size;
				ApplyShadow(verts, uvs, cols, size, size2, num2, num3);
				size = size2;
				size2 = verts.size;
				ApplyShadow(verts, uvs, cols, size, size2, 0f - num2, 0f - num3);
			}
		}
	}
}
