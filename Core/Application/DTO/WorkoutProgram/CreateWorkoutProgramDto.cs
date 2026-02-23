using Application.DTO.ProgramExercise;

namespace Application.DTO.WorkoutProgram
{
    public class CreateWorkoutProgramDto
    {
        public int? UserId { get; set; } 
        public string Title { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }

        public string Ambition { get; set; }
        public bool IsPublic { get; set; }
        
        public List<CreateProgramExerciseDto> ProgramExercises { get; set; } = new List<CreateProgramExerciseDto>();
    }
}
