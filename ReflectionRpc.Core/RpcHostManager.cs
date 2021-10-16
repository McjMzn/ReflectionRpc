using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public RegisteredRpcHost RegisterHost(object rpcTarget, string tag = null)
        {
            var registeredHost = this.RegisterHostInternal(rpcTarget, tag);
            this.topHosts.Add(registeredHost);

            return registeredHost;
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

        private bool RequiresHost(PropertyInfo propertyInfo)
        {
            return
                !propertyInfo.PropertyType.IsPrimitive &&
                !propertyInfo.PropertyType.IsEnum &&
                 propertyInfo.PropertyType != typeof(string);
        }

        private Dictionary<string, RegisteredRpcHost> RegisterRequiredProperties(object rpcTarget)
        {
            var properties = new Dictionary<string, RegisteredRpcHost>();
            rpcTarget.GetType().GetProperties().ToList().ForEach(property =>
            {
                if (this.RequiresHost(property))
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
