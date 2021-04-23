using System;

namespace Guardian.Features.Properties
{
    public class Property : Feature
    {
        private object _Value;
        public object Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                OnValueChanged.Invoke();
            }
        }
        public Action OnValueChanged = () => { return; };

        public Property(string name, string[] aliases, object value) : base(name, aliases)
        {
            this.Value = value;
        }
    }

    public class Property<T> : Property
    {
        public new T Value
        {
            get
            {
                return (T)base.Value;
            }
            set
            {
                base.Value = value;
            }
        }

        public Property(string name, string[] aliases, T value) : base(name, aliases, value) { }
    }
}
