using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.AppDbContext;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly FitnessTrackerDbContext _context;

        public TestController(FitnessTrackerDbContext context)
        {
            _context = context;
        }

        [HttpPost("create-dummy-user")]
        public async Task<IActionResult> CreateDummyUser()
        {
            var user = new User { Name = "Test Kullanýcýsý", UserName = "test_user" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { UserId = user.Id });
        }
    }
}
