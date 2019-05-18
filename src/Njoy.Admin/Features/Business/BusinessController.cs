using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nensure;
using Njoy.Data;

namespace Njoy.Admin
{
    [ApiController, Route(Route)]
    [Authorize(Roles = AppRole.AdminStandard)]
    public sealed class BusinessController : ControllerBase
    {
        public const string Route = "api/business";
        private readonly IMediator _mediator;

        public BusinessController(IMediator mediator)
        {
            Ensure.NotNull(mediator);
            _mediator = mediator;
        }

        [HttpPost, Route("Create")]
        public async void Create()
        {
        }
    }
}