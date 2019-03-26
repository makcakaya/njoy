using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Njoy.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class TestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TestController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("[action]")]
        public string Test()
        {
            return "hello World";
        }

        [HttpGet("[action]")]
        [Authorize]
        public string TestAuth()
        {
            return "hello World";
        }
    }
}