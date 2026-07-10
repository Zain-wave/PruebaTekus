using PruebaTekus.Application.Common.Exceptions;
using PruebaTekus.Application.Providers.Commands.CreateProvider;
using PruebaTekus.Application.Tests.TestDoubles;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Tests.Providers;

public class CreateProviderCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithNewNit_CreatesProviderAndReturnsGeneratedId()
    {
        var repository = new FakeProviderRepository();
        var handler = new CreateProviderCommandHandler(repository);
        var command = new CreateProviderCommand("900123456", "Acme", "https://acme.test", "contact@acme.test", "Colombia");

        var id = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(1, id);
        Assert.Single(repository.Providers);
        Assert.Equal("Acme", repository.Providers[0].Name);
    }

    [Fact]
    public async Task Handle_WithExistingNit_ThrowsConflictException()
    {
        var repository = new FakeProviderRepository();
        repository.Seed(new Provider("900123456", "Acme", "https://acme.test", "contact@acme.test", "Colombia"));
        var handler = new CreateProviderCommandHandler(repository);
        var command = new CreateProviderCommand("900123456", "Other", "https://other.test", "info@other.test", "Colombia");

        await Assert.ThrowsAsync<ConflictException>(() => handler.Handle(command, CancellationToken.None));
    }
}
