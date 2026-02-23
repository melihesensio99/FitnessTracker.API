namespace Application.DTO.Exercise
{
    public class CreateExerciseDto
    {
        public string ExerciseName { get; set; }
        public string TargetMuscle { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImageUrl { get; set; }
    }
}
