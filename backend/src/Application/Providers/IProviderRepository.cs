using PruebaTekus.Application.Common;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Providers;

public interface IProviderRepository
{
    Task<Provider?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<bool> ExistsByNitAsync(string nit, int? excludeId, CancellationToken cancellationToken);

    Task<PagedResult<Provider>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken);

    Task AddAsync(Provider provider, CancellationToken cancellationToken);

    Task UpdateAsync(Provider provider, CancellationToken cancellationToken);
}
