using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ProgramExercise
    {
        public int Id { get; set; }
        public int WorkoutProgramId { get; set; }
        public int ExerciseId { get; set; }
        public DayOfWeek DayOfWeek { get; set; } 
        public int TargetSet { get; set; }
        public int TargetRep { get; set; }
        public int? RestTime { get; set; }
        public int WeekNumber { get; set; }
        public int? TargetWeightPercent { get; set; }
        public WorkoutProgram WorkoutProgram { get; set; }
        public Exercise Exercise { get; set; }
        public ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
    }
}
