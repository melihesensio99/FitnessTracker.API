using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context.AppDbContext
{
    public class FitnessTrackerDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public FitnessTrackerDbContext(DbContextOptions<FitnessTrackerDbContext> options) : base(options)
        {
        }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutProgram> WorkoutPrograms { get; set; }
        public DbSet<ProgramExercise> ProgramExercises { get; set; }
        public DbSet<WorkoutLog> WorkoutLogs { get; set; }
    }
}
