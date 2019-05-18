using MediatR;
using Nensure;
using Njoy.Data;
using Njoy.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    public sealed class CreateAddressPartsFeature
    {
        public sealed class Handler : IRequestHandler<Request>
        {
            private readonly NjoyContext _context;
            private readonly IAddressService _addressService;

            public Handler(NjoyContext context, IAddressService addressService)
            {
                Ensure.NotNull(context, addressService);
                _context = context;
                _addressService = addressService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                Ensure.NotNull(request);
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    if (request.Cities == null) { return; }
                    foreach (var city in request.Cities)
                    {
                        var loadedCity = await _addressService.GetCity(city.Name);
                        var cityId = loadedCity?.Id ?? 0;
                        if (loadedCity is null)
                        {
                            var response = await _addressService.Create(new CreateCityRequest
                            {
                                Name = city.Name,
                                LicensePlateCode = city.LicensePlateCode
                            });
                            cityId = response.Id;
                        }

                        if (city.Counties is null) { continue; }
                        foreach (var county in city.Counties)
                        {
                            var loadedCounty = await _addressService.GetCounty(cityId, county.Name);
                            var countyId = loadedCounty?.Id ?? 0;
                            if (loadedCounty is null)
                            {
                                var response = await _addressService.Create(new CreateCountyRequest
                                {
                                    Name = county.Name,
                                    CityId = cityId
                                });
                            }

                            if (county.Districts is null) { continue; }
                            foreach (var district in county.Districts)
                            {
                                if (await _addressService.GetDistrict(countyId, district.Name) is null)
                                {
                                    await _addressService.Create(new CreateDistrictRequest
                                    {
                                        Name = district.Name,
                                        CountyId = countyId
                                    });
                                }
                            }
                        }
                    }
                    transaction.Commit();
                    return Unit.Value;
                }
            }
        }

        public sealed class Request : IRequest
        {
            public IEnumerable<City> Cities { get; set; }

            public sealed class City
            {
                public string Name { get; set; }
                public int LicensePlateCode { get; set; }
                public IEnumerable<County> Counties { get; set; }
            }

            public sealed class County
            {
                public string Name { get; set; }
                public IEnumerable<District> Districts { get; set; }
            }

            public sealed class District
            {
                public string Name { get; set; }
            }
        }
    }
}