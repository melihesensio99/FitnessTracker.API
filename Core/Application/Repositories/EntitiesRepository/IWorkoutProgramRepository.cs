using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repositories.EntitiesRepository
{
    public interface IWorkoutProgramRepository : IGenericRepository<Domain.Entities.WorkoutProgram>
    {
        Task<WorkoutProgram?> GetWorkoutProgramDetailWithExercisesAsync(int programId);
        Task<List<WorkoutProgram>> GetUserWorkoutProgramsAsync(int userId);
      Task<List<WorkoutProgram>> GetSystemWorkoutProgramsAsync();    



    }
}
