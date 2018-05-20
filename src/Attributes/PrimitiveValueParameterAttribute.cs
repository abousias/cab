using System;
using System.Linq;
using System.Reflection;

namespace ConsoleAppBase.Attributes
{


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class ParameterAttribute : Attribute
    {
        public abstract bool ValidateValue(PropertyInfo property, string value);

        public abstract void SetValue(Command command, PropertyInfo property, string value);
    }

    internal abstract class PrimitiveValueParameterAttribute : ParameterAttribute
    {
        public override void SetValue(Command command, PropertyInfo property, string value)
        {
            var valueObj = ConvertValueForProperty(property, value);
            property.SetValue(command, valueObj);
        }

        private object ConvertValueForProperty(PropertyInfo property, string arg)
        {
            var propertyType = property.PropertyType;

            if (propertyType.IsEnum)
                return Enum.Parse(propertyType, arg);
            else if (IsSimpleType(propertyType))
                return Convert.ChangeType(arg, propertyType);
            //else
            //    return JsonConvert.DeserializeObject(arg, propertyType);

            throw new NotSupportedException();
        }

        private bool IsSimpleType(Type type)
        {
            return
                type.IsPrimitive ||
                new Type[] {
                    typeof(String),
                    typeof(Decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]));
        }
    }
}
