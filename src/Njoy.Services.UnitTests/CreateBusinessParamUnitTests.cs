using Xunit;

namespace Njoy.Services.UnitTests
{
    public sealed class CreateBusinessParamUnitTests
    {
        [Fact]
        public void Can_Construct()
        {
            new CreateBusinessParam();
        }

        [Fact]
        public void Validate_Returns_True_If_Only_Mandatory_Properties_Are_Set()
        {
            var param = new CreateBusinessParam
            {
                Name = "Test business",
            };

            Assert.True(param.Validate(param).IsValid);
        }

        [Fact]
        public void Validate_Returns_False_If_Optional_Properties_Are_Set_To_Invalid_Values()
        {
            var param = new CreateBusinessParam
            {
                Name = "Test business",
                Address = new CreateBusinessAddressParam { PostalCode = null, StreetAddress = null }
            };

            Assert.False(param.Validate(param).IsValid);

            param = new CreateBusinessParam
            {
                Name = "Test business",
                Contact = new CreateBusinessContactParam { Email = null, Phone = "asd" }
            };

            Assert.False(param.Validate(param).IsValid);
        }
    }
}