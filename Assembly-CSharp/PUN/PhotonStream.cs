using System.Collections.Generic;
using UnityEngine;

public class PhotonStream
{
    private bool write;
    internal List<object> data;
    private byte currentItem;
    public bool isWriting => write;
    public bool isReading => !write;
    public int Count => data.Count;

    public PhotonStream(bool write, object[] incomingData)
    {
        this.write = write;
        if (incomingData == null)
        {
            data = new List<object>();
        }
        else
        {
            data = new List<object>(incomingData);
        }
    }

    public object ReceiveNext()
    {
        if (write)
        {
            Debug.LogError("Error: you cannot read this stream that you are writing!");
            return null;
        }
        object result = data[currentItem];
        currentItem++;
        return result;
    }

    public void SendNext(object obj)
    {
        if (!write)
        {
            Debug.LogError("Error: you cannot write/send to this stream that you are reading!");
        }
        else
        {
            data.Add(obj);
        }
    }

    public object[] ToArray()
    {
        return data.ToArray();
    }

    public void Serialize(ref bool myBool)
    {
        if (write)
        {
            data.Add(myBool);
        }
        else if (data.Count > currentItem)
        {
            myBool = (bool)data[currentItem];
            currentItem++;
        }
    }

    public void Serialize(ref int myInt)
    {
        if (write)
        {
            data.Add(myInt);
        }
        else if (data.Count > currentItem)
        {
            myInt = (int)data[currentItem];
            currentItem++;
        }
    }

    public void Serialize(ref string value)
    {
        if (write)
        {
            data.Add(value);
        }
        else if (data.Count > currentItem)
        {
            value = (string)data[currentItem];
            currentItem++;
        }
    }

    public void Serialize(ref char value)
    {
        if (write)
        {
            data.Add(value);
        }
        else if (data.Count > currentItem)
        {
            value = (char)data[currentItem];
            currentItem++;
        }
    }

    public void Serialize(ref short value)
    {
        if (write)
        {
            data.Add(value);
        }
        else if (data.Count > currentItem)
        {
            value = (short)data[currentItem];
            currentItem++;
        }
    }

    public void Serialize(ref float obj)
    {
        if (write)
        {
            data.Add(obj);
        }
        else if (data.Count > currentItem)
        {
            obj = (float)data[currentItem];
            currentItem++;
        }
    }

    public void Serialize(ref PhotonPlayer obj)
    {
        if (write)
        {
            data.Add(obj);
        }
        else if (data.Count > currentItem)
        {
            obj = (PhotonPlayer)data[currentItem];
            currentItem++;
        }
    }

    public void Serialize(ref Vector3 obj)
    {
        if (write)
        {
            data.Add(obj);
        }
        else if (data.Count > currentItem)
        {
            obj = (Vector3)data[currentItem];
            currentItem++;
        }
    }

    public void Serialize(ref Vector2 obj)
    {
        if (write)
        {
            data.Add(obj);
        }
        else if (data.Count > currentItem)
        {
            obj = (Vector2)data[currentItem];
            currentItem++;
        }
    }

    public void Serialize(ref Quaternion obj)
    {
        if (write)
        {
            data.Add(obj);
        }
        else if (data.Count > currentItem)
        {
            obj = (Quaternion)data[currentItem];
            currentItem++;
        }
    }
}
