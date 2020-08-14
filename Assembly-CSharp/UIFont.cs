using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Font")]
[ExecuteInEditMode]
public class UIFont : MonoBehaviour
{
	public enum SymbolStyle
	{
		None,
		Uncolored,
		Colored
	}

	public enum Alignment
	{
		Left,
		Center,
		Right
	}

	[SerializeField]
	[HideInInspector]
	private Material mMat;

	[HideInInspector]
	[SerializeField]
	private Rect mUVRect = new Rect(0f, 0f, 1f, 1f);

	[SerializeField]
	[HideInInspector]
	private BMFont mFont = new BMFont();

	[HideInInspector]
	[SerializeField]
	private int mSpacingX;

	[SerializeField]
	[HideInInspector]
	private int mSpacingY;

	[HideInInspector]
	[SerializeField]
	private UIAtlas mAtlas;

	[HideInInspector]
	[SerializeField]
	private UIFont mReplacement;

	[SerializeField]
	[HideInInspector]
	private float mPixelSize = 1f;

	[SerializeField]
	[HideInInspector]
	private List<BMSymbol> mSymbols = new List<BMSymbol>();

	[HideInInspector]
	[SerializeField]
	private Font mDynamicFont;

	[SerializeField]
	[HideInInspector]
	private int mDynamicFontSize = 16;

	[SerializeField]
	[HideInInspector]
	private FontStyle mDynamicFontStyle;

	[HideInInspector]
	[SerializeField]
	private float mDynamicFontOffset;

	private UIAtlas.Sprite mSprite;

	private int mPMA = -1;

	private bool mSpriteSet;

	private List<Color> mColors = new List<Color>();

	private static CharacterInfo mChar;

	public BMFont bmFont => (!(mReplacement != null)) ? mFont : mReplacement.bmFont;

	public int texWidth => (mReplacement != null) ? mReplacement.texWidth : ((mFont == null) ? 1 : mFont.texWidth);

	public int texHeight => (mReplacement != null) ? mReplacement.texHeight : ((mFont == null) ? 1 : mFont.texHeight);

	public bool hasSymbols => (!(mReplacement != null)) ? (mSymbols.Count != 0) : mReplacement.hasSymbols;

	public List<BMSymbol> symbols => (!(mReplacement != null)) ? mSymbols : mReplacement.symbols;

	public UIAtlas atlas
	{
		get
		{
			return (!(mReplacement != null)) ? mAtlas : mReplacement.atlas;
		}
		set
		{
			if (mReplacement != null)
			{
				mReplacement.atlas = value;
			}
			else
			{
				if (!(mAtlas != value))
				{
					return;
				}
				if (value == null)
				{
					if (mAtlas != null)
					{
						mMat = mAtlas.spriteMaterial;
					}
					if (sprite != null)
					{
						mUVRect = uvRect;
					}
				}
				mPMA = -1;
				mAtlas = value;
				MarkAsDirty();
			}
		}
	}

	public Material material
	{
		get
		{
			if (mReplacement != null)
			{
				return mReplacement.material;
			}
			if (mAtlas != null)
			{
				return mAtlas.spriteMaterial;
			}
			if (mMat != null)
			{
				if (mDynamicFont != null && mMat != mDynamicFont.material)
				{
					mMat.mainTexture = mDynamicFont.material.mainTexture;
				}
				return mMat;
			}
			if (mDynamicFont != null)
			{
				return mDynamicFont.material;
			}
			return null;
		}
		set
		{
			if (mReplacement != null)
			{
				mReplacement.material = value;
			}
			else if (mMat != value)
			{
				mPMA = -1;
				mMat = value;
				MarkAsDirty();
			}
		}
	}

	public float pixelSize
	{
		get
		{
			if (mReplacement != null)
			{
				return mReplacement.pixelSize;
			}
			if (mAtlas != null)
			{
				return mAtlas.pixelSize;
			}
			return mPixelSize;
		}
		set
		{
			if (mReplacement != null)
			{
				mReplacement.pixelSize = value;
				return;
			}
			if (mAtlas != null)
			{
				mAtlas.pixelSize = value;
				return;
			}
			float num = Mathf.Clamp(value, 0.25f, 4f);
			if (mPixelSize != num)
			{
				mPixelSize = num;
				MarkAsDirty();
			}
		}
	}

	public bool premultipliedAlpha
	{
		get
		{
			if (mReplacement != null)
			{
				return mReplacement.premultipliedAlpha;
			}
			if (mAtlas != null)
			{
				return mAtlas.premultipliedAlpha;
			}
			if (mPMA == -1)
			{
				Material material = this.material;
				mPMA = ((material != null && material.shader != null && material.shader.name.Contains("Premultiplied")) ? 1 : 0);
			}
			return mPMA == 1;
		}
	}

	public Texture2D texture
	{
		get
		{
			if (mReplacement != null)
			{
				return mReplacement.texture;
			}
			Material material = this.material;
			return (!(material != null)) ? null : (material.mainTexture as Texture2D);
		}
	}

	public Rect uvRect
	{
		get
		{
			if (mReplacement != null)
			{
				return mReplacement.uvRect;
			}
			if (mAtlas != null && mSprite == null && sprite != null)
			{
				Texture texture = mAtlas.texture;
				if (texture != null)
				{
					mUVRect = mSprite.outer;
					if (mAtlas.coordinates == UIAtlas.Coordinates.Pixels)
					{
						mUVRect = NGUIMath.ConvertToTexCoords(mUVRect, texture.width, texture.height);
					}
					if (mSprite.hasPadding)
					{
						Rect rect = mUVRect;
						mUVRect.xMin = rect.xMin - mSprite.paddingLeft * rect.width;
						mUVRect.yMin = rect.yMin - mSprite.paddingBottom * rect.height;
						mUVRect.xMax = rect.xMax + mSprite.paddingRight * rect.width;
						mUVRect.yMax = rect.yMax + mSprite.paddingTop * rect.height;
					}
					if (mSprite.hasPadding)
					{
						Trim();
					}
				}
			}
			return mUVRect;
		}
		set
		{
			if (mReplacement != null)
			{
				mReplacement.uvRect = value;
			}
			else if (sprite == null && mUVRect != value)
			{
				mUVRect = value;
				MarkAsDirty();
			}
		}
	}

	public string spriteName
	{
		get
		{
			return (!(mReplacement != null)) ? mFont.spriteName : mReplacement.spriteName;
		}
		set
		{
			if (mReplacement != null)
			{
				mReplacement.spriteName = value;
			}
			else if (mFont.spriteName != value)
			{
				mFont.spriteName = value;
				MarkAsDirty();
			}
		}
	}

	public int horizontalSpacing
	{
		get
		{
			return (!(mReplacement != null)) ? mSpacingX : mReplacement.horizontalSpacing;
		}
		set
		{
			if (mReplacement != null)
			{
				mReplacement.horizontalSpacing = value;
			}
			else if (mSpacingX != value)
			{
				mSpacingX = value;
				MarkAsDirty();
			}
		}
	}

	public int verticalSpacing
	{
		get
		{
			return (!(mReplacement != null)) ? mSpacingY : mReplacement.verticalSpacing;
		}
		set
		{
			if (mReplacement != null)
			{
				mReplacement.verticalSpacing = value;
			}
			else if (mSpacingY != value)
			{
				mSpacingY = value;
				MarkAsDirty();
			}
		}
	}

	public bool isValid => mDynamicFont != null || mFont.isValid;

	public int size => (mReplacement != null) ? mReplacement.size : ((!isDynamic) ? mFont.charSize : mDynamicFontSize);

	public UIAtlas.Sprite sprite
	{
		get
		{
			if (mReplacement != null)
			{
				return mReplacement.sprite;
			}
			if (!mSpriteSet)
			{
				mSprite = null;
			}
			if (mSprite == null)
			{
				if (mAtlas != null && !string.IsNullOrEmpty(mFont.spriteName))
				{
					mSprite = mAtlas.GetSprite(mFont.spriteName);
					if (mSprite == null)
					{
						mSprite = mAtlas.GetSprite(base.name);
					}
					mSpriteSet = true;
					if (mSprite == null)
					{
						mFont.spriteName = null;
					}
				}
				int i = 0;
				for (int count = mSymbols.Count; i < count; i++)
				{
					symbols[i].MarkAsDirty();
				}
			}
			return mSprite;
		}
	}

	public UIFont replacement
	{
		get
		{
			return mReplacement;
		}
		set
		{
			UIFont uIFont = value;
			if (uIFont == this)
			{
				uIFont = null;
			}
			if (mReplacement != uIFont)
			{
				if (uIFont != null && uIFont.replacement == this)
				{
					uIFont.replacement = null;
				}
				if (mReplacement != null)
				{
					MarkAsDirty();
				}
				mReplacement = uIFont;
				MarkAsDirty();
			}
		}
	}

	public bool isDynamic => mDynamicFont != null;

	public Font dynamicFont
	{
		get
		{
			return (!(mReplacement != null)) ? mDynamicFont : mReplacement.dynamicFont;
		}
		set
		{
			if (mReplacement != null)
			{
				mReplacement.dynamicFont = value;
			}
			else if (mDynamicFont != value)
			{
				if (mDynamicFont != null)
				{
					material = null;
				}
				mDynamicFont = value;
				MarkAsDirty();
			}
		}
	}

	public int dynamicFontSize
	{
		get
		{
			return (!(mReplacement != null)) ? mDynamicFontSize : mReplacement.dynamicFontSize;
		}
		set
		{
			if (mReplacement != null)
			{
				mReplacement.dynamicFontSize = value;
				return;
			}
			value = Mathf.Clamp(value, 4, 128);
			if (mDynamicFontSize != value)
			{
				mDynamicFontSize = value;
				MarkAsDirty();
			}
		}
	}

	public FontStyle dynamicFontStyle
	{
		get
		{
			return (!(mReplacement != null)) ? mDynamicFontStyle : mReplacement.dynamicFontStyle;
		}
		set
		{
			if (mReplacement != null)
			{
				mReplacement.dynamicFontStyle = value;
			}
			else if (mDynamicFontStyle != value)
			{
				mDynamicFontStyle = value;
				MarkAsDirty();
			}
		}
	}

	private Texture dynamicTexture
	{
		get
		{
			if ((bool)mReplacement)
			{
				return mReplacement.dynamicTexture;
			}
			if (isDynamic)
			{
				return mDynamicFont.material.mainTexture;
			}
			return null;
		}
	}

	private void Trim()
	{
		Texture texture = mAtlas.texture;
		if (texture != null && mSprite != null)
		{
			Rect rect = NGUIMath.ConvertToPixels(mUVRect, this.texture.width, this.texture.height, round: true);
			Rect rect2 = (mAtlas.coordinates != UIAtlas.Coordinates.TexCoords) ? mSprite.outer : NGUIMath.ConvertToPixels(mSprite.outer, texture.width, texture.height, round: true);
			int xMin = Mathf.RoundToInt(rect2.xMin - rect.xMin);
			int yMin = Mathf.RoundToInt(rect2.yMin - rect.yMin);
			int xMax = Mathf.RoundToInt(rect2.xMax - rect.xMin);
			int yMax = Mathf.RoundToInt(rect2.yMax - rect.yMin);
			mFont.Trim(xMin, yMin, xMax, yMax);
		}
	}

	private bool References(UIFont font)
	{
		if (font == null)
		{
			return false;
		}
		if (font == this)
		{
			return true;
		}
		return mReplacement != null && mReplacement.References(font);
	}

	public static bool CheckIfRelated(UIFont a, UIFont b)
	{
		if (a == null || b == null)
		{
			return false;
		}
		if (a.isDynamic && b.isDynamic && a.dynamicFont.fontNames[0] == b.dynamicFont.fontNames[0])
		{
			return true;
		}
		return a == b || a.References(b) || b.References(a);
	}

	public void MarkAsDirty()
	{
		if (mReplacement != null)
		{
			mReplacement.MarkAsDirty();
		}
		RecalculateDynamicOffset();
		mSprite = null;
		UILabel[] array = NGUITools.FindActive<UILabel>();
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			UILabel uILabel = array[i];
			if (uILabel.enabled && NGUITools.GetActive(uILabel.gameObject) && CheckIfRelated(this, uILabel.font))
			{
				UIFont font = uILabel.font;
				uILabel.font = null;
				uILabel.font = font;
			}
		}
		int j = 0;
		for (int count = mSymbols.Count; j < count; j++)
		{
			symbols[j].MarkAsDirty();
		}
	}

	public bool RecalculateDynamicOffset()
	{
		if (mDynamicFont != null)
		{
			mDynamicFont.RequestCharactersInTexture("j", mDynamicFontSize, mDynamicFontStyle);
			mDynamicFont.GetCharacterInfo('j', out CharacterInfo info, mDynamicFontSize, mDynamicFontStyle);
			float num = (float)mDynamicFontSize + info.vert.yMax;
			if (!object.Equals(mDynamicFontOffset, num))
			{
				mDynamicFontOffset = num;
				return true;
			}
		}
		return false;
	}

	public Vector2 CalculatePrintedSize(string text, bool encoding, SymbolStyle symbolStyle)
	{
		if (mReplacement != null)
		{
			return mReplacement.CalculatePrintedSize(text, encoding, symbolStyle);
		}
		Vector2 zero = Vector2.zero;
		bool isDynamic = this.isDynamic;
		if (isDynamic || (mFont != null && mFont.isValid && !string.IsNullOrEmpty(text)))
		{
			if (encoding)
			{
				text = NGUITools.StripSymbols(text);
			}
			if (isDynamic)
			{
				mDynamicFont.textureRebuildCallback = OnFontChanged;
				mDynamicFont.RequestCharactersInTexture(text, mDynamicFontSize, mDynamicFontStyle);
				mDynamicFont.textureRebuildCallback = null;
			}
			int length = text.Length;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int size = this.size;
			int num5 = size + mSpacingY;
			bool flag = encoding && symbolStyle != 0 && hasSymbols;
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				if (c == '\n')
				{
					if (num2 > num)
					{
						num = num2;
					}
					num2 = 0;
					num3 += num5;
					num4 = 0;
				}
				else if (c < ' ')
				{
					num4 = 0;
				}
				else if (!isDynamic)
				{
					BMSymbol bMSymbol = (!flag) ? null : MatchSymbol(text, i, length);
					if (bMSymbol == null)
					{
						BMGlyph glyph = mFont.GetGlyph(c);
						if (glyph != null)
						{
							num2 += mSpacingX + ((num4 == 0) ? glyph.advance : (glyph.advance + glyph.GetKerning(num4)));
							num4 = c;
						}
					}
					else
					{
						num2 += mSpacingX + bMSymbol.width;
						i += bMSymbol.length - 1;
						num4 = 0;
					}
				}
				else if (mDynamicFont.GetCharacterInfo(c, out mChar, mDynamicFontSize, mDynamicFontStyle))
				{
					num2 += (int)((float)mSpacingX + mChar.width);
				}
			}
			float num6 = (size <= 0) ? 1f : (1f / (float)size);
			zero.x = num6 * (float)((num2 <= num) ? num : num2);
			zero.y = num6 * (float)(num3 + num5);
		}
		return zero;
	}

	private static void EndLine(ref StringBuilder s)
	{
		int num = s.Length - 1;
		if (num > 0 && s[num] == ' ')
		{
			s[num] = '\n';
		}
		else
		{
			s.Append('\n');
		}
	}

	public string GetEndOfLineThatFits(string text, float maxWidth, bool encoding, SymbolStyle symbolStyle)
	{
		if (mReplacement != null)
		{
			return mReplacement.GetEndOfLineThatFits(text, maxWidth, encoding, symbolStyle);
		}
		int num = Mathf.RoundToInt(maxWidth * (float)size);
		if (num < 1)
		{
			return text;
		}
		int length = text.Length;
		int num2 = num;
		BMGlyph bMGlyph = null;
		int num3 = length;
		bool flag = encoding && symbolStyle != 0 && hasSymbols;
		bool isDynamic = this.isDynamic;
		if (isDynamic)
		{
			mDynamicFont.textureRebuildCallback = OnFontChanged;
			mDynamicFont.RequestCharactersInTexture(text, mDynamicFontSize, mDynamicFontStyle);
			mDynamicFont.textureRebuildCallback = null;
		}
		while (num3 > 0 && num2 > 0)
		{
			char c = text[--num3];
			BMSymbol bMSymbol = (!flag) ? null : MatchSymbol(text, num3, length);
			int num4 = mSpacingX;
			if (!isDynamic)
			{
				if (bMSymbol != null)
				{
					num4 += bMSymbol.advance;
				}
				else
				{
					BMGlyph glyph = mFont.GetGlyph(c);
					if (glyph == null)
					{
						bMGlyph = null;
						continue;
					}
					num4 += glyph.advance + (bMGlyph?.GetKerning(c) ?? 0);
					bMGlyph = glyph;
				}
			}
			else if (mDynamicFont.GetCharacterInfo(c, out mChar, mDynamicFontSize, mDynamicFontStyle))
			{
				num4 += (int)mChar.width;
			}
			num2 -= num4;
		}
		if (num2 < 0)
		{
			num3++;
		}
		return text.Substring(num3, length - num3);
	}

	public string WrapText(string text, float maxWidth, int maxLineCount, bool encoding, SymbolStyle symbolStyle)
	{
		if (mReplacement != null)
		{
			return mReplacement.WrapText(text, maxWidth, maxLineCount, encoding, symbolStyle);
		}
		int num = Mathf.RoundToInt(maxWidth * (float)size);
		if (num < 1)
		{
			return text;
		}
		StringBuilder s = new StringBuilder();
		int length = text.Length;
		int num2 = num;
		int num3 = 0;
		int i = 0;
		int j = 0;
		bool flag = true;
		bool flag2 = maxLineCount != 1;
		int num4 = 1;
		bool flag3 = encoding && symbolStyle != 0 && hasSymbols;
		bool isDynamic = this.isDynamic;
		if (isDynamic)
		{
			mDynamicFont.textureRebuildCallback = OnFontChanged;
			mDynamicFont.RequestCharactersInTexture(text, mDynamicFontSize, mDynamicFontStyle);
			mDynamicFont.textureRebuildCallback = null;
		}
		for (; j < length; j++)
		{
			char c = text[j];
			if (c == '\n')
			{
				if (!flag2 || num4 == maxLineCount)
				{
					break;
				}
				num2 = num;
				if (i < j)
				{
					s.Append(text.Substring(i, j - i + 1));
				}
				else
				{
					s.Append(c);
				}
				flag = true;
				num4++;
				i = j + 1;
				num3 = 0;
				continue;
			}
			if (c == ' ' && num3 != 32 && i < j)
			{
				s.Append(text.Substring(i, j - i + 1));
				flag = false;
				i = j + 1;
				num3 = c;
			}
			if (encoding && c == '[' && j + 2 < length)
			{
				if (text[j + 1] == '-' && text[j + 2] == ']')
				{
					j += 2;
					continue;
				}
				if (j + 7 < length && text[j + 7] == ']' && NGUITools.EncodeColor(NGUITools.ParseColor(text, j + 1)) == text.Substring(j + 1, 6).ToUpper())
				{
					j += 7;
					continue;
				}
			}
			BMSymbol bMSymbol = (!flag3) ? null : MatchSymbol(text, j, length);
			int num5 = mSpacingX;
			if (!isDynamic)
			{
				if (bMSymbol != null)
				{
					num5 += bMSymbol.advance;
				}
				else
				{
					BMGlyph bMGlyph = (bMSymbol != null) ? null : mFont.GetGlyph(c);
					if (bMGlyph == null)
					{
						continue;
					}
					num5 += ((num3 == 0) ? bMGlyph.advance : (bMGlyph.advance + bMGlyph.GetKerning(num3)));
				}
			}
			else if (mDynamicFont.GetCharacterInfo(c, out mChar, mDynamicFontSize, mDynamicFontStyle))
			{
				num5 += Mathf.RoundToInt(mChar.width);
			}
			num2 -= num5;
			if (num2 < 0)
			{
				if (!flag && flag2 && num4 != maxLineCount)
				{
					for (; i < length && text[i] == ' '; i++)
					{
					}
					flag = true;
					num2 = num;
					j = i - 1;
					num3 = 0;
					if (!flag2 || num4 == maxLineCount)
					{
						break;
					}
					num4++;
					EndLine(ref s);
					continue;
				}
				s.Append(text.Substring(i, Mathf.Max(0, j - i)));
				if (!flag2 || num4 == maxLineCount)
				{
					i = j;
					break;
				}
				EndLine(ref s);
				flag = true;
				num4++;
				if (c == ' ')
				{
					i = j + 1;
					num2 = num;
				}
				else
				{
					i = j;
					num2 = num - num5;
				}
				num3 = 0;
			}
			else
			{
				num3 = c;
			}
			if (!isDynamic && bMSymbol != null)
			{
				j += bMSymbol.length - 1;
				num3 = 0;
			}
		}
		if (i < j)
		{
			s.Append(text.Substring(i, j - i));
		}
		return s.ToString();
	}

	public string WrapText(string text, float maxWidth, int maxLineCount, bool encoding)
	{
		return WrapText(text, maxWidth, maxLineCount, encoding, SymbolStyle.None);
	}

	public string WrapText(string text, float maxWidth, int maxLineCount)
	{
		return WrapText(text, maxWidth, maxLineCount, encoding: false, SymbolStyle.None);
	}

	private void Align(BetterList<Vector3> verts, int indexOffset, Alignment alignment, int x, int lineWidth)
	{
		if (alignment == Alignment.Left)
		{
			return;
		}
		int size = this.size;
		if (size <= 0)
		{
			return;
		}
		float num = 0f;
		if (alignment == Alignment.Right)
		{
			num = Mathf.RoundToInt(lineWidth - x);
			if (num < 0f)
			{
				num = 0f;
			}
			num /= (float)this.size;
		}
		else
		{
			num = Mathf.RoundToInt((float)(lineWidth - x) * 0.5f);
			if (num < 0f)
			{
				num = 0f;
			}
			num /= (float)this.size;
			if ((lineWidth & 1) == 1)
			{
				num += 0.5f / (float)size;
			}
		}
		for (int i = indexOffset; i < verts.size; i++)
		{
			Vector3 vector = verts.buffer[i];
			vector.x += num;
			verts.buffer[i] = vector;
		}
	}

	private void OnFontChanged()
	{
		MarkAsDirty();
	}

	public void Print(string text, Color32 color, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, bool encoding, SymbolStyle symbolStyle, Alignment alignment, int lineWidth, bool premultiply)
	{
		if (mReplacement != null)
		{
			mReplacement.Print(text, color, verts, uvs, cols, encoding, symbolStyle, alignment, lineWidth, premultiply);
		}
		else
		{
			if (text == null)
			{
				return;
			}
			if (!isValid)
			{
				Debug.LogError("Attempting to print using an invalid font!");
				return;
			}
			bool isDynamic = this.isDynamic;
			if (isDynamic)
			{
				mDynamicFont.textureRebuildCallback = OnFontChanged;
				mDynamicFont.RequestCharactersInTexture(text, mDynamicFontSize, mDynamicFontStyle);
				mDynamicFont.textureRebuildCallback = null;
			}
			mColors.Clear();
			mColors.Add(color);
			int size = this.size;
			Vector2 vector = (size <= 0) ? Vector2.one : new Vector2(1f / (float)size, 1f / (float)size);
			int size2 = verts.size;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = size + mSpacingY;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Vector2 zero3 = Vector2.zero;
			Vector2 zero4 = Vector2.zero;
			float num6 = this.uvRect.width / (float)mFont.texWidth;
			float num7 = mUVRect.height / (float)mFont.texHeight;
			int length = text.Length;
			bool flag = encoding && symbolStyle != 0 && hasSymbols && sprite != null;
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				if (c == '\n')
				{
					if (num2 > num)
					{
						num = num2;
					}
					if (alignment != 0)
					{
						Align(verts, size2, alignment, num2, lineWidth);
						size2 = verts.size;
					}
					num2 = 0;
					num3 += num5;
					num4 = 0;
					continue;
				}
				if (c < ' ')
				{
					num4 = 0;
					continue;
				}
				if (encoding && c == '[')
				{
					int num8 = NGUITools.ParseSymbol(text, i, mColors, premultiply);
					if (num8 > 0)
					{
						color = mColors[mColors.Count - 1];
						i += num8 - 1;
						continue;
					}
				}
				if (!isDynamic)
				{
					BMSymbol bMSymbol = (!flag) ? null : MatchSymbol(text, i, length);
					if (bMSymbol == null)
					{
						BMGlyph glyph = mFont.GetGlyph(c);
						if (glyph == null)
						{
							continue;
						}
						if (num4 != 0)
						{
							num2 += glyph.GetKerning(num4);
						}
						if (c == ' ')
						{
							num2 += mSpacingX + glyph.advance;
							num4 = c;
							continue;
						}
						zero.x = vector.x * (float)(num2 + glyph.offsetX);
						zero.y = (0f - vector.y) * (float)(num3 + glyph.offsetY);
						zero2.x = zero.x + vector.x * (float)glyph.width;
						zero2.y = zero.y - vector.y * (float)glyph.height;
						zero3.x = mUVRect.xMin + num6 * (float)glyph.x;
						zero3.y = mUVRect.yMax - num7 * (float)glyph.y;
						zero4.x = zero3.x + num6 * (float)glyph.width;
						zero4.y = zero3.y - num7 * (float)glyph.height;
						num2 += mSpacingX + glyph.advance;
						num4 = c;
						if (glyph.channel == 0 || glyph.channel == 15)
						{
							for (int j = 0; j < 4; j++)
							{
								cols.Add(color);
							}
						}
						else
						{
							Color c2 = color;
							c2 *= 0.49f;
							switch (glyph.channel)
							{
							case 1:
								c2.b += 0.51f;
								break;
							case 2:
								c2.g += 0.51f;
								break;
							case 4:
								c2.r += 0.51f;
								break;
							case 8:
								c2.a += 0.51f;
								break;
							}
							for (int k = 0; k < 4; k++)
							{
								cols.Add(c2);
							}
						}
					}
					else
					{
						zero.x = vector.x * (float)(num2 + bMSymbol.offsetX);
						zero.y = (0f - vector.y) * (float)(num3 + bMSymbol.offsetY);
						zero2.x = zero.x + vector.x * (float)bMSymbol.width;
						zero2.y = zero.y - vector.y * (float)bMSymbol.height;
						Rect uvRect = bMSymbol.uvRect;
						zero3.x = uvRect.xMin;
						zero3.y = uvRect.yMax;
						zero4.x = uvRect.xMax;
						zero4.y = uvRect.yMin;
						num2 += mSpacingX + bMSymbol.advance;
						i += bMSymbol.length - 1;
						num4 = 0;
						if (symbolStyle == SymbolStyle.Colored)
						{
							for (int l = 0; l < 4; l++)
							{
								cols.Add(color);
							}
						}
						else
						{
							Color32 item = Color.white;
							item.a = color.a;
							for (int m = 0; m < 4; m++)
							{
								cols.Add(item);
							}
						}
					}
					verts.Add(new Vector3(zero2.x, zero.y));
					verts.Add(new Vector3(zero2.x, zero2.y));
					verts.Add(new Vector3(zero.x, zero2.y));
					verts.Add(new Vector3(zero.x, zero.y));
					uvs.Add(new Vector2(zero4.x, zero3.y));
					uvs.Add(new Vector2(zero4.x, zero4.y));
					uvs.Add(new Vector2(zero3.x, zero4.y));
					uvs.Add(new Vector2(zero3.x, zero3.y));
				}
				else if (mDynamicFont.GetCharacterInfo(c, out mChar, mDynamicFontSize, mDynamicFontStyle))
				{
					zero.x = vector.x * ((float)num2 + mChar.vert.xMin);
					zero.y = (0f - vector.y) * ((float)num3 - mChar.vert.yMax + mDynamicFontOffset);
					zero2.x = zero.x + vector.x * mChar.vert.width;
					zero2.y = zero.y - vector.y * mChar.vert.height;
					zero3.x = mChar.uv.xMin;
					zero3.y = mChar.uv.yMin;
					zero4.x = mChar.uv.xMax;
					zero4.y = mChar.uv.yMax;
					num2 += mSpacingX + (int)mChar.width;
					for (int n = 0; n < 4; n++)
					{
						cols.Add(color);
					}
					if (mChar.flipped)
					{
						uvs.Add(new Vector2(zero3.x, zero4.y));
						uvs.Add(new Vector2(zero3.x, zero3.y));
						uvs.Add(new Vector2(zero4.x, zero3.y));
						uvs.Add(new Vector2(zero4.x, zero4.y));
					}
					else
					{
						uvs.Add(new Vector2(zero4.x, zero3.y));
						uvs.Add(new Vector2(zero3.x, zero3.y));
						uvs.Add(new Vector2(zero3.x, zero4.y));
						uvs.Add(new Vector2(zero4.x, zero4.y));
					}
					verts.Add(new Vector3(zero2.x, zero.y));
					verts.Add(new Vector3(zero.x, zero.y));
					verts.Add(new Vector3(zero.x, zero2.y));
					verts.Add(new Vector3(zero2.x, zero2.y));
				}
			}
			if (alignment != 0 && size2 < verts.size)
			{
				Align(verts, size2, alignment, num2, lineWidth);
				size2 = verts.size;
			}
		}
	}

	private BMSymbol GetSymbol(string sequence, bool createIfMissing)
	{
		int i = 0;
		for (int count = mSymbols.Count; i < count; i++)
		{
			BMSymbol bMSymbol = mSymbols[i];
			if (bMSymbol.sequence == sequence)
			{
				return bMSymbol;
			}
		}
		if (createIfMissing)
		{
			BMSymbol bMSymbol2 = new BMSymbol();
			bMSymbol2.sequence = sequence;
			mSymbols.Add(bMSymbol2);
			return bMSymbol2;
		}
		return null;
	}

	private BMSymbol MatchSymbol(string text, int offset, int textLength)
	{
		int count = mSymbols.Count;
		if (count == 0)
		{
			return null;
		}
		textLength -= offset;
		for (int i = 0; i < count; i++)
		{
			BMSymbol bMSymbol = mSymbols[i];
			int length = bMSymbol.length;
			if (length == 0 || textLength < length)
			{
				continue;
			}
			bool flag = true;
			for (int j = 0; j < length; j++)
			{
				if (text[offset + j] != bMSymbol.sequence[j])
				{
					flag = false;
					break;
				}
			}
			if (flag && bMSymbol.Validate(atlas))
			{
				return bMSymbol;
			}
		}
		return null;
	}

	public void AddSymbol(string sequence, string spriteName)
	{
		BMSymbol symbol = GetSymbol(sequence, createIfMissing: true);
		symbol.spriteName = spriteName;
		MarkAsDirty();
	}

	public void RemoveSymbol(string sequence)
	{
		BMSymbol symbol = GetSymbol(sequence, createIfMissing: false);
		if (symbol != null)
		{
			symbols.Remove(symbol);
		}
		MarkAsDirty();
	}

	public void RenameSymbol(string before, string after)
	{
		BMSymbol symbol = GetSymbol(before, createIfMissing: false);
		if (symbol != null)
		{
			symbol.sequence = after;
		}
		MarkAsDirty();
	}

	public bool UsesSprite(string s)
	{
		if (!string.IsNullOrEmpty(s))
		{
			if (s.Equals(spriteName))
			{
				return true;
			}
			int i = 0;
			for (int count = symbols.Count; i < count; i++)
			{
				BMSymbol bMSymbol = symbols[i];
				if (s.Equals(bMSymbol.spriteName))
				{
					return true;
				}
			}
		}
		return false;
	}
}
