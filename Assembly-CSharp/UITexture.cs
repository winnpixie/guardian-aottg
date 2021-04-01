using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Texture")]
public class UITexture : UIWidget
{
	[HideInInspector]
	[SerializeField]
	private Rect mRect = new Rect(0f, 0f, 1f, 1f);

	[SerializeField]
	[HideInInspector]
	private Shader mShader;

	[SerializeField]
	[HideInInspector]
	private Texture mTexture;

	private Material mDynamicMat;

	private bool mCreatingMat;

	private int mPMA = -1;

	public Rect uvRect
	{
		get
		{
			return mRect;
		}
		set
		{
			if (mRect != value)
			{
				mRect = value;
				MarkAsChanged();
			}
		}
	}

	public Shader shader
	{
		get
		{
			if (mShader == null)
			{
				Material material = this.material;
				if (material != null)
				{
					mShader = material.shader;
				}
				if (mShader == null)
				{
					mShader = Shader.Find("Unlit/Texture");
				}
			}
			return mShader;
		}
		set
		{
			if (mShader != value)
			{
				mShader = value;
				Material material = this.material;
				if (material != null)
				{
					material.shader = value;
				}
				mPMA = -1;
			}
		}
	}

	public bool hasDynamicMaterial => mDynamicMat != null;

	public override bool keepMaterial => true;

	public override Material material
	{
		get
		{
			if (!mCreatingMat && mMat == null)
			{
				mCreatingMat = true;
				if (mainTexture != null)
				{
					if (mShader == null)
					{
						mShader = Shader.Find("Unlit/Texture");
					}
					mDynamicMat = new Material(mShader);
					mDynamicMat.hideFlags = HideFlags.DontSave;
					mDynamicMat.mainTexture = mainTexture;
					base.material = mDynamicMat;
					mPMA = 0;
				}
				mCreatingMat = false;
			}
			return mMat;
		}
		set
		{
			if (mDynamicMat != value && mDynamicMat != null)
			{
				NGUITools.Destroy(mDynamicMat);
				mDynamicMat = null;
			}
			base.material = value;
			mPMA = -1;
		}
	}

	public bool premultipliedAlpha
	{
		get
		{
			if (mPMA == -1)
			{
				Material material = this.material;
				mPMA = ((material != null && material.shader != null && material.shader.name.Contains("Premultiplied")) ? 1 : 0);
			}
			return mPMA == 1;
		}
	}

	public override Texture mainTexture
	{
		get
		{
			return mTexture == null ? base.mainTexture : mTexture;
		}
		set
		{
			if (mPanel != null && mMat != null)
			{
				mPanel.RemoveWidget(this);
			}
			if (mMat == null)
			{
				mDynamicMat = new Material(shader);
				mDynamicMat.hideFlags = HideFlags.DontSave;
				mMat = mDynamicMat;
			}
			mPanel = null;
			mTex = value;
			mTexture = value;
			mMat.mainTexture = value;
			if (base.enabled)
			{
				CreatePanel();
			}
		}
	}

	private void OnDestroy()
	{
		NGUITools.Destroy(mDynamicMat);
	}

	public override void MakePixelPerfect()
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture != null)
		{
			Vector3 localScale = base.cachedTransform.localScale;
			localScale.x = (float)mainTexture.width * uvRect.width;
			localScale.y = (float)mainTexture.height * uvRect.height;
			localScale.z = 1f;
			base.cachedTransform.localScale = localScale;
		}
		base.MakePixelPerfect();
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Color color = base.color;
		color.a *= mPanel.alpha;
		Color32 item = (!premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
		verts.Add(new Vector3(1f, 0f, 0f));
		verts.Add(new Vector3(1f, -1f, 0f));
		verts.Add(new Vector3(0f, -1f, 0f));
		verts.Add(new Vector3(0f, 0f, 0f));
		uvs.Add(new Vector2(mRect.xMax, mRect.yMax));
		uvs.Add(new Vector2(mRect.xMax, mRect.yMin));
		uvs.Add(new Vector2(mRect.xMin, mRect.yMin));
		uvs.Add(new Vector2(mRect.xMin, mRect.yMax));
		cols.Add(item);
		cols.Add(item);
		cols.Add(item);
		cols.Add(item);
	}
}
