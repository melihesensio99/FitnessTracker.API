using Application.Abstraction.Services;
using Application.Abstraction.Storage;
using Application.Abstraction.Token;
using Infrastructure.Consumers;
using Infrastructure.Services;
using Infrastructure.Services.Event;
using Infrastructure.Storage.AWS;
using Infrastructure.Token;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceRegister
    {
        public static void AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IWorkoutProgramService, WorkoutProgramService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IStorageService, AwsS3StorageService>();
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEventBus, MassTransitEventBus>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserRegisteredConsumer>();
                x.AddConsumer<EmailVerifiedConsumer>();
                x.AddConsumer<MediaUploadConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });

                    cfg.ReceiveEndpoint("user-registered", e =>
                        e.ConfigureConsumer<UserRegisteredConsumer>(context));

                    cfg.ReceiveEndpoint("email-verified", e =>
                        e.ConfigureConsumer<EmailVerifiedConsumer>(context));

                    cfg.ReceiveEndpoint("media-upload", e =>
                        e.ConfigureConsumer<MediaUploadConsumer>(context));
                });
            });
        }
    }
}
