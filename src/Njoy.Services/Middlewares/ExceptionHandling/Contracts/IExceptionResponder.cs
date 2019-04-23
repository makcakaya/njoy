using Microsoft.AspNetCore.Http;
using System;

namespace Njoy.Services
{
    public interface IExceptionResponder
    {
        void Respond(HttpContext context, Exception exception);
    }
}