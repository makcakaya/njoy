using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class CreateMerchantUserFeatureTests
    {
        [Fact]
        public void Can_Create_Merchant()
        {
            var feature = new CreateMerchantUserFeature.Request
            {
            };
        }
    }
}