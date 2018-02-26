using FluentAssertions;
using System;
using Xunit;

namespace Fluency.Net.Standard.Tests.Builders
{
    public class Given_AnImmutableBuilderForDefaultCtor
    {
        protected readonly FluentImmutableBuilder<TestClass> Sut;

        protected class TestClass
        {
            public string StringProperty { get; set; }
        }

        public class WhenInitialisingTheBuilder : Given_AnImmutableBuilderForDefaultCtor
        {
            [Fact]
            public void It_Throws_FluencyException()
            {
                Action act = () => new FluentImmutableBuilder<TestClass>();
                act.ShouldThrow<FluencyException>();
            }
        }
    }
}
