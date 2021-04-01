using UnityEngine;
using UnityEngine.UI;

public class StyledItemButtonImageText : StyledItem
{
    public class Data
    {
        public string text;

        public Texture2D image;

        public Data(string t, Texture2D tex)
        {
            text = t;
            image = tex;
        }
    }

    public RawImage rawImageCtrl;

    public Text textCtrl;

    public Button buttonCtrl;

    public override Button GetButton()
    {
        return buttonCtrl;
    }

    public override Text GetText()
    {
        return textCtrl;
    }

    public override RawImage GetRawImage()
    {
        return rawImageCtrl;
    }

    public override void Populate(object o)
    {
        Texture2D texture2D = o as Texture2D;
        if (texture2D != null)
        {
            if (rawImageCtrl != null)
            {
                rawImageCtrl.texture = texture2D;
            }
            return;
        }
        Data data = o as Data;
        if (data == null)
        {
            if (textCtrl != null)
            {
                textCtrl.text = o.ToString();
            }
            return;
        }
        if (rawImageCtrl != null)
        {
            rawImageCtrl.texture = data.image;
        }
        if (textCtrl != null)
        {
            textCtrl.text = data.text;
        }
    }
}
