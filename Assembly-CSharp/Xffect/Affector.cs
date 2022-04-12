public class Affector
{
    protected EffectNode Node;

    public Affector(EffectNode node)
    {
        Node = node;
    }

    public virtual void Reset() { }

    public virtual void Update() { }
}
