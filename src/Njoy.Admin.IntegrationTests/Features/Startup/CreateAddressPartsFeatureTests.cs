using Njoy.Data;
using Njoy.Services;
using System.Threading;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class CreateAddressPartsFeatureTests
    {
        private ServiceProviderHelper ServiceProvider { get; } = ServiceProviderHelper.CreateInstance<CreateAddressPartsFeatureTests>();

        [Fact]
        public async void Can_Create_Whole_Hierarchy()
        {
            var context = ServiceProvider.Get<NjoyContext>();
            var addressService = ServiceProvider.Get<IAddressService>();
            var logger = ServiceProvider.Get<ILogger>();
            var handler = new CreateAddressPartsFeature.Handler(context, addressService, logger);
            var request = new CreateAddressPartsFeature.Request
            {
                Cities = null
            };

            await handler.Handle(null, new CancellationToken());
        }
    }
}