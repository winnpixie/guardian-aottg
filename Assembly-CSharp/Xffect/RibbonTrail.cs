using UnityEngine;

public class RibbonTrail
{
    public class Element
    {
        public Vector3 Position;
        public float Width;

        public Element(Vector3 position, float width)
        {
            Position = position;
            Width = width;
        }
    }

    public const int CHAIN_EMPTY = 99999;
    protected VertexPool.VertexSegment Vertexsegment;
    public int MaxElements;
    public Element[] ElementArray;
    public int Head;
    public int Tail;
    protected Vector3 HeadPosition;
    protected float TrailLength;
    protected float ElemLength;
    public float SquaredElemLength;
    protected float UnitWidth;
    protected bool IndexDirty;
    protected Vector2 LowerLeftUV;
    protected Vector2 UVDimensions;
    protected Color Color = Color.white;
    public int ElemCount;
    protected int StretchType;
    protected float ElapsedTime;
    protected float Fps;

    public RibbonTrail(VertexPool.VertexSegment segment, float width, int maxelemnt, float len, Vector3 pos, int stretchType, float maxFps)
    {
        if (maxelemnt <= 2)
        {
            Debug.LogError("ribbon trail's maxelement should > 2!");
        }
        MaxElements = maxelemnt;
        Vertexsegment = segment;
        ElementArray = new Element[MaxElements];
        Head = (Tail = 99999);
        SetTrailLen(len);
        UnitWidth = width;
        HeadPosition = pos;
        StretchType = stretchType;
        Element dtls = new Element(HeadPosition, UnitWidth);
        IndexDirty = false;
        Fps = 1f / maxFps;
        AddElememt(dtls);
        Element dtls2 = new Element(HeadPosition, UnitWidth);
        AddElememt(dtls2);
    }

    public void ResetElementsPos()
    {
        if (Head == 99999 || Head == Tail)
        {
            return;
        }
        int num = Head;
        while (true)
        {
            int num2 = num;
            if (num2 == MaxElements)
            {
                num2 = 0;
            }
            ElementArray[num2].Position = HeadPosition;
            if (num2 == Tail)
            {
                break;
            }
            num = num2 + 1;
        }
    }

    public void Reset()
    {
        ResetElementsPos();
    }

    public void SetUVCoord(Vector2 lowerleft, Vector2 dimensions)
    {
        LowerLeftUV = lowerleft;
        UVDimensions = dimensions;
    }

    public void SetColor(Color color)
    {
        Color = color;
    }

    public void SetTrailLen(float len)
    {
        TrailLength = len;
        ElemLength = TrailLength / (float)(MaxElements - 1);
        SquaredElemLength = ElemLength * ElemLength;
    }

    public void SetHeadPosition(Vector3 pos)
    {
        HeadPosition = pos;
    }

    public void Smooth()
    {
        if (ElemCount > 3)
        {
            Element element = ElementArray[Head];
            int num = Head + 1;
            if (num == MaxElements)
            {
                num = 0;
            }
            int num2 = num + 1;
            if (num2 == MaxElements)
            {
                num2 = 0;
            }
            Element element2 = ElementArray[num];
            Element element3 = ElementArray[num2];
            Vector3 from = element.Position - element2.Position;
            Vector3 to = element2.Position - element3.Position;
            float num3 = Vector3.Angle(from, to);
            if (num3 > 60f)
            {
                Vector3 a = (element.Position + element3.Position) / 2f;
                Vector3 vector = a - element2.Position;
                Vector3 currentVelocity = Vector3.zero;
                float smoothTime = 0.1f / (num3 / 60f);
                element2.Position = Vector3.SmoothDamp(element2.Position, element2.Position + vector.normalized * element2.Width, ref currentVelocity, smoothTime);
            }
        }
    }

    public void Update()
    {
        ElapsedTime += Time.deltaTime;
        if (ElapsedTime < Fps)
        {
            return;
        }
        ElapsedTime -= Fps;
        bool flag = false;
        while (!flag)
        {
            Element element = ElementArray[Head];
            int num = Head + 1;
            if (num == MaxElements)
            {
                num = 0;
            }
            Element element2 = ElementArray[num];
            Vector3 headPosition = HeadPosition;
            Vector3 a = headPosition - element2.Position;
            float sqrMagnitude = a.sqrMagnitude;
            if (sqrMagnitude >= SquaredElemLength)
            {
                Vector3 b = a * (ElemLength / a.magnitude);
                element.Position = element2.Position + b;
                Element dtls = new Element(headPosition, UnitWidth);
                AddElememt(dtls);
                a = headPosition - element.Position;
                if (a.sqrMagnitude <= SquaredElemLength)
                {
                    flag = true;
                }
            }
            else
            {
                element.Position = headPosition;
                flag = true;
            }
            if ((Tail + 1) % MaxElements == Head)
            {
                Element element3 = ElementArray[Tail];
                int num2 = (Tail != 0) ? (Tail - 1) : (MaxElements - 1);
                Element element4 = ElementArray[num2];
                Vector3 b2 = element3.Position - element4.Position;
                float magnitude = b2.magnitude;
                if ((double)magnitude > 1E-06)
                {
                    float num3 = ElemLength - a.magnitude;
                    b2 *= num3 / magnitude;
                    element3.Position = element4.Position + b2;
                }
            }
        }
        Vector3 position = Camera.main.transform.position;
        UpdateVertices(position);
        UpdateIndices();
    }

    public void UpdateIndices()
    {
        if (!IndexDirty)
        {
            return;
        }
        VertexPool pool = Vertexsegment.Pool;
        if (Head != 99999 && Head != Tail)
        {
            int num = Head;
            int num2 = 0;
            while (true)
            {
                int num3 = num + 1;
                if (num3 == MaxElements)
                {
                    num3 = 0;
                }
                if (num3 * 2 >= 65536)
                {
                    Debug.LogError("Too many elements!");
                }
                int num4 = Vertexsegment.VertStart + num3 * 2;
                int num5 = Vertexsegment.VertStart + num * 2;
                int num6 = Vertexsegment.IndexStart + num2 * 6;
                pool.Indices[num6] = num5;
                pool.Indices[num6 + 1] = num5 + 1;
                pool.Indices[num6 + 2] = num4;
                pool.Indices[num6 + 3] = num5 + 1;
                pool.Indices[num6 + 4] = num4 + 1;
                pool.Indices[num6 + 5] = num4;
                if (num3 == Tail)
                {
                    break;
                }
                num = num3;
                num2++;
            }
            pool.IndiceChanged = true;
        }
        IndexDirty = false;
    }

    public void UpdateVertices(Vector3 eyePos)
    {
        float num = 0f;
        float num2 = 0f;
        float num3 = ElemLength * (float)(MaxElements - 2);
        if (Head == 99999 || Head == Tail)
        {
            return;
        }
        int num4 = Head;
        int num5 = Head;
        while (true)
        {
            if (num5 == MaxElements)
            {
                num5 = 0;
            }
            Element element = ElementArray[num5];
            if (num5 * 2 >= 65536)
            {
                Debug.LogError("Too many elements!");
            }
            int num6 = Vertexsegment.VertStart + num5 * 2;
            int num7 = num5 + 1;
            if (num7 == MaxElements)
            {
                num7 = 0;
            }
            Vector3 lhs = (num5 == Head) ? (ElementArray[num7].Position - element.Position) : ((num5 != Tail) ? (ElementArray[num7].Position - ElementArray[num4].Position) : (element.Position - ElementArray[num4].Position));
            Vector3 rhs = eyePos - element.Position;
            Vector3 b = Vector3.Cross(lhs, rhs);
            b.Normalize();
            b *= element.Width * 0.5f;
            Vector3 vector = element.Position - b;
            Vector3 vector2 = element.Position + b;
            VertexPool pool = Vertexsegment.Pool;
            num = ((StretchType != 0) ? (num2 / num3 * Mathf.Abs(UVDimensions.x)) : (num2 / num3 * Mathf.Abs(UVDimensions.y)));
            Vector2 zero = Vector2.zero;
            pool.Vertices[num6] = vector;
            pool.Colors[num6] = Color;
            if (StretchType == 0)
            {
                zero.x = LowerLeftUV.x + UVDimensions.x;
                zero.y = LowerLeftUV.y - num;
            }
            else
            {
                zero.x = LowerLeftUV.x + num;
                zero.y = LowerLeftUV.y;
            }
            pool.UVs[num6] = zero;
            pool.Vertices[num6 + 1] = vector2;
            pool.Colors[num6 + 1] = Color;
            if (StretchType == 0)
            {
                zero.x = LowerLeftUV.x;
                zero.y = LowerLeftUV.y - num;
            }
            else
            {
                zero.x = LowerLeftUV.x + num;
                zero.y = LowerLeftUV.y - Mathf.Abs(UVDimensions.y);
            }
            pool.UVs[num6 + 1] = zero;
            if (num5 == Tail)
            {
                break;
            }
            num4 = num5;
            num2 += (ElementArray[num7].Position - element.Position).magnitude;
            num5++;
        }
        Vertexsegment.Pool.UVChanged = true;
        Vertexsegment.Pool.VertChanged = true;
        Vertexsegment.Pool.ColorChanged = true;
    }

    public void AddElememt(Element dtls)
    {
        if (Head == 99999)
        {
            Tail = MaxElements - 1;
            Head = Tail;
            IndexDirty = true;
            ElemCount++;
        }
        else
        {
            if (Head == 0)
            {
                Head = MaxElements - 1;
            }
            else
            {
                Head--;
            }
            if (Head == Tail)
            {
                if (Tail == 0)
                {
                    Tail = MaxElements - 1;
                }
                else
                {
                    Tail--;
                }
            }
            else
            {
                ElemCount++;
            }
        }
        ElementArray[Head] = dtls;
        IndexDirty = true;
    }
}
