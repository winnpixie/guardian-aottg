public class KickState
{
    public string name;
    public int id;
    private string kickers;
    private int kickCount;

    public void init(string n)
    {
        name = n;
        kickers = string.Empty;
        kickCount = 0;
    }

    public void addKicker(string n)
    {
        if (!kickers.Contains(n))
        {
            kickers += n;
            kickCount++;
        }
    }

    public int getKickCount()
    {
        return kickCount;
    }
}
