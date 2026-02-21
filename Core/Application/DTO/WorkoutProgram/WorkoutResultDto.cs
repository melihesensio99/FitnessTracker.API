using Application.DTO.ProgramExercise;
using System.Collections.Generic;

namespace Application.DTO.WorkoutProgram
{
    public class WorkoutResultDto
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public ICollection<ProgramExerciseDto> ProgramExercises { get; set; } = new List<ProgramExerciseDto>();
    }
}
