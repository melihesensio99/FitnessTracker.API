using System;

namespace Domain.Entities.Community
{
    public class CommentLike
    {
        public int CommentId { get; set; }
        public Comment Comment { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
