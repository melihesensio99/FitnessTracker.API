using Application.Abstraction.Services;
using Application.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Consumers
{
    public class EmailVerifiedConsumer : IConsumer<EmailVerifiedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailVerifiedConsumer> _logger;

        public EmailVerifiedConsumer(ILogger<EmailVerifiedConsumer> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<EmailVerifiedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation("[Email] Hoşgeldin maili gönderiliyor: {Email}", message.Email);

            await _emailService.SendWelcomeEmailAsync(message.Email, message.Name);
        }
    }
}
