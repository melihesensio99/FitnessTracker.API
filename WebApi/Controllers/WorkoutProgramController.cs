using Application.DTO.WorkoutProgram;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutProgramController : ControllerBase
    {
        private readonly IWorkoutProgramService _workoutProgramService;

        public WorkoutProgramController(IWorkoutProgramService workoutProgramService)
        {
            _workoutProgramService = workoutProgramService;
        }

        [HttpGet("system")]
        public async Task<IActionResult> GetSystemWorkoutPrograms()
        {
            var programs = await _workoutProgramService.GetSystemWorkoutProgramsAsync();
            return Ok(programs);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserWorkoutPrograms(int userId)
        {
            var programs = await _workoutProgramService.GetUserWorkoutProgramsAsync(userId);
            return Ok(programs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutProgramDetail(int id)
        {
            var program = await _workoutProgramService.GetWorkoutProgramDetailByIdAsync(id);
            return Ok(program);
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkoutProgram([FromBody] CreateWorkoutProgramDto createDto)
        {
            var result = await _workoutProgramService.AddWorkoutProgramAsync(createDto);
            return CreatedAtAction(nameof(GetWorkoutProgramDetail), new { id = result.Id }, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWorkoutProgram([FromBody] UpdateWorkoutProgramDto updateDto)
        {
            await _workoutProgramService.UpdateWorkoutProgramAsync(updateDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutProgram(int id)
        {
            await _workoutProgramService.DeleteWorkoutProgramAsync(id);
            return NoContent();
        }

        [HttpPost("clone/{programId}")]
        public async Task<IActionResult> CloneSystemProgramToUser(int programId, [FromQuery] int userId)
        {
            var newProgramId = await _workoutProgramService.CloneSystemProgramToUserAsync(programId, userId);
            return Ok(new { NewProgramId = newProgramId });
        }
    }
}
