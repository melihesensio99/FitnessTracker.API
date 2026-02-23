using Application.Abstraction.Services;
using Application.Abstraction.Storage;
using Infrastructure.Services;
using Infrastructure.Storage.AWS;
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
            services.AddScoped<IStorageService, AwsS3StorageService>();
        }
    }
}
