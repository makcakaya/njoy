using Njoy.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Njoy.Services
{
    public interface IAddressService
    {
        Task<CreateCityResponse> Create(CreateCityRequest createCity);

        Task<CreateCountyResponse> Create(CreateCountyRequest createCounty);

        Task<CreateDistrictResponse> Create(CreateDistrictRequest createDistrict);

        Task<City> GetCity(string name);

        Task<IEnumerable<City>> GetCities(Func<City, bool> predicate = null);

        Task<County> GetCounty(int cityId, string name);

        Task<District> GetDistrict(int countyId, string name);
    }
}