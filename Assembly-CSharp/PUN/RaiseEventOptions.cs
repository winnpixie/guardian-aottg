using ExitGames.Client.Photon.Lite;

public class RaiseEventOptions
{
    public static readonly RaiseEventOptions Default = new RaiseEventOptions();
    public EventCaching CachingOption;
    public byte InterestGroup;
    public int[] TargetActors;
    public ReceiverGroup Receivers;
    public byte SequenceChannel;
    public bool ForwardToWebhook;
    public int CacheSliceIndex;
}
