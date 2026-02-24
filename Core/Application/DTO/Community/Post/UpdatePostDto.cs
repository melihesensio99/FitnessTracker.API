using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Application.DTO.Community.Post
{
    public class UpdatePostDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public VisibilityType Visibility { get; set; }
        public List<IFormFile> MediaFiles { get; set; }
    }
}
