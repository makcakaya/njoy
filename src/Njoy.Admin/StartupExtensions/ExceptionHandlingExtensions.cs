using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Njoy.Admin
{
    public static class ExceptionHandlingExtensions
    {
        public static void CustomUseExceptionHandling(this IApplicationBuilder app,
            IHostingEnvironment environment)
        {
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