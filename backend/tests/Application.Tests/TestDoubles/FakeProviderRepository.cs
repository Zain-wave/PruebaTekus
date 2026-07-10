using PruebaTekus.Application.Common;
using PruebaTekus.Application.Providers;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Tests.TestDoubles;

public class FakeProviderRepository : IProviderRepository
{
    private readonly List<Provider> _providers = new();
    private int _nextId = 1;

    public IReadOnlyList<Provider> Providers => _providers;

    public void Seed(Provider provider)
    {
        _providers.Add(provider);
    }

    public Task<Provider?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_providers.FirstOrDefault(provider => provider.Id == id));
    }

    public Task<bool> ExistsByNitAsync(string nit, int? excludeId, CancellationToken cancellationToken)
    {
        var exists = _providers.Any(provider => provider.Nit == nit && provider.Id != excludeId);
        return Task.FromResult(exists);
    }

    public Task<PagedResult<Provider>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken)
    {
        var items = _providers
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return Task.FromResult(new PagedResult<Provider>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = _providers.Count,
        });
    }

    public Task AddAsync(Provider provider, CancellationToken cancellationToken)
    {
        EntityTestHelper.SetProperty(provider, nameof(Provider.Id), _nextId++);
        _providers.Add(provider);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Provider provider, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
