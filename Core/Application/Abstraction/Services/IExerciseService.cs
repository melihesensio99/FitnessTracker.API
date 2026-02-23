using Application.DTO.Exercise;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstraction.Services
{
    public interface IExerciseService
    {
        Task<List<ExerciseDto>> GetAllExercisesAsync();
        Task<ExerciseDto> GetExerciseByIdAsync(int exerciseId);
        Task<ExerciseDto> GetExercisesByMuscleGroup(string muscleGroup);
        Task<List<ExerciseDto>> SearchExercisesByNameAsync(string name);
        Task<ExerciseDto> AddExerciseAsync(CreateExerciseDto createDto);
        Task<bool> UpdateExerciseAsync(UpdateExerciseDto updateDto);
        Task<bool> DeleteExerciseAsync(int exerciseId);
    }
}
