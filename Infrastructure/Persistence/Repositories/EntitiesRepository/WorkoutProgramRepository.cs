using Application.Repositories.EntitiesRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.AppDbContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntitiesRepository
{
    public class WorkoutProgramRepository : GenericRepository<Domain.Entities.WorkoutProgram>, IWorkoutProgramRepository
    {
        private readonly FitnessTrackerDbContext _context;
        public WorkoutProgramRepository(FitnessTrackerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<WorkoutProgram>> GetSystemWorkoutProgramsAsync()
        {
            return await _context.WorkoutPrograms.Where(wp => wp.IsPublic == true && wp.UserId == null).ToListAsync();
        }

        public async Task<List<WorkoutProgram>> GetUserWorkoutProgramsAsync(int userId)
        {
            return await _context.WorkoutPrograms.Where(wp => wp.UserId == userId).ToListAsync();
        }

        public async Task<WorkoutProgram?> GetWorkoutProgramDetailWithExercisesAsync(int programId)
        {
            return await _context.WorkoutPrograms.Include
                 (wp => wp.ProgramExercises)
                 .FirstOrDefaultAsync(wp => wp.Id == programId);

        }
    }
}
