using NUnit.Framework;
using SampleApplication.Domain;
using SampleApplication.Tests.FluentBuilders;


namespace SampleApplication.Tests.UnitTests.FluentBuilders
{
    [ TestFixture ]
    public class LineItemBuilderTests
    {
        [ Test ]
		[Ignore("Work In Progress")]
        public void Specifying_a_ProductBuilder_should_use_that_ProductBuilder_to_return_the_result()
        {
            Product product = a.Product.build();

            LineItem lineItem = a.LineItem.For( a.Product.AliasFor( product ) ).build();

            Assert.AreSame( product, lineItem.Product );
        }
    }
}