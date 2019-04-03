using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Njoy.Admin
{
    public class Startup
    {
        private Container _container;
        private readonly JwtSettings _jwtSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            _container = services.CustomAddSimpleInjector();

            services.CustomAddContext(Configuration);

            services.CustomAddIdentity(_jwtSettings);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.CustomAddSpa();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.CustomUseSimpleInjector(_container, Configuration);
            app.CustomUseIdentity();
            _container.Verify();

            app.UseMvcWithDefaultRoute();

            app.CustomUseSpa(env);

            RunCustomInitializationTasks();
        }

        private void RunCustomInitializationTasks()
        {
            using (AsyncScopedLifestyle.BeginScope(_container))
            {
                var mediator = _container.GetService<IMediator>();
                Configuration.CreateRootAccount(mediator);
            }
        }
    }
}