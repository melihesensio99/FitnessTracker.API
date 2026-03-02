using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repositories.EntitiesRepository
{
    public interface IWorkoutLogRepository : IGenericRepository<WorkoutLog>
    {
        Task<List<WorkoutLog>> GetUserLogsAsync(int userId);
        Task<List<WorkoutLog>> GetExerciseLogsAsync(int programExerciseId);
        Task<List<WorkoutLog>> GetLogsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<WorkoutLog?> GetUserLastLogAsync(int userId, int programExerciseId);
    }
}
