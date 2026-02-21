using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class WorkoutProgram
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }

        public bool IsPublic { get; set; }
        public User User { get; set; }
        public ICollection<ProgramExercise> ProgramExercises { get; set; } = new List<ProgramExercise>();
    }
}
