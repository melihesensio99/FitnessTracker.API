using Application.Repositories.EntitiesRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.AppDbContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntitiesRepository
{
    public class ExerciseRepository : GenericRepository<Exercise>, IExerciseRepository
    {
        private readonly FitnessTrackerDbContext _context;

        public ExerciseRepository(FitnessTrackerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Exercise>> GetExercisesByMuscleGroup(string muscleGroup)
        {
            var exercises = await _context.Exercises
                   .Where(e => e.TargetMuscle.ToLower() == muscleGroup.ToLower())
                   .ToListAsync();

            return exercises;
        }

        public async Task<List<Exercise>> SearchExercisesByNameAsync(string name)
        {
            var exercises = await _context.Exercises
             .Where(e => e.ExerciseName.ToLower().Contains(name.ToLower()))
             .ToListAsync();

            return exercises;
        }
    }
}
