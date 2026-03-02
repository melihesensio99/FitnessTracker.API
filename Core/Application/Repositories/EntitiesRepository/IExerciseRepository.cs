using Application.Common.Pagination;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repositories.EntitiesRepository
{
    public interface IExerciseRepository : IGenericRepository<Domain.Entities.Exercise>
    {
        Task<PagedResponse<Exercise>> GetExercisesByMuscleGroup(string muscleGroup, PagedRequest request);
        Task<PagedResponse<Exercise>> SearchExercisesByNameAsync(string name, PagedRequest request);
    }
}
