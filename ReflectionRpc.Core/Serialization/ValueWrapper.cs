﻿namespace ReflectionRpc.Core.Serialization
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
            if (o is null)
            {
                return null;
            }

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

        public static object UnwrapIfPossible(object o)
        {
            return o is ValueWrapper wrapper ? wrapper.Unwrap() : o;
        }

        public object Unwrap()
        {
            var type = Type.GetType(this.TypeName);

            if (this.Value.GetType() == type)
            {
                return this.Value;
            }

            if (type.IsEnum)
            {
                return Enum.Parse(type, (string)this.Value);
            }

            return Convert.ChangeType(this.Value, type);
        }
    }
}
