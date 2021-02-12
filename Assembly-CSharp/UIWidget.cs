using System;
using UnityEngine;

public abstract class UIWidget : MonoBehaviour
{
	public enum Pivot
	{
		TopLeft,
		Top,
		TopRight,
		Left,
		Center,
		Right,
		BottomLeft,
		Bottom,
		BottomRight
	}

	[HideInInspector]
	[SerializeField]
	protected Material mMat;

	[SerializeField]
	[HideInInspector]
	protected Texture mTex;

	[SerializeField]
	[HideInInspector]
	private Color mColor = Color.white;

	[HideInInspector]
	[SerializeField]
	private Pivot mPivot = Pivot.Center;

	[SerializeField]
	[HideInInspector]
	private int mDepth;

	protected GameObject mGo;

	protected Transform mTrans;

	protected UIPanel mPanel;

	protected bool mChanged = true;

	protected bool mPlayMode = true;

	private Matrix4x4 mLocalToPanel;

	private bool mVisibleByPanel = true;

	private float mLastAlpha;

	private UIGeometry mGeom = new UIGeometry();

	private bool mForceVisible;

	private Vector3 mOldV0;

	private Vector3 mOldV1;

	public bool isVisible => mVisibleByPanel && finalAlpha > 0.001f;

	public Color color
	{
		get
		{
			return mColor;
		}
		set
		{
			if (!mColor.Equals(value))
			{
				mColor = value;
				mChanged = true;
			}
		}
	}

	public float alpha
	{
		get
		{
			return mColor.a;
		}
		set
		{
			Color color = mColor;
			color.a = value;
			this.color = color;
		}
	}

	public float finalAlpha
	{
		get
		{
			if (mPanel == null)
			{
				CreatePanel();
			}
			return mPanel != null ? mColor.a : (mColor.a * mPanel.alpha);
		}
	}

	public Pivot pivot
	{
		get
		{
			return mPivot;
		}
		set
		{
			if (mPivot != value)
			{
				Vector3 vector = NGUIMath.CalculateWidgetCorners(this)[0];
				mPivot = value;
				mChanged = true;
				Vector3 vector2 = NGUIMath.CalculateWidgetCorners(this)[0];
				Transform cachedTransform = this.cachedTransform;
				Vector3 vector3 = cachedTransform.position;
				Vector3 localPosition = cachedTransform.localPosition;
				float z = localPosition.z;
				vector3.x += vector.x - vector2.x;
				vector3.y += vector.y - vector2.y;
				this.cachedTransform.position = vector3;
				vector3 = this.cachedTransform.localPosition;
				vector3.x = Mathf.Round(vector3.x);
				vector3.y = Mathf.Round(vector3.y);
				vector3.z = z;
				this.cachedTransform.localPosition = vector3;
			}
		}
	}

	public int depth
	{
		get
		{
			return mDepth;
		}
		set
		{
			if (mDepth != value)
			{
				mDepth = value;
				if (mPanel != null)
				{
					mPanel.MarkMaterialAsChanged(material, sort: true);
				}
			}
		}
	}

	public Vector2 pivotOffset
	{
		get
		{
			Vector2 zero = Vector2.zero;
			Vector4 relativePadding = this.relativePadding;
			Pivot pivot = this.pivot;
			switch (pivot)
			{
			case Pivot.Top:
			case Pivot.Center:
			case Pivot.Bottom:
				zero.x = (relativePadding.x - relativePadding.z - 1f) * 0.5f;
				break;
			case Pivot.TopRight:
			case Pivot.Right:
			case Pivot.BottomRight:
				zero.x = -1f - relativePadding.z;
				break;
			default:
				zero.x = relativePadding.x;
				break;
			}
			switch (pivot)
			{
			case Pivot.Left:
			case Pivot.Center:
			case Pivot.Right:
				zero.y = (relativePadding.w - relativePadding.y + 1f) * 0.5f;
				break;
			case Pivot.BottomLeft:
			case Pivot.Bottom:
			case Pivot.BottomRight:
				zero.y = 1f + relativePadding.w;
				break;
			default:
				zero.y = 0f - relativePadding.y;
				break;
			}
			return zero;
		}
	}

	public GameObject cachedGameObject
	{
		get
		{
			if (mGo == null)
			{
				mGo = base.gameObject;
			}
			return mGo;
		}
	}

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

	public virtual Material material
	{
		get
		{
			return mMat;
		}
		set
		{
			if (mMat != value)
			{
				if (mMat != null && mPanel != null)
				{
					mPanel.RemoveWidget(this);
				}
				mPanel = null;
				mMat = value;
				mTex = null;
				if (mMat != null)
				{
					CreatePanel();
				}
			}
		}
	}

	public virtual Texture mainTexture
	{
		get
		{
			Material material = this.material;
			if (material != null)
			{
				if (material.mainTexture != null)
				{
					mTex = material.mainTexture;
				}
				else if (mTex != null)
				{
					if (mPanel != null)
					{
						mPanel.RemoveWidget(this);
					}
					mPanel = null;
					mMat.mainTexture = mTex;
					if (base.enabled)
					{
						CreatePanel();
					}
				}
			}
			return mTex;
		}
		set
		{
			Material material = this.material;
			if (!(material == null) && !(material.mainTexture != value))
			{
				return;
			}
			if (mPanel != null)
			{
				mPanel.RemoveWidget(this);
			}
			mPanel = null;
			mTex = value;
			material = this.material;
			if (material != null)
			{
				material.mainTexture = value;
				if (base.enabled)
				{
					CreatePanel();
				}
			}
		}
	}

	public UIPanel panel
	{
		get
		{
			if (mPanel == null)
			{
				CreatePanel();
			}
			return mPanel;
		}
		set
		{
			mPanel = value;
		}
	}

	public virtual Vector2 relativeSize => Vector2.one;

	public virtual Vector4 relativePadding => Vector4.zero;

	public virtual Vector4 border => Vector4.zero;

	public virtual bool keepMaterial => false;

	public virtual bool pixelPerfectAfterResize => false;

	public static BetterList<UIWidget> Raycast(GameObject root, Vector2 mousePos)
	{
		BetterList<UIWidget> betterList = new BetterList<UIWidget>();
		UICamera uICamera = UICamera.FindCameraForLayer(root.layer);
		if (uICamera != null)
		{
			Camera cachedCamera = uICamera.cachedCamera;
			UIWidget[] componentsInChildren = root.GetComponentsInChildren<UIWidget>();
			foreach (UIWidget uIWidget in componentsInChildren)
			{
				Vector3[] worldPoints = NGUIMath.CalculateWidgetCorners(uIWidget);
				if (NGUIMath.DistanceToRectangle(worldPoints, mousePos, cachedCamera) == 0f)
				{
					betterList.Add(uIWidget);
				}
			}
			betterList.Sort((UIWidget w1, UIWidget w2) => w2.mDepth.CompareTo(w1.mDepth));
		}
		return betterList;
	}

	public static int CompareFunc(UIWidget left, UIWidget right)
	{
		if (left.mDepth > right.mDepth)
		{
			return 1;
		}
		if (left.mDepth < right.mDepth)
		{
			return -1;
		}
		return 0;
	}

	public void MarkAsChangedLite()
	{
		mChanged = true;
	}

	public virtual void MarkAsChanged()
	{
		mChanged = true;
		if (mPanel != null && base.enabled && NGUITools.GetActive(base.gameObject) && !Application.isPlaying && material != null)
		{
			mPanel.AddWidget(this);
			CheckLayer();
		}
	}

	public void CreatePanel()
	{
		if (mPanel == null && base.enabled && NGUITools.GetActive(base.gameObject) && material != null)
		{
			mPanel = UIPanel.Find(cachedTransform);
			if (mPanel != null)
			{
				CheckLayer();
				mPanel.AddWidget(this);
				mChanged = true;
			}
		}
	}

	public void CheckLayer()
	{
		if (mPanel != null && mPanel.gameObject.layer != base.gameObject.layer)
		{
			Debug.LogWarning("You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", this);
			base.gameObject.layer = mPanel.gameObject.layer;
		}
	}

	[Obsolete("Use ParentHasChanged() instead")]
	public void CheckParent()
	{
		ParentHasChanged();
	}

	public void ParentHasChanged()
	{
		if (mPanel == null)
		{
			return;
		}
		UIPanel y = UIPanel.Find(cachedTransform);
		if (mPanel != y)
		{
			mPanel.RemoveWidget(this);
			if (!keepMaterial || Application.isPlaying)
			{
				material = null;
			}
			mPanel = null;
			CreatePanel();
		}
	}

	protected virtual void Awake()
	{
		mGo = base.gameObject;
		mPlayMode = Application.isPlaying;
	}

	protected virtual void OnEnable()
	{
		mChanged = true;
		if (!keepMaterial)
		{
			mMat = null;
			mTex = null;
		}
		mPanel = null;
	}

	private void Start()
	{
		OnStart();
		CreatePanel();
	}

	public virtual void Update()
	{
		if (mPanel == null)
		{
			CreatePanel();
		}
	}

	private void OnDisable()
	{
		if (!keepMaterial)
		{
			material = null;
		}
		else if (mPanel != null)
		{
			mPanel.RemoveWidget(this);
		}
		mPanel = null;
	}

	private void OnDestroy()
	{
		if (mPanel != null)
		{
			mPanel.RemoveWidget(this);
			mPanel = null;
		}
	}

	public bool UpdateGeometry(UIPanel p, bool forceVisible)
	{
		if (material != null && p != null)
		{
			mPanel = p;
			bool flag = false;
			float finalAlpha = this.finalAlpha;
			bool flag2 = finalAlpha > 0.001f;
			bool flag3 = forceVisible || mVisibleByPanel;
			if (cachedTransform.hasChanged)
			{
				mTrans.hasChanged = false;
				if (!mPanel.widgetsAreStatic)
				{
					Vector2 relativeSize = this.relativeSize;
					Vector2 pivotOffset = this.pivotOffset;
					Vector4 relativePadding = this.relativePadding;
					float num = pivotOffset.x * relativeSize.x - relativePadding.x;
					float num2 = pivotOffset.y * relativeSize.y + relativePadding.y;
					float x = num + relativeSize.x + relativePadding.x + relativePadding.z;
					float y = num2 - relativeSize.y - relativePadding.y - relativePadding.w;
					mLocalToPanel = p.worldToLocal * cachedTransform.localToWorldMatrix;
					flag = true;
					Vector3 v = new Vector3(num, num2, 0f);
					Vector3 v2 = new Vector3(x, y, 0f);
					v = mLocalToPanel.MultiplyPoint3x4(v);
					v2 = mLocalToPanel.MultiplyPoint3x4(v2);
					if (Vector3.SqrMagnitude(mOldV0 - v) > 1E-06f || Vector3.SqrMagnitude(mOldV1 - v2) > 1E-06f)
					{
						mChanged = true;
						mOldV0 = v;
						mOldV1 = v2;
					}
				}
				if (flag2 || mForceVisible != forceVisible)
				{
					mForceVisible = forceVisible;
					flag3 = (forceVisible || mPanel.IsVisible(this));
				}
			}
			else if (flag2 && mForceVisible != forceVisible)
			{
				mForceVisible = forceVisible;
				flag3 = mPanel.IsVisible(this);
			}
			if (mVisibleByPanel != flag3)
			{
				mVisibleByPanel = flag3;
				mChanged = true;
			}
			if (mVisibleByPanel && mLastAlpha != finalAlpha)
			{
				mChanged = true;
			}
			mLastAlpha = finalAlpha;
			if (mChanged)
			{
				mChanged = false;
				if (isVisible)
				{
					mGeom.Clear();
					OnFill(mGeom.verts, mGeom.uvs, mGeom.cols);
					if (mGeom.hasVertices)
					{
						Vector3 pivotOffset2 = this.pivotOffset;
						Vector2 relativeSize2 = this.relativeSize;
						pivotOffset2.x *= relativeSize2.x;
						pivotOffset2.y *= relativeSize2.y;
						if (!flag)
						{
							mLocalToPanel = p.worldToLocal * cachedTransform.localToWorldMatrix;
						}
						mGeom.ApplyOffset(pivotOffset2);
						mGeom.ApplyTransform(mLocalToPanel, p.generateNormals);
					}
					return true;
				}
				if (mGeom.hasVertices)
				{
					mGeom.Clear();
					return true;
				}
			}
		}
		return false;
	}

	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		mGeom.WriteToBuffers(v, u, c, n, t);
	}

	public virtual void MakePixelPerfect()
	{
		Vector3 localScale = cachedTransform.localScale;
		int num = Mathf.RoundToInt(localScale.x);
		int num2 = Mathf.RoundToInt(localScale.y);
		localScale.x = num;
		localScale.y = num2;
		localScale.z = 1f;
		Vector3 localPosition = cachedTransform.localPosition;
		localPosition.z = Mathf.RoundToInt(localPosition.z);
		if (num % 2 == 1 && (pivot == Pivot.Top || pivot == Pivot.Center || pivot == Pivot.Bottom))
		{
			localPosition.x = Mathf.Floor(localPosition.x) + 0.5f;
		}
		else
		{
			localPosition.x = Mathf.Round(localPosition.x);
		}
		if (num2 % 2 == 1 && (pivot == Pivot.Left || pivot == Pivot.Center || pivot == Pivot.Right))
		{
			localPosition.y = Mathf.Ceil(localPosition.y) - 0.5f;
		}
		else
		{
			localPosition.y = Mathf.Round(localPosition.y);
		}
		cachedTransform.localPosition = localPosition;
		cachedTransform.localScale = localScale;
	}

	protected virtual void OnStart()
	{
	}

	public virtual void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
	}
}
