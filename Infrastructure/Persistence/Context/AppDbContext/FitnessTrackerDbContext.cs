using Domain.Entities;
using Domain.Entities.Community;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
        public DbSet<Comment> comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PostMedia> PostMedias { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PostLike>()
         .HasIndex(x => new { x.PostId, x.UserId })
         .IsUnique();

            builder.Entity<Comment>()
    .HasOne<Post>()
    .WithMany(p => p.Comments)
    .HasForeignKey(c => c.PostId)
    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
