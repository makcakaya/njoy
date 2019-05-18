using Njoy.Data;
using Njoy.Services;
using System.Linq;
using System.Threading;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class CreateBusinessFeatureTests
    {
        private ServiceProviderHelper ServiceProvider { get; } = ServiceProviderHelper.CreateInstance<SearchMerchantsFeatureTests>();

        [Fact]
        public void Can_Construct_Handler()
        {
            new CreateBusinessFeature.Handler(ServiceProvider.Get<IBusinessService>());
        }

        [Fact]
        public async void Can_Create_Business_With_Full_Details()
        {
            var request = new CreateBusinessFeature.Request
            {
                Name = "Test Business",
                Contact = new CreateBusinessContactParam
                {
                    Email = "test@testbusiness.com",
                    Phone = "3122220404",
                },
                Address = new CreateBusinessAddressParam
                {
                    DistrictId = 1,
                    PostalCode = "06100",
                    StreetAddress = "Test street address"
                }
            };

            var handler = new CreateBusinessFeature.Handler(ServiceProvider.Get<IBusinessService>());
            await handler.Handle(request, default(CancellationToken));

            var context = ServiceProvider.Get<NjoyContext>();
            var business = context.Set<Business>().First();
            Assert.Equal(request.Name, business.Name);
            Assert.NotNull(business.Address);
        }
    }
}