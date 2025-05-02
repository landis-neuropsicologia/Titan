using System.Threading;
using System.Threading.Tasks;

namespace Titan.Shared.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken token = default);
    }
}