using Microsoft.AspNetCore.Http;

namespace Application.DTO.Media
{
    public class UploadMediaDto
    {
        public IFormFile File { get; set; } = null!;
    }
}
