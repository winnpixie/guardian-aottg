using System.Collections.Generic;

public class VoteKick
{
    public int Id;
    public HashSet<int> Voters = new HashSet<int>();

    public void Init(int targetId)
    {
        this.Id = targetId;
    }

    public void AddVote(int voterId)
    {
        Voters.Add(voterId);
    }

    public int GetVotes()
    {
        return Voters.Count;
    }
}
