using Microsoft.EntityFrameworkCore;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<Service> Services => Set<Service>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(provider => provider.Id);
            entity.Property(provider => provider.Nit).IsRequired().HasMaxLength(20);
            entity.Property(provider => provider.Name).IsRequired().HasMaxLength(200);
            entity.Property(provider => provider.Website).HasMaxLength(200);
            entity.Property(provider => provider.Email).IsRequired().HasMaxLength(200);
            entity.Property(provider => provider.Country).IsRequired().HasMaxLength(100);
            entity.HasIndex(provider => provider.Nit).IsUnique();

            entity.HasMany(provider => provider.Services)
                .WithOne(service => service.Provider)
                .HasForeignKey(service => service.ProviderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(service => service.Id);
            entity.Property(service => service.Name).IsRequired().HasMaxLength(200);
            entity.Property(service => service.HourlyRate).HasColumnType("decimal(18,2)");
        });
    }
}
