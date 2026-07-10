using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Tests.Domain;

public class ServiceTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesService()
    {
        var service = new Service("Consulting", 100m, providerId: 1);

        Assert.Equal("Consulting", service.Name);
        Assert.Equal(100m, service.HourlyRate);
        Assert.Equal(1, service.ProviderId);
    }

    [Fact]
    public void Constructor_WithNegativeHourlyRate_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Service("Consulting", -1m, providerId: 1));
    }

    [Fact]
    public void Update_WithValidData_UpdatesFields()
    {
        var service = new Service("Consulting", 100m, providerId: 1);

        service.Update("Consulting Plus", 150m);

        Assert.Equal("Consulting Plus", service.Name);
        Assert.Equal(150m, service.HourlyRate);
    }
}
