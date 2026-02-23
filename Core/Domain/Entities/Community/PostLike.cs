using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Community
{
    public class PostLike //todo unique ayari!
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
