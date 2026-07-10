using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PruebaTekus.Application.Common.Email;
using PruebaTekus.Application.Dashboard;
using PruebaTekus.Application.Providers;
using PruebaTekus.Application.Services;
using PruebaTekus.Infrastructure.Email;
using PruebaTekus.Infrastructure.Persistence;

namespace PruebaTekus.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProviderRepository, ProviderRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IDashboardRepository, DashboardRepository>();

        services.Configure<NotificationSettings>(configuration.GetSection("Notifications"));
        services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));
        services.AddScoped<IEmailSender, MailKitEmailSender>();

        return services;
    }
}
