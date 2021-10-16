using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Core
{
    public class RpcHost : IRpcHost
    {
        public RpcHost(object target)
        {
            this.Target = target;
        }
        
        public object Target { get; }
        public Type TargetType => this.Target.GetType();


        public object ExecuteMethod(string methodName, params object[] arguments)
        {
            var methods = this.TargetType
                .GetMethods()
                .Where(m => m.Name == methodName)
                .Where(m => ArgumentsMatchParameterInfos(m.GetParameters(), arguments))
                .ToList();

            if (methods.Count == 0)
            {
                throw new NotSupportedException("No matching method has been found.");
            }

            var method = methods.Single();
            return method.Invoke(this.Target, arguments);
        }

        public void SetPropertyValue(string propertyName, object value)
        {
            var property = this.TargetType.GetProperty(propertyName);
            if (property.PropertyType.IsEnum && value is string)
            {
                value = Enum.Parse(property.PropertyType, value as string);
            }

            property.SetValue(Target, value);
        }

        public object GetPropertyValue(string propertyName)
        {
            return this.TargetType.GetProperty(propertyName).GetValue(Target);
        }

        private bool ArgumentsMatchParameterInfos(ParameterInfo[] parameters, object[] arguments)
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
