using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    [ApiController, Route(Route)]
    public sealed class MerchantUserController : ControllerBase
    {
        public const string Route = "api/merchantuser";

        private readonly IMediator _mediator;

        public MerchantUserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost, Route("create")]
        public async Task Create(CreateMerchantUserFeature.Request request)
        {
            await _mediator.Send(request);
            return;
        }
    }
}