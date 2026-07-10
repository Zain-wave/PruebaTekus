using Microsoft.EntityFrameworkCore;
using PruebaTekus.Application.Common;
using PruebaTekus.Application.Providers;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Infrastructure.Persistence;

public class ProviderRepository(AppDbContext context) : IProviderRepository
{
    public Task<Provider?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return context.Providers
            .Include(provider => provider.Services)
            .FirstOrDefaultAsync(provider => provider.Id == id, cancellationToken);
    }

    public Task<bool> ExistsByNitAsync(string nit, int? excludeId, CancellationToken cancellationToken)
    {
        return context.Providers.AnyAsync(
            provider => provider.Nit == nit && provider.Id != excludeId,
            cancellationToken);
    }

    public async Task<PagedResult<Provider>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken)
    {
        var query = context.Providers.Include(provider => provider.Services).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(provider =>
                EF.Functions.Like(provider.Nit, $"%{search}%") ||
                EF.Functions.Like(provider.Name, $"%{search}%") ||
                EF.Functions.Like(provider.Email, $"%{search}%"));
        }

        query = ApplySort(query, request.SortBy, request.SortDescending ?? false);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Provider>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
        };
    }

    private static IQueryable<Provider> ApplySort(IQueryable<Provider> query, string? sortBy, bool descending)
    {
        return sortBy?.ToLowerInvariant() switch
        {
            "nit" => descending ? query.OrderByDescending(provider => provider.Nit) : query.OrderBy(provider => provider.Nit),
            "email" => descending ? query.OrderByDescending(provider => provider.Email) : query.OrderBy(provider => provider.Email),
            _ => descending ? query.OrderByDescending(provider => provider.Name) : query.OrderBy(provider => provider.Name),
        };
    }

    public async Task AddAsync(Provider provider, CancellationToken cancellationToken)
    {
        await context.Providers.AddAsync(provider, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Provider provider, CancellationToken cancellationToken)
    {
        context.Providers.Update(provider);
        await context.SaveChangesAsync(cancellationToken);
    }
}
