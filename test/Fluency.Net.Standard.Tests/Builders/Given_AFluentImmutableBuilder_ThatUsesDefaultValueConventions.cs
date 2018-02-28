using Fluency.DataGeneration;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fluency.Net.Standard.Tests.Builders
{
    public class Given_AFluentImmutableBuilder_ThatUsesDefaultValueConventions
    {
        protected readonly FluentImmutableBuilder<ImmutableTestClass> Sut;

        protected class ImmutableTestClass
        {
            public string StringProperty { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int IntegerProperty { get; set; }
            public DateTime DateTimeProperty { get; set; }
            public ImmutableTestClass ReferenceProperty { get; set; }
            public IList<ImmutableTestClass> ListProperty { get; set; }

            public ImmutableTestClass(string stringProperty, string firstName, string lastName, int integerProperty, ImmutableTestClass referenceProperty, IList<ImmutableTestClass> listProperty)
            {
                StringProperty = stringProperty;
                FirstName = firstName;
                LastName = lastName;
                IntegerProperty = integerProperty;
                ReferenceProperty = referenceProperty;
                ListProperty = listProperty;
            }
        }

        public Given_AFluentImmutableBuilder_ThatUsesDefaultValueConventions()
        {
            Fluency.Initialize(x => x.UseDefaultValueConventions());
            Sut = new FluentImmutableBuilder<ImmutableTestClass>(true);
        }

        public class WhenBuildingTheObject : Given_AFluentImmutableBuilder_ThatUsesDefaultValueConventions
        {
            private readonly ImmutableTestClass _result;

            public WhenBuildingTheObject()
            {
                _result = Sut.build();
            }

            [Fact]
            public void It_Should_UseDefaultConventionFor_FirstNameProperty() =>
                _result.FirstName.Should().BeOneOf(RandomData.FirstNames);
        }
    }
}
