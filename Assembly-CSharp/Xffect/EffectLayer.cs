using System.Collections;
using UnityEngine;

public class EffectLayer : MonoBehaviour
{
    public VertexPool Vertexpool;
    protected Camera MainCamera;
    public Transform ClientTransform;
    public bool SyncClient;
    public Material Material;
    public int RenderType;
    public float StartTime;
    public float MaxFps = 60f;
    public int SpriteType;
    public int OriPoint;
    public float SpriteWidth = 1f;
    public float SpriteHeight = 1f;
    public int SpriteUVStretch;
    public int OriRotationMin;
    public int OriRotationMax;
    public bool RotAffectorEnable;
    public RSTYPE RotateType;
    public float DeltaRot;
    public AnimationCurve RotateCurve;
    public float OriScaleXMin = 1f;
    public float OriScaleXMax = 1f;
    public float OriScaleYMin = 1f;
    public float OriScaleYMax = 1f;
    public bool ScaleAffectorEnable;
    public RSTYPE ScaleType;
    public float DeltaScaleX;
    public float DeltaScaleY;
    public AnimationCurve ScaleXCurve;
    public AnimationCurve ScaleYCurve;
    public bool ColorAffectorEnable;
    public int ColorAffectType;
    public float ColorGradualTimeLength = 1f;
    public COLOR_GRADUAL_TYPE ColorGradualType;
    public Color Color1 = Color.white;
    public Color Color2;
    public Color Color3;
    public Color Color4;
    public float RibbonWidth = 0.5f;
    public int MaxRibbonElements = 6;
    public float RibbonLen = 1f;
    public float TailDistance;
    public int StretchType;
    public int EmitType;
    public Vector3 BoxSize;
    public Vector3 EmitPoint;
    public float Radius;
    public Vector3 CircleDir;
    public float LineLengthLeft = -1f;
    public float LineLengthRight = 1f;
    public int MaxENodes = 1;
    public bool IsNodeLifeLoop = true;
    public float NodeLifeMin = 1f;
    public float NodeLifeMax = 1f;
    public bool IsEmitByDistance;
    public float DiffDistance = 0.1f;
    public float ChanceToEmit = 100f;
    public float EmitDuration = 10f;
    public int EmitRate = 20;
    public int EmitLoop = 1;
    public float EmitDelay;
    public bool IsRandomDir;
    public Vector3 OriVelocityAxis;
    public int AngleAroundAxis;
    public float OriSpeed;
    public bool AlongVelocity;
    public bool LinearForceAffectorEnable;
    public Vector3 LinearForce;
    public float LinearMagnitude = 1f;
    public bool JetAffectorEnable;
    public float JetMin;
    public float JetMax;
    public bool VortexAffectorEnable;
    public bool UseVortexCurve;
    public float VortexMag = 0.1f;
    public AnimationCurve VortexCurve;
    public Vector3 VortexDirection;
    public bool AttractionAffectorEnable;
    public bool UseAttractCurve;
    public float AttractMag = 0.1f;
    public AnimationCurve AttractionCurve;
    public Vector3 AttractionPosition;
    public bool UVAffectorEnable;
    public int UVType;
    public Vector2 OriLowerLeftUV = Vector2.zero;
    public Vector2 OriUVDimensions = Vector2.one;
    public int Cols = 1;
    public int Rows = 1;
    public int LoopCircles = -1;
    public float UVTime = 30f;
    public string EanPath = "none";
    public int EanIndex;
    public bool RandomOriScale;
    public bool RandomOriRot;
    protected Emitter emitter;
    public EffectNode[] AvailableENodes;
    public EffectNode[] ActiveENodes;
    public int AvailableNodeCount;
    public Vector3 LastClientPos;

    protected ArrayList InitAffectors(EffectNode node)
    {
        ArrayList arrayList = new ArrayList();
        if (UVAffectorEnable)
        {
            UVAnimation uVAnimation = new UVAnimation();
            Texture texture = Vertexpool.GetMaterial().GetTexture("_MainTex");
            if (UVType == 2)
            {
                uVAnimation.BuildFromFile(EanPath, EanIndex, UVTime, texture);
                OriLowerLeftUV = uVAnimation.frames[0];
                OriUVDimensions = uVAnimation.UVDimensions[0];
            }
            else if (UVType == 1)
            {
                float num = texture.width / Cols;
                float num2 = texture.height / Rows;
                Vector2 vector = new Vector2(num / (float)texture.width, num2 / (float)texture.height);
                Vector2 vector2 = new Vector2(0f, 1f);
                uVAnimation.BuildUVAnim(vector2, vector, Cols, Rows, Cols * Rows);
                OriLowerLeftUV = vector2;
                OriUVDimensions = vector;
                OriUVDimensions.y = 0f - OriUVDimensions.y;
            }
            if (uVAnimation.frames.Length == 1)
            {
                OriLowerLeftUV = uVAnimation.frames[0];
                OriUVDimensions = uVAnimation.UVDimensions[0];
            }
            else
            {
                uVAnimation.loopCycles = LoopCircles;
                Affector value = new UVAffector(uVAnimation, UVTime, node);
                arrayList.Add(value);
            }
        }
        if (RotAffectorEnable && RotateType != 0)
        {
            Affector value2 = (RotateType != RSTYPE.CURVE) ? new RotateAffector(DeltaRot, node) : new RotateAffector(RotateCurve, node);
            arrayList.Add(value2);
        }
        if (ScaleAffectorEnable && ScaleType != 0)
        {
            Affector value3 = (ScaleType != RSTYPE.CURVE) ? new ScaleAffector(DeltaScaleX, DeltaScaleY, node) : new ScaleAffector(ScaleXCurve, ScaleYCurve, node);
            arrayList.Add(value3);
        }
        if (ColorAffectorEnable && ColorAffectType != 0)
        {
            ColorAffector value4 = (ColorAffectType != 2) ? new ColorAffector(new Color[2]
            {
                Color1,
                Color2
            }, ColorGradualTimeLength, ColorGradualType, node) : new ColorAffector(new Color[4]
            {
                Color1,
                Color2,
                Color3,
                Color4
            }, ColorGradualTimeLength, ColorGradualType, node);
            arrayList.Add(value4);
        }
        if (LinearForceAffectorEnable)
        {
            Affector value5 = new LinearForceAffector(LinearForce.normalized * LinearMagnitude, node);
            arrayList.Add(value5);
        }
        if (JetAffectorEnable)
        {
            Affector value6 = new JetAffector(JetMin, JetMax, node);
            arrayList.Add(value6);
        }
        if (VortexAffectorEnable)
        {
            Affector value7 = (!UseVortexCurve) ? new VortexAffector(VortexMag, VortexDirection, node) : new VortexAffector(VortexCurve, VortexDirection, node);
            arrayList.Add(value7);
        }
        if (AttractionAffectorEnable)
        {
            Affector value8 = (!UseVortexCurve) ? new AttractionForceAffector(AttractMag, AttractionPosition, node) : new AttractionForceAffector(AttractionCurve, AttractionPosition, node);
            arrayList.Add(value8);
        }
        return arrayList;
    }

    protected void Init()
    {
        AvailableENodes = new EffectNode[MaxENodes];
        ActiveENodes = new EffectNode[MaxENodes];
        for (int i = 0; i < MaxENodes; i++)
        {
            EffectNode effectNode = new EffectNode(i, ClientTransform, SyncClient, this);
            ArrayList affectorList = InitAffectors(effectNode);
            effectNode.SetAffectorList(affectorList);
            if (RenderType == 0)
            {
                effectNode.SetType(SpriteWidth, SpriteHeight, (STYPE)SpriteType, (ORIPOINT)OriPoint, SpriteUVStretch, MaxFps);
            }
            else
            {
                effectNode.SetType(RibbonWidth, MaxRibbonElements, RibbonLen, ClientTransform.position, StretchType, MaxFps);
            }
            AvailableENodes[i] = effectNode;
        }
        AvailableNodeCount = MaxENodes;
        emitter = new Emitter(this);
    }

    public VertexPool GetVertexPool()
    {
        return Vertexpool;
    }

    public void RemoveActiveNode(EffectNode node)
    {
        if (AvailableNodeCount == MaxENodes)
        {
            Debug.LogError("out index!");
        }
        if (ActiveENodes[node.Index] != null)
        {
            ActiveENodes[node.Index] = null;
            AvailableENodes[node.Index] = node;
            AvailableNodeCount++;
        }
    }

    public void AddActiveNode(EffectNode node)
    {
        if (AvailableNodeCount == 0)
        {
            Debug.LogError("out index!");
        }
        if (AvailableENodes[node.Index] != null)
        {
            ActiveENodes[node.Index] = node;
            AvailableENodes[node.Index] = null;
            AvailableNodeCount--;
        }
    }

    protected void AddNodes(int num)
    {
        int num2 = 0;
        for (int i = 0; i < MaxENodes; i++)
        {
            if (num2 == num)
            {
                break;
            }
            EffectNode effectNode = AvailableENodes[i];
            if (effectNode != null)
            {
                AddActiveNode(effectNode);
                num2++;
                emitter.SetEmitPosition(effectNode);
                effectNode.Init(life: (!IsNodeLifeLoop) ? Random.Range(NodeLifeMin, NodeLifeMax) : (-1f), oriDir: emitter.GetEmitRotation(effectNode).normalized, speed: OriSpeed, oriRot: Random.Range(OriRotationMin, OriRotationMax), oriScaleX: Random.Range(OriScaleXMin, OriScaleXMax), oriScaleY: Random.Range(OriScaleYMin, OriScaleYMax), oriColor: Color1, oriLowerUv: OriLowerLeftUV, oriUVDimension: OriUVDimensions);
            }
        }
    }

    public void Reset()
    {
        for (int i = 0; i < MaxENodes; i++)
        {
            if (ActiveENodes == null)
            {
                return;
            }
            EffectNode effectNode = ActiveENodes[i];
            if (effectNode != null)
            {
                effectNode.Reset();
                RemoveActiveNode(effectNode);
            }
        }
        emitter.Reset();
    }

    public void FixedUpdateCustom()
    {
        int nodes = emitter.GetNodes();
        AddNodes(nodes);
        for (int i = 0; i < MaxENodes; i++)
        {
            ActiveENodes[i]?.Update();
        }
    }

    public void StartCustom()
    {
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }
        Init();
        LastClientPos = ClientTransform.position;
    }

    private void OnDrawGizmosSelected()
    {
    }

    public RibbonTrail GetRibbonTrail()
    {
        if (((ActiveENodes == null) | (ActiveENodes.Length != 1)) || MaxENodes != 1 || RenderType != 1)
        {
            return null;
        }
        return ActiveENodes[0].Ribbon;
    }
}
