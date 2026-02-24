using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Community.Post
{
    public class ResultPostDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public List<string> MediaUrls { get; set; }
    }
}
