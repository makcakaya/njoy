using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Njoy.Admin.Features;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Threading;

namespace Njoy.Admin
{
    public class Startup
    {
        private Container _container;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            _container = services.CustomAddSimpleInjector();

            services.AddMediatR(typeof(Startup).Assembly);

            services.CustomAddContext(Configuration);

            services.CustomAddIdentity();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.CustomAddSpa();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.CustomUseSimpleInjector(_container);

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.CustomUseIdentity();

            _container.Verify();

            var blocker = new ManualResetEvent(false);

            {
                var scope = AsyncScopedLifestyle.BeginScope(_container);
                var userManager = _container.GetInstance<UserManager<AdminUser>>();
                var feature = new CreateDefaultUserFeature(userManager);
                feature.Run(new CreateDefaultUserParam
                {
                    Username = "root",
                    Password = "Password@123"
                }).ContinueWith((t) =>
                {
                    scope.Dispose();
                    blocker.Set();
                });
            }

            blocker.WaitOne();

            app.UseMvcWithDefaultRoute();

            app.CustomUseSpa(env);
        }
    }
}