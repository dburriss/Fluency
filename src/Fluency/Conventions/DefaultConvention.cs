namespace Fluency.Conventions
{
    public abstract class DefaultConvention<T> : IDefaultConvention<T>
    {
        public abstract bool AppliesTo(Variable v);
        public abstract T DefaultValue(Variable v);


        /// <summary>
        /// Gets the default value for the specirfied property.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
        object IDefaultConvention.DefaultValue(Variable v)
        {
            // Fake covariance by returning object when cast as IDefaultConvetion.
            return DefaultValue(v);
        }
    }
}