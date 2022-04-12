public class KickState
{
    public string Name;
    public int Id;
    private string Kickers;
    private int KickCount;

    public void Init(string n)
    {
        Name = n;
        Kickers = string.Empty;
        KickCount = 0;
    }

    public void AddKicker(string n)
    {
        if (!Kickers.Contains(n))
        {
            Kickers += n;
            KickCount++;
        }
    }

    public int GetKickCount()
    {
        return KickCount;
    }
}
