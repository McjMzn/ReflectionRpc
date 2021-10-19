using ReflectionRpc.Core.Communication;
using System.Reflection;

namespace ReflectionRpc.Core.Interfaces
{
    public interface IRpcHost
    {
        object Target { get; set; }

        void SetPropertyValue(string propertyName, object value);
        object GetPropertyValue(string propertyName);
        object ExecuteMethod(string methodName, params object[] arguments);
    }
}
