using Application.Common.Pagination;
using Application.DTO.Exercise;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.Services
{
    public interface IExerciseService
    {
        Task<List<ExerciseDto>> GetAllExercisesAsync();
        Task<ExerciseDto> GetExerciseByIdAsync(int exerciseId);
        Task<PagedResponse<ExerciseDto>> GetExercisesByMuscleGroup(string muscleGroup, PagedRequest request);
        Task<PagedResponse<ExerciseDto>> SearchExercisesByNameAsync(string name, PagedRequest request);
        Task<ExerciseDto> AddExerciseAsync(CreateExerciseDto createDto);
        Task<bool> UpdateExerciseAsync(UpdateExerciseDto updateDto);
        Task<bool> DeleteExerciseAsync(int exerciseId);
    }
}
