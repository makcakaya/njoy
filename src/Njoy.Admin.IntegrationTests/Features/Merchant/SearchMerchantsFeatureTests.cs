using Njoy.Services;
using System.Threading;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class SearchMerchantsFeatureTests
    {
        private ServiceProviderHelper ServiceProvider { get; } = ServiceProviderHelper.CreateInstance<SearchMerchantsFeatureTests>();

        [Fact]
        public async void Returns_Empty_If_Not_Found()
        {
            var merchantService = ServiceProvider.Get<IMerchantService>();
            var handler = new SearchMerchantsFeature.Handler(merchantService);
            var request = new SearchMerchantsFeature.Request { SearchPhrase = "test" };

            var result = await handler.Handle(request, new CancellationToken());

            Assert.NotNull(result.Merchants);
            Assert.Empty(result.Merchants);
        }

        [Fact]
        public async void Does_Not_Throw_If_Id_Does_Not_Exist()
        {
            var merchantService = ServiceProvider.Get<IMerchantService>();
            var handler = new GetMerchantFeature.Handler(merchantService);
            var request = new GetMerchantFeature.Request { MerchantId = 13 };

            var result = await handler.Handle(request, new CancellationToken());
            Assert.Null(result);
        }
    }
}