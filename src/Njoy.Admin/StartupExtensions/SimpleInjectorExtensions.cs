using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System;
using System.Linq;

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

        public static void CustomUseSimpleInjector(this IApplicationBuilder app, Container container)
        {
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Register MediatR
            {
                container.RegisterSingleton<IMediator, Mediator>();
                container.Register(typeof(IRequestHandler<,>), typeof(Startup).Assembly);

                container.Collection.Register(typeof(IPipelineBehavior<,>), Enumerable.Empty<Type>());
                container.Collection.Register(typeof(IRequestPreProcessor<>), Enumerable.Empty<Type>());
                container.Collection.Register(typeof(IRequestPostProcessor<,>), Enumerable.Empty<Type>());

                container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);
            }

            container.AutoCrossWireAspNetComponents(app);
        }
    }
}