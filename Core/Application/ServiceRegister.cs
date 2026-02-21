using Application.Interfaces.Services;
using Application.Mapping;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ServiceRegister
    {

        public static void AddApplicationServices(this IServiceCollection services)
        {

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<Mapping.GeneralMapping>();
            });

        }
    }
}
