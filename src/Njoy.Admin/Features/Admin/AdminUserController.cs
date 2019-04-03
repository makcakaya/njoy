using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Njoy.Admin.Features;
using System;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    [Authorize]
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

        [HttpPost, Route("create")]
        public async Task<AdminUserRowModel> Create(CreateAdminUserFeature.Request request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost, Route("login")]
        [AllowAnonymous]
        public async Task<string> Login(LoginAdminUserFeature.Request request)
        {
            return await _mediator.Send(request);
        }
    }
}