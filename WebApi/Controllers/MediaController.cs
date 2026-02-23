using Application.Abstraction.Storage;
using Application.Common;
using Application.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public MediaController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost("upload/{folder?}")]
        public async Task<IActionResult> UploadMedia(IFormFile file, [FromRoute] string? folder = null)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Geçersiz veya boş dosya yüklendi."));
            }

            var url = await MediaHelper.UploadMediaAsync(file, _storageService, folder);

            if (string.IsNullOrEmpty(url))
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Dosya yüklenirken bir hata oluştu."));
            }

            return Ok(ApiResponse<object>.SuccessResponse(new { Url = url }, "Dosya başarıyla yüklendi."));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteMedia([FromQuery] string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Geçersiz veya boş URL."));
            }

            var success = await MediaHelper.DeleteMediaAsync(url, _storageService);

            if (!success)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Dosya silinirken bir hata oluştu veya URL geçersiz."));
            }

            return Ok(ApiResponse<object>.SuccessMessage("Medya başarıyla silindi."));
        }
    }
}
