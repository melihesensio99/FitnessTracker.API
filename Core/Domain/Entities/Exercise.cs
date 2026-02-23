using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Exercise
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; }
        public string TargetMuscle { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<ProgramExercise> ProgramExercises { get; set; } = new List<ProgramExercise>();
    }
}
