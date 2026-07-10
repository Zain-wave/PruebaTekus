using PruebaTekus.Application.Common;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Services;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<PagedResult<Service>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken);

    Task AddAsync(Service service, CancellationToken cancellationToken);

    Task UpdateAsync(Service service, CancellationToken cancellationToken);
}
