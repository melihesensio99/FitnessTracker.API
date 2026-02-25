using Application.Abstraction.Services;
using Application.Abstraction.Storage;
using Application.Abstraction.Token;
using Infrastructure.Services;
using Infrastructure.Storage.AWS;
using Infrastructure.Token;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Infrastructure
{

    public static class ServiceRegister
    {

        public static void AddInfrastructureService(this IServiceCollection services)
        {

            services.AddScoped<IWorkoutProgramService, WorkoutProgramService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IStorageService, AwsS3StorageService>();
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
