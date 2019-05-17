using Njoy.Data;
using System.Threading.Tasks;

namespace Njoy.Services
{
    public interface IAddressService
    {
        Task<CreateCityResponse> Create(CreateCityRequest createCity);

        Task<CreateCountyResponse> Create(CreateCountyRequest createCounty);

        Task<CreateDistrictResponse> Create(CreateDistrictRequest createDistrict);

        Task<City> GetCity(string name);

        Task<County> GetCounty(int cityId, string name);

        Task<District> GetDistrict(int countyId, string name);
    }
}