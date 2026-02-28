using Application.Abstraction.Storage;
using Application.Abstraction.Services;
using Application.Common;
using Application.DTO.Media;
using Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MediaController : ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly IEventBus _eventBus;

        public MediaController(IStorageService storageService, IEventBus eventBus)
        {
            _storageService = storageService;
            _eventBus = eventBus;
        }

        [HttpPost("upload/{folder?}")]
        public async Task<IActionResult> UploadMedia([FromForm] UploadMediaDto request, [FromRoute] string? folder = null)
        {
            var url = await MediaHelper.UploadMediaAsync(request.File, _storageService, folder);

            if (string.IsNullOrEmpty(url))
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Dosya yüklenirken bir hata oluştu."));
            }

            return Ok(ApiResponse<object>.SuccessResponse(new { Url = url }, "Dosya başarıyla yüklendi."));
        }

        [HttpPost("upload-async/{folder?}")]
        public async Task<IActionResult> UploadMediaAsync([FromForm] UploadMediaDto request, [FromRoute] string? folder = null)
        {
            await MediaHelper.EnqueueMediaUploadAsync(request.File, _eventBus, folder);

            return Accepted(ApiResponse<object>.SuccessResponse(null, "Medya yükleme kuyruğuna alındı. Arka planda işlenecektir."));
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

            return Ok(ApiResponse<object>.SuccessMessages("Medya başarıyla silindi."));
        }
    }
}
