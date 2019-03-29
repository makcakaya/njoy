using MediatR;
using Microsoft.AspNetCore.Mvc;
using Njoy.Admin.Features;
using System;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    [ApiController, Route("api/[controller]")]
    public sealed class AdminUserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminUserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<AdminUserRowModel> Update(EditAdminUserFeature.Request request)
        {
            return await _mediator.Send(request);
        }
    }
}