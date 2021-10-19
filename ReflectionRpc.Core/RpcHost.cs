using System.Reflection;
using ReflectionRpc.Core.Communication;
using ReflectionRpc.Core.Communication.RpcResponses;
using ReflectionRpc.Core.Interfaces;

namespace ReflectionRpc.Core
{
    public class RpcHost : IRpcHost
    {
        public RpcHost(object target)
        {
            this.Target = target;
        }
        
        public object Target { get; set; }
        public Type TargetType => this.Target.GetType();

        public object ExecuteMethod(string methodName, params object[] arguments)
        {
            var methods = this.TargetType.GetMethods().Where(m => m.Name == methodName);
            var method = (MethodInfo)Type.DefaultBinder.SelectMethod(BindingFlags.Public | BindingFlags.Instance, methods.ToArray(), arguments.Select(a => a.GetType()).ToArray(), null);

            if (method == null)
            {
                method = methods.First(m => this.TryToConvertArguments(m.GetParameters(), arguments));
            }

            return method.Invoke(this.Target, arguments);

            //var methods = this.TargetType
            //    .GetMethods()
            //    .Where(m => m.Name == methodName)
            //    .Where(m => ArgumentsMatchParameterInfos(m.GetParameters(), arguments))
            //    .ToList();

            //if (methods.Count == 0)
            //{
            //    throw new NotSupportedException("No matching method has been found.");
            //}

            //var method = methods.Single();
            //return method.Invoke(this.Target, arguments);
        }

        public void SetPropertyValue(string propertyName, object value)
        {
            var property = this.TargetType.GetProperty(propertyName);
            property.SetValue(this.Target, value);
        }

        public object GetPropertyValue(string propertyName)
        {
            return this.TargetType.GetProperty(propertyName).GetValue(this.Target);
        }

        private bool TryToConvertArguments(ParameterInfo[] parameters, object[] arguments)
        {
            if (parameters.Length != arguments.Length)
            {
                return false;
            }

            var properlyTypedArguments = new List<object>();

            for (int i = 0; i < parameters.Length; i++)
            {
                var sourceType = arguments[i].GetType();
                var targetType = parameters[i].ParameterType;

                if (sourceType.IsAssignableTo(targetType))
                {
                    properlyTypedArguments.Add(arguments[i]);
                    continue;
                }

                try
                {
                    var converted = Convert.ChangeType(arguments[i], targetType);
                    properlyTypedArguments.Add(converted);
                }
                catch
                {
                    return false;
                }
            }

            properlyTypedArguments.CopyTo(arguments);
            return true;
        }
    }
}
