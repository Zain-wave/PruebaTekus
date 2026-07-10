using Microsoft.EntityFrameworkCore;
using PruebaTekus.Application.Providers;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Infrastructure.Persistence;

public class ProviderRepository(AppDbContext context) : IProviderRepository
{
    public Task<Provider?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return context.Providers.FirstOrDefaultAsync(provider => provider.Id == id, cancellationToken);
    }

    public Task<bool> ExistsByNitAsync(string nit, int? excludeId, CancellationToken cancellationToken)
    {
        return context.Providers.AnyAsync(
            provider => provider.Nit == nit && provider.Id != excludeId,
            cancellationToken);
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
