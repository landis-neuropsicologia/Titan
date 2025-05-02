using Titan.Domain.Entities;
using Titan.Shared.Repositories;

namespace Titan.Domain.Interfaces.Repositories;

public interface IActivityLogRepository : IRepository<ActivityLog>
{
    Task<IEnumerable<ActivityLog>> GetByUserIdAsync(Guid userId);

    Task<IEnumerable<ActivityLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    Task<(IEnumerable<ActivityLog> Logs, int TotalCount)> GetPaginatedAsync(int page, int pageSize);
}