using MediatR;
using Nensure;
using Njoy.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    public sealed class GetCitiesFeature
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAddressService _addressService;

            public Handler(IAddressService addressService)
            {
                Ensure.NotNull(addressService);
                _addressService = addressService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var cities = await _addressService.GetCities();
                return new Response
                {
                    Cities = cities?.Select(c => new Response.City
                    {
                        Id = c.Id,
                        Name = c.Name,
                        LicensePlateCode = c.LicensePlateCode
                    }).ToArray()
                };
            }
        }

        public sealed class Request : IRequest<Response>
        {
        }

        public sealed class Response
        {
            public IEnumerable<City> Cities { get; set; }

            public sealed class City
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public int LicensePlateCode { get; set; }
            }
        }
    }
}