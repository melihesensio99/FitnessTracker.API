using Application.Common.Pagination;
using Application.DTO.WorkoutProgram;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repositories.EntitiesRepository
{
    public interface IWorkoutProgramRepository : IGenericRepository<Domain.Entities.WorkoutProgram>
    {
        Task<WorkoutProgram?> GetWorkoutProgramDetailWithExercisesAsync(int programId);
        Task<List<WorkoutProgram>> GetUserProgramsAsync(int userId);
        Task<PagedResponse<WorkoutProgram>> GetFilteredProgramsAsync(WorkoutProgramFilteredDto filter);
        Task<bool> ActivateProgramByUserIdAsync(int programId, int userId);
    }
}
