using Application.DTO.Exercise;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(exercises);
        }

        // api/Exercise/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            return Ok(exercise);
        }

        // api/Exercise/Search?name=Bench
        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var exercises = await _exerciseService.SearchExercisesByNameAsync(name);
            return Ok(exercises);
        }

        // api/Exercise/ByMuscleGroup/Chest
        [HttpGet("ByMuscleGroup/{muscleGroup}")]
        public async Task<IActionResult> GetByMuscleGroup([FromRoute] string muscleGroup)
        {
            var exercises = await _exerciseService.GetExercisesByMuscleGroup(muscleGroup);
            return Ok(exercises);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExerciseDto createDto)
        {
            var created = await _exerciseService.AddExerciseAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateExerciseDto updateDto)
        {
            var result = await _exerciseService.UpdateExerciseAsync(updateDto);
            return Ok(result);
        }

        // api/Exercise/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _exerciseService.DeleteExerciseAsync(id);
            return Ok(result);
        }
    }
}
