using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using FluentNHibernate.Utils;
using Rhino.Mocks;


namespace FluentObjectBuilder
{
	public abstract class TestDataBuilder< T > : ITestDataBuilder< T > where T : class, new()
	{
		private static bool _executedBuildOnceAlready;
		private static int _uniqueId = -10;
		private readonly Dictionary< string, ITestDataBuilder > _builders = new Dictionary< string, ITestDataBuilder >();
		private readonly MockRepository _mocks;
		protected T _preBuiltResult;
		protected T _prototype;


		/// <summary>
		/// Initializes a new instance of the <see cref="TestDataBuilder&lt;T&gt;"/> class.
		/// </summary>
		public TestDataBuilder()
		{
			_mocks = new MockRepository();
			_prototype = _mocks.Stub< T >();
		}


		#region ITestDataBuilder<T> Members

		/// <summary>
		/// Either returns the override result, or builds this object.
		/// </summary>
		/// <returns></returns>
		public T build()
		{
			if ( _executedBuildOnceAlready )
				throw new Exception( "Cannot build more than once [" + GetType().FullName + "]." );

			foreach ( var pair in _builders )
			{
				string propertyName = pair.Key;
				object propertyValue = pair.Value.GetType().GetMethod( "build" ).Invoke( pair.Value, null ); // Calls builder.build()
				_prototype.GetType().GetProperty( propertyName ).SetValue( _prototype, propertyValue, null );
			}

			return _preBuiltResult ?? _build();//BuildFromPrototype( _prototype );
			//_executedBuildOnceAlready = true;
		}

		#endregion


		private T BuildFromPrototype( T prototype )
		{
			// User Automapper to copy values
			
			//IMappingExpression< T, T > map = Mapper.CreateMap< T, T >();
			// TODO: Must fix... readonly properties break.
			//map.IgnoreReadOnlyProperties();
			//var result = new T();
			var  result = Mapper.DynamicMap<T,T>( prototype);
			return result;
		}


		public void SetPropertyValue< TPropertyType >( Expression< Func< T, TPropertyType > > propertyExpression, TPropertyType propertyValue )
		{
			Accessor accessor = ReflectionHelper.GetAccessor( propertyExpression );
			accessor.SetValue( _prototype, propertyValue );
		}


		public void SetPropertyBuilder< TPropertyType >( Expression< Func< T, TPropertyType > > propertyExpression, TestDataBuilder< TPropertyType > builder )
				where TPropertyType : class, new()
		{
			PropertyInfo property = ReflectionHelper.GetProperty( propertyExpression );

			if ( _builders.ContainsKey( property.Name ) )
				_builders.Remove( property.Name );

			_builders.Add( property.Name, builder );

//			var func = new Function< T, TPropertyType >( propertyExpression.Compile() );
//			_prototype
//					.Stub( func )
//					.WhenCalled( x => { x.ReturnValue = ( (TestDataBuilder< TPropertyType >)_builders[propertyName] ).build(); } );
		}


		/// <summary>
		/// Builds an object of type T based on the specs specified in the builder.
		/// </summary>
		/// <returns></returns>
		protected abstract T _build();


		/// <summary>
		/// Performs an implicit conversion from <see cref="BancVue.Tests.Common.TestDataBuilders.TestDataBuilder&lt;T&gt;"/> to <see cref="T"/>.
		/// This is so we don't have to explicitly call "build()" in the code.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator T( TestDataBuilder< T > builder )
		{
			return builder.build();
		}


		/// <summary>
		/// Gets a unique id with a negative value so that it does not conflict with existing data in the database.
		/// </summary>
		/// <returns></returns>
		protected int GetUniqueId()
		{
			return _uniqueId--;
		}


		/// <summary>
		/// Overrides the build result with the specified object. If this is called, the builder will not perform the build, but will rather, return the prebuilt result.
		/// </summary>
		/// <param name="buildResult">The build result.</param>
		public TestDataBuilder< T > AliasFor( T buildResult )
		{
			return UsePreBuiltResult( buildResult );
		}


		/// <summary>
		/// Overrides the build result with the specified object. If this is called, the builder will not perform the build, but will rather, return the prebuilt result.
		/// </summary>
		/// <param name="buildResult">The build result.</param>
		public TestDataBuilder< T > UsePreBuiltResult( T buildResult )
		{
			_preBuiltResult = buildResult;
			return this;
		}
	}
}