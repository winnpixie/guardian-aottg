class SynchronizedList<T> : System.Collections.Generic.IList<T>
{
    private readonly System.Collections.Generic.List<T> RealList = new System.Collections.Generic.List<T>();
    private readonly object AccessLock = new object();

    public T this[int index]
    {
        get
        {
            lock (AccessLock)
            {
                return RealList[index];
            }
        }
        set
        {
            lock (AccessLock)
            {
                RealList[index] = value;
            }
        }
    }

    public int Count
    {
        get
        {
            lock (AccessLock)
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
        lock (AccessLock)
        {
            RealList.Add(item);
        }
    }

    public void Clear()
    {
        lock (AccessLock)
        {
            RealList.Clear();
        }
    }

    public bool Contains(T item)
    {
        lock (AccessLock)
        {
            return RealList.Contains(item);
        }
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        lock (AccessLock)
        {
            RealList.CopyTo(array, arrayIndex);
        }
    }

    public System.Collections.Generic.IEnumerator<T> GetEnumerator()
    {
        lock (AccessLock)
        {
            return RealList.GetEnumerator();
        }
    }

    public int IndexOf(T item)
    {
        lock (AccessLock)
        {
            return RealList.IndexOf(item);
        }
    }

    public void Insert(int index, T item)
    {
        lock (AccessLock)
        {
            RealList.Insert(index, item);
        }
    }

    public bool Remove(T item)
    {
        lock (AccessLock)
        {
            return RealList.Remove(item);
        }
    }

    public void RemoveAt(int index)
    {
        lock (AccessLock)
        {
            RealList.RemoveAt(index);
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        lock (AccessLock)
        {
            return RealList.GetEnumerator();
        }
    }

    // BEGIN Custom
    public void RemoveAll(System.Predicate<T> predicate)
    {
        lock (AccessLock)
        {
            RealList.RemoveAll(predicate);
        }
    }
    // END Custom
}