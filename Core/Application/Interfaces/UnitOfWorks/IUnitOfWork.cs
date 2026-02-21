using System;
using System.Threading.Tasks;

namespace Application.Interfaces.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<int> SaveAsync();
    }
}
