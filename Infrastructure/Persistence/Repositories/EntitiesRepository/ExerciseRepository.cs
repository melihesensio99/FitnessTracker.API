using Application.Common.Pagination;
using Application.Repositories.EntitiesRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.AppDbContext;
using Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.EntitiesRepository
{
    public class ExerciseRepository : GenericRepository<Exercise>, IExerciseRepository
    {
        private readonly FitnessTrackerDbContext _context;

        public ExerciseRepository(FitnessTrackerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResponse<Exercise>> GetExercisesByMuscleGroup(string muscleGroup, PagedRequest request)
        {
            return await _context.Exercises
                   .Where(e => e.TargetMuscle.ToLower() == muscleGroup.ToLower())
                   .ToPagedResponseAsync(request);
        }

        public async Task<PagedResponse<Exercise>> SearchExercisesByNameAsync(string name, PagedRequest request)
        {
            return await _context.Exercises
             .Where(e => e.ExerciseName.ToLower().Contains(name.ToLower()))
             .ToPagedResponseAsync(request);
        }
    }
}
