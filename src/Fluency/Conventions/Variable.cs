using System;
using System.Reflection;

namespace Fluency.Conventions
{
    public class Variable
    {
        public string Name { get; }
        public Type Type { get; }
        public object DefaultValue { get; }

        private Variable(string name, Type type, object defaultValue = null)
        {
            Name = name;
            Type = type;
            if (defaultValue == DBNull.Value)
            {
                defaultValue = null;
            }
            if (defaultValue == null && type.IsValueType)
            {
                defaultValue = Activator.CreateInstance(type);
            }
            DefaultValue = defaultValue;
        }

        internal static Variable From(PropertyInfo propertyInfo)
        {
            return new Variable(propertyInfo.Name, propertyInfo.PropertyType);
        }

        internal static Variable From(ParameterInfo parameterInfo)
        {
            return new Variable(parameterInfo.Name, parameterInfo.ParameterType, parameterInfo.DefaultValue);
        }
    }
}
