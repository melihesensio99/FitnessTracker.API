namespace Application.DTO.WorkoutProgram
{
    public class CreateWorkoutProgramDto
    {
        public int? UserId { get; set; } 
        public string Title { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        
        public List<Application.DTO.ProgramExercise.CreateProgramExerciseDto> Exercises { get; set; } = new List<Application.DTO.ProgramExercise.CreateProgramExerciseDto>();
    }
}
