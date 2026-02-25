using Application.Abstraction.Services;
using Application.Common;
using Application.DTO.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IAuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public IAuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _authService.LoginAsync(loginDto);
            return Ok(ApiResponse<object>.SuccessMessages("Kullanıcı başarıyla giriş yapti!."));
        }
    }
}
