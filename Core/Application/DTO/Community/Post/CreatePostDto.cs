using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Community.Post
{
    public class CreatePostDto
    {
        public string Content { get; set; }
        
        public Domain.Enums.VisibilityType Visibility { get; set; } = Domain.Enums.VisibilityType.Public;
        
        public List<Microsoft.AspNetCore.Http.IFormFile> MediaFiles { get; set; } = new List<Microsoft.AspNetCore.Http.IFormFile>();
    }
}
