using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Nensure;
using Njoy.Services;
using System.Net;

namespace Njoy.Admin
{
    public static class ExceptionHandlingExtensions
    {
        public static void CustomAddExceptionHandling(this IServiceCollection services)
        {
            services.AddTransient<ExceptionHandlingMiddleware>();
        }

        public static void CustomUseExceptionHandling(this IApplicationBuilder app,
            IHostingEnvironment environment)
        {
            Ensure.NotNull(environment);
            GlobalConfiguration
            ExceptionHandlingMiddleware.Configure(GetExceptionResponders());
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
        }

        private static ExceptionResponderCollection GetExceptionResponders()
        {
            var responders = new ExceptionResponderCollection();
            responders.Add<AssertionException>(new ExceptionResponder((context, exception) => (int)HttpStatusCode.BadRequest));
            responders.Add<ValidationException>(new ExceptionResponder((context, exception) => (int)HttpStatusCode.BadRequest));
            return responders;
        }
    }
}