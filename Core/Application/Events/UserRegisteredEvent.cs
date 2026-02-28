using System;

namespace Application.Events
{
    public class UserRegisteredEvent
    {
        public string UserId { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string Name { get; init; } = default!;
        public string VerificationToken { get; init; } = default!;
        public DateTime RegisteredAt { get; init; } = DateTime.UtcNow;
    }
}
