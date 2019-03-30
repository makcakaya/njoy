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

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            _container = services.CustomAddSimpleInjector();

            //services.AddMediatR(typeof(Startup).Assembly);

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