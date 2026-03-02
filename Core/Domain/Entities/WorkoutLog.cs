using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class WorkoutLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProgramExerciseId { get; set; }
        public int ActualSet { get; set; }
        public int ActualRep { get; set; }
        public int Weight { get; set; }
        public DateTime Date { get; set; }
        public WorkoutStatus Status { get; set; }
        public User User { get; set; }
        public ProgramExercise ProgramExercise { get; set; }
    }
}
