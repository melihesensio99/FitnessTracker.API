using Application.DTO.ProgramExercise;

namespace Application.DTO.WorkoutProgram
{
    public class UpdateWorkoutProgramDto
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsPublic { get; set; }
        public string Ambition { get; set; }
        public List<UpdateProgramExerciseDto> ProgramExercises { get; set; } = new List<UpdateProgramExerciseDto>();   

    }
}
