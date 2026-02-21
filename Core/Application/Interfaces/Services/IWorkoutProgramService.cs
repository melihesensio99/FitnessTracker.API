using Application.DTO.ProgramExercise;
using Application.DTO.WorkoutProgram;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IWorkoutProgramService
    {
        Task<List<WorkoutResultDto>> GetUserWorkoutProgramsAsync(int userId);
        Task<List<WorkoutResultDto>> GetSystemWorkoutProgramsAsync();
        Task<WorkoutResultDto> GetWorkoutProgramDetailByIdAsync(int Id);
        Task<WorkoutResultDto> AddWorkoutProgramAsync(CreateWorkoutProgramDto createDto);
        Task<bool> DeleteWorkoutProgramAsync(int programId);
        Task<bool> UpdateWorkoutProgramAsync(UpdateWorkoutProgramDto updateDto);
        Task<bool> IsProgramPublic(int programId);
        Task<int> CloneSystemProgramToUserAsync(int programId, int userId);
    }
}
