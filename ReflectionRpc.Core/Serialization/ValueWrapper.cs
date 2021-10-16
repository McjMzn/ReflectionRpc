namespace ReflectionRpc.Core.Serialization
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

        public static object UnwrapIfPossible(object o)
        {
            return o is ValueWrapper wrapper ? wrapper.Unwrap() : o;
        }

        public object Unwrap()
        {
            return
                IsEnum ?
                Enum.Parse(Type.GetType(TypeName), (string)Value) :
                Value;
        }
    }
}
