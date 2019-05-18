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
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly NjoyContext _context;
            private readonly IAddressService _addressService;
            private readonly ILogger _logger;

            public Handler(NjoyContext context, IAddressService addressService, ILogger logger)
            {
                Ensure.NotNull(context, addressService, logger);
                _context = context;
                _addressService = addressService;
                _logger = logger;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                Ensure.NotNull(request);
                if (request.Cities is null)
                {
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, "There is no city to be created. Skipping.");
                    return new Response();
                }

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
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
                                countyId = response.Id;
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
                    _context.SaveChanges();
                    transaction.Commit();
                    return new Response();
                }
            }
        }

        public sealed class Request : IRequest<Response>
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

        public sealed class Response
        {
        }
    }
}