using UnityEngine;

[RequireComponent(typeof(UILabel))]
[AddComponentMenu("NGUI/Examples/Typewriter Effect")]
public class TypewriterEffect : MonoBehaviour
{
	public int charsPerSecond = 40;

	private UILabel mLabel;

	private string mText;

	private int mOffset;

	private float mNextChar;

	private void Update()
	{
		if (mLabel == null)
		{
			mLabel = GetComponent<UILabel>();
			mLabel.supportEncoding = false;
			mLabel.symbolStyle = UIFont.SymbolStyle.None;
			UIFont font = mLabel.font;
			string text = mLabel.text;
			float num = mLabel.lineWidth;
			Vector3 localScale = mLabel.cachedTransform.localScale;
			mText = font.WrapText(text, num / localScale.x, mLabel.maxLineCount, encoding: false, UIFont.SymbolStyle.None);
		}
		if (mOffset < mText.Length)
		{
			if (mNextChar <= Time.time)
			{
				charsPerSecond = Mathf.Max(1, charsPerSecond);
				float num2 = 1f / (float)charsPerSecond;
				char c = mText[mOffset];
				if (c == '.' || c == '\n' || c == '!' || c == '?')
				{
					num2 *= 4f;
				}
				mNextChar = Time.time + num2;
				mLabel.text = mText.Substring(0, ++mOffset);
			}
		}
		else
		{
			Object.Destroy(this);
		}
	}
}
