using Application.DTO.WorkoutLog;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstraction.Services
{
    public interface IWorkoutLogService
    {
        Task<List<WorkoutLogDto>> GetUserLogsAsync(int userId);
        Task<List<WorkoutLogDto>> GetExerciseLogsAsync(int programExerciseId);
        Task<List<WorkoutLogDto>> GetLogsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<WorkoutLogDto?> GetUserLastLogAsync(int userId, int programExerciseId);
        Task<WorkoutLogDto> AddWorkoutLogAsync(CreateWorkoutLogDto createDto);

    }
}
