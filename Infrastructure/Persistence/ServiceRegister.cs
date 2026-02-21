using Application.Interfaces.UnitOfWorks;
using Application.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context.AppDbContext;
using Persistence.Repositories;
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
            .AddEntityFrameworkStores<FitnessTrackerDbContext>();

          
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
