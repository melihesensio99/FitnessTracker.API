using System;

namespace Application.DTO.WorkoutLog
{
    public class CreateWorkoutLogDto
    {
        public int ProgramExerciseId { get; set; }
        public int ActualSet { get; set; }
        public int ActualRep { get; set; }
        public decimal ActualWeight { get; set; }
        public DateTime Date { get; set; }
        public bool IsSuccess { get; set; }
    }
}
