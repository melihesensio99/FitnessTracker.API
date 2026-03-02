using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
       
        protected int GetCurrentUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new InvalidOperationException("Kullanıcı kimliği token'da bulunamadı.");
            return int.Parse(claim);
        }

        protected int GetCurrentUserIdOrDefault()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim) : 0;
        }
    }
}
