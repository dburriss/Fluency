using System;

namespace Fluency.Conventions
{
    public class LambdaConvention<T> : IDefaultConvention<T>
    {
        readonly Func<Variable, T> _defaultValue;
        readonly Predicate<Variable> _appliesTo;


        public LambdaConvention(Predicate<Variable> appliesTo, Func<Variable, T> defaultValue)
        {
            _appliesTo = appliesTo;
            _defaultValue = defaultValue;
        }


        public bool AppliesTo(Variable v)
        {
            return _appliesTo.Invoke(v);
        }


        object IDefaultConvention.DefaultValue(Variable v)
        {
            return DefaultValue(v);
        }


        public T DefaultValue(Variable v)
        {
            return AppliesTo(v) ? _defaultValue.Invoke(v) : default(T);
        }
    }
}