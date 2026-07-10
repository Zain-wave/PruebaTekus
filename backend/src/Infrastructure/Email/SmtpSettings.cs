namespace PruebaTekus.Infrastructure.Email;

public class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 25;
    public string From { get; set; } = "noreply@pruebatekus.local";
    public string? User { get; set; }
    public string? Password { get; set; }
    public bool UseSsl { get; set; }
}
