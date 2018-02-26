using Fluency.Conventions;
using Fluency.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fluency
{
    public class FluentImmutableBuilder<T> : IFluentBuilder<T> where T : class
    {
        private IDictionary<string, object> _values = new Dictionary<string, object>();
        private ConstructorInfo _constructor;
        private readonly IList<IDefaultConvention> _defaultConventions = new List<IDefaultConvention>();
        protected IIdGenerator IdGenerator;

        public FluentImmutableBuilder()
        {
            IdGenerator = Fluency.Configuration.GetIdGenerator<T>();
            _defaultConventions = Fluency.Configuration.DefaultValueConventions;
            SetupDefaultValues();
            if(_constructor.GetParameters().Length == 0)
            {
                throw new FluencyException($"Using parameterless constructor for Type {typeof(T).Name}. Use `FluentBuilder<{typeof(T).Name}>` instead.");
            }
        }

        protected virtual void SetupDefaultValues()
        {
            _constructor = CtorWithMostArgs(typeof(T));
            if (_constructor != null)
            {
                SetDefaultsForArgs(_constructor);
            }
        }

        private ConstructorInfo CtorWithMostArgs(Type type)
        {
            var ci = type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();
            return ci;
        }

        private ConstructorInfo CtorWithLeastArgs(Type type)
        {
            var ci = type.GetConstructors()
                .OrderBy(c => c.GetParameters().Length)
                .FirstOrDefault();
            return ci;
        }

        private void SetDefaultsForArgs(ConstructorInfo ctor)
        {
            var ps = ctor.GetParameters();
            foreach (var pi in ps)
            {
                var name = pi.Name;
                var t = pi.ParameterType;
                var defaultValue = pi.DefaultValue;
                if (defaultValue == DBNull.Value)
                {
                    defaultValue = null;
                }
                else
                {
                    defaultValue = GetDefault(t);
                }
                _values[name] = defaultValue;
            }
        }

        public static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        protected void SetCtorValue<TPropertyType>(string argName, TPropertyType value)
        {
            _values[argName] = value;
        }

        protected virtual T GetNewInstance()
        {
            if (_constructor != null)
            {
                var ps = _constructor.GetParameters();

                if (ps.Length > 0)
                {
                    object[] args = new object[ps.Length];
                    for (int i = 0; i < ps.Length; i++)
                    {
                        var name = ps[i].Name;
                        args[i] = _values[name];
                    }
                    var instance = _constructor.Invoke(args);
                    return (T)instance;
                }
            }
            return default(T);
        }

        protected object GetObject(string key)
        {
            return _values[key];
        }

        protected TValue GetValue<TValue>(string key)
        {
            return (TValue)GetObject(key);
        }

        public T build()
        {
            return GetNewInstance();
        }
    }
}
