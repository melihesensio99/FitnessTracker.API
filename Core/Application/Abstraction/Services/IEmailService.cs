namespace Application.Abstraction.Services;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string toEmail, string name);
    Task SendVerificationEmailAsync(string toEmail, string name, string verificationLink);
}
