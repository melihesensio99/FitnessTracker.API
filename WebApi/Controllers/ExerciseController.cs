using Application.Abstraction.Services;
using Application.Common;
using Application.DTO.Exercise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exercises = await _exerciseService.GetAllExercisesAsync();
            return Ok(ApiResponse<List<ExerciseDto>>.SuccessResponse(exercises));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            return Ok(ApiResponse<ExerciseDto>.SuccessResponse(exercise));
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var exercises = await _exerciseService.SearchExercisesByNameAsync(name);
            return Ok(ApiResponse<List<ExerciseDto>>.SuccessResponse(exercises));
        }

        [HttpGet("ByMuscleGroup/{muscleGroup}")]
        public async Task<IActionResult> GetByMuscleGroup([FromRoute] string muscleGroup)
        {
            var exercises = await _exerciseService.GetExercisesByMuscleGroup(muscleGroup);
            return Ok(ApiResponse<ExerciseDto>.SuccessResponse(exercises));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateExerciseDto createDto)
        {
            var created = await _exerciseService.AddExerciseAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                ApiResponse<ExerciseDto>.SuccessResponse(created, "Egzersiz başarıyla oluşturuldu."));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateExerciseDto updateDto)
        {
            await _exerciseService.UpdateExerciseAsync(updateDto);
            return Ok(ApiResponse<object>.SuccessMessages("Egzersiz başarıyla güncellendi."));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _exerciseService.DeleteExerciseAsync(id);
            return Ok(ApiResponse<object>.SuccessMessages("Egzersiz başarıyla silindi."));
        }
    }
}
