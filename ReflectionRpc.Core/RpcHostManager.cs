using System.Reflection;
using ReflectionRpc.Core.Communication;
using ReflectionRpc.Core.Communication.RpcRequests;
using ReflectionRpc.Core.Communication.RpcResponses;
using ReflectionRpc.Core.Interfaces;

namespace ReflectionRpc.Core
{
    public class RpcHostManager : IRpcHostManager
    {
        private List<RegisteredRpcHost> registeredHosts = new List<RegisteredRpcHost>();
        private List<RegisteredRpcHost> topHosts = new List<RegisteredRpcHost>();

        public List<RegisteredRpcHost> GetHosts()
        {
            return this.topHosts;
        }

        public RegisteredRpcHost GetHost(Guid guid)
        {
            return this.registeredHosts.SingleOrDefault(registeredHost => registeredHost.Guid == guid);
        }

        public RegisteredRpcHost GetHost(string tag)
        {
            return this.registeredHosts.SingleOrDefault(registeredHost => registeredHost.Tag == tag);
        }

        public RegisteredRpcHost RegisterAsRpcHost(object rpcTarget, string tag = null)
        {
            var registeredHost = this.RegisterHostInternal(rpcTarget, tag);
            this.topHosts.Add(registeredHost);

            return registeredHost;
        }

        public IRpcResponse ExecuteRequest(Guid hostGuid, IRpcRequest request)
        {
            var host = this.GetHost(hostGuid);

            return request switch
            {
                ExecuteMethodRequest executeMethodRequest => this.HandleExecuteMethodRequest(host, executeMethodRequest),
                GetPropertyValueRequest getPropertyValueRequest => this.HandleGetPropertyValueRequest(host, getPropertyValueRequest),
                SetPropertyValueRequest setPropertyValueRequest => this.HandleSetPropertyValueRequest(host, setPropertyValueRequest),
                _ => throw new NotSupportedException($"RPC request of type {request.GetType().Name} is not supported.")
            };
        }

        private IRpcResponse HandleSetPropertyValueRequest(RegisteredRpcHost host, SetPropertyValueRequest request)
        {
            try
            {
                var propertyName = request.PropertyName;
                var property = host.RpcHost.Target.GetType().GetProperty(propertyName);

                if (!property.PropertyType.RequiresRpcHost())
                {
                    host.RpcHost.SetPropertyValue(request.PropertyName, request.Value);
                    return new VoidRpcResponse();
                }

                var registered = this.RegisterAsRpcHost(request.Value);
                host.Properties[propertyName] = registered;
                return new HostRpcResponse(registered.Guid);
            }
            catch (Exception e)
            {
                return new ExceptionRpcResponse(e);
            }
        }

        private IRpcResponse HandleGetPropertyValueRequest(RegisteredRpcHost host, GetPropertyValueRequest request)
        {
            try
            {
                var propertyName = request.PropertyName;
                var propertyValue = host.RpcHost.GetPropertyValue(propertyName);

                if (!host.Properties.ContainsKey(propertyName))
                {
                    return new ValueRpcResponse(propertyValue);
                }

                // TODO: The actual object in property may have been changed during registration time.
                return new HostRpcResponse(host.Properties[propertyName].Guid);
            }
            catch (Exception e)
            {
                return new ExceptionRpcResponse(e);
            }
        }

        private IRpcResponse HandleExecuteMethodRequest(RegisteredRpcHost host, ExecuteMethodRequest request)
        {
            try
            {
                var returned = host.RpcHost.ExecuteMethod(request.MethodName, request.Arguments);
                
                // TODO: Obviously, this needs to be changed to support complex types.
                return new ValueRpcResponse(returned);
            }
            catch (Exception e)
            {
                return new ExceptionRpcResponse(e);
            }
        }

        private RegisteredRpcHost RegisterHostInternal(object rpcTarget, string tag = null)
        {
            if (this.registeredHosts.Select(rpcHost => rpcHost.RpcHost.Target).Contains(rpcTarget))
            {
                throw new Exception("Target already registered.");
            }

            var host = new RpcHost(rpcTarget);
            var registeredProperties = this.RegisterRequiredProperties(rpcTarget);

            var registeredHost = new RegisteredRpcHost(host, registeredProperties) { Tag = tag };
            this.registeredHosts.Add(registeredHost);

            return registeredHost;
        }

        private Dictionary<string, RegisteredRpcHost> RegisterRequiredProperties(object rpcTarget)
        {
            var properties = new Dictionary<string, RegisteredRpcHost>();
            rpcTarget.GetType().GetProperties().ToList().ForEach(property =>
            {
                if (property.PropertyType.RequiresRpcHost())
                {
                    var value = property.GetValue(rpcTarget);
                    var propertyHostInformation = this.RegisterHostInternal(value);
                    properties.Add(property.Name, propertyHostInformation);
                }
            });

            return properties;
        }
    }
}
