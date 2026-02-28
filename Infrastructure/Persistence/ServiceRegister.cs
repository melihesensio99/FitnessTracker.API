using Application.Abstraction.Event;
using Application.Abstraction.Services;
using Application.Abstraction.UnitOfWorks;
using Application.Repositories;
using Application.Repositories.CommunityRepository;
using Application.Repositories.EntitiesRepository;
using Domain.Entities;
using Infrastructure.Services.Event;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context.AppDbContext;
using Persistence.Repositories;
using Persistence.Repositories.CommunityRepository;
using Persistence.Repositories.EntitiesRepository;
using Persistence.UnitOfWorks;

namespace Persistence
{
    public static class ServiceRegister
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<FitnessTrackerDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSQLConnection")));


            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<FitnessTrackerDbContext>()
            .AddDefaultTokenProviders();

          
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<IWorkoutProgramRepository, WorkoutProgramRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostLikeRepository, PostLikeRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentLikeRepository, CommentLikeRepository>();

           

        }
    }
}
