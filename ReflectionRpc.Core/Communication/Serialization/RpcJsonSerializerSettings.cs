using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ReflectionRpc.Core.Communication.Serialization
{

    public class RpcJsonSerializer
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented,
            Converters = new[] { new StringEnumConverter() }
        };

        public static string Serialize(object o)
        {
            return o is null ? null : JsonConvert.SerializeObject(ValueWrapper.WrapIfRequired(o), SerializerSettings);
        }

        public static IRpcResponse DeserializeRpcResponse(string json)
        {
            return (IRpcResponse)DeserializeObject(json);
        }

        public static object DeserializeObject(string json)
        {
            if (json is null)
            {
                return null;
            }

            var deserialized = JsonConvert.DeserializeObject(json, SerializerSettings);
            return deserialized is ValueWrapper wrapper ? wrapper.Unwrap() : deserialized;
        }
    }
}
