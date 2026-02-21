using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repositories.EntitiesRepository
{
    public interface IExerciseRepository : IGenericRepository<Domain.Entities.Exercise>
    {
        Task<List<Exercise>> GetExercisesByMuscleGroup(string muscleGroup);
        Task<List<Exercise>> SearchExercisesByNameAsync(string name);
    }
}
