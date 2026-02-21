using System.Linq;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(int id);
        
        Task AddAsync(T model);
        void Update(T model);
        void Remove(T model);
    }
}
