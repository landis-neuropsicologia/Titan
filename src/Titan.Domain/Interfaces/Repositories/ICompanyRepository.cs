using Titan.Domain.Entities;
using Titan.Shared.Repositories;

namespace Titan.Domain.Interfaces.Repositories;

public interface ICompanyRepository : IRepository<Company>
{
    Task<Company> GetByNameAsync(string name);

    Task<Company> GetByDomainAsync(string domain);

    Task<int> CountAllAsync();

    Task<(IEnumerable<Company> Companies, int TotalCount)> GetPaginatedAsync(string searchTerm, int page, int pageSize);
}