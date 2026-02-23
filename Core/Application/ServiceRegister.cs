using Amazon.S3;
using Application.Common.AWS;
using Application.Abstraction.Services;
using Application.Mapping;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ServiceRegister
    {

        public static void AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
        {

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<Mapping.GeneralMapping>();
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            var options = configuration
           .GetSection("AWS")
           .Get<AWSOptions>();

            services.AddSingleton(options);
            services.AddSingleton<IAmazonS3>(
                AwsS3ClientFactory.Create(options));

        }
    }
}
