namespace ReflectionRpc.Core.Interfaces
{
    public interface IRpcClient
    {
        void SetRemotePropertyValue(string propertyName, object value);
        object GetRemotePropertyValue(string propertyName);
        object ExecuteRemoteMethod(string methodName, params object[] arguments);
    }
}
