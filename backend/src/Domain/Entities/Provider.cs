namespace PruebaTekus.Domain.Entities;

public class Provider
{
    public int Id { get; private set; }
    public string Nit { get; private set; }
    public string Name { get; private set; }
    public string Website { get; private set; }
    public string Email { get; private set; }
    public string Country { get; private set; }

    public ICollection<Service> Services { get; private set; }

    private Provider()
    {
        Nit = string.Empty;
        Name = string.Empty;
        Website = string.Empty;
        Email = string.Empty;
        Country = string.Empty;
        Services = new List<Service>();
    }

    public Provider(string nit, string name, string website, string email, string country)
    {
        if (string.IsNullOrWhiteSpace(nit))
        {
            throw new ArgumentException("Nit is required.", nameof(nit));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new ArgumentException("Email must be a valid email address.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException("Country is required.", nameof(country));
        }

        Nit = nit;
        Name = name;
        Website = website;
        Email = email;
        Country = country;
        Services = new List<Service>();
    }

    public void Update(string name, string website, string email, string country)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new ArgumentException("Email must be a valid email address.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException("Country is required.", nameof(country));
        }

        Name = name;
        Website = website;
        Email = email;
        Country = country;
    }
}
