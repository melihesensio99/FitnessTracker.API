using System;
using System.Threading.Tasks;

namespace Application.Abstraction.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<int> SaveAsync();
    }
}
