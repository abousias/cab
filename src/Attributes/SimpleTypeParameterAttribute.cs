using System;
using System.Linq;
using System.Reflection;

namespace ConsoleAppBase.Attributes
{


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class BaseParameterAttribute : Attribute
    {
        public abstract bool ValidateValue(PropertyInfo property, string value);
        public abstract void SetValue(Command command, PropertyInfo property, string value);
    }

    public abstract class SimpleTypeParameterAttribute : BaseParameterAttribute
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
            {
                return Enum.Parse(propertyType, arg);
            }

            return Convert.ChangeType(arg, propertyType);
        }
    }
}
