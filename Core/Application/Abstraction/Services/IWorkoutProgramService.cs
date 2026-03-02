using Application.DTO.ProgramExercise;
using Application.DTO.WorkoutProgram;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Pagination;

namespace Application.Abstraction.Services
{
    public interface IWorkoutProgramService
    {
        Task<List<WorkoutProgramDto>> GetUserProgramsAsync(int userId);
        Task<WorkoutProgramDto> GetWorkoutProgramDetailByIdAsync(int Id);
        Task<WorkoutProgramDto> AddWorkoutProgramAsync(CreateWorkoutProgramDto createDto, int userId);
        Task<bool> DeleteWorkoutProgramAsync(int programId, int userId);
        Task<bool> UpdateWorkoutProgramAsync(UpdateWorkoutProgramDto updateDto, int userId);
        Task<int> CloneSystemProgramToUserAsync(int programId, int userId);
        Task<PagedResponse<WorkoutProgramDto>> GetFilteredProgramsAsync(WorkoutProgramFilteredDto filter);
        Task<bool> ActivateProgramAsync(int programId, int userId);
    }
}
