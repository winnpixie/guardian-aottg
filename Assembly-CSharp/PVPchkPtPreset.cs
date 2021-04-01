using UnityEngine;

public class PVPchkPtPreset : MonoBehaviour
{
    public int id;
    public float size = 30f;
    public float humanPt;
    public float titanPt;
    public int humanPtMax = 15;
    public int titanPtMax = 15;
    public float interval = 20f;
    public int[] nextChkPtId;
    public int[] prevChkPtId;
}
