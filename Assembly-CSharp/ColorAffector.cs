using UnityEngine;

public class ColorAffector : Affector
{
    protected Color[] ColorArr;
    protected float GradualLen;
    protected COLOR_GRADUAL_TYPE Type;
    protected float ElapsedTime;
    protected bool IsNodeLife;

    public ColorAffector(Color[] colorArr, float gradualLen, COLOR_GRADUAL_TYPE type, EffectNode node)
        : base(node)
    {
        ColorArr = colorArr;
        Type = type;
        GradualLen = gradualLen;
        if (GradualLen < 0f)
        {
            IsNodeLife = true;
        }
    }

    public override void Reset()
    {
        ElapsedTime = 0f;
    }

    public override void Update()
    {
        ElapsedTime += Time.deltaTime;
        if (IsNodeLife)
        {
            GradualLen = Node.GetLifeTime();
        }
        if (GradualLen <= 0f)
        {
            return;
        }
        if (ElapsedTime > GradualLen)
        {
            if (Type == COLOR_GRADUAL_TYPE.CLAMP)
            {
                return;
            }
            if (Type == COLOR_GRADUAL_TYPE.LOOP)
            {
                ElapsedTime = 0f;
                return;
            }
            Color[] array = new Color[ColorArr.Length];
            ColorArr.CopyTo(array, 0);
            for (int i = 0; i < array.Length / 2; i++)
            {
                ColorArr[array.Length - i - 1] = array[i];
                ColorArr[i] = array[array.Length - i - 1];
            }
            ElapsedTime = 0f;
        }
        else
        {
            int num = (int)((float)(ColorArr.Length - 1) * (ElapsedTime / GradualLen));
            if (num == ColorArr.Length - 1)
            {
                num--;
            }
            int num2 = num + 1;
            float num3 = GradualLen / (float)(ColorArr.Length - 1);
            float t = (ElapsedTime - num3 * (float)num) / num3;
            Node.Color = Color.Lerp(ColorArr[num], ColorArr[num2], t);
        }
    }
}
