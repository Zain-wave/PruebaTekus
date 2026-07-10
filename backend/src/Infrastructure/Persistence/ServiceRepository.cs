using Microsoft.EntityFrameworkCore;
using PruebaTekus.Application.Common;
using PruebaTekus.Application.Services;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Infrastructure.Persistence;

public class ServiceRepository(AppDbContext context) : IServiceRepository
{
    public Task<Service?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return context.Services
            .Include(service => service.Provider)
            .FirstOrDefaultAsync(service => service.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Service>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken)
    {
        var query = context.Services.Include(service => service.Provider).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(service => EF.Functions.Like(service.Name, $"%{search}%"));
        }

        query = ApplySort(query, request.SortBy, request.SortDescending ?? false);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Service>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
        };
    }

    public async Task AddAsync(Service service, CancellationToken cancellationToken)
    {
        await context.Services.AddAsync(service, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Service service, CancellationToken cancellationToken)
    {
        context.Services.Update(service);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static IQueryable<Service> ApplySort(IQueryable<Service> query, string? sortBy, bool descending)
    {
        return sortBy?.ToLowerInvariant() switch
        {
            "hourlyrate" => descending ? query.OrderByDescending(service => service.HourlyRate) : query.OrderBy(service => service.HourlyRate),
            _ => descending ? query.OrderByDescending(service => service.Name) : query.OrderBy(service => service.Name),
        };
    }
}
