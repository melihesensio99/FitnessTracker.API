using System;

namespace Application.DTO.ProgramExercise
{
    public class CreateProgramExerciseDto
    {
        public int ExerciseId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public int TargetSet { get; set; }
        public int TargetRep { get; set; }
        public int RestTime { get; set; }
        public int WeekNumber { get; set; }
        public decimal? TargetWeightPercent { get; set; }
    }
}
