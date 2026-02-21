using Application.DTO.ProgramExercise;

namespace Application.DTO.WorkoutProgram
{
    public class UpdateWorkoutProgramDto
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        
        public List<UpdateProgramExerciseDto> Exercises { get; set; } = new List<UpdateProgramExerciseDto>();   

    }
}
