using UnityEngine;
using UnityEngine.UI;

public class StyledItem : MonoBehaviour
{
    public virtual void Populate(object o)
    {
    }

    public virtual Selectable GetSelectable()
    {
        return null;
    }

    public virtual Button GetButton()
    {
        return null;
    }

    public virtual Text GetText()
    {
        return null;
    }

    public virtual RawImage GetRawImage()
    {
        return null;
    }

    public virtual Image GetImage()
    {
        return null;
    }
}
