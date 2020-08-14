using System;
using System.Collections.Generic;

namespace Guardian.Features
{
    class FeatureManager<T> where T : Feature
    {
        public List<T> Elements = new List<T>();

        public virtual void Load() { }

        public virtual void Save() { }

        public void Add(T element)
        {
            Elements.Add(element);
        }

        public T Find(string name)
        {
            foreach (T element in Elements)
            {
                if (element.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return element;
                }
                foreach (string alias in element.aliases)
                {
                    if (alias.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return element;
                    }
                }
            }
            return null;
        }
    }
}
