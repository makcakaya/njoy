﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace Njoy.Admin
{
    public static class SimpleInjectorExtensions
    {
        public static Container CustomAddSimpleInjector(this IServiceCollection services)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(container));

            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);

            return container;
        }

        public static void CustomUseSimpleInjector(this IApplicationBuilder app, Container container,
            IConfiguration configuration)
        {
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);
            container.RegisterMediator();
            container.RegisterNLog();
            container.AutoCrossWireAspNetComponents(app);

            // Register custom dependencies (services etc)
            container.Register<IJwtService, JwtService>();

            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            container.RegisterInstance(jwtSettings);
        }
    }
}