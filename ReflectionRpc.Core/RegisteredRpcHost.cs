using System.Text.Json.Serialization;

namespace ReflectionRpc.Core
{
    public record RegisteredRpcHost
    {
        [JsonIgnore]
        public IRpcHost RpcHost { get; }
        public Guid Guid { get; }
        public string TypeName { get; }
        public List<string> ImplementedInterfaces { get; }
        public Dictionary<string, RegisteredRpcHost> Properties { get; }
        public string Tag { get; set; }


        public RegisteredRpcHost(IRpcHost host, Dictionary<string, RegisteredRpcHost> properties)
        {
            this.Guid = Guid.NewGuid();
            this.RpcHost = host;
            this.TypeName = host.TargetType.Name;
            this.ImplementedInterfaces = host.TargetType.GetInterfaces().Select(i => i.Name).ToList();
            this.Properties = properties;
        }
    }
}
