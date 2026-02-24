using System;
using System.Collections.Generic;

namespace Application.DTO.Community.Comment
{
    public class ResultCommentDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? ParentCommentId { get; set; }
        public int LikeCount { get; set; }
        public int ReplyCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public List<ResultCommentDto> Replies { get; set; }
    }
}
