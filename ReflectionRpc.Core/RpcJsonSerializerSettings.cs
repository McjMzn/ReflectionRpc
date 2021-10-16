using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ReflectionRpc.Core.RpcResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Core
{
    public class ValueWrapper
    {
        public ValueWrapper()
        {
        }

        public object Value { get; set; }
        public string TypeName { get; set; }
        public bool IsEnum { get; set; }

        public static object WrapIfRequired(object o)
        {
            if (o.GetType().IsEnum)
            {
                return new ValueWrapper
                {
                    Value = o.ToString(),
                    TypeName = o.GetType().AssemblyQualifiedName,
                    IsEnum = true
                };
            }

            if (o.GetType().IsPrimitive || o is string)
            {
                return new ValueWrapper
                {
                    Value = o,
                    TypeName = o.GetType().AssemblyQualifiedName,
                    IsEnum = false
                };
            }

            return o;
        }

        public object Unwrap()
        {
           return this.IsEnum ? Enum.Parse(Type.GetType(this.TypeName), this.Value as string) : this.Value;
        }
    }

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
            return JsonConvert.SerializeObject(ValueWrapper.WrapIfRequired(o), SerializerSettings);
        }

        public static IRpcResponse DeserializeRpcResult(string json)
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
