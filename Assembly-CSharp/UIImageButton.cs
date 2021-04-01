using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour
{
	public UISprite target;

	public string normalSprite;

	public string hoverSprite;

	public string pressedSprite;

	public string disabledSprite;

	public bool isEnabled
	{
		get
		{
			Collider collider = base.collider;
			return (bool)collider && collider.enabled;
		}
		set
		{
			Collider collider = base.collider;
			if ((bool)collider && collider.enabled != value)
			{
				collider.enabled = value;
				UpdateImage();
			}
		}
	}

	private void Awake()
	{
		if (target == null)
		{
			target = GetComponentInChildren<UISprite>();
		}
	}

	private void OnEnable()
	{
		UpdateImage();
	}

	private void UpdateImage()
	{
		if (target != null)
		{
			if (isEnabled)
			{
				target.spriteName = ((!UICamera.IsHighlighted(base.gameObject)) ? normalSprite : hoverSprite);
			}
			else
			{
				target.spriteName = disabledSprite;
			}
			target.MakePixelPerfect();
		}
	}

	private void OnHover(bool isOver)
	{
		if (isEnabled && target != null)
		{
			target.spriteName = ((!isOver) ? normalSprite : hoverSprite);
			target.MakePixelPerfect();
		}
	}

	private void OnPress(bool pressed)
	{
		if (pressed)
		{
			target.spriteName = pressedSprite;
			target.MakePixelPerfect();
		}
		else
		{
			UpdateImage();
		}
	}
}
