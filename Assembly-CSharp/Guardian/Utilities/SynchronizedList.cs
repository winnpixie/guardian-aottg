class SynchronizedList<T> : System.Collections.Generic.IList<T>
{
    private readonly System.Collections.Generic.List<T> RealList = new System.Collections.Generic.List<T>();
    private readonly object Lock = new object();

    public T this[int index]
    {
        get
        {
            lock (Lock)
            {
                return RealList[index];
            }
        }
        set
        {
            lock (Lock)
            {
                RealList[index] = value;
            }
        }
    }

    public int Count
    {
        get
        {
            lock (Lock)
            {
                return RealList.Count;
            }
        }
    }

    public bool IsReadOnly
    {
        get
        {
            return ((System.Collections.Generic.ICollection<T>)RealList).IsReadOnly;
        }
    }

    public void Add(T item)
    {
        lock (Lock)
        {
            RealList.Add(item);
        }
    }

    public void Clear()
    {
        lock (Lock)
        {
            RealList.Clear();
        }
    }

    public bool Contains(T item)
    {
        lock (Lock)
        {
            return RealList.Contains(item);
        }
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        lock (Lock)
        {
            RealList.CopyTo(array, arrayIndex);
        }
    }

    public System.Collections.Generic.IEnumerator<T> GetEnumerator()
    {
        lock (Lock)
        {
            return RealList.GetEnumerator();
        }
    }

    public int IndexOf(T item)
    {
        lock (Lock)
        {
            return RealList.IndexOf(item);
        }
    }

    public void Insert(int index, T item)
    {
        lock (Lock)
        {
            RealList.Insert(index, item);
        }
    }

    public bool Remove(T item)
    {
        lock (Lock)
        {
            return RealList.Remove(item);
        }
    }

    public void RemoveAt(int index)
    {
        lock (Lock)
        {
            RealList.RemoveAt(index);
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        lock (Lock)
        {
            return RealList.GetEnumerator();
        }
    }

    // BEGIN Custom
    public void RemoveAll(System.Predicate<T> predicate)
    {
        lock (Lock)
        {
            RealList.RemoveAll(predicate);
        }
    }
    // END Custom
}