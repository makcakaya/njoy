using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Njoy.Admin
{
    public static class FluentValidationExtensions
    {
        public static void CustomAddValidation(this IMvcBuilder builder)
        {
            builder.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        }
    }
}