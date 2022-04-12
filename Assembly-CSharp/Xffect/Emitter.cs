using System;
using UnityEngine;

public class Emitter
{
    public EffectLayer Layer;
    private float EmitterElapsedTime;
    private float EmitDelayTime;
    private bool IsFirstEmit = true;
    private float EmitLoop;
    private Vector3 LastClientPos = Vector3.zero;

    public Emitter(EffectLayer owner)
    {
        Layer = owner;
        EmitLoop = Layer.EmitLoop;
        LastClientPos = Layer.ClientTransform.position;
    }

    public void Reset()
    {
        EmitterElapsedTime = 0f;
        EmitDelayTime = 0f;
        IsFirstEmit = true;
        EmitLoop = Layer.EmitLoop;
    }

    protected int EmitByDistance()
    {
        if ((Layer.ClientTransform.position - LastClientPos).magnitude >= Layer.DiffDistance)
        {
            LastClientPos = Layer.ClientTransform.position;
            return 1;
        }
        return 0;
    }

    protected int EmitByRate()
    {
        int num = UnityEngine.Random.Range(0, 100);
        if (num >= 0 && (float)num > Layer.ChanceToEmit)
        {
            return 0;
        }
        EmitDelayTime += Time.deltaTime;
        if (EmitDelayTime < Layer.EmitDelay && !IsFirstEmit)
        {
            return 0;
        }
        EmitterElapsedTime += Time.deltaTime;
        if (EmitterElapsedTime >= Layer.EmitDuration)
        {
            if (EmitLoop > 0f)
            {
                EmitLoop -= 1f;
            }
            EmitterElapsedTime = 0f;
            EmitDelayTime = 0f;
            IsFirstEmit = false;
        }
        if (EmitLoop == 0f)
        {
            return 0;
        }
        if (Layer.AvailableNodeCount == 0)
        {
            return 0;
        }
        int num2 = (int)(EmitterElapsedTime * (float)Layer.EmitRate) - (Layer.ActiveENodes.Length - Layer.AvailableNodeCount);
        int num3 = 0;
        num3 = ((num2 <= Layer.AvailableNodeCount) ? num2 : Layer.AvailableNodeCount);
        if (num3 <= 0)
        {
            return 0;
        }
        return num3;
    }

    public Vector3 GetEmitRotation(EffectNode node)
    {
        Vector3 zero = Vector3.zero;
        if (Layer.EmitType == 2)
        {
            if (!Layer.SyncClient)
            {
                return node.Position - (Layer.ClientTransform.position + Layer.EmitPoint);
            }
            return node.Position - Layer.EmitPoint;
        }
        if (Layer.EmitType == 3)
        {
            Vector3 vector = Layer.SyncClient ? (node.Position - Layer.EmitPoint) : (node.Position - (Layer.ClientTransform.position + Layer.EmitPoint));
            Vector3 toDirection = Vector3.RotateTowards(vector, Layer.CircleDir, (float)(90 - Layer.AngleAroundAxis) * ((float)Math.PI / 180f), 1f);
            Quaternion rotation = Quaternion.FromToRotation(vector, toDirection);
            return rotation * vector;
        }
        if (Layer.IsRandomDir)
        {
            Quaternion rhs = Quaternion.Euler(0f, 0f, Layer.AngleAroundAxis);
            Quaternion rhs2 = Quaternion.Euler(0f, UnityEngine.Random.Range(0, 360), 0f);
            Quaternion lhs = Quaternion.FromToRotation(Vector3.up, Layer.OriVelocityAxis);
            return lhs * rhs2 * rhs * Vector3.up;
        }
        return Layer.OriVelocityAxis;
    }

    public void SetEmitPosition(EffectNode node)
    {
        Vector3 vector = Vector3.zero;
        if (Layer.EmitType == 1)
        {
            Vector3 emitPoint = Layer.EmitPoint;
            float x = UnityEngine.Random.Range(emitPoint.x - Layer.BoxSize.x / 2f, emitPoint.x + Layer.BoxSize.x / 2f);
            float y = UnityEngine.Random.Range(emitPoint.y - Layer.BoxSize.y / 2f, emitPoint.y + Layer.BoxSize.y / 2f);
            float z = UnityEngine.Random.Range(emitPoint.z - Layer.BoxSize.z / 2f, emitPoint.z + Layer.BoxSize.z / 2f);
            vector.x = x;
            vector.y = y;
            vector.z = z;
            if (!Layer.SyncClient)
            {
                vector = Layer.ClientTransform.position + vector;
            }
        }
        else if (Layer.EmitType == 0)
        {
            vector = Layer.EmitPoint;
            if (!Layer.SyncClient)
            {
                vector = Layer.ClientTransform.position + Layer.EmitPoint;
            }
        }
        else if (Layer.EmitType == 2)
        {
            vector = Layer.EmitPoint;
            if (!Layer.SyncClient)
            {
                vector = Layer.ClientTransform.position + Layer.EmitPoint;
            }
            Vector3 point = Vector3.up * Layer.Radius;
            Quaternion rotation = Quaternion.Euler(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
            vector = rotation * point + vector;
        }
        else if (Layer.EmitType == 4)
        {
            Vector3 vector2 = Layer.EmitPoint + Layer.ClientTransform.localRotation * Vector3.forward * Layer.LineLengthLeft;
            Vector3 a = Layer.EmitPoint + Layer.ClientTransform.localRotation * Vector3.forward * Layer.LineLengthRight;
            Vector3 vector3 = a - vector2;
            float num = (float)(node.Index + 1) / (float)Layer.MaxENodes;
            float d = vector3.magnitude * num;
            vector = vector2 + vector3.normalized * d;
            if (!Layer.SyncClient)
            {
                vector = Layer.ClientTransform.TransformPoint(vector);
            }
        }
        else if (Layer.EmitType == 3)
        {
            float num2 = (float)(node.Index + 1) / (float)Layer.MaxENodes;
            float y2 = 360f * num2;
            Quaternion rotation2 = Quaternion.Euler(0f, y2, 0f);
            Vector3 point2 = rotation2 * (Vector3.right * Layer.Radius);
            Quaternion rotation3 = Quaternion.FromToRotation(Vector3.up, Layer.CircleDir);
            vector = rotation3 * point2;
            if (!Layer.SyncClient)
            {
                vector = Layer.ClientTransform.position + vector + Layer.EmitPoint;
            }
            else
            {
                vector += Layer.EmitPoint;
            }
        }
        node.SetLocalPosition(vector);
    }

    public int GetNodes()
    {
        if (Layer.IsEmitByDistance)
        {
            return EmitByDistance();
        }
        return EmitByRate();
    }
}
