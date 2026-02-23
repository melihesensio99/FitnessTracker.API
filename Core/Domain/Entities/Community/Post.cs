using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Domain.Entities.Community
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public VisibilityType Visibility { get; set; }
        public ICollection<PostMedia> Media { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public ICollection<PostLike> Likes { get; set; }
        public int LikeCount { get; set; }  
        public int CommentCount { get; set; }
    }
}
