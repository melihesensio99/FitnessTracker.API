using System;
using Domain.Enums;

namespace Application.DTO.WorkoutLog
{
    public class CreateWorkoutLogDto
    {
        // UserId burada YOK — token'dan alınır, client'tan gelmez
        public int ProgramExerciseId { get; set; }
        public int ActualSet { get; set; }
        public int ActualRep { get; set; }
        public decimal ActualWeight { get; set; }
        public DateTime Date { get; set; }
        public WorkoutStatus Status { get; set; } = WorkoutStatus.Success;
    }
}
