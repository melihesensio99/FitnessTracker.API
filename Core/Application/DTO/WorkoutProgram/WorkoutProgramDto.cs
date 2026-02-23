using Application.DTO.ProgramExercise;
using System.Collections.Generic;

namespace Application.DTO.WorkoutProgram
{
    public class WorkoutProgramDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Title { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public string Ambition { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProgramExerciseDto> ProgramExercises { get; set; } = new List<ProgramExerciseDto>();
    }
}
