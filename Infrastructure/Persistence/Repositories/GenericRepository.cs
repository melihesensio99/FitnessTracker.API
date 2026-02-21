using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.AppDbContext;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly FitnessTrackerDbContext _context;

        public GenericRepository(FitnessTrackerDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll()
        {
            return Table; 
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await Table.FindAsync(id);
        }

        public async Task AddAsync(T model)
        {
            await Table.AddAsync(model);
        }

        public void Update(T model)
        {
            Table.Update(model);
        }

        public void Remove(T model)
        {
            Table.Remove(model);
        }
    }
}
