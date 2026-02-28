using Application.Abstraction.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendWelcomeEmailAsync(string toEmail, string name)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["Email:From"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Fitness Tracker'a Hoş Geldiniz! 🏋️";

            var builder = new BodyBuilder();
            builder.HtmlBody = $"""
            <h2>Merhaba {name}! 💪</h2>
            <p>FitnessTracker ailesine hoş geldin.</p>
            <p>Hedeflerine ulaşmana yardımcı olmak için buradayız.</p>
            <a href="https://senin-uygulamaniz.com" 
               style="background:#6366f1;color:white;padding:12px 24px;border-radius:8px;text-decoration:none;">
               Hemen Başla
            </a>
        """;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["Email:Host"], 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendVerificationEmailAsync(string toEmail, string name, string verificationLink)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["Email:From"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "E-posta Adresinizi Doğrulayın ✉️";

            var builder = new BodyBuilder();
            builder.HtmlBody = $"""
            <h2>Merhaba {name}!</h2>
            <p>Hesabını aktifleştirmek için aşağıdaki butona tıkla:</p>
            <a href="{verificationLink}"
               style="background:#6366f1;color:white;padding:12px 24px;border-radius:8px;text-decoration:none;">
               E-postamı Doğrula
            </a>
            <p>Bu link 24 saat geçerlidir.</p>
        """;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["Email:Host"], 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
