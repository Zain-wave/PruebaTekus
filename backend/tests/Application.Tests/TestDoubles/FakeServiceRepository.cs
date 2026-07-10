using PruebaTekus.Application.Common;
using PruebaTekus.Application.Services;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Tests.TestDoubles;

public class FakeServiceRepository : IServiceRepository
{
    private readonly List<Service> _services = new();
    private int _nextId = 1;

    public IReadOnlyList<Service> Services => _services;

    public void Seed(Service service)
    {
        _services.Add(service);
    }

    public Task<Service?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_services.FirstOrDefault(service => service.Id == id));
    }

    public Task<PagedResult<Service>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken)
    {
        var items = _services
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return Task.FromResult(new PagedResult<Service>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = _services.Count,
        });
    }

    public Task AddAsync(Service service, CancellationToken cancellationToken)
    {
        EntityTestHelper.SetProperty(service, nameof(Service.Id), _nextId++);
        _services.Add(service);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Service service, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
