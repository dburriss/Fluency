using System.Reflection;

namespace Fluency.Conventions
{
    public interface IDefaultConvention
    {
        bool AppliesTo(Variable v);
        object DefaultValue(Variable v);
    }


    public interface IDefaultConvention<T> : IDefaultConvention
    {
        new T DefaultValue(Variable v);
    }
}