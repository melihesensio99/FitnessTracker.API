using Application.Abstraction.Services;
using Application.Common;
using Application.Common.Pagination;
using Application.DTO.WorkoutProgram;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpGet("discover")]
        public async Task<IActionResult> GetDiscoverPrograms([FromQuery] WorkoutProgramFilteredDto filter)
        {
            var response = await _workoutProgramService.GetFilteredProgramsAsync(filter);
            return Ok(ApiResponse<PagedResponse<WorkoutProgramDto>>.SuccessResponse(response));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPrograms(int userId)
        {
            var programs = await _workoutProgramService.GetUserProgramsAsync(userId);
            return Ok(ApiResponse<List<WorkoutProgramDto>>.SuccessResponse(programs));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutProgramDetail(int id)
        {
            var program = await _workoutProgramService.GetWorkoutProgramDetailByIdAsync(id);
            return Ok(ApiResponse<WorkoutProgramDto>.SuccessResponse(program));
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkoutProgram([FromBody] CreateWorkoutProgramDto createDto)
        {
            var result = await _workoutProgramService.AddWorkoutProgramAsync(createDto);
            return CreatedAtAction(nameof(GetWorkoutProgramDetail), new { id = result.Id },
                ApiResponse<WorkoutProgramDto>.SuccessResponse(result, "Program başarıyla oluşturuldu."));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWorkoutProgram([FromBody] UpdateWorkoutProgramDto updateDto)
        {
            await _workoutProgramService.UpdateWorkoutProgramAsync(updateDto);
            return Ok(ApiResponse<object>.SuccessMessage("Program başarıyla güncellendi."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutProgram(int id)
        {
            await _workoutProgramService.DeleteWorkoutProgramAsync(id);
            return Ok(ApiResponse<object>.SuccessMessage("Program başarıyla silindi."));
        }

        [HttpPost("clone/{programId}")]
        public async Task<IActionResult> CloneSystemProgramToUser(int programId, [FromQuery] int userId)
        {
            var newProgramId = await _workoutProgramService.CloneSystemProgramToUserAsync(programId, userId);
            return Ok(ApiResponse<object>.SuccessResponse(new { NewProgramId = newProgramId }, "Program başarıyla kopyalandı."));
        }

        [HttpPost("activate")]
        public async Task<IActionResult> ActivateProgram([FromQuery] int programId, [FromQuery] int userId)
        {
            await _workoutProgramService.ActivateProgramAsync(programId, userId);
            return Ok(ApiResponse<object>.SuccessMessage("Program başarıyla aktifleştirildi."));
        }
    }
}
