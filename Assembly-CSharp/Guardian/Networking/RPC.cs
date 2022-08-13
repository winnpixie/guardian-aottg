using System;

namespace Guardian.Networking
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class RPC : Attribute
    {
        public string Name = string.Empty;
    }
}
