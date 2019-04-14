using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.CustomAddMvc();
            services.CustomAddSpa();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.CustomUseExceptionHandling(env);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.CustomUseSimpleInjector(_container, Configuration);
            app.CustomUseIdentity();
            app.UseMvcWithDefaultRoute();
            app.CustomUseSpa(env);
            RunCustomInitializationTasks();
        }

        private void RunCustomInitializationTasks()
        {
            using (AsyncScopedLifestyle.BeginScope(_container))
            {
                var mediator = _container.GetService<IMediator>();
                CustomStartupTasksExtensions.Run(mediator);
            }
        }
    }
}