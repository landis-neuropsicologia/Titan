using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Titan.Shared.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<T>> GetAsync(CancellationToken token = default);

        Task AddAsync(T entity, CancellationToken token = default);

        Task UpdateAsync(T entity, CancellationToken token = default);
            
        Task DeleteAsync(T entity, CancellationToken token = default);
    }
}
