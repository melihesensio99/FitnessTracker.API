using Application.Abstraction.Services;
using Application.Common;
using Application.DTO.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserDto createUser)
        {
            var result = await _userService.CreateUser(createUser);
            if (result.Succeeded)
                return Ok(ApiResponse<object>.SuccessMessages("Kullanıcı başarıyla oluşturuldu!"));
            else
                return BadRequest(ApiResponse<object>.ErrorResponse(result.Message));
        }
    }
}
