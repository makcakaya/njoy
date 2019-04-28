using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Nensure;

namespace Njoy.Admin
{
    public static class ExceptionHandlingExtensions
    {
        public static void CustomAddExceptionHandling(this IServiceCollection services)
        {
            services.AddTransient<AdminExceptionHandlingMiddleware>();
        }

        public static void CustomUseExceptionHandling(this IApplicationBuilder app,
            IHostingEnvironment environment)
        {
            Ensure.NotNull(environment);
            app.UseMiddleware<AdminExceptionHandlingMiddleware>();
            if (environment.IsDevelopment())
            {
                app.UseStatusCodePages();
            }
            else
            {
                app.UseHsts();
            }
        }
    }
}