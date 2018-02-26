using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fluency.Net.Standard.Tests.Builders
{
    public class Given_AnImmutableClass
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

        public Given_AnImmutableClass()
        {
            Sut = new FluentImmutableBuilder<ImmutableTestClass>();
        }

        public class WhenBuildingTheObject : Given_AnImmutableClass
        {
            private readonly ImmutableTestClass _result;

            public WhenBuildingTheObject()
            {
                _result = Sut.build();
            }

            [Fact]
            public void It_Should_NotBeNull() =>
                _result.Should().NotBeNull();

            [Fact]
            public void String_Should_Be_Default()
            {
                _result.StringProperty.Should().Be(default(string));
                _result.FirstName.Should().Be(default(string));
                _result.LastName.Should().Be(default(string));
            }

            [Fact]
            public void Int_Should_Be_Default()
            {
                _result.IntegerProperty.Should().Be(default(int));
            }

            [Fact]
            public void DateTime_Should_Be_Default()
            {
                _result.DateTimeProperty.Should().Be(default(DateTime));
            }

            [Fact]
            public void Reference_Should_Be_Default()
            {
                _result.ReferenceProperty.Should().Be(default(ImmutableTestClass));
            }

            [Fact]
            public void List_Should_Be_Default()
            {
                _result.ReferenceProperty.Should().Be(default(IList<ImmutableTestClass>));
            }
        }
    }
}
