using Microsoft.Extensions.Options;
using PruebaTekus.Application.Common.Email;
using PruebaTekus.Application.Common.Exceptions;
using PruebaTekus.Application.Services.Commands.CreateService;
using PruebaTekus.Application.Tests.TestDoubles;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Tests.Services;

public class CreateServiceCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithConfiguredNotificationEmail_CreatesServiceAndSendsEmail()
    {
        var serviceRepository = new FakeServiceRepository();
        var providerRepository = new FakeProviderRepository();
        var provider = new Provider("900123456", "Acme", "https://acme.test", "contact@acme.test", "Colombia");
        EntityTestHelper.SetProperty(provider, nameof(Provider.Id), 1);
        providerRepository.Seed(provider);
        var emailSender = new FakeEmailSender();
        var notificationSettings = Options.Create(new NotificationSettings
        {
            NewServiceNotificationEmail = "ops@pruebatekus.local",
        });
        var handler = new CreateServiceCommandHandler(serviceRepository, providerRepository, emailSender, notificationSettings);
        var command = new CreateServiceCommand("Consulting", 100m, provider.Id);

        var id = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(1, id);
        Assert.Single(emailSender.SentMessages);
        Assert.Equal("ops@pruebatekus.local", emailSender.SentMessages[0].To);
    }

    [Fact]
    public async Task Handle_WithoutConfiguredNotificationEmail_DoesNotSendEmail()
    {
        var serviceRepository = new FakeServiceRepository();
        var providerRepository = new FakeProviderRepository();
        var provider = new Provider("900123456", "Acme", "https://acme.test", "contact@acme.test", "Colombia");
        EntityTestHelper.SetProperty(provider, nameof(Provider.Id), 1);
        providerRepository.Seed(provider);
        var emailSender = new FakeEmailSender();
        var notificationSettings = Options.Create(new NotificationSettings());
        var handler = new CreateServiceCommandHandler(serviceRepository, providerRepository, emailSender, notificationSettings);
        var command = new CreateServiceCommand("Consulting", 100m, provider.Id);

        await handler.Handle(command, CancellationToken.None);

        Assert.Empty(emailSender.SentMessages);
    }

    [Fact]
    public async Task Handle_WithMissingProvider_ThrowsNotFoundException()
    {
        var serviceRepository = new FakeServiceRepository();
        var providerRepository = new FakeProviderRepository();
        var emailSender = new FakeEmailSender();
        var notificationSettings = Options.Create(new NotificationSettings());
        var handler = new CreateServiceCommandHandler(serviceRepository, providerRepository, emailSender, notificationSettings);
        var command = new CreateServiceCommand("Consulting", 100m, 99);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}
