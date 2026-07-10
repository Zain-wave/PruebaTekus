using MediatR;
using Microsoft.Extensions.Options;
using PruebaTekus.Application.Common.Email;
using PruebaTekus.Application.Common.Exceptions;
using PruebaTekus.Application.Providers;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Services.Commands.CreateService;

public class CreateServiceCommandHandler(
    IServiceRepository serviceRepository,
    IProviderRepository providerRepository,
    IEmailSender emailSender,
    IOptions<NotificationSettings> notificationSettings) : IRequestHandler<CreateServiceCommand, int>
{
    public async Task<int> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var provider = await providerRepository.GetByIdAsync(request.ProviderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Provider), request.ProviderId);

        var service = new Service(request.Name, request.HourlyRate, request.ProviderId);

        await serviceRepository.AddAsync(service, cancellationToken);

        var destination = notificationSettings.Value.NewServiceNotificationEmail;

        if (!string.IsNullOrWhiteSpace(destination))
        {
            await emailSender.SendAsync(
                destination,
                "New service enabled",
                $"Provider '{provider.Name}' has enabled a new service: '{service.Name}'.",
                cancellationToken);
        }

        return service.Id;
    }
}
