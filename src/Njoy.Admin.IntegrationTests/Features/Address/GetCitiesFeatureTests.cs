using Njoy.Services;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class GetCitiesFeatureTests
    {
        private ServiceProviderHelper ServiceProvider { get; } = ServiceProviderHelper.CreateInstance<SearchMerchantsFeatureTests>();

        [Fact]
        public async void Can_Get_All_Cities()
        {
            var numOfCitiesToCreate = 10;
            var addressService = ServiceProvider.Get<IAddressService>();
            var handler = new GetCitiesFeature.Handler(addressService);
            var request = new GetCitiesFeature.Request();

            async Task CreateCity(string name, int licensePlateCode)
            {
                await addressService.Create(new CreateCityRequest
                {
                    Name = name,
                    LicensePlateCode = licensePlateCode
                });
            }

            for (var i = 1; i < numOfCitiesToCreate; i++)
            {
                await CreateCity("TestCity" + i, i);
            }

            var response = await handler.Handle(request, new System.Threading.CancellationToken());

            Assert.NotNull(response);
            Assert.NotEmpty(response.Cities);
            Assert.Equal(numOfCitiesToCreate, response.Cities.Count());
        }
    }
}