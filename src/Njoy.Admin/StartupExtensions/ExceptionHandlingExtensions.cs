using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Nensure;

namespace Njoy.Admin
{
    public static class ExceptionHandlingExtensions
    {
        public static void CustomUseExceptionHandling(this IApplicationBuilder app,
            IHostingEnvironment environment)
        {
            Ensure.NotNull(environment);
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
    }
}