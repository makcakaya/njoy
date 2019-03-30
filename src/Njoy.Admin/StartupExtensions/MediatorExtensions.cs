using MediatR;
using MediatR.Pipeline;
using SimpleInjector;
using System.Linq;

namespace Njoy.Admin
{
    public static class MediatorExtensions
    {
        public static void RegisterMediator(this Container container)
        {
            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), typeof(Startup).Assembly);

            container.Collection.Register(typeof(IPipelineBehavior<,>), Enumerable.Empty<Type>());
            container.Collection.Register(typeof(IRequestPreProcessor<>), Enumerable.Empty<Type>());
            container.Collection.Register(typeof(IRequestPostProcessor<,>), Enumerable.Empty<Type>());

            container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);
        }
    }
}