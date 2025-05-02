using Titan.Domain.Entities.User;
using Titan.Shared.Repositories;

namespace Titan.Domain.Interfaces.Repositories;

public interface IUserRepository : IRepository<UserBase>
{
    Task<UserBase> GetByEmailAsync(string email, CancellationToken token = default);

    Task<IEnumerable<UserBase>> GetByCompanyIdAsync(Guid companyId, CancellationToken token = default);
    
    Task<int> CountByCompanyIdAsync(Guid companyId, CancellationToken token = default);
    
    Task<int> CountAllAsync(CancellationToken token = default);
    
    Task<int> CountActiveAsync(CancellationToken token = default);
    
    Task<int> CountCreatedTodayAsync(CancellationToken token = default);
    
    Task<(IEnumerable<UserBase> Users, int TotalCount)> GetPaginatedAsync(string searchTerm, int page, int pageSize, CancellationToken token = default);
}