using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Core
{
    public interface IRpcHost
    {
        object Target { get; }
        Type TargetType { get; }

        void SetPropertyValue(string propertyName, object value);
        object GetPropertyValue(string propertyName);
        object ExecuteMethod(string methodName, params object[] arguments);
    }
}
