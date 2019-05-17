using FluentValidation;
using Nensure;
using Njoy.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Njoy.Services
{
    public sealed class AddressService : IAddressService
    {
        private readonly NjoyContext _context;

        public AddressService(NjoyContext context)
        {
            Ensure.NotNull(context);
            _context = context;
        }

        public async Task<CreateCityResponse> Create(CreateCityRequest createCity)
        {
            Ensure.NotNull(createCity);
            await createCity.ValidateAndThrowAsync(createCity);

            var city = new City
            {
                Name = createCity.Name,
                LicensePlateCode = createCity.LicensePlateCode
            };
            _context.Set<City>().Add(city);
            await _context.SaveChangesAsync();
            return new CreateCityResponse
            {
                Id = city.Id,
                Name = city.Name,
                LicensePlateCode = city.LicensePlateCode
            };
        }

        public async Task<CreateCountyResponse> Create(CreateCountyRequest createCounty)
        {
            Ensure.NotNull(createCounty);
            await createCounty.ValidateAndThrowAsync(createCounty);

            var county = new County
            {
                Name = createCounty.Name,
                CityId = createCounty.CityId
            };
            _context.Set<County>().Add(county);
            await _context.SaveChangesAsync();
            return new CreateCountyResponse
            {
                Id = county.Id,
                Name = county.Name
            };
        }

        public async Task<CreateDistrictResponse> Create(CreateDistrictRequest createDistrict)
        {
            Ensure.NotNull(createDistrict);
            await createDistrict.ValidateAndThrowAsync(createDistrict);

            var district = new District
            {
                Name = createDistrict.Name,
                CountyId = createDistrict.CountyId
            };
            _context.Set<District>().Add(district);
            await _context.SaveChangesAsync();

            return new CreateDistrictResponse
            {
                Id = district.Id,
                Name = district.Name
            };
        }

        public Task<City> GetCity(string name)
        {
            return _context.Set<City>().ToAsyncEnumerable().FirstOrDefault(e => e.Name == name);
        }

        public Task<County> GetCounty(int cityId, string name)
        {
            return _context.Set<County>().ToAsyncEnumerable().FirstOrDefault(e => e.Name == name && e.CityId == cityId);
        }

        public Task<District> GetDistrict(int countyId, string name)
        {
            return _context.Set<District>().ToAsyncEnumerable().FirstOrDefault(e => e.Name == name && e.CountyId == countyId);
        }
    }
}