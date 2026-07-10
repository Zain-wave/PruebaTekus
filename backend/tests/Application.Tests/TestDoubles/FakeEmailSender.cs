using PruebaTekus.Application.Common.Email;

namespace PruebaTekus.Application.Tests.TestDoubles;

public class FakeEmailSender : IEmailSender
{
    public List<(string To, string Subject, string Body)> SentMessages { get; } = new();

    public Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken)
    {
        SentMessages.Add((to, subject, body));
        return Task.CompletedTask;
    }
}
