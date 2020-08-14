using UnityEngine;

[AddComponentMenu("NGUI/UI/Anchor")]
[ExecuteInEditMode]
public class UIAnchor : MonoBehaviour
{
    public enum Side
    {
        BottomLeft,
        Left,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        Center
    }

    private bool mNeedsHalfPixelOffset;
    public Camera uiCamera;
    public UIWidget widgetContainer;
    public UIPanel panelContainer;
    public Side side = Side.Center;
    public bool halfPixelOffset = true;
    public bool runOnlyOnce;
    public Vector2 relativeOffset = Vector2.zero;
    private Transform mTrans;
    private Animation mAnim;
    private Rect mRect = default(Rect);
    private UIRoot mRoot;

    private void Awake()
    {
        mTrans = base.transform;
        mAnim = base.animation;
    }

    private void Start()
    {
        mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
        mNeedsHalfPixelOffset = (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.XBOX360 || Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.WindowsEditor);
        if (mNeedsHalfPixelOffset)
        {
            mNeedsHalfPixelOffset = (SystemInfo.graphicsShaderLevel < 40);
        }
        if (uiCamera == null)
        {
            uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
        }
        Update();
    }

    private void Update()
    {
        if (mAnim != null && mAnim.enabled && mAnim.isPlaying)
        {
            return;
        }
        bool flag = false;
        if (panelContainer != null)
        {
            if (panelContainer.clipping == UIDrawCall.Clipping.None)
            {
                float num = (!(mRoot != null)) ? 0.5f : ((float)mRoot.activeHeight / (float)Screen.height * 0.5f);
                mRect.xMin = (float)(-Screen.width) * num;
                mRect.yMin = (float)(-Screen.height) * num;
                mRect.xMax = 0f - mRect.xMin;
                mRect.yMax = 0f - mRect.yMin;
            }
            else
            {
                Vector4 clipRange = panelContainer.clipRange;
                mRect.x = clipRange.x - clipRange.z * 0.5f;
                mRect.y = clipRange.y - clipRange.w * 0.5f;
                mRect.width = clipRange.z;
                mRect.height = clipRange.w;
            }
        }
        else if (widgetContainer != null)
        {
            Transform cachedTransform = widgetContainer.cachedTransform;
            Vector3 localScale = cachedTransform.localScale;
            Vector3 localPosition = cachedTransform.localPosition;
            Vector3 vector = widgetContainer.relativeSize;
            Vector3 vector2 = widgetContainer.pivotOffset;
            vector2.y -= 1f;
            float x = vector2.x;
            Vector2 relativeSize = widgetContainer.relativeSize;
            vector2.x = x * (relativeSize.x * localScale.x);
            float y = vector2.y;
            Vector2 relativeSize2 = widgetContainer.relativeSize;
            vector2.y = y * (relativeSize2.y * localScale.y);
            mRect.x = localPosition.x + vector2.x;
            mRect.y = localPosition.y + vector2.y;
            mRect.width = vector.x * localScale.x;
            mRect.height = vector.y * localScale.y;
        }
        else
        {
            if (!(uiCamera != null))
            {
                return;
            }
            flag = true;
            mRect = uiCamera.pixelRect;
        }
        float x2 = (mRect.xMin + mRect.xMax) * 0.5f;
        float y2 = (mRect.yMin + mRect.yMax) * 0.5f;
        Vector3 vector3 = new Vector3(x2, y2, 0f);
        if (side != Side.Center)
        {
            if (side == Side.Right || side == Side.TopRight || side == Side.BottomRight)
            {
                vector3.x = mRect.xMax;
            }
            else if (side == Side.Top || side == Side.Center || side == Side.Bottom)
            {
                vector3.x = x2;
            }
            else
            {
                vector3.x = mRect.xMin;
            }
            if (side == Side.Top || side == Side.TopRight || side == Side.TopLeft)
            {
                vector3.y = mRect.yMax;
            }
            else if (side == Side.Left || side == Side.Center || side == Side.Right)
            {
                vector3.y = y2;
            }
            else
            {
                vector3.y = mRect.yMin;
            }
        }
        float width = mRect.width;
        float height = mRect.height;
        vector3.x += relativeOffset.x * width;
        vector3.y += relativeOffset.y * height;
        if (flag)
        {
            if (uiCamera.orthographic)
            {
                vector3.x = Mathf.Round(vector3.x);
                vector3.y = Mathf.Round(vector3.y);
                if (halfPixelOffset && mNeedsHalfPixelOffset)
                {
                    vector3.x -= 0.5f;
                    vector3.y += 0.5f;
                }
            }
            Vector3 vector4 = uiCamera.WorldToScreenPoint(mTrans.position);
            vector3.z = vector4.z;
            vector3 = uiCamera.ScreenToWorldPoint(vector3);
        }
        else
        {
            vector3.x = Mathf.Round(vector3.x);
            vector3.y = Mathf.Round(vector3.y);
            if (panelContainer != null)
            {
                vector3 = panelContainer.cachedTransform.TransformPoint(vector3);
            }
            else if (widgetContainer != null)
            {
                Transform parent = widgetContainer.cachedTransform.parent;
                if (parent != null)
                {
                    vector3 = parent.TransformPoint(vector3);
                }
            }
            Vector3 position = mTrans.position;
            vector3.z = position.z;
        }
        if (mTrans.position != vector3)
        {
            mTrans.position = vector3;
        }
        if (runOnlyOnce && Application.isPlaying)
        {
            Object.Destroy(this);
        }
    }
}
