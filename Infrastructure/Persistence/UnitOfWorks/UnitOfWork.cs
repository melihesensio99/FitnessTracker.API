using Application.Interfaces.UnitOfWorks;
using Persistence.Context.AppDbContext;
using System;
using System.Threading.Tasks;

namespace Persistence.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FitnessTrackerDbContext _context;

        public UnitOfWork(FitnessTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
