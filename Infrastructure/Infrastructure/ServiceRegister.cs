using Application.Interfaces.Services;
using Infrastructure.Services;
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
        }
    }
}
