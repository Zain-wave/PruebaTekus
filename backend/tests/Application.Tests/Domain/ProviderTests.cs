using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Tests.Domain;

public class ProviderTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesProvider()
    {
        var provider = new Provider("900123456", "Acme", "https://acme.test", "contact@acme.test");

        Assert.Equal("Acme", provider.Name);
        Assert.Equal("900123456", provider.Nit);
        Assert.Empty(provider.Services);
    }

    [Fact]
    public void Constructor_WithInvalidEmail_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Provider("900123456", "Acme", "https://acme.test", "not-an-email"));
    }

    [Fact]
    public void Update_WithValidData_UpdatesFields()
    {
        var provider = new Provider("900123456", "Acme", "https://acme.test", "contact@acme.test");

        provider.Update("Acme Renamed", "https://acme.new", "new@acme.test");

        Assert.Equal("Acme Renamed", provider.Name);
        Assert.Equal("https://acme.new", provider.Website);
        Assert.Equal("new@acme.test", provider.Email);
    }
}
