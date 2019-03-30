using SimpleInjector;

namespace Njoy.Admin
{
    public static class NLogExtensions
    {
        public static void RegisterNLog(this Container container)
        {
            container.RegisterConditional(typeof(ILogger),
                context => typeof(NLogProxy<>).MakeGenericType(context.Consumer.ImplementationType),
                Lifestyle.Singleton, context => true);
        }
    }
}