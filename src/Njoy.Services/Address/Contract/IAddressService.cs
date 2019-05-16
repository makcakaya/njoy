using System.Threading.Tasks;

namespace Njoy.Services
{
    public interface IAddressService
    {
        Task<CreateCityResponse> Create(CreateCityRequest createCity);

        Task<CreateCountyResponse> Create(CreateCountyRequest createCounty);

        Task<CreateDistrictResponse> Create(CreateDistrictRequest createDistrict);
    }
}