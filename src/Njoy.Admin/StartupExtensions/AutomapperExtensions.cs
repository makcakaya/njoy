using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Njoy.Services;

namespace Njoy.Admin
{
    public static class AutomapperExtensions
    {
        public static void CustomAddMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<CreateMerchantUserFeature.Request, CreateUserRequest>();
            });

            var mapper = new Mapper(config);
            services.AddSingleton<IMapper>(mapper);
        }
    }
}