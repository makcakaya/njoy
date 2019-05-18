using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nensure;
using Njoy.Data;
using System;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    [ApiController, Route(Route)]
    [Authorize(Roles = AppRole.AdminStandard)]
    public sealed class AddressController : ControllerBase
    {
        public const string Route = "api/address";
        private readonly IMediator _mediator;

        public AddressController(IMediator mediator)
        {
            Ensure.NotNull(mediator);
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<GetCitiesFeature.Response> GetCities()
        {
            throw new NotImplementedException();
        }
    }
}