using Application.Repositories.EntitiesRepository;
using Domain.Entities;
using Persistence.Context.AppDbContext;

namespace Persistence.Repositories.EntitiesRepository
{
    public class ProgramExerciseRepository : GenericRepository<ProgramExercise>, IProgramExerciseRepository
    {
        public ProgramExerciseRepository(FitnessTrackerDbContext context) : base(context)
        {
        }
    }
}
