using System;

namespace Application.Events
{
    public class EmailVerifiedEvent
    {
        public string UserId { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string Name { get; init; } = default!;
        public DateTime VerifiedAt { get; init; } = DateTime.UtcNow;
    }
}
