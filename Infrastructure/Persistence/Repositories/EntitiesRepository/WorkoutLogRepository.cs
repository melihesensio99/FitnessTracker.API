using Application.Repositories.EntitiesRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.AppDbContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntitiesRepository
{
    public class WorkoutLogRepository : GenericRepository<WorkoutLog>, IWorkoutLogRepository
    {
        private readonly FitnessTrackerDbContext _context;

        public WorkoutLogRepository(FitnessTrackerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<WorkoutLog>> GetExerciseLogsAsync(int programExerciseId)
        {
            return await _context.WorkoutLogs.Include(wl => wl.ProgramExercise)
                .Where(wl => wl.ProgramExerciseId == programExerciseId).ToListAsync();
        }

        public async Task<List<WorkoutLog>> GetLogsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.WorkoutLogs
                 .Include(wl => wl.ProgramExercise)
                    .ThenInclude(pe => pe.Exercise)
                 .AsNoTracking()
                 .Where(wl => wl.UserId == userId && wl.Date >= startDate && wl.Date <= endDate)
                 .OrderByDescending(wl => wl.Date)
                 .ToListAsync();
        }

        public async Task<WorkoutLog?> GetUserLastLogAsync(int userId, int programExerciseId)
        {
           return await _context.WorkoutLogs
                .Where(wl => wl.UserId == userId && wl.ProgramExerciseId == programExerciseId)
                .OrderByDescending(wl => wl.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<List<WorkoutLog>> GetUserLogsAsync(int userId)
        {
            return await _context.WorkoutLogs
                .Include(wl => wl.ProgramExercise)
                    .ThenInclude(pe => pe.Exercise)
                .AsNoTracking()
                .Where(wl => wl.UserId == userId)
                .OrderByDescending(wl => wl.Date)
                .ToListAsync();
        }
    }
}
