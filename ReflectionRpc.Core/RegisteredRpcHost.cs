using ReflectionRpc.Core.Interfaces;
using System.Text.Json.Serialization;

namespace ReflectionRpc.Core
{
    public record RegisteredRpcHost
    {
        [JsonIgnore]
        public IRpcHost RpcHost { get; }
        public string TypeName { get; }
        public List<string> ImplementedInterfaces { get; }
        public Dictionary<string, RegisteredRpcHost> Properties { get; }
        public string Tag { get; set; }
        public Guid Guid { get; }


        public RegisteredRpcHost(IRpcHost host, Dictionary<string, RegisteredRpcHost> properties)
        {
            this.Guid = Guid.NewGuid();
            this.RpcHost = host;
            this.TypeName = host.Target.GetType().Name;
            this.ImplementedInterfaces = host.Target.GetType().GetInterfaces().Select(i => i.Name).ToList();
            this.Properties = properties;
        }
    }
}
