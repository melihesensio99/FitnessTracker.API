using Application.Abstraction.Services;
using Application.Common;
using Application.DTO.WorkoutLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutLogController : BaseController
    {
        private readonly IWorkoutLogService _workoutLogService;

        public WorkoutLogController(IWorkoutLogService workoutLogService)
        {
            _workoutLogService = workoutLogService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddWorkoutLog([FromBody] CreateWorkoutLogDto createDto)
        {
            var userId = GetCurrentUserId();
            var result = await _workoutLogService.AddWorkoutLogAsync(createDto, userId);
            return Ok(ApiResponse<WorkoutLogDto>.SuccessResponse(result, "Antrenman kaydı başarıyla eklendi."));
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUserLogs()
        {
            var userId = GetCurrentUserId();
            var logs = await _workoutLogService.GetUserLogsAsync(userId);
            return Ok(ApiResponse<List<WorkoutLogDto>>.SuccessResponse(logs));
        }


        [HttpGet("exercise/{programExerciseId}")]
        [Authorize]
        public async Task<IActionResult> GetExerciseLogs(int programExerciseId)
        {
            var logs = await _workoutLogService.GetExerciseLogsAsync(programExerciseId);
            return Ok(ApiResponse<List<WorkoutLogDto>>.SuccessResponse(logs));
        }

        [HttpGet("history")]
        [Authorize]
        public async Task<IActionResult> GetLogsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var userId = GetCurrentUserId();
            var logs = await _workoutLogService.GetLogsByDateRangeAsync(userId, startDate, endDate);
            return Ok(ApiResponse<List<WorkoutLogDto>>.SuccessResponse(logs));
        }

        [HttpGet("last/{programExerciseId}")]
        [Authorize]
        public async Task<IActionResult> GetUserLastLog(int programExerciseId)
        {
            var userId = GetCurrentUserId();
            var log = await _workoutLogService.GetUserLastLogAsync(userId, programExerciseId);
            return Ok(ApiResponse<WorkoutLogDto?>.SuccessResponse(log));
        }
    }
}