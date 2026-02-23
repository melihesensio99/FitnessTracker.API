using Application.Common.Pagination;
using Application.DTO.WorkoutProgram;
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


        public async Task<List<WorkoutProgram>> GetUserProgramsAsync(int userId)
        {
            return await _context.WorkoutPrograms
                 .Include(wp => wp.ProgramExercises)
                 .Where(wp => wp.UserId == userId)
                 .ToListAsync();
        }

        public async Task<WorkoutProgram?> GetWorkoutProgramDetailWithExercisesAsync(int programId)
        {
            return await _context.WorkoutPrograms.Include
                 (wp => wp.ProgramExercises)
                 .ThenInclude(pe => pe.Exercise)
                 .FirstOrDefaultAsync(wp => wp.Id == programId);

        }

        public async Task<PagedResponse<WorkoutProgram>> GetFilteredProgramsAsync(WorkoutProgramFilteredDto filter)
        {
            IQueryable<WorkoutProgram> query = _context.WorkoutPrograms.AsQueryable();

            query = query.Where(wp => wp.IsPublic == true);

            if (!string.IsNullOrEmpty(filter.Level))
                query = query.Where(x => x.Level == filter.Level);

            if (filter.IsSystemProgram && !filter.IsUserProgram)
                query = query.Where(x => x.UserId == null);
            else if (filter.IsUserProgram && !filter.IsSystemProgram)
                query = query.Where(x => x.UserId != null);

            if (filter.Ambition?.Count > 0)
                query = query.Where(x => filter.Ambition.Contains(x.Ambition));

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                var lowerSearch = filter.SearchText.ToLower();
                query = query.Where(x => x.Title.ToLower().Contains(lowerSearch));
            }

            var totalCount = await query.CountAsync();

            var items = await query
         .OrderBy(x => x.Id)
         .Skip(filter.Skip)
         .Take(filter.PageSize)
         .ToListAsync();

            return new PagedResponse<WorkoutProgram>
            {
                Data = items,
                CurrentPage = filter.Page,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<bool> ActivateProgramByUserIdAsync(int programId, int userId)
        {
            var userPrograms = await _context.WorkoutPrograms
                .Where(wp => wp.UserId == userId)
                .ToListAsync();

            var programToActivate = userPrograms.FirstOrDefault(wp => wp.Id == programId);

            if (programToActivate == null)
            {
                return false;
            }

            foreach (var program in userPrograms)
            {
                program.IsActive = false;
            }

            programToActivate.IsActive = true;

            return true;
        }
    }
}