using Application.Abstraction.Services;
using Application.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Consumers
{
    public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<UserRegisteredConsumer> _logger;

        public UserRegisteredConsumer(ILogger<UserRegisteredConsumer> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var message = context.Message;

            var verificationLink = $"https://localhost:7001/api/auth/confirm-email" +
                                   $"?userId={message.UserId}&token={message.VerificationToken}";

            _logger.LogInformation("[Email] Doğrulama maili gönderiliyor: {Email}", message.Email);

            await _emailService.SendVerificationEmailAsync(message.Email, message.Name, verificationLink);
        }
    }
}
