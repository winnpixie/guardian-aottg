class SynchronizedList<T> : System.Collections.Generic.IList<T>
{
    private readonly System.Collections.Generic.List<T> _List = new System.Collections.Generic.List<T>();
    private readonly object AccessLock = new object();

    public T this[int index]
    {
        get
        {
            lock (AccessLock)
            {
                return _List[index];
            }
        }
        set
        {
            lock (AccessLock)
            {
                _List[index] = value;
            }
        }
    }

    public int Count
    {
        get
        {
            lock (AccessLock)
            {
                return _List.Count;
            }
        }
    }

    public bool IsReadOnly
    {
        get
        {
            return ((System.Collections.Generic.ICollection<T>)_List).IsReadOnly;
        }
    }

    public void Add(T item)
    {
        lock (AccessLock)
        {
            _List.Add(item);
        }
    }

    public void Clear()
    {
        lock (AccessLock)
        {
            _List.Clear();
        }
    }

    public bool Contains(T item)
    {
        lock (AccessLock)
        {
            return _List.Contains(item);
        }
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        lock (AccessLock)
        {
            _List.CopyTo(array, arrayIndex);
        }
    }

    public System.Collections.Generic.IEnumerator<T> GetEnumerator()
    {
        lock (AccessLock)
        {
            return _List.GetEnumerator();
        }
    }

    public int IndexOf(T item)
    {
        lock (AccessLock)
        {
            return _List.IndexOf(item);
        }
    }

    public void Insert(int index, T item)
    {
        lock (AccessLock)
        {
            _List.Insert(index, item);
        }
    }

    public bool Remove(T item)
    {
        lock (AccessLock)
        {
            return _List.Remove(item);
        }
    }

    public void RemoveAt(int index)
    {
        lock (AccessLock)
        {
            _List.RemoveAt(index);
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        lock (AccessLock)
        {
            return _List.GetEnumerator();
        }
    }

    // BEGIN Custom
    public void RemoveAll(System.Predicate<T> predicate)
    {
        lock (AccessLock)
        {
            _List.RemoveAll(predicate);
        }
    }
    // END Custom
}