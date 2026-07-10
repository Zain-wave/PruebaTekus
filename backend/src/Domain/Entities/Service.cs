namespace PruebaTekus.Domain.Entities;

public class Service
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal HourlyRate { get; private set; }
    public int ProviderId { get; private set; }
    public Provider Provider { get; private set; }

    private Service()
    {
        Name = string.Empty;
        Provider = null!;
    }

    public Service(string name, decimal hourlyRate, int providerId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        if (hourlyRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hourlyRate), "HourlyRate cannot be negative.");
        }

        Name = name;
        HourlyRate = hourlyRate;
        ProviderId = providerId;
        Provider = null!;
    }

    public void Update(string name, decimal hourlyRate)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        if (hourlyRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hourlyRate), "HourlyRate cannot be negative.");
        }

        Name = name;
        HourlyRate = hourlyRate;
    }
}