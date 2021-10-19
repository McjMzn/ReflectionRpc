using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Core
{
    public static class Extensions
    {
        public static bool RequiresRpcHost(this Type type)
        {
            return !(type.IsPrimitive || type.IsEnum || type == typeof(string));
        }
    }
}
